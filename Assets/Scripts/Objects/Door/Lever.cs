using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : NPCBase
{
    /// <summary>
    /// 스위치의 상태
    /// </summary>
    enum State
    {
        Off = 0,    // 스위치가 꺼진 상태
        On,         // 스위치가 켜진 상태
    }

    /// <summary>
    /// 스위치의 현재 상태
    /// </summary>
    State state = State.Off;

    /// <summary>
    /// 스위치가 조작할 문을 가지고 있는 게임 오브젝트
    /// </summary>
    public GameObject target;

    readonly int SwitchOnHash = Animator.StringToHash("SwitchOn");

    DoorSwitch targetDoor;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        if (target != null)
        {
            targetDoor = target.GetComponent<DoorSwitch>(); // target에서 문 찾기
        }
        if (targetDoor == null)
        {
            Debug.LogWarning($"{gameObject.name}에게 사용할 문이 없습니다.");  // 문이 없으면 경고 출력
        }
        isNPC = false;
        otherObject = true;
        id = 299;
    }

    /// <summary>
    /// 스위치 사용
    /// </summary>
    public void Use()
    {
        if (targetDoor != null)  // 조작할 문이 있어야 한다.
        {
            targetDoor.isLock = false;
            switch (state)
            {
                case State.Off:
                    // 스위치를 켜는 상황
                    targetDoor.OpenDoor();                  // 문열고
                    animator.SetBool(SwitchOnHash, true);   // 스위치 애니메이션 재생
                    state = State.On;                       // 상태 변경
                    break;
                case State.On:
                    // 스위치를 끄려는 상황
                    targetDoor.OpenDoor();                  // 문 닫고
                    animator.SetBool(SwitchOnHash, false);  // 스위치 애니메이션 재생
                    state = State.Off;                      // 상태 변경
                    break;
            }
        }
    }
}
