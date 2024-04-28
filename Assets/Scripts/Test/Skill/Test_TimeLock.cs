using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TimeLock : TestBase
{
    public ReactionObject target;
    public ReactionObject bomb;
    Vector3 targetPos;

    void Start()
    {
        targetPos = target.transform.position;
        Debug.Log("타임록 폭발 테스트");
        Debug.Log("키 1: 테스트타겟의 왼쪽에서 폭발");
        Debug.Log("키 2: 테스트타겟의 오른쪽에서 폭발");
        Debug.Log("키 3: 테스트타겟의 뒤쪽에서 폭발");
        Debug.Log("키 4: 테스트타겟의 앞쪽에서 폭발");
        Debug.Log("키 4: 테스트타겟의 아래쪽에서 폭발");
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Instantiate(bomb);
        Vector3 pos = targetPos;
        pos.x -= 0.5f;
        bomb.transform.position = pos;
        bomb.TestBoom();
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Instantiate(bomb);
        Vector3 pos = targetPos;
        pos.x += 0.5f;
        bomb.transform.position = pos;
        bomb.TestBoom();
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Instantiate(bomb);
        Vector3 pos = targetPos;
        pos.z += 0.5f;
        bomb.transform.position = pos;
        bomb.TestBoom();
    }
    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Instantiate(bomb);
        Vector3 pos = targetPos;
        pos.z -= 0.5f;
        bomb.transform.position = pos;
        bomb.TestBoom();
    }
    protected override void OnTest5(InputAction.CallbackContext context)
    {
        Instantiate(bomb);
        Vector3 pos = targetPos;
        pos.y -= 0.5f;
        bomb.transform.position = pos;
        bomb.TestBoom();
    }

}
