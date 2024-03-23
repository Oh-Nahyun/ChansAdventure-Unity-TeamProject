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
        target(false);
    }

    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (colliders.Length > 0)
        {
            float shortestDistance = Vector3.Distance(transform.position, colliders[0].transform.position);
            short_enemy = colliders[0]; // 일단 첫 번째 요소를 가장 가까운 것으로 설정

            foreach (Collider col in colliders)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    short_enemy = col; // 더 가까운 것을 찾으면 short_enemy 업데이트
                }
            }
        }
        else
        {
            short_enemy = null; // colliders가 없을 때 short_enemy 초기화
        }

        target(short_enemy != null); // short_enemy가 null이 아닐 때만 실행
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
            // 최상위 부모 GameObject를 찾아서 scanIbgect에 할당
            scanIbgect = FindTopParentWithCollider(short_enemy.gameObject);
        }
        else
        {
            scanIbgect = null;
        }
    }

    // Collider를 가진 GameObject의 최상위 부모 GameObject를 반환하는 메서드
    GameObject FindTopParentWithCollider(GameObject childObject)
    {
        Transform parentTransform = childObject.transform.parent;

        if (parentTransform == null)
        {
            return childObject;
        }

        // 부모 GameObject에 Collider가 있으면 현재 GameObject를 반환
        if (parentTransform.GetComponent<Collider>() != null)
        {
            return childObject;
        }

        // 부모 GameObject의 부모 GameObject를 재귀적으로 검색하여 최상위 부모 GameObject를 반환
        return FindTopParentWithCollider(parentTransform.gameObject);
    }
}
 

   

