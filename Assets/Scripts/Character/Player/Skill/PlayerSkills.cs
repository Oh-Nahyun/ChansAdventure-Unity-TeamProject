using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 스킬내용만 받는 스크립트
/// </summary>
public class PlayerSkills : MonoBehaviour
{
    Skill[] skills;

    // Delegates
    // 스킬 발동 관련 델리게이트
    public Action onSKillAction;
    public Action useSkillAction;
    public Action offSkillAction;
    /// <summary>
    /// 특수스킬키를 눌렀을 때 발동되는 델리게이트
    /// </summary>
    public Action<SpecialKey> onSpecialKey;
    public Func<SpecialKey, SkillName> onSpecialKeyUI;

    Action onDrop;

    public enum SpecialKey
    {
        None = 0,               // 키를 뗐을 때
        NumPad8_Up,
        NumPad5_Down,
        NumPad4_Left,
        NumPad6_Right,
    }

    /// <summary>
    /// 현재 활성화된 스킬
    /// </summary>
    SkillName currentSkillName = SkillName.RemoteBomb_Cube;

    public SkillName CurrentSkillName
    {
        get => currentSkillName;
        private set
        {
            if (currentSkillName != value)
            {
                currentSkillName = value;
                ConnectSkill(currentSkillName);
                onSkillChange?.Invoke(GameManager.Instance.Skill[currentSkillName]);
            }
        }
    }

    int CurrentSkillIndex => (int)CurrentSkillName;

    /// <summary>
    /// 선택된 스킬이 바뀌었음을 알리는 델리게이트 (F1:리모컨폭탄 F2:리모컨폭탄큐브 F3:마그넷캐치 F4:아이스메이커 F5:타임록)
    /// </summary>
    public Action<SkillName> onSkillSelect;

    public Action<Skill> onSkillChange;

    public Action<float> onCooltimeChange;

    public Action<SkillName> onSkillAcquisition;

    public Action<SkillName> onSKillSuccess;
    public Action offSkill;

    float[] maxCooltimes;
    float[] cooltimes;

    bool[] isUsableSkills;

    public bool[] IsUableSkills => isUsableSkills; // 사용 가능한 스킬 배열

    bool isEmptySkill = true;


    int SkillCount => Enum.GetValues(typeof(SkillName)).Length;


    Action<bool> onSkillMotionChange;


    void Awake()
    {
        PlayerSkillRelatedAction relatedAction = GetComponent<PlayerSkillRelatedAction>();

        relatedAction.onSkill += OnSkill;
        relatedAction.onSkillInteraction += () =>
        {
            useSkillAction?.Invoke();
        };

        relatedAction.onSkillCancel += () => offSkillAction?.Invoke();

        relatedAction.onSkillChange += (skillName) =>
        {
            if (isUsableSkills[(int)skillName])
            {
                CurrentSkillName = skillName;
            }
        };

        relatedAction.onSpecialKey += (specialKey) =>
        {
            onSpecialKey?.Invoke(specialKey);

            MagnetCatch magnet = skills[CurrentSkillIndex] as MagnetCatch;
            if((magnet == null || !magnet.IsActivate) && !isEmptySkill)
            {
                SkillName? skillName = onSpecialKeyUI?.Invoke(specialKey);
                if (skillName.HasValue)
                {
                    CurrentSkillName = skillName.Value;
                }
            }
        };

        onSkillSelect += relatedAction.SetSelectSkill;

        //relatedAction.onPickUp += (isPickUp) => isUsableSkill = !isPickUp;

        onDrop += relatedAction.Drop;

        onSkillMotionChange += (isUse) =>
        {
            relatedAction.SetSkillUseAnimation(isUse);
            if (isUse)
            {
                offSkill?.Invoke();
            }
        };

        skills = new Skill[SkillCount];
        cooltimes = new float[SkillCount];
        maxCooltimes = new float[SkillCount];

        int skillCount = Enum.GetValues(typeof(SkillName)).Length;

        isUsableSkills = new bool[skillCount];
    }

    private void Start()
    {
        CurrentSkillName = SkillName.RemoteBomb;    // 첫 스킬 리모컨폭탄으로 설장하기 위해 시작하자마자 바꿔줌


        SkillManager skillManager = GameManager.Instance.Skill;
        for (int i = 0; i < SkillCount; i++)
        {
            maxCooltimes[i] = skillManager[(SkillName)i].Cooltime;
            cooltimes[i] = maxCooltimes[i];
        }

        // 저장된 스킬 불러오기
        for (int i = 0; i < 5; i++)
        {
            if (GameManager.Instance.ActivatedSkill[i]) SkillAcquisition((SkillName)i);
        }
    }

    private void Update()
    {
        for(int i = 0; i < SkillCount;i++)
        {
            cooltimes[i] += Time.deltaTime;
            if(i == CurrentSkillIndex)
            {
                float ratio = cooltimes[i] / maxCooltimes[i];
                //Debug.Log(ratio);
                onCooltimeChange?.Invoke(ratio);                // 쿨타임 표시 재설정
            }
        }
    }

    void OnSkill()
    {
        if (cooltimes[CurrentSkillIndex] > maxCooltimes[CurrentSkillIndex] && isUsableSkills[CurrentSkillIndex])
        {
            Skill skill = skills[CurrentSkillIndex];

            if (skill == null)                                                               // 스킬이 현재 소환되어 있지 않으면
            {
                skill = Factory.Instance.GetSkill(currentSkillName).GetComponent<Skill>();  // 팩토리에서 해당 스킬 가져오기
                skill.OnSKillInitialize(transform);                                         // 사용자는 이스크립트를 가진 트랜스폼 (= 플레이어)
                skills[CurrentSkillIndex] = skill;
                onSKillSuccess?.Invoke(CurrentSkillName);
            }

            ConnectSkill(CurrentSkillName);

            onSpecialKey = skill.InputSpecialKey;

            // 순서 중요: 스킬발동 -> 들기
            onSKillAction?.Invoke();                     // 스킬 발동
            skill?.TryPickUp(transform);    // 들기 (해당 transform의(Player) 다른 스크립트인 PlayerRelatedAction에 ILifter가 존재)
        }
        
    }



    void ConnectSkill(SkillName skiilName)
    {
        // 현재 스킬 종류로 설정
        CurrentSkillName = skiilName;
        onSkillSelect?.Invoke(CurrentSkillName);

        // 스킬 변경시 델리게이트 연결 해제
        onSKillAction = null;
        useSkillAction = null;
        offSkillAction = null;
        onSpecialKey = null;



        // 설정된 스킬이 현재 발동 중이면 스킬 관련(시작, 사용중, 종료) 델리게이트 연결
        // 설정된 스킬로 현재사용중인 스킬 연결 (각 스킬이 없으면 null)
        Skill skill = skills[(int)skiilName];
        if (skill != null)
        {
            onSKillAction = skill.OnSkill;
            useSkillAction = skill.UseSkill;
            offSkillAction = skill.OffSkill;

            skill.cancelSkill = CancelSkill;
            skill.cooltimeReset = CoolTimeReset;
            skill.onMotionChange = onSkillMotionChange;

        }
    }

    void CancelSkill(SkillName skillName)
    {
        onDrop?.Invoke();
        offSkill?.Invoke();

        onSKillAction = null;
        useSkillAction = null;
        offSkillAction = null;
        onSpecialKey = null;

        //skills[(int)CurrentSkillName].cancelSkill = null;
        skills[(int)skillName] = null;
    }

    void CancelSkill()
    {
        CancelSkill(CurrentSkillName);
    }

    void CoolTimeReset(SkillName skillName)
    {
        cooltimes[(int)skillName] = 0;
    }

    public void SkillAcquisition(SkillName name)
    {
        if (!isUsableSkills[(int)name])
        {
            isEmptySkill = false;
            isUsableSkills[(int)name] = true;
            onSkillAcquisition?.Invoke(name);
            CurrentSkillName = name;

            GameManager.Instance.ActivatedSkill[(int)name] = true;
        }
    }
}
