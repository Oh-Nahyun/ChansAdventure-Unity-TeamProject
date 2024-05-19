using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ReactionDoor : ReactionObject
{
    Transform leftDoor;
    Transform rightDoor;

    const float MaxLeftAngle = 270f;
    const float MinLeftAngle = 180f;

    Renderer leftRenderer;
    Renderer rightRenderer;

    protected override void Awake()
    {
        base.Awake();

        leftDoor = transform.GetChild(0);
        rightDoor = transform.GetChild(1);

        Transform child = transform.GetChild(0);
        leftRenderer = child.GetComponentInChildren<Renderer>();

        child = transform.GetChild(1);
        rightRenderer = child.GetComponentInChildren<Renderer>();

        rightRenderer.materials = leftRenderer.materials;

    }
    public override void AttachRotate(Vector3 euler)
    {


        float leftAngle = (leftDoor.localRotation.eulerAngles + euler).y;
        if (leftAngle < MaxLeftAngle && leftAngle > MinLeftAngle)
        {
            leftDoor.localRotation = Quaternion.Euler(leftDoor.localRotation.eulerAngles + euler);
            rightDoor.localRotation = Quaternion.Euler(rightDoor.localRotation.eulerAngles - euler);
        }
    }

    public override void AttachMagnet(float moveSpeed, Material skillColor)
    {
        base.AttachMagnet(moveSpeed, skillColor);
        if (IsMagnetic)
        {
            rightRenderer.materials = leftRenderer.materials;
        }
    }

    public override void DettachMagnet()
    {
        base.DettachMagnet();
        if (IsMagnetic)
        {
            rightRenderer.materials = leftRenderer.materials;
        }
    }

    protected override void OnSkill(SkillName skillName)
    {
        base.OnSkill(skillName);

        rightRenderer.materials = leftRenderer.materials;
    }

    protected override void OffSKill()
    {
        base.OffSKill();

        rightRenderer.materials = leftRenderer.materials;
    }
}
