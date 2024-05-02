using System;
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

    TestNPC test;

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

    private int questCount = 0;
    private int questMaxCount = 0;

    public Action<int> QuestClearId;



    private void Awake()
    {
        textQuestName = GetComponentInChildren<TextMeshProUGUI>();
        questInfoData = FindAnyObjectByType<QuestInfoData>();
        test = FindAnyObjectByType<TestNPC>();
       
    }

    private void Start()
    {
        test.EnemyId += (id) => getEnemyID(id);
        test.KillEnemy += (count) => 
        {
            UpdateQuestProgress(count);
            UpdateQuestUI();
        };
    }

    private void Update()
    {
        textQuestName.text = questName;
        
    }

    public void Initialize(QuestType type, int id, string name, string contents, string objectives, int count)
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
                HuntQuest(count);
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

    public void HuntQuest(int maxKill)
    {
        questMaxCount = maxKill;
        UpdateQuestUI();
        Debug.Log($"{questCount} 갱신");
        // 현재 퀘스트 진행 상황을 기준으로 클리어 여부를 확인합니다.
        if (questCount >= maxKill)
        {
            QuestClear();
        }
    }

    public void GiveItemQuest()
    {
        // Give item quest logic
    }

    public void ClearDungeonQuest()
    {
        // Clear dungeon quest logic
    }

    public void UpdateQuestProgress(int currentKill)
    {
        this.questCount = currentKill;
    }
    void UpdateQuestUI()
    {
        questObjectives = $"처치 {questCount}/{questMaxCount} ";
    }

    private void QuestClear()
    {
        QuestClearId?.Invoke(questId);
    }

    private void getEnemyID(int EnemyId)
    {
        
    }

}
