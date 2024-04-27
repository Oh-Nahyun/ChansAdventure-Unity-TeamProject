using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    [Header("�� �⺻ ������")]
    // �ִ� ü��
    public float maxHealth;

    // ���� ü�� ������Ƽ
    private float _currentHealth;
    public float CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            // ���� ü���� �ִ� ü���� ���� �ʵ��� ����
            _currentHealth = Mathf.Clamp(value, 0f, maxHealth);

            // ü���� 0 ���Ϸ� �������� �� ���� ó��
            if (_currentHealth <= 0f)
            {
                Die();
            }
        }
    }

    // ������
    public float damage;

    // ��ȸ ������ �� �̵� �ӵ�
    public float patrollingSpeed;

    // �߰� ������ �� �̵� �ӵ�
    public float chasingSpeed;

    // ���� ���� ��Ÿ��
    public float attackCooldown;

    // �߰� ��Ÿ�
    public float chaseRange;

    // ���� ��Ÿ�
    public float attackRange;

    // �׾����� �˸��� ��������Ʈ
    public Action onDeath;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    // ���� ó�� �޼���
    private void Die()
    {
        // �׾����� �˸��� ��������Ʈ ȣ��
        onDeath?.Invoke();
        // �� ĳ���� ���� ������Ʈ ��Ȱ��ȭ �Ǵ� ���� ���� ������ ó��
    }
}
