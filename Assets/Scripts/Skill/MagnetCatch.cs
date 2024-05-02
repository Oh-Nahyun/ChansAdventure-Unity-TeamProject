using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
using System;
using UnityEngine.InputSystem;
using System.Security.Cryptography;
using static UnityEditor.PlayerSettings;



#if UNITY_EDITOR
using UnityEditor;
#endif

// 카메라 막혔을 때 등뒤로 붙이는 것만 하면 끝
public class MagnetCatch : Skill
{
    [Header("마그넷캐치 데이터")]
    /// <summary>
    /// 타겟의 속도
    /// </summary>
    public float targetMoveSpeed = 5.0f;
    /// <summary>
    /// 타겟이 움직일 수 있는 최대 거리
    /// (사용자와 떨어질 수 있는 최대 거리)
    /// </summary>
    public float maxDistanceFromUser = 20.0f;
    /// <summary>
    /// 타겟이 움직일 수 있는 최소 거리
    /// (사용자와 붙을 수 있는 최소 거리)
    /// </summary>
    public float minDistanceFromUser = 1.0f;
    /// <summary>
    /// 타겟이 붙어 있는지 (true: 붙어있음)
    /// </summary>
    bool isMagnetActivate = false;
    /// <summary>
    /// 타겟이 붙어 있는지 외부 확인용 프로퍼티 (true: 붙어있음)
    /// </summary>
    public bool IsActivate => isMagnetActivate;
    /// <summary>
    /// 이전 마우스 위치 (위아래 움직임 파악용)
    /// </summary>
    Vector2 preMousePos = Vector2.zero;
    /// <summary>
    /// 이전 프레임의 카메라 회전각 (타겟오브젝트 회전각 계산용)
    /// </summary>
    float preAngleY;
    /// <summary>
    /// 자석에 붙은 오브젝트 (= 타겟)
    /// </summary>
    Transform target;
    /// <summary>
    /// 타겟에 있는 스크립트 (반응형 오브젝트)
    /// </summary>
    ReactionObject reactionTarget;
    /// <summary>
    /// 탐지된 타겟의 정중앙
    /// </summary>
    Vector3 hitPoint;
    /// <summary>
    /// 마그넷 카메라 (타겟 오브젝트와 플레이어가 묶인 타겟 그룹을 바라봄)
    /// </summary>
    MagnetVCam magnetVcam;
    /// <summary>
    /// 마그넷 카메라가 바라볼 타겟 그룹
    /// </summary>
    Cinemachine.CinemachineTargetGroup targetGroup;
    /// <summary>
    /// 마그넷 카메라의 각도
    /// </summary>
    public const float CameraRotateVertical = 15.0f;

    Vector3 targetMoveY = Vector3.zero;
    Vector3 targetMoveZ = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
        targetGroup = GetComponentInChildren<Cinemachine.CinemachineTargetGroup>();
        skillName = SkillName.MagnetCatch;                  // 스킬 이름: 마그넷캐치로 설정
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (magnetVcam == null)         // 마그넷캠이 없으면 게임매니저에서 찾음
        {
            magnetVcam = GameManager.Instance.Cam.MagnetCam;
        }

    }

    protected override void OnDisable()
    {
        base.OnDisable();

        StopAllCoroutines();

        // 비활성화시 초기화
        reactionTarget = null;
        target = null;

        isMagnetActivate = false;
    }

    private void FixedUpdate()
    {
        if (isActivate)
        {
            // 이전과 현재의 카메라 회전각 차이 계산
            float angleY = Camera.main.transform.eulerAngles.y;
            float resultY = angleY - preAngleY;
            preAngleY = angleY;

            LookForwardUser(Camera.main.transform.forward);     // 카메라가 바라보는 곳이 사용자의 정면

            // y축 움직이면 z축 안움직이는 이유 찾기
            InputMouseMove();

            Vector3 targetLocalPos = target.localPosition;
            targetLocalPos.x = 0;
            reactionTarget.AttachMagnetMove(targetLocalPos + targetMoveY + targetMoveZ, user);
            //reactionTarget.AttachRotate(resultY * Vector3.up);              // 타겟 회전하기


            targetMoveY = Vector3.zero;
            targetMoveZ = Vector3.zero;
        }
    }
    /// <summary>
    /// 마그넷캐치 스킬 소환했을 때 행동
    /// </summary>
    protected override void OnSKillAction()
    {
        base.OnSKillAction();

        targetGroup.m_Targets[1].target = user;     // 카메라가 바라볼 타겟그룹에 사용자 등록

        StartCoroutine(TargetCheck());              // 자석에 붙는 오브젝트 체크
    }
    /// <summary>
    /// 마그넷캐치 스킬 발동 했을 때 행동
    /// </summary>
    protected override void UseSkillAction()
    {
        if (isMagnetActivate)
        {
            StopAllCoroutines();    // 자석에 붙는 오브젝트 탐지용 코루틴 정지

            base.UseSkillAction();

            OnMagnetAction();

            LookForwardUser(Camera.main.transform.forward);

            // 타겟 관련 설정
            reactionTarget.AttachMagnet(user, targetMoveSpeed);    // 반응형 오브젝트 붙이기

            targetGroup.m_Targets[0].target = target;   // 마그넷 카메라가 바라볼 그룹

            preMousePos = Mouse.current.position.value;         // y축 이동을 위한 마우스 설정
            preAngleY = Camera.main.transform.eulerAngles.y;    // 타겟과 사용자의 y축 회전(좌우)을 맞추기 위한 카메라각도 설정

            targetMoveY = Vector3.zero;
            targetMoveZ = Vector3.zero;
        }
    }
    /// <summary>
    /// 마그넷캐치 스킬 종료 행동
    /// </summary>
    protected override void OffSKillAction()
    {
        OffMagnetAction();

        if (reactionTarget != null)     // 붙어있는 타겟이 있으면 자석에서 떼기
        {
            reactionTarget.DettachMagnet();
            target.SetParent(originParent);               // 타겟의 부모 원래대로 돌리기
        }

        base.OffSKillAction();
    }
    /// <summary>
    /// 마그넷이 발동될 때 메서드 (물체가 붙었을 때)
    /// </summary>
    void OnMagnetAction()
    {
        magnetVcam.OnSkillCamera();                               // 마그넷카메라 실행
        magnetVcam.SetLookAtTransform(targetGroup.transform);     // 물체와 사용자의 그룹 바라보기

        // 현재 사용자가 플레이어라면
        Player player = user.GetComponent<Player>();
        if (player != null)
            player.SetMagnetCamera(false, CameraRotateVertical);  // 카메라루트의 x축 회전 막기
    }
    /// <summary>
    /// 마그넷이 발동해제될 때 메서드 (물체가 떨어졌을 때)
    /// </summary>
    void OffMagnetAction()
    {
        magnetVcam.OffSkillCamera();    // 마그넷카메라 종료

        // 현재 사용자가 플레이어라면
        Player player = user.GetComponent<Player>();
        if (player != null)
            player.SetMagnetCamera(true, CameraRotateVertical);     // 카메라루트의 x축 회전 동작
    }

    /// <summary>
    /// 사용자의 정면을 설정하는 메서드 (y축 제외)
    /// </summary>
    /// <param name="forward">바라볼 방향</param>
    public void LookForwardUser(Vector3 forward)
    {
        forward.y = 0;
        user.forward = forward;
    }

    /// <summary>
    /// 목적지의 Y축(위아래) 위치를 설정하는(움직이는) 메서드
    /// </summary>
    void InputMouseMove()
    {
        // 이전과 현재의 마우스 위아래 포지션 차이 계산
        Vector2 curMousePos = Mouse.current.position.value;
        Vector2 mouseDir = (curMousePos - preMousePos).normalized;
        preMousePos = curMousePos;


        targetMoveY = SetDistance(target.localPosition.y, mouseDir.y * Time.fixedDeltaTime, Vector3.up);

        // 마우스 입력에 따라 목적지 위치에서 이동한 위치 계산
        /*float movePosY = mouseDir.y * Time.fixedDeltaTime * targetMoveSpeed;

        float targetDistance = MathF.Abs(user.position.y - movePosY);      // 사용자와 떨어진 거리의 절대값 계산

        movePosY = (targetDistance < maxDistanceFromUser) ? movePosY : 0;   // 사용자와 최대 거리보다 멀면 움직이지 않기

        reactionTarget.AttachMagnetMove(target.position + movePosY * Vector3.up);*/
    }


    int directionAxisZ = 0;

        /// <summary>
        /// 앞뒤 움직임을 처리하는 메서드
        /// </summary>
        /// <param name="input">[ , ] 두가지 키만 받음</param>
    public void InputSpecialKey(PlayerSkills.SpecialKey input)
    {
        switch (input)
        {
            case PlayerSkills.SpecialKey.SquareBracket_Open:
                directionAxisZ = -1;
                break;
            case PlayerSkills.SpecialKey.SquareBracket_Close:
                directionAxisZ = 1;
                break;
            case PlayerSkills.SpecialKey.None:
            default:
                directionAxisZ = 0;
                break;
        }
        targetMoveZ = SetDistance(target.localPosition.z, directionAxisZ * Time.deltaTime, Vector3.forward);

    }

    Vector3 SetDistance(float localPosAxis, float moveValue, Vector3 direction)
    {
        // 입력([,]키 or 사용자의 움직임)에 따라 목적지 위치에서 이동한 위치 계산
        localPosAxis += moveValue;
        moveValue *= targetMoveSpeed;

        if (localPosAxis > maxDistanceFromUser && moveValue > 0)                               // 떨어진 거리가 최대치보다 크면 최대치로 변경
        {
            moveValue = 0;
        }
        else if(localPosAxis < minDistanceFromUser && moveValue < 0)
        {
            moveValue = 0;
        }

        return moveValue * direction;

        //reactionTarget.AttachMagnetMove(localPos);
    }


    /// <summary>
    /// 자석에 붙는 오브젝트 검사
    /// </summary>
    /// <returns></returns>
    IEnumerator TargetCheck()
    {
        while (true)
        {
            Ray ray = Camera.main.ViewportPointToRay(Center);           // 카메라의 중앙에서 레이 생성
            Physics.Raycast(ray, out RaycastHit hit, skillDistance);

            target = hit.transform;
            // 부딪친 물체가 있으면
            if (target != null)
            {
                reactionTarget = target.GetComponent<ReactionObject>(); // 반응형 오브젝트인지 확인

                isMagnetActivate = (reactionTarget != null) && (reactionTarget.IsMagnetic);   // 반응형 오브젝트이고 자석에 반응하는 오브젝트라면 자석에 붙음
                if (isMagnetActivate)
                {
                    hitPoint = hit.collider.bounds.center;  // 해당 오브젝트의 중앙에서 시작
                    //hitPoint = hit.point;
                }
            }

            yield return null;
        }
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ViewportPointToRay(Center);
        Physics.Raycast(ray, out RaycastHit hit, skillDistance);
        // 레이캐스트 보여주는 기즈모
        if (hit.transform != null)
        {
            Gizmos.color = Color.red;
            Vector3 vec = Camera.main.ViewportToWorldPoint(Center);
            Gizmos.DrawLine(vec, hit.point);
        }

    }

    public void TestStartSkill()
    {
        OnSkill();
    }
    public void TestUseSkill()
    {
        UseSkill();
    }
    public void TestFinishSkill()
    {
        OffSkill();
    }
#endif
}
