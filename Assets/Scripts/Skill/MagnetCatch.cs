using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MagnetCatch : Skill
{
    public float magnetDistance;
    public float playerRotateSpeed = 2.0f;
    public float moveXSpeed = 5.0f;
    public float moveYSpeed = 2.0f;
    public float moveZSpeed = 1.0f;

    bool IsMagnetic => target != null;
    bool activatedSkill = false;

    Vector3 cameraDir = Vector3.zero;

    Transform destinationX;

    ObjectEditor target;
    Transform targetTransform;
    Transform targetOriginParent;
    Rigidbody targetRigid;
    Vector3 hitPoint;
    PlayerVCam vcam;
    Cinemachine.CinemachineTargetGroup targetGroup;


    readonly Vector3 Center = new Vector3(0.5f, 0.5f, 0.0f);
    readonly Vector3 Top = new Vector3(0f, 1.0f, 0f);


    private void Awake()
    {
        destinationX = transform.GetChild(0);
        targetGroup = GetComponentInChildren<Cinemachine.CinemachineTargetGroup>();
        
    }
    /*protected override void Start()
    {
        base.Start();
        if (vcam == null)
        {
            vcam = GameManager.Instance.PlayerCam;
        }
        vcam.onMouseMove += CameraMove;
    }*/

    protected override void OnEnable()
    {
        base.OnEnable();
        if(vcam == null)
        {
            vcam = GameManager.Instance.PlayerCam;
        }

    }

    private void FixedUpdate()
    {
        if(activatedSkill)
        {
            MoveTarget();
        }
    }

    public override void OnSkillAction()
    {
        base.OnSkillAction();
        Debug.Log("마그넷");
        StartCoroutine(TargetCheck());
    }
    public override void UseSkillAction()
    {
        base.UseSkillAction();
        StopAllCoroutines();
        if (IsMagnetic && !activatedSkill)
        {
            //vcam.
            destinationX.position = hitPoint;
            destinationX.parent = owner.transform.GetChild(1);

            targetGroup.m_Targets[0].target = targetTransform;
            //targetOriginParent = targetTransform.parent;
            //targetTransform.parent = destinationX;
            targetRigid = targetTransform.GetComponent<Rigidbody>();
            //target.OnSkillAffect(skillName);

            activatedSkill = true;
        }
    }

    public override void OffSkillAction()
    {
        base.OffSkillAction();
        if (activatedSkill)
        {
            //target.FinishSkillAffect(skillName);
            target = null;
            activatedSkill = false;
            //targetTransform.parent = targetOriginParent;

            destinationX.parent = transform;
        }
    }

    IEnumerator TargetCheck()
    {
        while (true) {
        Ray ray = Camera.main.ViewportPointToRay(Center);
            Physics.Raycast(ray, out RaycastHit hit, magnetDistance);
            targetTransform = hit.transform;
            if (targetTransform != null)
            {
                target = targetTransform.GetComponent<ObjectEditor>();
                if (IsMagnetic)
                {
                    hitPoint = hit.point;
                }
            }

            yield return null;
        }
    }

    void MoveTarget()
    {

        float dirX = Camera.main.ViewportToWorldPoint(Center).x - targetTransform.position.x * moveXSpeed;
        float dirY = cameraDir.y * moveYSpeed;
        float dirZ = 0f;

        Vector3 dir = new Vector3(dirX, dirY, dirZ);

        targetRigid.MovePosition(targetRigid.position + dir * Time.fixedDeltaTime);
    }

    void CameraMove(Vector3 pos)
    {
        cameraDir = pos;
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
        OnSkillAction();
    }
    public void TestUseSkill()
    {
        UseSkillAction();
    }
    public void TestFinishSkill()
    {
        OffSkillAction();
    }
#endif
}
