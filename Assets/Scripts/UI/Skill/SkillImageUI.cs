using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillImageUI : MonoBehaviour
{
    Image image;
    bool isEmptySkill = true;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        SkillManager skillManager = GameManager.Instance.Skill;
        skillManager.PlayerSkill.onSkillChange += ImageRefresh;
        skillManager.PlayerSkill.onSkillAcquisition += (skiilName) =>
        {
            isEmptySkill = false;
            ImageRefresh(skillManager[skiilName]);
        };

        ImageRefresh(skillManager[skillManager.PlayerSkill.CurrentSkillName]);
    }

    void ImageRefresh(Skill skill)
    {
        if (!isEmptySkill)
        {
            image.sprite = skill.SkillImage;
        }
    }
}
