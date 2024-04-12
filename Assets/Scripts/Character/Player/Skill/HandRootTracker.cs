using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRootTracker : MonoBehaviour
{
    private void Awake()
    {
        transform.localPosition = Vector3.zero;
    }
    public void OnTracking(Transform target)
    {
        StartCoroutine(Trakcking(target));
    }

    public void OffTracking()
    {
        transform.localPosition = Vector3.zero;
        StopAllCoroutines();
    }

    IEnumerator Trakcking(Transform target)
    {
        while (true)
        {
            transform.position = target.position;
            yield return null;
        }
    }
}
