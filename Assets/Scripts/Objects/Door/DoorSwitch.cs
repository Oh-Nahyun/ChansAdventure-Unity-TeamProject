using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorSwitch : DoorBase
{
    readonly int IsOpenHash = Animator.StringToHash("Open");

    protected override void Awake()
    {
        isLock = true;
        base.Awake();
    }

    /// <summary>
    /// 문 열림 처리함수
    /// </summary>
    public new void OpenDoor()
    {
        if (otherObject)
        {
            open = !open;
            animator.SetBool(IsOpenHash, open);
        }
    }
}
