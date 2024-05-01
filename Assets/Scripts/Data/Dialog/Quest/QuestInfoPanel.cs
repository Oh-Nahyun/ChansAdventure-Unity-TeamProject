using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static QuestData;

public class QuestInfoPanel : MonoBehaviour, IPointerClickHandler
{
    TextMeshProUGUI textQuestName;
    QuestInfoData questInfoData;

    public int questId;
    /// <summary>
    /// 퀘스트 이름
    /// </summary>
    public string questName;
    /// <summary>
    /// 퀘스트 설명/내용
    /// </summary>
    public string questContents;
    /// <summary>
    /// 퀘스트 목표
    /// </summary>
    public string questObjectives;

    private void Awake()
    {
        textQuestName = GetComponentInChildren<TextMeshProUGUI>();
        questInfoData = FindAnyObjectByType<QuestInfoData>();
    }

    private void Update()
    {
        textQuestName.text = questName;
    }

    public void Initialize(QuestType type, int id, string name, string contents, string objectives)
    {
        questId = id;
        questName = name;
        questContents = contents;
        questObjectives = objectives;

        switch (type)
        {
            case QuestType.None:
                break;
            case QuestType.Hunt:
                HuntQuest(1, 10);
                break;
            case QuestType.GiveItem:
                GiveItemQuest();
                break;
            case QuestType.ClearDungeon:
                ClearDungeonQuest();
                break;
            default:
                break;
        }
        // 이 메서드는 QuestManager에서 호출될 때마다 호출됩니다.
        // 여기에 필요한 UI 업데이트 등의 로직을 추가할 수 있습니다.
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        questInfoData.OnQuestInfo(this.gameObject);
        questInfoData.setQuestList(questName, questContents, questObjectives);
    }

    public void HuntQuest(int currentKill, int maxKill)
    {
        int killCount = maxKill - currentKill;
        Debug.Log(killCount);
    }

    public void GiveItemQuest()
    {
        // Give item quest logic
    }

    public void ClearDungeonQuest()
    {
        // Clear dungeon quest logic
    }


}
