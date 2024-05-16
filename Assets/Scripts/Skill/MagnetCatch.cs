using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    /// 타겟이 문일때 초당 돌아가는 속도
    /// </summary>
    public float doorRotateAngle = 90.0f;
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
    /// 타겟이 움직일 목적지 (거리 계산 간편화 용)
    /// </summary>
    Transform targetDestination;
    /// <summary>
    /// 자석에 붙은 오브젝트 (= 타겟)
    /// </summary>
    Transform target;
    /// <summary>
    /// 마그넷캐치 현재 모양
    /// </summary>
    Transform[] shapeTransform;
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
    /// <summary>
    /// 스페셜키([,])를 통한 앞뒤 방향 계산용 변수
    /// </summary>
    int directionAxisZ = 0;
    /// <summary>
    /// 현재 프레임에 이동할 Y(로컬좌표) 거리
    /// </summary>
    float targetMoveY = 0.0f;
    /// <summary>
    /// 현재 프레임에 이동할 Z(로컬좌표) 거리
    /// </summary>
    float targetMoveZ = 0.0f;
    /// <summary>
    /// 붙은 타겟이 문인지 확인하는 변수 (true: 문)
    /// </summary>
    bool isTarget_Door = false;

    /// <summary>
    /// 이전 프레임의 문과 유저의 거리(제곱)
    /// </summary>
    float preSqrDistance = 0f;

    /// <summary>
    /// 이전 프레임의 유저 위치 (문열기에 사용)
    /// </summary>
    Vector3 preUserPosition = Vector3.zero;

    Transform interpolation1;
    Transform interpolation2;
    MagnetCatch_Line line;

    const float Interpolation1Value = 0.2f;
    const float Interpolation2Value = 0.7f;

    /// <summary>
    /// 앞뒤 이동 속도 조절용
    /// </summary>
    public float horizontalMoveSpeedRatio = 0.3f;


    enum MagnetShape
    {
        SheikahStone,
        Magnet
    }

    MagnetShape shape = MagnetShape.Magnet;
    MagnetShape Shape
    {
        get => shape;
        set
        {
            if (shape != value)
            {
                shapeTransform[(int)shape].gameObject.SetActive(false);
                shape = value;
                shapeTransform[(int)shape].gameObject.SetActive(true);

            }

        }
    }

    protected override void Awake()
    {
        base.Awake();
        targetGroup = GetComponentInChildren<Cinemachine.CinemachineTargetGroup>(); // 사용자와 타겟을 묶은 타겟 그룹 (카메라용)
        line = GetComponentInChildren<MagnetCatch_Line>();

        skillName = SkillName.MagnetCatch;                                          // 스킬 이름: 마그넷캐치로 설정
        shapeTransform = new Transform[2];
        shapeTransform[0] = transform.GetChild(0);
        shapeTransform[1] = transform.GetChild(1);
        targetDestination = transform.GetChild(2);                                  // 타겟이 이동할 목적지
        interpolation1 = transform.GetChild(3);
        interpolation2 = transform.GetChild(4);

        Shape = MagnetShape.SheikahStone;

        line.gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (magnetVcam == null)         // 활성화 될 때 마그넷캠이 없으면 게임매니저에서 찾음
        {
            magnetVcam = GameManager.Instance.Cam.MagnetCam;
        }

        Shape = MagnetShape.SheikahStone;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // 비활성화시 초기화
        // 타겟 제거 (OffSkill에서 처리하지만 확실하게)
        reactionTarget = null;
        target = null;

        isMagnetActivate = false;   // 자석 활성화 상태 false
    }

    private void FixedUpdate()
    {
        // 작동 중이면 실행
        if (isActivate)
        {
            LookForwardUser(Camera.main.transform.forward);                 // 사용자는 카메라 정면을 바라봄

            // 이전 프레임과 현재 프레임의 Y축 기준 카메라 회전각 차이 계산
            float angleY = Camera.main.transform.eulerAngles.y;
            float resultY = angleY - preAngleY;
            preAngleY = angleY;

            if (isTarget_Door)
            {
                float angle = Vector3.Angle(user.forward, target.forward);
                float dir = Mathf.Sign(target.localScale.x);    // 회전값은 스케일(문의 방향)에 따라 영향을 받음

                if(angle > 90)
                {
                    dir = -dir;     // 문과 유저의 방향이 반대면 회전값 반대로 (원래 가까워지면 커짐 -> 가까워지면 작아짐)
                }
                float sqrDistance = (user.position - target.position).sqrMagnitude;
                float distanceDiff = sqrDistance - preSqrDistance;          // 가까워지면 양수, 멀어지면 음수
                if(distanceDiff > 0f)
                {
                    distanceDiff = 1;
                }
                else if(distanceDiff < 0f)
                {
                    distanceDiff = -1;
                }
                else
                {
                    distanceDiff = 0;
                }

                float rotate = dir * distanceDiff * Time.fixedDeltaTime * doorRotateAngle;

                float specialRotate = -directionAxisZ * dir * Time.fixedDeltaTime * doorRotateAngle;

                reactionTarget.AttachRotate((rotate + specialRotate - resultY) * Vector3.up);              // 문 회전

                preSqrDistance = sqrDistance;
                preUserPosition = user.position;
            }
            else
            {

                InputMouseMove();                                               // 마우스 움직임 처리

                DestinationMove();                                              // 목적지 이동

                reactionTarget.AttachMagnetMove(targetDestination.position);    // 타겟 이동
                reactionTarget.AttachRotate((resultY * Vector3.up));              // 타겟 회전
            }
           
        }
    }

    /// <summary>
    /// 마그넷캐치 스킬 소환했을 때 행동
    /// </summary>
    protected override void OnSKillAction()
    {
        base.OnSKillAction();

        targetGroup.m_Targets[1].target = user;     // 카메라가 바라볼 타겟 그룹에 사용자 등록

        StartCoroutine(TargetCheck());              // 타겟 체크
    }
    /// <summary>
    /// 마그넷캐치 스킬 발동 했을 때 행동
    /// </summary>
    protected override void UseSkillAction()
    {
        onMotionChange?.Invoke(true);
        Shape = MagnetShape.Magnet;
        // 붙을 타겟이 적절하면
        if (isMagnetActivate)
        {
            StopAllCoroutines();                                // 타겟 탐지용 코루틴 정지
            // TODO: 끝까지 날아가게

            base.UseSkillAction();

            OnMagnetAction();                                   // 카메라가 실행될 때 처리할 행동들 처리

            LookForwardUser(Camera.main.transform.forward);     // 플레이어와 카메라 정면 맞추기

            // 타겟 목적지 설정
            targetDestination.SetParent(user);                  // 목적지 부모는 사용자
            targetDestination.position = target.position;       // 목적지의 위치 옮기기
            interpolation1.SetParent(user);
            interpolation1.localPosition = targetDestination.localPosition * Interpolation1Value;
            interpolation2.SetParent(user);
            interpolation2.localPosition = targetDestination.localPosition * Interpolation2Value;

            // 타겟 관련 설정
            reactionTarget.AttachMagnet(targetMoveSpeed);       // 타겟 붙이기

            targetGroup.m_Targets[0].target = target;           // 타겟 그룹에 타겟 추가

            // 초기값 설정
            preMousePos = Mouse.current.position.value;         // y축 이동을 위한 마우스 초기값 설정 (현재 위치)
            preAngleY = Camera.main.transform.eulerAngles.y;    // 타겟과 사용자의 y축 회전(좌우)을 맞추기 위한 카메라각도 설정
            preSqrDistance = (user.position - target.position).sqrMagnitude;
            preUserPosition = user.position;    

            targetMoveY = 0f;
            targetMoveZ = 0f;
        }
        else
        {
            // TODO: 날아가다 끊기게
            onMotionChange?.Invoke(false);
            Shape = MagnetShape.SheikahStone;
        }
    }
    /// <summary>
    /// 마그넷캐치 스킬 종료 행동
    /// </summary>
    protected override void OffSKillAction()
    {
        OffMagnetAction();

        if (reactionTarget != null)             // 붙어있는 타겟이 있으면
        {
            reactionTarget.DettachMagnet();     // 타겟 자석에서 떼기
        }

        targetDestination.SetParent(transform); // 목적지의 부모 원래대로(이 트랜스폼) 돌리기
        interpolation1.SetParent(transform); // 목적지의 부모 원래대로(이 트랜스폼) 돌리기
        interpolation2.SetParent(transform); // 목적지의 부모 원래대로(이 트랜스폼) 돌리기

        base.OffSKillAction();
    }
    /// <summary>
    /// 마그넷이 발동될 때 메서드 (물체가 붙었을 때)
    /// </summary>
    void OnMagnetAction()
    {
        magnetVcam.OnSkillCamera();                               // 마그넷카메라 실행
        magnetVcam.SetLookAtTransform(targetGroup.transform);     // 물체와 사용자의 그룹 바라보기

        line.gameObject.SetActive(true);
        line.transform.SetParent(null);
        line.transform.localPosition = Vector3.zero;
        line.transform.localRotation = Quaternion.identity;
        line.Initialize(transform, interpolation1, interpolation2, target);

        // 현재 사용자가 플레이어라면
        Player player = user.GetComponent<Player>();
        if (player != null)
            player.SetMagnetCamera(false, CameraRotateVertical);  // 카메라루트의 x축 회전(위아래) 막기
    }
    /// <summary>
    /// 마그넷이 발동해제될 때 메서드 (물체가 떨어졌을 때)
    /// </summary>
    void OffMagnetAction()
    {
        onMotionChange?.Invoke(false);

        line.transform.SetParent(transform);
        line.transform.localRotation = Quaternion.identity;
        line.transform.localPosition = Vector3.zero;
        line.gameObject.SetActive(false);
        magnetVcam.OffSkillCamera();                            // 마그넷카메라 끄기

        isMagnetActivate = false;
        isTarget_Door = false;

        // 현재 사용자가 플레이어라면
        Player player = user.GetComponent<Player>();
        if (player != null)
            player.SetMagnetCamera(true, CameraRotateVertical);  // 카메라루트의 x축 회전(위아래) 동작
    }

    /// <summary>
    /// 사용자의 정면을 설정하는 메서드 (y축 제외)
    /// </summary>
    /// <param name="forward">바라볼 방향</param>
    public void LookForwardUser(Vector3 forward)
    {
        forward.y = 0;              // y축(위아래)는 신경 안씀
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

        targetMoveY = SetDistance(mouseDir.y, targetDestination.localPosition.y);   // 거리 설정
    }


    /// <summary>
    /// 앞뒤 움직임을 처리하는 메서드
    /// </summary>
    /// <param name="input">[ , ] 두가지 키만 받음</param>
    public override void InputSpecialKey(PlayerSkills.SpecialKey input)
    {
        switch (input)
        {
            case PlayerSkills.SpecialKey.NumPad5_Down:        // [ : 가까워짐
                directionAxisZ = -1;
                break;
            case PlayerSkills.SpecialKey.NumPad8_Up:       // [ : 멀어짐
                directionAxisZ = 1;
                break;
            case PlayerSkills.SpecialKey.None:                      // 입력 없으면 그대로
            default:
                directionAxisZ = 0;
                break;
        }

        targetMoveZ = SetDistance(directionAxisZ * horizontalMoveSpeedRatio, targetDestination.localPosition.z);   // 이동할 거리가 최대거리 안인지 계산

        float finalDistance = targetDestination.localPosition.z + targetMoveZ;          
        if (finalDistance < minDistanceFromUser && directionAxisZ < 0)                  // 가까워 질 경우 최소거리보다 작아지면 이동할 거리 제거
        {
            targetMoveZ = 0;
        }
    }

    /// <summary>
    /// y, z축의 이동거리를 처리하는 메서드
    /// </summary>
    /// <param name="moveDirection">이동할 방향 (+, -)</param>
    /// <param name="currentPositionAxis">타겟의 위치(현재 축: y, z)</param>
    /// <returns>계산된 이동할 양</returns>
    float SetDistance(float moveDirection, float currentPositionAxis)
    {
        float moveValue = moveDirection * Time.deltaTime * targetMoveSpeed;     // 목적지가 한프레임에 이동할 양을 계산하고
        float finalDistance = currentPositionAxis + moveValue;                  // 이동할 거리와 현재 타겟 위치를 더해 최종 위치를 구하여

        if (finalDistance > maxDistanceFromUser && moveDirection > 0)           // 최종 위치가 최대 떨어질 수 있는 거리보다 크면 이동할 거리 제거
        {
            moveValue = 0;
        }

        return moveValue;
    }


    /// <summary>
    /// 자석에 붙는 오브젝트 검사
    /// </summary>
    /// <returns>프레임당 코루틴</returns>
    IEnumerator TargetCheck()
    {
        while (true)
        {
            Ray ray = Camera.main.ViewportPointToRay(Center);           // 카메라의 중앙에서 레이 생성
            Physics.Raycast(ray, out RaycastHit hit, skillDistance);    // 생성한 레이로 스킬 거리만큼 레이캐스트 검사

            target = hit.transform;
            if (target != null)                                                             // 부딪친 물체가 있으면
            {
                reactionTarget = target.GetComponent<ReactionObject>();                     // 반응형 오브젝트인지 확인
                if(reactionTarget != null)
                {
                    isMagnetActivate = reactionTarget.IsMagnetic; // 자석에 반응하는 오브젝트면 자석 활성화
                    isTarget_Door = reactionTarget is ReactionDoor;
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// 목적지를 이동시키는 메서드
    /// </summary>
    void DestinationMove()
    {
        Vector3 destinationLocalPos = targetDestination.localPosition;  // 로컬 좌표 받아와서
        destinationLocalPos.x = 0f;                                     // x축은 0 고정 (사용자와 같은 위치 = 사용자는 카메라 정면을 바라봄 = 카메라의 정면에 고정)
        destinationLocalPos.y += targetMoveY;                           // y축 이동거리만큼 더하기
        destinationLocalPos.z += targetMoveZ;                           // z축 이동거리만큼 더하기
        targetDestination.localPosition = destinationLocalPos;          // 목적지 위치 설정
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
