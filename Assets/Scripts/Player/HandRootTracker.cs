using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRootTracker : MonoBehaviour
{
    public void OnTracking(Transform target)
    {
        StartCoroutine(Trakcking(target));
    }

    public void OffTracking()
    {
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
