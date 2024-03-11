using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적의 이동을 제어하느 ㄴ스크립트
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    /// <summary>
    ///  이동 속도
    /// </summary>
    [SerializeField]
    float moveSpeed = 0.0f;

    /// <summary>
    /// 이동 방향
    /// </summary>
    [SerializeField]
    Vector3 moveDirection = Vector3.zero;

    private void Update()
    {
        transform.position += Time.deltaTime * moveSpeed * moveDirection;
    }

    /// <summary>
    /// 이동 방향을 설정할 함수
    /// </summary>
    /// <param name="direction">이동 방향</param>
    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
