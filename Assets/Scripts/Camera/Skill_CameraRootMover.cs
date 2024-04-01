using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Skill_CameraRootMover : MonoBehaviour
{
    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 180.0f;

    public float maxUpAngle = 30.0f;
    public float maxDownAngle = 15.0f;


    float angleY = 0f;
    float angleX = 0f;

    Vector2 currMousePos;
    Vector2 preMousePos;

    private void Awake()
    {
        currMousePos = Mouse.current.position.value;
        preMousePos = currMousePos;
    }

    protected virtual void LateUpdate()
    {
        Move();
    }

    void Move()
    {
        preMousePos = currMousePos;
        currMousePos = Mouse.current.position.value;
        Vector2 dir = currMousePos - preMousePos;
        dir = dir.normalized;
        angleY += dir.x * Time.deltaTime * rotateSpeed;
        angleX -= dir.y * Time.deltaTime * rotateSpeed;
        angleX = Mathf.Clamp(angleX, -maxDownAngle, maxUpAngle);


        Quaternion rotate = Quaternion.Euler(angleX, angleY, 0);
        transform.rotation = rotate;
    }
}
