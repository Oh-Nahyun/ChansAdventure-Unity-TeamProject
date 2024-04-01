using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legacy_MovableObject : Legacy_ReactionObject
{
    

    public void ExplosionReaction(Vector3 power)
    {
        Debug.Log("이동");
        rigid.AddForce(power * reducePower , ForceMode.Impulse);
    }
}
