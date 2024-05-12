using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWindowUI : MonoBehaviour
{
    public GameObject skillBox;
    SkillBoxUI[] icons;
    int skillCount;
    SkillManager skillManager;
    RectTransform skillBoxAreaRect;
    Transform skillBoxArea;
    SkillName currentSkill;
    SkillName CurrentSkill
    {
        get => currentSkill;
        set
        {
            currentSkill = (SkillName)Mathf.Clamp((int)value, 0, skillCount-1);
        }
    }
    bool isBeforeOpen = false;

    const float CloseTime = 0.5f;

    const int BasePosX = -100;
    const int MovePosX = -200;

    private void Start()
    {
        skillBoxArea = transform.GetChild(0);
        skillBoxAreaRect = skillBoxArea.GetComponent<RectTransform>();

        skillManager = GameManager.Instance.Skill;
        skillCount = skillManager.SkillCount;

        skillManager.PlayerSkill.onSpecialKeyUI += InputSpecialKey;
        skillManager.PlayerSkill.onSkillAcquisition += SkillAcquisition;

        icons = new SkillBoxUI[skillCount];
        for (int i = 0; i < icons.Length; i++)
        {
            GameObject obj = Instantiate(skillBox, skillBoxArea);
            obj.name = $"SkillBox_{(SkillName)i}";
            icons[i] = obj.GetComponent<SkillBoxUI>();
            icons[i].SetIcon(skillManager[(SkillName)i].SkillImage);
            icons[i].gameObject.SetActive(false);
        }

        Open(false);
    }

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    SkillName InputSpecialKey(PlayerSkills.SpecialKey special)
    {
        switch (special)
        {
            case PlayerSkills.SpecialKey.NumPad8_Up:
                if (!isBeforeOpen)
                {
                    CurrentSkill = skillManager.PlayerSkill.CurrentSkillName;
                    Open(true);
                    SelectSkill();
                    OverSkillBox(CurrentSkill);
                }
                isBeforeOpen = true;
                break;
            case PlayerSkills.SpecialKey.NumPad4_Left:
                if (gameObject.activeSelf)
                {
                    int count = 1;
                    int index = (int)CurrentSkill - 1;
                    while (index > -1 && !icons[index].gameObject.activeSelf)
                    {
                        count++;
                        index--;
                        if (index < 0)
                        {
                            count = 0;
                            break;
                        }
                    }
                    CurrentSkill -= count;
                    OverSkillBox(CurrentSkill);
                }
                break;
            case PlayerSkills.SpecialKey.NumPad6_Right:
                if (gameObject.activeSelf)
                {
                    int count = 1;
                    int index = (int)CurrentSkill + 1;
                    while (index < skillCount && !icons[index].gameObject.activeSelf)
                    {
                        count++;
                        index++;
                        if (index > skillCount - 1)
                        {
                            count = 0;
                            break;
                        }
                    }
                    CurrentSkill += count;
                    OverSkillBox(CurrentSkill);
                }
                break;
            case PlayerSkills.SpecialKey.None:
                if (gameObject.activeSelf)
                {
                    SelectSkill();
                    StartCoroutine(SlowClose());
                    isBeforeOpen = false;
                }
                break;
        }

        return CurrentSkill;
    }

    void SelectSkill()
    {
        if (gameObject.activeSelf)
        {
            for (int i = 0; i < skillCount; i++)
            {
                icons[i].Select(false);
            }
            icons[(int)CurrentSkill].Select(true);
        }
    }

    void Open(bool isOpen)
    {
        gameObject.SetActive(isOpen);
    }

    void OverSkillBox(SkillName name)
    {
        // 이전 작업
        /*int index = 0;
        if(centerNumber > (int)name)        // 선택된 스킬이 중앙보다 앞(리모컨폭탄,리모컨폭탄큐브)
        {
            int numberDiff = centerNumber - (int)name;
            for(int i = 0; i < numberDiff; i++)
            {
                icons[i].gameObject.SetActive(false);
            }
            for(int i = numberDiff; i < icons.Length; i++)
            {
                icons[i].gameObject.SetActive(true);
                icons[i].SetIcon(skillManager[(name+index)].SkillImage);
                index++;
            }

        }
        else if(centerNumber < (int)name)   // 선택된 스킬이 중앙보다 뒤(타임록, 아이스메이커)
        {
            int numberDiff = icons.Length + (centerNumber - (int)name);
            for (int i = icons.Length-1; i >= numberDiff; i--)
            {
                icons[i].gameObject.SetActive(false);
            }
            for (int i = numberDiff; i >= 0; i--)
            {
                icons[i].gameObject.SetActive(true);
                icons[i].SetIcon(skillManager[name-index].SkillImage);
                index++;
            }
        }
        else                                // 선택된 스킬이 중앙 (마그넷캐치)
        {
            for(int i = 0; i < icons.Length; i++)
            {
                icons[i].gameObject.SetActive(true);
                icons[i].SetIcon(skillManager[(SkillName)i].SkillImage);
            }
        }*/
        Vector2 pos = skillBoxAreaRect.anchoredPosition;
        int offset = 0;
        for(int i = 0; i < (int)name; i++)
        {
            if (skillManager.PlayerSkill.IsUableSkills[i])
            {
                offset++;
            }
        }
        pos.x = BasePosX + (MovePosX * offset);
        skillBoxAreaRect.anchoredPosition = pos;
    }

    void SkillAcquisition(SkillName name)
    {
        icons[(int)name].gameObject.SetActive(true);
        CurrentSkill = name;
    }

    IEnumerator SlowClose()
    {
        yield return new WaitForSeconds(CloseTime);
        Open(false);
    }


}
