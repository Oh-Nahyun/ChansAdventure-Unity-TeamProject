using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowVCam : MonoBehaviour
{
    /// <summary>
    /// 카메라 변경 속도
    /// </summary>
    float speed = 0.5f;

    /// <summary>
    /// 카메라 Zoom 정도
    /// </summary>
    readonly Vector3 zoomIn = new Vector3(0.25f, 0.0f, 2.0f);
    readonly Vector3 zoomOut = new Vector3(0.0f, 0.25f, -2.0f);

    // 컴포넌트들
    Weapon weapon;
    CinemachineVirtualCamera vcam;
    Cinemachine3rdPersonFollow follow;

    private void Awake()
    {
        weapon = FindAnyObjectByType<Weapon>();
        vcam = GetComponent<CinemachineVirtualCamera>();
        follow = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void Update()
    {
        ChangeCameraZoom();
    }

    void ChangeCameraZoom()
    {
        if (weapon.IsBowEquip) // 캐릭터가 활을 장비하고 있는 경우
        {
            // 플레이어가 마우스 왼쪽 버튼을 누르고 있는 경우
            if (Input.GetMouseButtonDown(0))
            {
                follow.ShoulderOffset = Vector3.Lerp(zoomIn, zoomOut, speed * Time.deltaTime);
                Debug.Log("Camera Zoom-In");
            }

            // 플레이어가 마우스 왼쪽 버튼에서 뗀 경우
            if (Input.GetMouseButtonUp(0))
            {
                follow.ShoulderOffset = Vector3.Lerp(zoomOut, zoomIn, speed * Time.deltaTime);
                Debug.Log("Camera Zoom-Out");
            }
        }
        else
        {
            follow.ShoulderOffset = zoomOut;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        
    }
#endif
}
