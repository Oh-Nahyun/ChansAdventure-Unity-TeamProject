using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ReactionDoor : ReactionObject
{
    Transform leftDoor;
    Transform rightDoor;

    const float MaxLeftAngle = 260f;
    const float MinLeftAngle = 180f;

    protected override void Awake()
    {
        base.Awake();

        leftDoor = transform.GetChild(0);
        rightDoor = transform.GetChild(1);

    }
    public override void AttachRotate(Vector3 euler)
    {
        float leftAngle = (leftDoor.rotation.eulerAngles + euler).y;
        if (leftAngle < MaxLeftAngle && leftAngle > MinLeftAngle)
        {
            leftDoor.rotation = Quaternion.Euler(leftDoor.rotation.eulerAngles + euler);
            rightDoor.rotation = Quaternion.Euler(rightDoor.rotation.eulerAngles - euler);
        }
    }
}
