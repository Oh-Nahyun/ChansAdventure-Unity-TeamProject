using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float radius = 0f;
    public LayerMask layer;
    public Collider[] colliders;
    public Collider short_enemy;

    public GameObject scanIbgect;

    void Start()
    {

    }

    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (colliders.Length > 0)
        {
            float short_distance = Vector3.Distance(transform.position, colliders[0].transform.position);
            foreach (Collider col in colliders)
            {
                float short_distance2 = Vector3.Distance(transform.position, col.transform.position);
               
                if (short_distance > short_distance2)
                {
                    short_distance = short_distance2;
                    //short_enemy = col;
                  
                }
                short_enemy = col;
                if (short_enemy != null)
                {
                    scanIbgect = short_enemy.gameObject;
                    target(true);
                }

            }
        }
        else
        {
            target(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void target(bool t)
    {
        if (t)
        {
            Debug.Log($"{scanIbgect}°¨Áö");
        }
    }

  

}
