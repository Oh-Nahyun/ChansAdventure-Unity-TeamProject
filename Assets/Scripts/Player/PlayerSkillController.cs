using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerSkillController : MonoBehaviour
{
    Transform handRootTracker;
    public Transform HandRoot => handRootTracker;
    Skill_Player player;

    public Action onSKillAction;
    public Action useSkillAction;
    public Action offSkillAction;

    SkillName currentSkill = SkillName.RemoteBomb;

    public ReactionObject CurrentOnSkill => currentOnSkill;
    ReactionObject currentOnSkill;

    RemoteBomb remoteBomb;
    RemoteBombCube remoteBombCube;
    MagnetCatch magnetCatch;

    private void Awake()
    {
        handRootTracker = transform.GetChild(3);
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        if (player == null)
        {
            Debug.LogError("플레이어가 없습니다.");
        }
        else
        {

            player.onSkillSelect += ConnectSkill;

            player.onSkill += () => onSKillAction?.Invoke();
            player.onSkill += OnSkill;
            player.rightClick += () => useSkillAction?.Invoke();
            player.onCancel += CancelSkill;

            //onSKillAction += OnSkill;

        }
    }

    void ConnectSkill(SkillName skiilName)
    {
        // 현재 스킬 종류로 설정
        currentSkill = skiilName;

        // 스킬 변경시 델리게이트 연결 해제
        onSKillAction = null;
        useSkillAction = null;
        offSkillAction = null;

        // 설정된 스킬이 현재 발동 중이면 스킬 관련(시작, 사용중, 종료) 델리게이트 연결
        // 설정된 스킬로 현재사용중인 스킬 연결 (각 스킬이 없으면 null)
        switch (currentSkill)
        {
            case SkillName.RemoteBomb:
                currentOnSkill = remoteBomb;
                if (remoteBomb != null)
                {
                    onSKillAction = remoteBomb.OnSkill;
                    useSkillAction = remoteBomb.UseSkill;
                    offSkillAction = remoteBomb.OffSkill;
                }

                break;
            case SkillName.RemoteBomb_Cube:
                currentOnSkill = remoteBombCube;
                if (remoteBombCube != null)
                {
                    onSKillAction = remoteBombCube.OnSkill;
                    useSkillAction = remoteBombCube.UseSkill;
                    offSkillAction = remoteBombCube.OffSkill;
                }
                break;
            case SkillName.MagnetCatch:
                currentOnSkill = magnetCatch;
                if (magnetCatch != null)
                {
                    onSKillAction = magnetCatch.OnSkill;
                    useSkillAction = magnetCatch.UseSkill;
                    offSkillAction = magnetCatch.OffSkill;
                }
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }

    }

    void OnSkill()
    {
        // 설정된 스킬별로 동작
        switch (currentSkill)
        {
            case SkillName.RemoteBomb:
                if (remoteBomb == null)     // 리모컨폭탄이 현재 소환되어 있지 않으면
                {
                    //Debug.Log("실행 : 리모컨 폭탄");
                    remoteBomb = SkillFactory.Instance.GetRemoteBomb(); // 팩토리에서 리모컨폭탄 가져온 뒤 리모컨 폭탄 변수에 설정
                    currentOnSkill = remoteBomb;                        // 현재 사용중인 스킬은 리모컨폭탄
                }
                else
                {
                    CancelSkill();          // 리모컨폭탄이 소환되어 있으면 터지면서 스킬 종료
                }

                break;
            case SkillName.RemoteBomb_Cube:
                if (remoteBombCube == null)
                {
                    //Debug.Log("실행 : 리모컨 폭탄 큐브");
                    remoteBombCube = SkillFactory.Instance.GetRemoteBombCube();
                    currentOnSkill = remoteBombCube;
                }
                else
                {
                    CancelSkill();
                }
                break;
            case SkillName.MagnetCatch:
                if (magnetCatch == null)
                {
                    //Debug.Log("실행 : 마그넷 캐치");
                    magnetCatch = SkillFactory.Instance.GetMagnetCatch();
                    currentOnSkill = magnetCatch;
                }
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }
        
        ConnectSkill(currentSkill);

        if (currentOnSkill != null)
        {
            currentOnSkill.PickUp(HandRoot);
            //currentOnSkill.transform.SetParent(HandRoot);
            //currentOnSkill.transform.position = HandRoot.position;
            //currentOnSkill.transform.forward = player.transform.forward;
        }

        onSKillAction?.Invoke();
    }

    void CancelSkill()
    {
        offSkillAction?.Invoke();
        switch (currentSkill)
        {
            case SkillName.RemoteBomb:
                remoteBomb = null;
                currentOnSkill = null;
                break;
            case SkillName.RemoteBomb_Cube:
                remoteBombCube = null;
                currentOnSkill = null;
                break;
            case SkillName.MagnetCatch:
                magnetCatch = null;
                currentOnSkill = null;
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }
    }


#if UNITY_EDITOR

    void TestSkillSelect(SkillName skillName)
    {
        Debug.Log(skillName);
    }

#endif
}
