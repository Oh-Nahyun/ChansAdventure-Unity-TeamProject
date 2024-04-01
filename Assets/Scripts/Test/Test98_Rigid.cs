using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test98_Rigid : Skill_TestBase
{

    public GameObject obj;
    bool toggle = false;
    Rigidbody rigid;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        rigid = obj.GetComponent<Rigidbody>();
        toggle = !toggle;
    }

    private void Update()
    {
        if(toggle && rigid != null)
        {
            rigid.velocity = Vector3.forward * 2.0f;

        }
    }
}
