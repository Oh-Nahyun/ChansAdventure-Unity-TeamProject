using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLockArrow : RecycleObject
{

    public float arrowMaxLength = 10.0f;

    float preCalculateLength;

    float preCalculateColor;

    Transform target;

    Material material;

    Color subtractColor = Color.green;

    Color originColor;

    readonly int ID_Emission = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        material = renderer.material;
        originColor = material.GetColor(ID_Emission);
        subtractColor.a = 0;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        transform.localScale = Vector3.zero;
        material.SetColor(ID_Emission, originColor);
    }

    public void Initialize(ReactionObject target, float maxDamage)
    {
        preCalculateLength = arrowMaxLength / maxDamage;
        preCalculateColor = preCalculateLength / arrowMaxLength;
        this.target = target.transform;
        target.onTimeLockDamageChange = SetArrowDirection;
        target.onTimeLockFinish = () => gameObject.SetActive(false);
    }

    void SetArrowDirection(Vector3 direction, float damage)
    {
        if (transform.localScale.z < 0.001f)
        {
            transform.localScale = Vector3.one;
        }

        transform.position = target.transform.position;
        transform.forward = direction;
        Vector3 scale = transform.localScale;
        scale.z = Mathf.Clamp(1 * damage * preCalculateLength, 1, arrowMaxLength);
        transform.localScale = scale;

        Debug.Log(damage + " " + preCalculateColor);
        Color color = originColor - (subtractColor * damage * preCalculateColor);
        material.SetColor(ID_Emission, color);
    }
}
