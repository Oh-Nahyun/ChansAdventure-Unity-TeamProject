using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVCam : MonoBehaviour
{

    protected CinemachineVirtualCamera vCam;
    public CinemachineVirtualCamera VCam => vCam;
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
    }

    void Start()
    {
        player = GameManager.Instance.Player;
        if (player != null )
        {
            FllowSelector();
        }
        else
        {
            Debug.LogWarning("Player가 없습니다.");
        }

    }

    protected virtual void FllowSelector()
    {
        cameraRoot = player.CameraRoot;
        vCam.Follow = cameraRoot;
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
