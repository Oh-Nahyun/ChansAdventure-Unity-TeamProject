using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 발사체를 제어하는 스크립트
/// </summary>
public class EnemyProjectile : MonoBehaviour
{
    /// <summary>
    ///  적 이동 스크립트
    /// </summary>
    EnemyMovement movement;

    /// <summary>
    /// 적 발사체의 최대 사거리
    /// </summary>
    float projectileDistance = 30.0f;

    /// <summary>
    /// 총알의 데미지
    /// </summary>
    int damage = 5;

    /// <summary>
    /// 이동을 실행할 함수
    /// </summary>
    /// <param name="position">지나가는 방향</param>
    public void Setup(Vector3 position)
    {
        movement = GetComponent<EnemyMovement>();

        StartCoroutine("OnMove", position); // 
    }

    /// <summary>
    /// 이동 방향 설정, 이동 초과 확인용 코루틴
    /// </summary>
    /// <param name="targetPosition">타겟의 위치</param>
    /// <returns></returns>
    IEnumerator OnMove(Vector3 targetPosition)
    {
        // 시작 위치
        Vector3 start = transform.position;

        // 이동 방향을 설정(타겟의 위치 - 자신의 위치)정규화
        movement.MoveTo((targetPosition - transform.position).normalized);

        while (true)
        {
            if(Vector3.Distance(transform.position, start) >= projectileDistance)
            {
                // 자신의 위치와 시작 위치의 거리가 적 발사체의 최대 사거리 보다 크면
                Destroy(gameObject); // 오브젝트 파괴

                yield break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))  // 닿은 대상의 태그가 Player 이면
        {
            //Debug.Log("플레이어 맞음");
            // PlayerController 스크립트에 있는 TakeDamage함수를 호출하고 매개변수에 damage를 넣어준다
            other.GetComponent<Test_PlayerController>().TakeDamage(damage);
            //other.GetComponent<PlayerController>().TakeDamage(damage);

            Destroy(gameObject); // 오브젝트 파괴
        }
    }
}
