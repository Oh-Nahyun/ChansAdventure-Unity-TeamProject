using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionGear : ReactionObject
{
    [Header("반응형 기어 데이터")]
    /// <summary>
    /// 최대 회전각
    /// </summary>
    public float maxRotateAngle = 30.0f;
    /// <summary>
    /// 회전하는 속도
    /// </summary>
    public float rotateSpeed = 20.0f;
    /// <summary>
    /// 회전하는 방향
    /// </summary>
    public int direction = 1;

    private void Update()
    {
        if (currentState != StateType.TimeLock)
        {
            float angle = Time.deltaTime * rotateSpeed;
            float resultAngle = angle * direction + rigid.rotation.eulerAngles.z;

            if (resultAngle > maxRotateAngle && resultAngle < 360 - maxRotateAngle)
            {
                direction = -direction;
                resultAngle = angle * direction + rigid.rotation.eulerAngles.z;
                rigid.angularVelocity = Vector3.zero;
            }

            Quaternion rotation = Quaternion.Euler(resultAngle * Vector3.forward);
            rigid.MoveRotation(rotation);
        }
        
    }
}
