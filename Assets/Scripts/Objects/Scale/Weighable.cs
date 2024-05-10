using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weighable : MonoBehaviour
{
    Rigidbody rb;
    public float weigh;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        weigh = rb.mass;
    }

}
