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

    protected override void Awake()
    {
        base.Awake();

        leftDoor = transform.GetChild(0);
        rightDoor = transform.GetChild(1);

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
}
