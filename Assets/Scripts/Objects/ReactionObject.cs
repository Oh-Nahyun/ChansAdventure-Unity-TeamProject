using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;


[Flags]
public enum ReactionType
{
    Move = 1,
    Throw = 2,
    Magnetic = 4,
    Destroy = 8,
    Explosion = 16,
    Hit = 32
}

[RequireComponent(typeof(Rigidbody))]
public class ReactionObject : RecycleObject
{
    [SerializeField] protected ExplosiveObject explosiveInfo;

    public float Weight = 1.0f;
    protected float reducePower = 1.0f;
    public float objectMaxHp = 1.0f;

    float objectHp;
    protected float ObjectHp
    {
        get => objectHp;
        set
        {
            objectHp = value;
            if (objectHp < 0.0f && (reactionType & ReactionType.Destroy) != 0)
            {
                DestroyReaction();
            }
        }
    }
    protected enum StateType
    {
        None = 0,
        PickUp,
        Throw,
        Move,
        Destroy,
        Boom,
        Magnet,
    }

    protected StateType currentState = StateType.None;

    //bool isCarried = false;
    //bool isThrow = false;

    public ReactionType reactionType;
    public ReactionType Type
    {
        get => reactionType;
        set
        {
            reactionType = value;
            if ((reactionType & ReactionType.Explosion) != 0)
            {
                reactionType |= ReactionType.Destroy;
            }
        }
    }


    protected Transform originParent;

    protected Rigidbody rigid;


    bool magnetReaction = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        if(rigid == null)
        {
            rigid = transform.AddComponent<Rigidbody>();
        }
        originParent = transform.parent;
        reducePower = 1.0f / Weight;
        
        if(Type == ReactionType.Explosion)
        {
            Type |= ReactionType.Destroy;
        }
        //isCarried = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        currentState = StateType.None;
        ObjectHp = objectMaxHp;
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        if (player != null)
        {
        //    player.onThrow += Throw;
        }
        else
        {
            Debug.LogWarning("플레이어가 없습니다.");
        }
    }


    protected void OnCollisionEnter(Collision collision)
    {
        if(currentState == StateType.Throw) // isThrow)
        {
            CollisionAfterThrow();
        }
    }

    protected virtual void CollisionAfterThrow()
    {
        currentState = StateType.None;
        if ((reactionType & ReactionType.Destroy) != 0)
        {
            DestroyReaction();
        }

    }

    protected void OnCollisionExit(Collision collision)
    {
        if (magnetReaction)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    public void HitReaction(bool isExplosion = false)
    {
        if (isExplosion)
        {
            Boom();
        }
        else
        {
            HitReaction(1.0f);
        }
    }

    public void HitReaction(float power)
    {
        if ((reactionType & ReactionType.Hit) != 0)
        {
            ObjectHp -= power;
        }
    }

    protected void DestroyReaction()
    {
        currentState = StateType.Destroy;
        Boom();
        // -- 파괴 동작 코루틴 추가해야됨
        ReturnToPool();
    }

    void Boom()
    {
        if ((reactionType & ReactionType.Explosion) != 0 && currentState != StateType.Boom)
        {
            currentState = StateType.Boom;
            // -- 폭발 동작 코루틴 추가해야됨
            Collider[] objects = Physics.OverlapSphere(transform.position, explosiveInfo.boomRange);
            foreach (Collider obj in objects)
            {
                // 밑에 수정
                ReactionObject reactionObj = obj.GetComponent<ReactionObject>();
                if (reactionObj != null)
                {
                    reactionObj.HitReaction(true);
                    Vector3 dir = obj.transform.position - transform.position;
                    Vector3 power = dir.normalized * explosiveInfo.force + obj.transform.up * explosiveInfo.forceY;
                    reactionObj.ExplosionReaction(power);
                }
            }
        }
    }

    public void ExplosionReaction(Vector3 power)
    {
        if ((reactionType & ReactionType.Move) != 0)
        {
            rigid.AddForce(power * reducePower, ForceMode.Impulse);
        }
    }

    public void PickUp(Transform root)
    {
        if ((reactionType & ReactionType.Throw) != 0 && currentState == StateType.None)
        {
            transform.parent = root;
            Vector3 destPos = root.position;

            transform.position = destPos;
            //isCarried = true;
            currentState = StateType.PickUp;
            rigid.isKinematic = true;
        }
    }

    public void PickUp()
    {
        if ((reactionType & ReactionType.Throw) != 0 && currentState == StateType.None)
        {
            currentState = StateType.PickUp;
            rigid.isKinematic = true;
        }
    }

    public void Throw(float throwPower, Transform user)
    {
        if ((reactionType & ReactionType.Throw) != 0 && currentState == StateType.PickUp)
        {
            rigid.isKinematic = false;
            //isCarried = false;
            //isThrow = true;
            currentState = StateType.Throw;

            rigid.AddForce((user.forward + user.up) * throwPower * reducePower, ForceMode.Impulse);
            //rigid.AddRelativeForce((transform.forward + transform.up) * throwPower, ForceMode.Impulse);
            transform.parent = originParent;
        }
    }

    protected void ReturnToPool()
    {
        transform.SetParent(originParent);
        gameObject.SetActive(false);
    }

    public void Drop()
    {
        if ((reactionType & ReactionType.Throw) != 0)
        {
            transform.parent = originParent;
            //isCarried = true;
            currentState = StateType.None;
            rigid.isKinematic = false;
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if ((reactionType & ReactionType.Explosion) != 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, explosiveInfo.boomRange);
        }
    }

    private void OnValidate()
    {
        if(Type == ReactionType.Explosion)
        {
            Type |= ReactionType.Destroy;
        }
    }
#endif

}

[Serializable]
public class ExplosiveObject
{
    public float force = 4.0f;
    public float forceY = 5.0f;
    public float boomRange = 3.0f;
    public float damage = 3.0f;
}
