using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    private Transform scaleObjR; // 오른쪽 저울 판(Transform)
    private Transform scaleObjL; // 왼쪽 저울 판(Transform)

    public float weightPerUnit = 1f; // 단위 무게 (1 유니티 단위당 몇 kg인지)

    private float transitionDuration = 1f; // 이동에 걸리는 시간

    private float transitionTimer = 0f; // 현재 이동된 시간

    private float initialHeightR; // 오른쪽 저울 판의 초기 Y값
    private float initialHeightL; // 왼쪽 저울 판의 초기 Y값

    private Vector3 targetHeightR; // 오른쪽 저울 판의 목표 높이
    private Vector3 targetHeightL; // 왼쪽 저울 판의 목표 높이

    private float totalWeightR; // 오른쪽 저울에 올려진 총 무게
    private float totalWeightL; // 왼쪽 저울에 올려진 총 무게

    ScaleObj scaleColliderR;
    ScaleObj scaleColliderL;

    private float heightR;
    private float heightL;

    private void Awake()
    {

        Transform child = transform.GetChild(0);
        scaleObjR = child.GetComponent<Transform>();
        scaleColliderR = child.GetComponent<ScaleObj>();
        child = transform.GetChild(1);
        scaleObjL = child.GetComponent<Transform>();
        scaleColliderL = child.GetComponent<ScaleObj>();

    }

    private void Start()
    {
        initialHeightR = scaleObjR.localPosition.y;
        initialHeightL = scaleObjL.localPosition.y;

        heightR = initialHeightR;
        heightL = initialHeightL;
    }

    private void Update()
    {
        transitionTimer += Time.deltaTime;
        
        totalWeightR = scaleColliderR.totalWeight - scaleColliderL.totalWeight;
        totalWeightL = scaleColliderL.totalWeight - scaleColliderR.totalWeight;
        
        float t = Mathf.Clamp01(transitionTimer / transitionDuration); // 시간의 경과에 따른 보간 값 계산

        // 보간된 위치 설정
        Vector3 newHeightR = Vector3.Lerp(scaleObjR.localPosition, targetHeightR, t);
        Vector3 newHeightL = Vector3.Lerp(scaleObjL.localPosition, targetHeightL, t);

        InitialHeight();
        // 각 저울 판의 위치 업데이트
        scaleObjR.localPosition = newHeightR;
        scaleObjL.localPosition = newHeightL;
    }

    /// <summary>
    /// 저울의 무게를 비교하는 함수
    /// </summary>
    private void InitialHeight()
    {

        if (totalWeightR == totalWeightL)
        {
            heightR = initialHeightR;
            heightL = initialHeightL;
        }
        else if (totalWeightR < totalWeightL)
        {
            heightR = initialHeightR + (totalWeightR / weightPerUnit);
            heightL = initialHeightL - (totalWeightR / weightPerUnit);
        }
        else if (totalWeightL < totalWeightR)
        {
            heightR = initialHeightR - (totalWeightL / weightPerUnit);
            heightL = initialHeightL + (totalWeightL / weightPerUnit);
        }

        // 각 저울 판의 목표 높이 계산

        // 목표 높이 설정
        targetHeightR = scaleObjR.localPosition;
        targetHeightR.y = heightR;

        targetHeightL = scaleObjL.localPosition;
        targetHeightL.y = heightL;

        targetHeightR.y = Mathf.Clamp(heightR, -8f, 0f);
        targetHeightL.y = Mathf.Clamp(heightL, -8f, 0f);
        // 이동된 시간 초기화
        transitionTimer = 0f;
        
    }
}

