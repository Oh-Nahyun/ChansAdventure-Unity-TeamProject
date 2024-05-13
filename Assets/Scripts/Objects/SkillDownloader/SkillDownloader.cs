using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDownloader : NPCBase
{

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
            id = 299;
        }
    }
}
