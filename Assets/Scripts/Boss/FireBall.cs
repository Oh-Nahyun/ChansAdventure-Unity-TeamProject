using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : ReactionObject
{
    float speed = 30.0f;

    private void FixedUpdate()
    {
        transform.Translate(speed * transform.forward * Time.fixedDeltaTime, Space.World);
    }
}
