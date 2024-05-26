using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 스킬 순서: 리모컨폭탄, 리모컨폭탄큐브, 마그넷캐치, 아이스메이커, 타임록

public class SkillCooltimeUI : MonoBehaviour
{
    Image cooltimeImage;
    Image[] allImage;
    Color[] originColors;
    const float VisibleAlpha = 1.0f;
    const float UnvisibleAlpha = 0.0f;

    private void Awake()
    {
        Transform child = transform.GetChild(0); 
        cooltimeImage = child.GetComponent<Image>();
        allImage = GetComponentsInChildren<Image>();
        originColors = new Color[allImage.Length];
        for(int i = 0; i < allImage.Length; i++)
        {
            originColors[i] = allImage[i].color;
        }

        SetAllImage(UnvisibleAlpha);
    }

    private void Start()
    {
        SkillManager skillManager = GameManager.Instance.Skill;
        skillManager.PlayerSkill.onCooltimeChange += CooltimeRefresh;
    }


    void CooltimeRefresh(float ratio)
    {
        if(ratio > 0.99f)
        {
            SetAllImage(UnvisibleAlpha);
        }
        else
        {
            SetAllImage(VisibleAlpha);
        }
        cooltimeImage.fillAmount = ratio;
    }

    void SetAllImage(float alpha)
    {
        for(int i = 0; i < allImage.Length; i++)
        {
            Color color = originColors[i];
            color.a = alpha;
            allImage[i].color = color;
        }
    }
}
