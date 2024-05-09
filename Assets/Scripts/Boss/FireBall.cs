using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    float speed = 10.0f;

    private void FixedUpdate()
    {
        transform.Translate(speed * transform.forward * Time.fixedDeltaTime, Space.World);
    }
}
