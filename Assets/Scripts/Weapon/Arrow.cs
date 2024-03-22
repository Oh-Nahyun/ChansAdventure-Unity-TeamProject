using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    /// <summary>
    /// 화살 속도
    /// </summary>
    public float arrowSpeed = 7.0f;

    /// <summary>
    /// 화살 사거리
    /// </summary>
    public float arrowRange = 10.0f;

    // 컴포넌트들
    ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        transform.Rotate(90.0f, 0f, 0f); // 화살 오브젝트 회전

        // 이펙트 켜고 끄기
        //ps.Play();
        //ps.Stop();
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * arrowSpeed * Vector3.up); // 화살 발사
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 적에게 화살을 맞췄을 경우
        }
    }
}
