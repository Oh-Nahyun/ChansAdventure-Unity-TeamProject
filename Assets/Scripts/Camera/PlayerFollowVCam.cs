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
        if (weapon.IsBowEquip && weapon.IsArrowEquip) // 캐릭터가 활을 장비하고 있고 화살을 장전하고 있는 경우
        {
            ///// CanZoom이 true인 상태
            ///// 마우스 왼쪽 버튼을 누르면 줌인이 되고, ZoomIn이 true가 되어야한다. (활시위를 당긴다.)
            ///// 마우스 왼쪽 버튼을 누르다가 떼면 줌아웃이 되고, ZoomIn이 false가 되어야한다. (화살을 쏜다.)

            // 플레이어가 마우스 왼쪽 버튼을 누르고 있는 경우
            if (Input.GetMouseButtonDown(0))
            {
                // vcam.transform.rotation = Quaternion.LookRotation(player.transform.forward, Vector3.up); // 활시위를 당길 때, 캐릭터와 카메라가 같은 방향 바라보기
                StopAllCoroutines();
                StartCoroutine(Timer(true));
                //weapon.IsZoomIn = true;
                //weapon.LoadArrowAfter();
                Debug.Log("Camera Zoom-In");
            }

            // 플레이어가 마우스 왼쪽 버튼에서 뗀 경우
            if (Input.GetMouseButtonUp(0))
            {
                StopAllCoroutines();
                StartCoroutine(Timer(false));
                //weapon.IsZoomIn = false;
                //weapon.LoadArrowAfter();
                Debug.Log("Camera Zoom-Out");
            }
        }
        else
        {
            follow.ShoulderOffset = zoomOut;
            weapon.IsZoomIn = false;
        }
    }

    ///// [오류1] 빠르게 누르고 뗐더니 화살이 없다고 표시되어 화살 공격이 아닌 활 공격 애니메이션이 재생된다.
    ///// [원인1] 활을 전부 당겼을 때, IsZoom은 true & IsZoomIn도 true
    ///// [원인2] 활을 조금만 당겼다가 놓으면 IsZoom은 false이므로, IsZoomIn와 ZoomInHash가 false가 되어 애니메이션이 넘어가지 못한다.
    ///// [해결방안] IsZoom이 false이 경우, IsArrowEquip가 true이면 다시 IsZoomIn을 true로 변경해주고 아닌 경우에는 false가 되도록 한다.

    ///// [오류2] IsArrowEquip는 true인데 HaveArrowHash는 false이다. (가지고 있는 화살이 없다.)
    ///// [원인1] 플레이어가 마우스 왼쪽 버튼에서 뗀 경우인 StartCoroutine(Timer(false));가 계속 실행되어 Arrow 장착이 제대로 실행되지 않는다.
    ///// [원인2] 그 후 IsArrowEquip는 false가 떴는데 ChangeCameraZoom()가 계속 실행된다..?
    ///// 장전 R 버튼을 누른다 >> IsArrowEquip는 true >> Update 실행 >> 마우스에 입력이 없으니 Timer(false) 실행 >> 하지만 줌아웃 디버그는 안찍힘..?

    IEnumerator Timer(bool IsZoom)
    {
        float timeElapsed = 0.0f;
        while (timeElapsed < 1.0f)
        {
            timeElapsed += speed * Time.deltaTime;

            if (IsZoom)
            {
                follow.ShoulderOffset = Vector3.Lerp(zoomOut, zoomIn, timeElapsed);
                //if (timeElapsed > 0.1f) // 활이 조금이라도 당겨지면 ZoomIn이 true가 된다.
                //{
                //    weapon.IsZoomIn = true;
                //}
                weapon.IsZoomIn = true;
                weapon.LoadArrowAfter();
            }
            else
            {
                follow.ShoulderOffset = Vector3.Lerp(zoomIn, zoomOut, timeElapsed);
                weapon.IsZoomIn = false;
                weapon.LoadArrowAfter();
            }

            yield return null;
        }
    }
}
