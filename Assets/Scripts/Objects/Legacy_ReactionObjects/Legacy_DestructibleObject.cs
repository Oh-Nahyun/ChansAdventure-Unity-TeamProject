using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legacy_DestructibleObject : Legacy_ReactionObject
{
    public float objectMaxHp = 1.0f;

    float objectHp;
    protected float ObjectHp
    {
        get => objectHp;
        set
        {
            objectHp = value;
            if(objectHp < 0.0f)
            {
                ObjectDestroy();
            }
        }
    }


    private void Start()
    {
        ObjectHp = objectMaxHp;
    }

    public void HitReaction(float power)
    {
        ObjectHp -= power;
    }

    public void HitReaction()
    {
        HitReaction(1.0f);
    }

    protected virtual void ObjectDestroy()
    {
        Debug.Log("파괴");
        gameObject.SetActive(false);
    }
}
