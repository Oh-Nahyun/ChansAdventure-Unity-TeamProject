using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowVCam : MonoBehaviour
{
    /// <summary>
    /// 카메라 변경 속도
    /// </summary>
    public float speed = 1.0f;

    /// <summary>
    /// 카메라 Zoom 정도
    /// </summary>
    readonly Vector3 zoomIn = new Vector3(0.25f, 0.0f, 2.0f);
    readonly Vector3 zoomOut = new Vector3(0.0f, 0.25f, -2.0f);

    /// <summary>
    /// 플레이어 Forward 방향에 위치한 트랜스폼
    /// </summary>
    Transform lookAtPosition;

    // 컴포넌트들
    Player player;
    Weapon weapon;
    CinemachineVirtualCamera vcam;
    Cinemachine3rdPersonFollow follow;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
        weapon = FindAnyObjectByType<Weapon>();
        vcam = GetComponent<CinemachineVirtualCamera>();
        follow = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        lookAtPosition = GameObject.FindWithTag("LookAtPosition").transform;
    }

    private void Update()
    {
        ChangeCameraZoom();
    }

    /// <summary>
    /// 플레이어가 화살을 사용할 경우의 카메라 세팅 함수
    /// </summary>
    void ChangeCameraZoom()
    {
        if (weapon.IsBowEquip && weapon.IsArrowEquip) // 캐릭터가 활을 장비하고 있고 화살을 장전하고 있는 경우
        {
            // 플레이어가 마우스 왼쪽 버튼을 누르고 있는 경우
            if (Input.GetMouseButtonDown(0))
            {
                // vcam.transform.rotation = Quaternion.LookRotation(player.transform.forward, Vector3.up); // 활시위를 당길 때, 캐릭터와 카메라가 같은 방향 바라보기
                StopAllCoroutines();
                StartCoroutine(Timer(true));
                // Debug.Log("Camera Zoom-In");
            }

            // 플레이어가 마우스 왼쪽 버튼에서 뗀 경우
            if (Input.GetMouseButtonUp(0))
            {
                StopAllCoroutines();
                StartCoroutine(Timer(false));
                // Debug.Log("Camera Zoom-Out");
            }
        }
        else
        {
            follow.ShoulderOffset = zoomOut;
            weapon.IsZoomIn = false;
        }
    }

    /// <summary>
    /// 마우스 입력에 따른 카메라 줌 관련 함수
    /// </summary>
    /// <param name="IsZoom">true이면 마우스 왼쪽 버튼을 누르고 있다는 의미이고, false이면 뗐다는 의미이다.</param>
    IEnumerator Timer(bool IsZoom)
    {
        float timeElapsed = 0.0f;
        while (timeElapsed < 1.0f)
        {
            timeElapsed += speed * Time.deltaTime;

            if (IsZoom)
            {
                follow.ShoulderOffset = Vector3.Lerp(zoomOut, zoomIn, timeElapsed);
                follow.Damping = new Vector3(0.0f, 0.0f, 0.0f); // 카메라 Damping 제거
                vcam.LookAt = lookAtPosition;
                weapon.IsZoomIn = true; // 활이 조금이라도 당겨지면 ZoomIn이 true가 된다.
                weapon.LoadArrowAfter();

                vcam.transform.position = lookAtPosition.transform.position + new Vector3(0.25f, 0.0f, -1.0f); /////////////
            }
            else
            {
                follow.ShoulderOffset = Vector3.Lerp(zoomIn, zoomOut, timeElapsed);
                follow.Damping = new Vector3(0.1f, 0.5f, 0.3f); // 카메라 Damping 생성
                vcam.LookAt = null;
                weapon.IsZoomIn = false;
                weapon.LoadArrowAfter();
            }

            yield return null;
        }
    }
}
