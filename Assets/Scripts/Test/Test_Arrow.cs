using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Bullet : TestBase
{
    public PoolObjectType type;
    public float interval = 0.1f;

    Transform fireTransform;
    public GameObject arrowPrefab;
    ArrowFirePoint arrowFirePoint;

    private void Start()
    {
        fireTransform = transform.GetChild(0);
        arrowFirePoint = FindAnyObjectByType<ArrowFirePoint>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Instantiate(arrowPrefab, fireTransform); // 화살 생성 후 발사
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        StartCoroutine(FireCountinuos());
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Factory.Instance.GetObject(type, fireTransform.position);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        StartCoroutine(FireCountinuos2());
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        arrowFirePoint.FireArrow();
    }

    IEnumerator FireCountinuos()
    {
        while (true)
        {
            Instantiate(arrowPrefab, fireTransform);
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator FireCountinuos2()
    {
        while (true)
        {
            Factory.Instance.GetObject(type, fireTransform.position);
            yield return new WaitForSeconds(interval);
        }
    }
}
