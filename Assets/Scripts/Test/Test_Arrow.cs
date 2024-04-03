using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Arrow : TestBase
{
    public float arrowSpeed = 7.0f;

    public GameObject arrowPrefab;

    Transform fireTransform;

    //Arrow arrow;

    private void Awake()
    {
        fireTransform = transform.GetChild(0);
        //arrow = GetComponent<Arrow>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //transform.Translate(Time.deltaTime * arrowSpeed * Vector3.up); // 화살 발사
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Instantiate(arrowPrefab, fireTransform); // 화살 생성 후 발사
    }
}
