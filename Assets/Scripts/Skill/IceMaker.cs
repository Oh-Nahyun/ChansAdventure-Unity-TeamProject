using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class IceMaker : Skill
{
    [Header("아이스메이커 데이터")]

    /// <summary>
    /// 멀어졌을 때 파괴되는 범위
    /// </summary>
    public float destroyMaxDistance = 50.0f;

    /// <summary>
    /// 생성할 얼음 오브젝트의 프리펩
    /// </summary>
    public IceMaker_Ice icePrefab;

    /// <summary>
    /// 생성할 얼음의 크기
    /// </summary>
    Vector3 iceSize = new Vector3(1.7f, 2f, 1.7f);

    /// <summary>
    /// 설치 불가능한 공간일 때 프리뷰가 깜빡이는 속도
    /// </summary>
    public float previewBlinkInterval = 0.2f;

    /// <summary>
    /// 사용중인 얼음이 들어갈 큐 (최대 3개)
    /// </summary>
    List<IceMaker_Ice> usingIceList;       // 앞에서 부터 삭제하기 위해 큐로 사용했었지만 중간값을 삭제할 일이 생겨서 리스트로 변경

    /// <summary>
    /// 움직이지 않는 물체와 겹치는 부분이 들어갈 리스트 (얼음 생성 가능한지 여부를 확인하기 위해, 태그로 검사)
    /// </summary>
    List<Transform> collisionGround;

    /// <summary>
    /// 얼음을 생성할 수 있는 위치인지 아닌지 (true: 생성 가능)
    /// </summary>
    bool isValid = false;

    /// <summary>
    /// 중심부가 얼음인지 파악하는 프로퍼티 (true면 중심이 얼음)
    /// </summary>
    bool IsIce => hitIce != null;

    /// <summary>
    /// 중심 레이캐스트
    /// </summary>
    IceMaker_Ice hitIce = null;

    /// <summary>
    /// 얼음을 생성할 위치 (레이캐스트가 닿은 곳)
    /// </summary>
    Vector3 createPosition = Vector3.zero;

    /// <summary>
    /// 얼음이 생성 가능한지 파악하기 위한 콜라이더의 피벗
    /// </summary>
    Transform validChecker;

    /// <summary>
    /// 얼음의 최대 생성 개수
    /// </summary>
    const int IceCount = 3;

    /// <summary>
    /// 표면 전체가 물인지 파악할 박스오버랩의 높이
    /// </summary>
    const float waterCheckHeight = 0.01f;

    /// <summary>
    /// 생성 위치를 미리 보여줄 프리뷰
    /// </summary>
    IceMaker_Preview preview;

    /// <summary>
    /// 레이캐스트에 플레이어를 제외할 레이어마스크
    /// </summary>
    int layerMask_PlayerIgnore;

    /// <summary>
    /// 스페셜키([)를 눌러 유저 아래쪽에 얼음 생성하는지 여부(true: 플레이어의 아래에 설치)
    /// </summary>
    bool isSpecialAction = false;

    /// <summary>
    /// 얼음의 생성위치에 플레이어가 올라가 있는지 여부(true: 플레이어의 발밑에 설치)
    /// </summary>
    bool isUnderUser = false;

    PlayerSkills.SpecialKey inputSpecialKey;

    Action<bool, Vector3> crosshairPositionChange;

    const float YpositionForCrosshair = 0.2f;

    protected override void Awake()
    {
        base.Awake();

        // 얼음이 들어갈 큐는 얼음의 개수(자식) 만큼만 
        usingIceList = new List<IceMaker_Ice>(IceCount);

        collisionGround = new List<Transform>();

        iceSize = icePrefab.Size;       // 얼음 크기 설정

        Transform child = transform.GetChild(1);

        preview = child.GetComponent<IceMaker_Preview>();
        preview.Initialize(previewBlinkInterval, iceSize);  // 프리뷰 초기화 (깜빡임 속도, 얼음크기)

        validChecker = transform.GetChild(2);
        validChecker.GetComponent<BoxCollider>().size = iceSize;    // 얼음 생성가능 체크용 컬라이더 크기를 얼음 크기로 맞추기

        // 레이어마스크 설정
        layerMask_PlayerIgnore = (1 << LayerMask.NameToLayer("Player"));
        layerMask_PlayerIgnore = ~layerMask_PlayerIgnore;
    }

    void Start()
    {
        if (crosshair != null)
        {
            crosshairPositionChange += (isSpecialAction, worldPosition) => crosshair.SetPosition(isSpecialAction, worldPosition);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        collisionGround.Clear();
        StopAllCoroutines();
        crosshairPositionChange?.Invoke(true, Vector3.zero);
        base.OnDisable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsObstacle(other.transform))
        {
            // 겹친 부분이 못움직이는 오브젝트면 추가
            collisionGround.Add(other.transform);
        }
        if (other.transform == user)
        {
            isUnderUser = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsObstacle(other.transform))
        {
            // 추가 됐던 부분이 트리거 밖으로 나가면 제거
            collisionGround.Remove(other.transform);
        }
        if(other.transform == user)
        {
            isUnderUser = false;
        }
    }

    protected override void OnSKillAction()
    {
        base.OnSKillAction();
        StartCoroutine(FindValidPosition());
    }

    /// <summary>
    /// 아이스메이커 발동 했을 때 동작
    /// </summary>
    protected override void UseSkillAction()
    {
        // base 상속 안받음 : cam 변화 x, isActive 변화 x
        if(isValid)
        {
            base.UseSkillAction();
            onMotionChange?.Invoke(true);
            if(usingIceList.Count >= IceCount)     // IceCount만큼 설치되어 있다면 파괴 (이상으로 해놓은 이유는 안전장치)
            {
                DestroyIce(usingIceList[0]);
            }
            if (isUnderUser)
            {
                Vector3 pos = user.position;
                if(pos.y < createPosition.y)
                { 
                    pos.y = createPosition.y + 0.1f;
                    user.position = pos;
                }
            }
            GenerateIce();

            OffSkill();
        }
        else if (IsIce)
        {
            DestroyIce(hitIce);
        }

    }

    /// <summary>
    /// 얼음을 생성하는 메서드
    /// </summary>
    void GenerateIce()
    {
        IceMaker_Ice ice = Factory.Instance.GetIceMaker_Ice(createPosition);
        Vector3 euler = user.rotation.eulerAngles;
        ice.Initialize(user, destroyMaxDistance, Vector3.up * euler.y);
        ice.onDestroy += DestroyIce;
        usingIceList.Add(ice);
    }

    /// <summary>
    /// 얼음을 파괴하는 메서드
    /// </summary>
    /// <param name="destroyCount">파괴할 얼음의 수 (범위: 1 ~ IceCount 자동조정)</param>
    void DestroyIce(IceMaker_Ice ice)
    {
        if (ice != null)          // 나중에 주석 지우기 (오류 확인 이후)
        {
            usingIceList.Remove(ice);
            collisionGround.Remove(ice.transform);
            ice.SetDestroy();
        }
    }

    /// <summary>
    /// 얼음의 생성 가능 한지, 생성된 얼음을 파괴할 지를 확인하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FindValidPosition()
    {
        while (true)
        {
            isValid = false;
            hitIce = null;

            Ray ray = Camera.main.ViewportPointToRay(Center);   // 카메라 중심에서 레이   

            if (isSpecialAction)
            {
                float startRayPoint = 0.1f;
                Vector3 pos = user.position;
                pos.y += startRayPoint;
                ray = new Ray(pos, -user.up);

                Vector3 userPos = user.position;
                userPos.y += YpositionForCrosshair;
                crosshairPositionChange?.Invoke(!isSpecialAction, userPos);
            }

            if (Physics.Raycast(ray, out RaycastHit hitInfo, skillDistance, layerMask_PlayerIgnore))
            {

                // 레이캐스트에 닿은 위치의 Layer가 Water레이어 인지 확인
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
                {
                    validChecker.position = hitInfo.point;
                    preview.transform.position = hitInfo.point;     // 프리뷰 위치 옮기기

                    if (WaterCheck(hitInfo.point))
                    {
                        preview.ValidPosition(true);
                        preview.SetVisible();
                        createPosition = hitInfo.point;             // 생성 위치는 레이캐스트가 닿은 곳
                        isValid = true;
                    }
                    else
                    {
                        preview.ValidPosition(false);
                    }
                }
                else
                {
                    //preview.gameObject.SetActive(false);    // 프리뷰 비활성화
                    preview.SetInvisible();
                    if (hitInfo.transform.CompareTag("Skill"))
                    {
                        hitIce = hitInfo.transform.GetComponentInParent<IceMaker_Ice>();
                    }
                }
            }
            else        // 닿는 부분이 없을 때
            {
                //preview.gameObject.SetActive(false);    // 프리뷰 비활성화
                preview.SetInvisible();
            }

            yield return null;
        }
    }

    /// <summary>
    /// 생성가능한 위치인지 확인하는 메서드
    /// </summary>
    /// <param name="hitPoint">생성할 위치</param>
    /// <returns></returns>
    bool WaterCheck(Vector3 hitPoint)
    {
        // 수면에 걸리는 물체가 없는지 확인
        Vector3 boxSize = iceSize;
        boxSize.y = waterCheckHeight;
        Collider[] colliders = Physics.OverlapBox(hitPoint, boxSize * 0.5f);
        
        foreach (var collider in colliders)
        {
            if (IsObstacle(collider.transform))
            {
                return false;
            }
        }
        //Debug.Log("콜라이더 통과");

        // 생성할 공간 안에 움직이지 못하는 충돌체가 있는지 확인
        if (collisionGround.Count > 0)
        {
            //foreach(var collider in collisionGround)
            //{
            //    Debug.Log(collider.transform.name);
            //}
            return false;
        }
        //Debug.Log("콜리전 통과");

        // 평면 전체가 물로 가득 차있는지 확인
        // 모서리 4군데만 확인함
        float rayY = 1.0f;
        float x = boxSize.x * 0.5f;
        float z = boxSize.z * 0.5f;

        Vector3 origin = hitPoint;
        origin.y += rayY;

        origin.x = hitPoint.x + x;
        origin.z = hitPoint.z + z;
        if (!BoundaryCheck(origin)) return false;
        origin.x = hitPoint.x - x;
        origin.z = hitPoint.z + z;
        if (!BoundaryCheck(origin)) return false;
        origin.x = hitPoint.x + x;
        origin.z = hitPoint.z - z;
        if (!BoundaryCheck(origin)) return false;
        origin.x = hitPoint.x - x;
        origin.z = hitPoint.z - z;
        if (!BoundaryCheck(origin)) return false;
        //Debug.Log("전부 통과");
        return true;
    }

    /// <summary>
    /// 경계면이 물로 되어있는지 확인하는 메서드
    /// </summary>
    /// <param name="origin">레이를 쏠 위치</param>
    /// <returns></returns>
    bool BoundaryCheck(Vector3 origin)
    {
        Ray ray = new Ray(origin, Vector3.down);        // 위에서 아래로 레이를 쏴서
        Physics.Raycast(ray, out RaycastHit hitInfo);
        if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Water"))   // 해당 위치가 물이면 맞다고 판단
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 생성할 위치에 충돌했을 때 생성하지 못하는 오브젝트들
    /// </summary>
    /// <param name="target">충돌한 오브젝트</param>
    /// <returns></returns>
    bool IsObstacle(Transform target)
    {
        return target.CompareTag("Ground") || target.GetComponent<IceMaker_Ice>() != null || target.gameObject.layer == LayerMask.NameToLayer("Map Object");
    }
    /// <summary>
    /// 스페셜키([)를 누르면 플레이어의 발밑으로 생성위치를 바꿈(토글 형식)
    /// </summary>
    /// <param name="key">[</param>
    public override void InputSpecialKey(PlayerSkills.SpecialKey key)
    {
        // 위치만 
        if(key == PlayerSkills.SpecialKey.None && inputSpecialKey == PlayerSkills.SpecialKey.NumPad5_Down)
        {
            isSpecialAction = !isSpecialAction;
            Vector3 userPos = user.position;
            userPos.y += YpositionForCrosshair;
            crosshairPositionChange?.Invoke(!isSpecialAction, userPos);
        }
        inputSpecialKey = key;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ViewportPointToRay(Center);
        //if (layerMask_PlayerIgnore > 0)
        //{
        //    Physics.Raycast(ray, out RaycastHit hit, iceMakerDistance);

        //    if (hit.transform != null)
        //    {
        //        Gizmos.color = Color.blue;
        //        Gizmos.DrawSphere(hit.point, 0.5f);
        //    }
        //    Gizmos.color = Color.black;
        //    Vector3 boxSize = iceSize;
        //    boxSize.y = 0.5f;
        //    Gizmos.DrawCube(hit.point, boxSize * 0.5f);
        //}

        // 레이캐스트 보여주는 기즈모
        /*if (hit.transform != null)
        {
            Handles.color = Color.green;
            Vector3 vec = Camera.main.ViewportToWorldPoint(Center);
            Handles.DrawLine(vec, hit.point, 2);
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                Vector3 pos = hit.point;
                pos.y = iceSize.y * 0.5f;
                Handles.DrawWireCube(pos, iceSize);
            }
        }*/

    }

    private void OnValidate()
    {
        
    }
#endif

}
