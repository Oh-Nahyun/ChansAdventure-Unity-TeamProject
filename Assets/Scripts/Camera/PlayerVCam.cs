using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVCam : MonoBehaviour
{
    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateAngle = 2.0f;
    public float maxAngle = 30.0f;


    protected float angleY = 0f;
    protected float angleX = 0f;

    protected CinemachineVirtualCamera vCam;
    public CinemachineVirtualCamera VCam => vCam;
    protected Vector2 currMousePos;
    protected Vector2 preMousePos;
    protected Transform cameraRoot;
    protected Cinemachine3rdPersonFollow personFollow;
    public Vector3 Offset => personFollow.ShoulderOffset;

    protected Player player;

    readonly protected Vector3 Center = new Vector3(0.5f, 0.5f, 0.0f);

    public Action<Quaternion> onCameraRotate;
    public Action<Vector3> onMouseMove;

    protected void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        personFollow = vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        currMousePos = Mouse.current.position.value;
        preMousePos = currMousePos;
    }

    protected virtual void Start()
    {
        player = GameManager.Instance.Player;
        if (player != null )
        {
            cameraRoot = player.transform.GetChild(1);
            vCam.Follow = cameraRoot;
        }
        else
        {
            Debug.LogWarning("Player가 없습니다.");
        }

    }


    protected virtual void LateUpdate()
    {
        preMousePos = currMousePos;
        currMousePos = Mouse.current.position.value;
        Vector2 dir = currMousePos - preMousePos;
        dir = dir.normalized;
        angleY += dir.x * Time.deltaTime * rotateAngle;
        angleX -= dir.y * Time.deltaTime * rotateAngle;
        angleX = Mathf.Clamp(angleX, -maxAngle, maxAngle);


        Quaternion rotate = Quaternion.Euler(angleX, angleY, 0);
        cameraRoot.rotation = rotate;

        onMouseMove?.Invoke(dir);
    }

    public Vector3 GetWorldPositionCenter()
    {
        Vector3 screenPoint = Camera.main.ViewportToScreenPoint(Center);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPoint);
        return worldPosition;
    }

#if UNITY_EDITOR
#endif
}
