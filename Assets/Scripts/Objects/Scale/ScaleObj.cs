using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObj : MonoBehaviour
{
    public float totalWeight; // 현재 저울에 올려진 총 무게

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            // 저울에 물체가 들어왔을 때
            Rigidbody rb = other.GetComponent<Rigidbody>();
            Weighable weighable = other.GetComponent<Weighable>();
            if (rb != null && weighable != null)
            {
                // 물체의 무게를 가져와서 저울의 무게에 추가
                totalWeight -= weighable.weigh;
            }
        }
        else
        {
            totalWeight = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            // 저울에 물체가 들어왔을 때
            Rigidbody rb = other.GetComponent<Rigidbody>();
            Weighable weighable = other.GetComponent<Weighable>();
            if (rb != null)
            {
                // 물체의 무게를 가져와서 저울의 무게에 추가
                totalWeight += weighable.weigh;
            }
        }
        else
        {
            totalWeight = 0f;
        }
    }
}
