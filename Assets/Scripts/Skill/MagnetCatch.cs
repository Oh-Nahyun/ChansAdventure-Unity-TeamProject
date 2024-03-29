using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
using System;
using UnityEngine.InputSystem;
using System.Security.Cryptography;





#if UNITY_EDITOR
using UnityEditor;
#endif

public class MagnetCatch : Skill
{
    [Header("마그넷캐치 데이터")]
    public float magnetDistance = 5.0f;
    public float targetMoveSpeed = 5.0f;
    public float verticalSpeed = 2.0f;
    public float horizontalDistanceAtOnce = 0.2f;

    bool isMagnetic = false;

    float preYDir = 0;

    Vector2 curMousePos = Vector2.zero;
    Vector2 preMousePos = Vector2.zero;

    Transform targetDestination;

    Transform target;
    ReactionObject reactionTarget;
    Vector3 hitPoint;
    MagnetVCam magnetVcam;
    Cinemachine.CinemachineTargetGroup targetGroup;

    Vector3 targetOriginRotate;

    readonly Vector3 Center = new Vector3(0.5f, 0.5f, 0.0f);

    Action magnetCamOn;
    Action magnetCamOff;

    protected override void Awake()
    {
        base.Awake();
        targetDestination = transform.GetChild(1);
        targetGroup = GetComponentInChildren<Cinemachine.CinemachineTargetGroup>();
        
    }

    
    protected override void Start()
    {
        base.Start();

        if(owner != null)
        {
            owner.onScroll += SetDestinationScroll;
        }
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(magnetVcam == null)
        {
            magnetVcam = GameManager.Instance.Cam.MagnetCam;
        }


        isActivate = false;
        isMagnetic = false;

        magnetCamOn = magnetVcam.OnSkillCamera;
        magnetCamOn += () => magnetVcam.SetLookAtTransform(targetGroup.transform);
        magnetCamOff = magnetVcam.OffSkillCamera;

        targetGroup.m_Targets[1].target = owner.transform;

        StartCoroutine(TargetCheck());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        reactionTarget = null;
        target = null;
    }

    private void FixedUpdate()
    {
        if(isActivate)
        {
            // 플레이어의 방향 설정
            Vector3 euler = owner.transform.rotation.eulerAngles;
            owner.LookForwardPlayer(Camera.main.transform.forward);
            euler = owner.transform.rotation.eulerAngles - euler;

            DestinationMover();
            reactionTarget.AttachRotate(euler);
        }
    }
    void DestinationMover()
    {

        preMousePos = curMousePos;
        curMousePos = Mouse.current.position.value;
        Vector2 mouseDir = (curMousePos - preMousePos).normalized;
        if(mouseDir.y * preYDir >= 0f)
        {
            targetDestination.position += new Vector3(0, mouseDir.y * Time.fixedDeltaTime * verticalSpeed, 0);
        }
        else
        {
            Vector3 pos = targetDestination.position;
            pos.y = target.position.y;
            targetDestination.position = pos;
        }

        preYDir = mouseDir.y > 0 ? 1 : -1;
    }

    protected override void OnSKillAction()
    {
        base.OnSKillAction();
        
    }
    protected override void UseSkillAction()
    {
        if (isMagnetic)
        {
            StopAllCoroutines();
            base.UseSkillAction();
            magnetCamOn?.Invoke();

            targetDestination.position = hitPoint;
            targetDestination.parent = owner.transform;      // 목적지의 부모를 플레이어로 설정해서 타켓을 플레이어의 정면에 위치하게하기

            targetGroup.m_Targets[0].target = target;

            targetOriginRotate = target.rotation.eulerAngles;

            curMousePos = Mouse.current.position.value;

            reactionTarget.AttachMagnet(targetDestination, targetMoveSpeed);
        }
    }

    protected override void OffSKillAction()
    {
        magnetCamOff?.Invoke();
        if (reactionTarget != null)
        {
            reactionTarget.DettachMagnet();
        }


        reactionTarget = null;
        target = null;
        isActivate = false;

        targetDestination.parent = transform;

        base.OffSKillAction();
    }

    void SetDestinationScroll(float scrollY)
    {
        Vector3 pos = targetDestination.localPosition;
        float y = scrollY > 0 ? 1 : -1;
        pos.z += y * horizontalDistanceAtOnce;
        targetDestination.localPosition = pos;
    }

    IEnumerator TargetCheck()
    {
        while (true) {
        Ray ray = Camera.main.ViewportPointToRay(Center);
            Physics.Raycast(ray, out RaycastHit hit, magnetDistance);
            target = hit.transform;
            if (target != null)
            {
                reactionTarget = target.GetComponent<ReactionObject>();
                
                isMagnetic = (reactionTarget != null) && (reactionTarget.IsMagnetic);
                if (isMagnetic)
                {
                    //hitPoint = hit.collider.bounds.center;
                    hitPoint = hit.point;
                }
            }

            yield return null;
        }
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ViewportPointToRay(Center);
        Physics.Raycast(ray, out RaycastHit hit, magnetDistance);
        // 레이캐스트 보여주는 기즈모
        if(hit.transform != null)
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
