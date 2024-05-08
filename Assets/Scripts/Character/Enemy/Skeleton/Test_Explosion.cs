using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Explosion : TestBase
{
    public GameObject target;

    Vector3 dir = Vector3.zero;

    public Transform exPos;

    Rigidbody rb;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        rb = target.GetComponent<Rigidbody>();
        dir = target.transform.position - exPos.position;

        rb.AddForce (dir * 5, ForceMode.Impulse);
    }
    // 프리팹에 콜라이더 넣고(안떨어지기위해)
    // 리지드 바디에 키네마틱 켜기(중력 받기위해)
    // Darg값 수정 하기 (중력 때문에 설정)
    // freeze rotation(설정하기)
}
