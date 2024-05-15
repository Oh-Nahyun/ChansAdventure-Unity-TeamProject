using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneObject : MonoBehaviour
{
    /// <summary>
    /// 트리거 범위
    /// </summary>
    public float range = 2f;

    /// <summary>
    /// 변경할 씬 이름
    /// </summary>
    public string targetSceneName;

    /// <summary>
    /// 다음 씬의 스폰 위치
    /// </summary>
    [Tooltip("변경할 씬의 스폰 위치")]
    public Vector3 nextSpawnPosition;

    /// <summary>
    /// 다음 씬이 필드인지 확인하는 변수
    /// </summary>
    [Tooltip("다음 씬이 필드씬인지 체크 ( 적 스포너 제어용 )")]
    public bool isField;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.spawnPoint = nextSpawnPosition;
            GameManager.Instance.isField = this.isField;
            GameManager.Instance.ChangeToTargetScene(targetSceneName, other.gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.up, range);
    }

#endif
}
