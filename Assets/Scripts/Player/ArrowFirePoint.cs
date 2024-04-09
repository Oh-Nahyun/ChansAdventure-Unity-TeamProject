using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFirePoint : MonoBehaviour
{
    /// <summary>
    /// 화살 사거리
    /// </summary>
    public float arrowFireRange = 1.0f;

    /// <summary>
    /// 풀 타입
    /// </summary>
    public PoolObjectType type;

    /// <summary>
    /// 화살 프리팹
    /// </summary>
    public GameObject arrowPrefab;

    /// <summary>
    /// 캐릭터의 오른손
    /// </summary>
    Transform rightHand;

    /// <summary>
    /// 화살 발사 위치
    /// </summary>
    Transform fireTransform;

    private void Start()
    {
        rightHand = GameObject.FindWithTag("RightHand").transform;
        fireTransform = transform.GetChild(0);
    }

    private void Update()
    {
        fireTransform.position = rightHand.position; // 화살 발사 위치는 캐릭터 오른손 위치와 일치
    }

    /// <summary>
    /// 화살 발사용 함수
    /// </summary>
    public void FireArrow()
    {
        //Instantiate(arrowPrefab, fireTransform); // 화살 생성 후 발사
        Factory.Instance.GetObject(type, fireTransform.position, new Vector3(90.0f, 0f, 0f));
    }
}
