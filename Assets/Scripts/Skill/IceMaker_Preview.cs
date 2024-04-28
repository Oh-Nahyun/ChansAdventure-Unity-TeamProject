using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMaker_Preview : MonoBehaviour
{
    float intervalTime = 1f;
    float elapsedTime = 0f;

    bool isNotBlink = false;

    const float cosineTime = 3f;

    Animator animator;
    Material material;

    Color originColor;
    Color clearColor;

    readonly int Hash_IsValid = Animator.StringToHash("IsValid");
    //readonly int Hash_Generate = Animator.StringToHash("Generate");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Renderer renderer = GetComponentInChildren<Renderer>();
        material = renderer.material;
        originColor = material.color;
    }

    private void OnEnable()
    {
        SetInvisible();         // 안보이는 상태로 시작 (처음에 잠깐 나타나는 경우가 있음)
    }

    private void Update()
    {
        if (!isNotBlink)
        {
            //Mathf.cos 1->0 3초
            elapsedTime += Time.deltaTime * intervalTime;
            float alpha = (Mathf.Cos(elapsedTime) + 1) * 0.5f;

            Color color = originColor;
            color.a = alpha * color.a;
            material.color = color;
        }
    }

    /// <summary>
    /// 아이스메이커 프리뷰 초기화 메서드
    /// </summary>
    /// <param name="blinkInterval">깜빡거릴 속도</param>
    /// <param name="size">얼음의 크기</param>
    public void Initialize(float blinkInterval, Vector3 size)
    {
        intervalTime = cosineTime / blinkInterval;  // 얼음 깜빡거릴 시간 설정
        transform.GetChild(0).localScale = size;    // 얼음 모양의 크기 설정
    }

    /// <summary>
    /// 프리뷰를 보이게 만드는 메서드
    /// </summary>
    public void SetVisible()
    {
        isNotBlink = true;
        material.color = originColor;
    }
    /// <summary>
    /// 프리뷰를 안보이게 만드는 메서드
    /// </summary>
    public void SetInvisible()
    {
        isNotBlink = true;
        material.color = Color.clear;
    }
    /// <summary>
    /// 얼음을 생성 가능한 위치를 가져오는 메서드
    /// </summary>
    /// <param name="isValid">얼음을 생성 가능한 위치인지 아닌지(true: 생성가능)</param>
    public void ValidPosition(bool isValid)
    {
        animator.SetBool(Hash_IsValid, isValid);    // 생성가능할 때 나오는 애니메이션 설정
        isNotBlink = isValid;                       // 깜빡거리지 않음
    }
}
