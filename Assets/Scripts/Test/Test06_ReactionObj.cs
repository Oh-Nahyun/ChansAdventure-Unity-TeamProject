using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Test06_ReactionObj : Skill_TestBase
{
    public ReactionObject obj;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        obj.PickUp(transform);
        obj.Throw(3.0f, transform);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        obj.HitReaction(10);
    }


}
