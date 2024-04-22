using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Map Mark 오브젝트가 사용하는 클래스
/// </summary>
public class MapPointMark : MonoBehaviour
{
    /// <summary>
    /// Mark 오브젝트를 오른쪽 클릭했을 때 실행하는 함수
    /// </summary>
    public void ShowMarkInfo()
    {
        Debug.Log($"GameObject Name : {transform.position}");

        Destroy(transform.parent.gameObject);  // 핑 오브젝트 삭제
    }
}
