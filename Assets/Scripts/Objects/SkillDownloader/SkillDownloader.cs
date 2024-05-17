using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDownloader : NPCBase
{
    public Sprite sprite;


    protected override void Awake()
    {
        base.Awake();
        isTextObject = true;
        isNPC = false;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        OpenChest(isTalk);
    }

    /// <summary>
    /// 상자를 열었을 때 처리하는 함수
    /// </summary>
    /// <param name="isOpen">상자 상태</param>
    private void OpenChest(bool isOpen)
    {
        if (isOpen)
        {
            if(id == 301)
            {
                GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.RemoteBomb_Cube);
                GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.RemoteBomb);
                Debug.Log("리모컨폭탄 등록");
            }
            else if (id == 302)
            {
                GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.IceMaker);
                Debug.Log("아이스메이커 등록");
            }
            else if (id == 303)
            {
                GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.MagnetCatch);
                Debug.Log("마그넷캐치 등록");
            }
            else if (id == 304)
            {
                GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.TimeLock);
                Debug.Log("타임록 등록");
            }
            gameObject.layer = 0;
        }
    }
}
