using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    private ParticleSystem explosion;

    /// <summary>
    /// 풍선이 폭발할 때 실행하는 델리게이트
    /// </summary>
    public Action onExplosion;

    private void Awake()
    {
        explosion = GetComponentInChildren<ParticleSystem>();
        explosion.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);

        StartCoroutine(DisableCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);

        StartCoroutine(DisableCoroutine());
    }

    IEnumerator DisableCoroutine()
    {
        explosion.Play();
        yield return new WaitForSeconds(3f);
        transform.gameObject.SetActive(false);
        onExplosion?.Invoke();
    }
}
