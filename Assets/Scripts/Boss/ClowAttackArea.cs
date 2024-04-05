using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClowAttackArea : MonoBehaviour
{
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit!");
        }

    }
}
