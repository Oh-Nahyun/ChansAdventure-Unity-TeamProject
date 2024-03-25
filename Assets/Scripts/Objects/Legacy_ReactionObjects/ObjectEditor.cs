using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class ObjectEditor : MonoBehaviour
{

#if UNITY_EDITOR
    public ReactionType reactionType;
    public ReactionType Type => reactionType;

    Legacy_MovableObject movableObject;
    Legacy_DestructibleObject destructibleObject;
    Legacy_MagneticObject magneticObject;
    Legacy_ThrowableObject throwableObject;
    Legacy_ExplosiveObject explosiveObject;
    Rigidbody rigid;

    private void OnValidate() => EditorApplication.delayCall += SafeOnValidate;

    private void SafeOnValidate()
    {

        rigid = GetComponent<Rigidbody>();
        if (rigid == null)
        {
            rigid = transform.AddComponent<Rigidbody>();
        }
        AddObjectComponent();
        RemoveObjectComponent();
    }


    void AddObjectComponent()
    {

        movableObject = transform.GetComponent<Legacy_MovableObject>();
        if (movableObject == null && (reactionType & ReactionType.Move) != 0)
        {
            movableObject = transform.AddComponent<Legacy_MovableObject>();
        }

        destructibleObject = transform.GetComponent<Legacy_DestructibleObject>();
        if (destructibleObject == null && (reactionType & ReactionType.Destroy) != 0)
        {
            destructibleObject = transform.AddComponent<Legacy_DestructibleObject>();
        }

        magneticObject = transform.GetComponent<Legacy_MagneticObject>();
        if (magneticObject == null && (reactionType & ReactionType.Magnetic) != 0)
        {
            magneticObject = transform.AddComponent<Legacy_MagneticObject>();
        }

        throwableObject = transform.GetComponent<Legacy_ThrowableObject>();
        if (throwableObject == null && (reactionType & ReactionType.Throw) != 0)
        {
            throwableObject = transform.AddComponent<Legacy_ThrowableObject>();
        }

        explosiveObject = transform.GetComponent<Legacy_ExplosiveObject>();
        if( explosiveObject == null && (reactionType & ReactionType.Explosion) != 0)
        {
            explosiveObject = transform.AddComponent<Legacy_ExplosiveObject>();
        }
    }

    void RemoveObjectComponent()
    {

        if (movableObject != null && (reactionType & ReactionType.Move) == 0)
            DestroyImmediate(movableObject);

        if (destructibleObject != null && (reactionType & ReactionType.Destroy) == 0)
            DestroyImmediate(destructibleObject);
        
        if (magneticObject != null && (reactionType & ReactionType.Magnetic) == 0)
            DestroyImmediate(magneticObject);

        if (throwableObject != null && (reactionType & ReactionType.Throw) == 0)
            DestroyImmediate(throwableObject);

        if (explosiveObject != null && (reactionType & ReactionType.Explosion) == 0)
            DestroyImmediate(explosiveObject);
    }

#endif

}
