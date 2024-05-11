using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 스킬 순서: 리모컨폭탄, 리모컨폭탄큐브, 마그넷캐치, 아이스메이커, 타임록

public class SkillCooltimeUI : MonoBehaviour
{
    Image cooltimeImage;

    private void Awake()
    {
        Transform child = transform.GetChild(0); 
        cooltimeImage = child.GetComponent<Image>();
    }

    private void Start()
    {
        SkillManager skillManager = GameManager.Instance.Skill;
        skillManager.PlayerSkill.onCooltimeChange += CooltimeRefresh;

        gameObject.SetActive(false);
        
    }


    void CooltimeRefresh(float ratio)
    {
        if(ratio > 0.99f)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        cooltimeImage.fillAmount = ratio;
    }
}
