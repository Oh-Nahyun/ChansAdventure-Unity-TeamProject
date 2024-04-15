using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// ������ 2�� �´� ���� ������ ���� �ʼ�
/// </summary>
public class AttackArea : MonoBehaviour
{
    /// <summary>
    /// �÷��̾ ������ �� ����Ǵ� ��������Ʈ
    /// </summary>
    public Action<IBattler> onPlayerIn;

    /// <summary>
    /// �÷��̾ ������ �� ����Ǵ� ��������Ʈ
    /// </summary>
    public Action<IBattler> onPlayerOut;

    /// <summary>
    /// ���� ������ �����ϴ� �ö��̴�
    /// </summary>
    public SphereCollider attackArea;   // ���������� ������ ǥ���ϱ� ���� public���� ������ �� �ν����� â���� ����

    private void Awake()
    {
        attackArea = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ ��������
        if (other.CompareTag("Player"))
        {
            IBattler target = other.GetComponent<IBattler>();
            onPlayerIn?.Invoke(target);     // �÷��̾ �������� �˸�
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IBattler target = other.GetComponent<IBattler>();
            onPlayerOut?.Invoke(target);    // �÷��̾ �������� �˸�
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (attackArea != null)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, attackArea.radius, 5);   // ���� ���� �׸���
        }
    }
#endif
}
