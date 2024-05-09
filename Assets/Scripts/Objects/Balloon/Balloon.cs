using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    private ParticleSystem explosion;

    private void Awake()
    {
        explosion = GetComponentInChildren<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        explosion.Play();
        transform.gameObject.SetActive(false);
    }
}
