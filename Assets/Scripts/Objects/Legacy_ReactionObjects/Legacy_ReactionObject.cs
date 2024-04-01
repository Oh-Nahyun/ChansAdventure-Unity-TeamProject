using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Legacy_ReactionObject : RecycleObject
{
    public float weight = 1.0f;

    protected float reducePower = 1.0f;
    protected Rigidbody rigid;

    private void Awake()
    {
        reducePower /= weight;
        rigid = GetComponent<Rigidbody>();
    }

   /* public void OnSkillAffect(SkillName skillName)
    {
        OnSkillAffect(skillName, Vector3.zero);
    }

    public void OnSkillAffect(SkillName skillName, Vector3 power)
    {
        switch (skillName)
        {
            case SkillName.RemoteBomb:
                if ((reactionType & ReactionType.Destroy) != 0)
                {
                    Destroy(gameObject);
                }
                else if (((reactionType & ReactionType.Move) != 0))
                {
                    rigid.AddForce(power, ForceMode.Impulse);
                }
                break;
            case SkillName.MagnetCatch:
                if ((reactionType & ReactionType.Magnetic) != 0)
                {
                    rigid.useGravity = false;
                    magnetReaction = true;
                }
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }
    }

    public void FinishSkillAffect(SkillName skillName)
    {
        switch (skillName)
        {
            case SkillName.RemoteBomb:
                break;
            case SkillName.MagnetCatch:
                if ((reactionType & ReactionType.Magnetic) != 0)
                {
                    rigid.useGravity = true;
                    magnetReaction = false;
                }
                break;
            case SkillName.IceMaker:
                break;
            case SkillName.TimeLock:
                break;
        }
    }*/
}
