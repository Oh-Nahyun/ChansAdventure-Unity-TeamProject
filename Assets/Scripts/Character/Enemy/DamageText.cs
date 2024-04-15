using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : RecycleObject
{
    /// <summary>
    /// �̵� ��ȭ ��Ÿ���� Ŀ��
    /// </summary>
    public AnimationCurve movement;

    /// <summary>
    /// ���� ������ ũ�� ��ȭ�� ��Ÿ���� Ŀ��
    /// </summary>
    public AnimationCurve fade;

    /// <summary>
    /// �ִ�� �ö󰡴� ����(�ִ���� = baseHeight + maxHeight)
    /// </summary>
    public float maxHeight = 1.5f;

    /// <summary>
    /// ��ü ��� �ð�
    /// </summary>
    public float duration = 1.0f;

    /// <summary>
    /// ���� ���� �ð�
    /// </summary>
    float elapsedTime = 0.0f;

    /// <summary>
    /// �⺻ ����(���� �Ǿ��� ���� ����)
    /// </summary>
    float baseHeight = 0.0f;

    // ������Ʈ
    TextMeshPro damageText;

    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // ���� �ʱ�ȭ
        elapsedTime = 0.0f;                 // ����ð� �ʱ�ȭ
        damageText.color = Color.white;     // ���� �ʱ�ȭ
        transform.localScale = Vector3.one; // ��ĳ�� �ʱ�ȭ
        baseHeight = transform.position.y;  // �⺻ ���� ����
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;              // �ð� �����ϰ�
        float timeRatio = elapsedTime / duration;   // �ð� ������ ���

        float curveMove = movement.Evaluate(timeRatio);             // Ŀ�꿡�� ���� ���� ��������
        float currentHeight = baseHeight + maxHeight * curveMove;   // �� ���� ���
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);    // �� ���� ����

        float curveAlpha = fade.Evaluate(timeRatio);        // Ŀ�꿡�� ����� �� ��ĳ�� �� ��������
        damageText.color = new Color(1, 1, 1, curveAlpha);  // ���� ����
        transform.localScale = new(curveAlpha, curveAlpha, curveAlpha); // ������ ����

        if (elapsedTime > duration)        // ����ð��� �ٵǸ�
        {
            gameObject.SetActive(false);    // ������ ��Ȱ��ȭ
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;    // ������ �����
    }

    /// <summary>
    /// ��µ� ���� ����
    /// </summary>
    /// <param name="damage">��µ� ������</param>
    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
}
