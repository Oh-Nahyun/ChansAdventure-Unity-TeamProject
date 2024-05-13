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
    /// <summary>
    /// 퀘스트 목표
    /// </summary>
    public int questObjectID;

    private int questCount = 0;
    public int QuestCount 
    {
        get => questCount;
        set 
        {
            QuestCount = Mathf.Clamp(value, 0, questMaxCount);
        } 
    }
    
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
        test.EnemyQuestData[0] += (id) => 
        {
            if (id == questObjectID)
            {
                GetEnemyID();
            }
        };
    }

    private void Update()
    {
        textQuestName.text = questName;
        
    }

    /// <summary>
    /// 퀘스트 패널이 생성되었을 때 실행되어질 함수
    /// </summary>
    /// <param name="type">퀘스트의 종류</param>
    /// <param name="id">퀘스트의 ID</param>
    /// <param name="name">퀘스트의 제목</param>
    /// <param name="contents">퀘스트의 내용</param>
    /// <param name="objectives">퀘스트 진행사항</param>
    /// <param name="count">퀘스트의 목표치</param>
    /// <param name="objectID">퀘스트의 목표 ID</param>
    public void Initialize(QuestType type, int id, string name, string contents, string objectives, int count, int objectID)
    {
        questId = id;
        questName = name;
        questContents = contents;
        questObjectives = objectives;
        questObjectID = objectID;

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

    /// <summary>
    /// 퀘스트 패널을 클릭했을 때 실행하는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        questInfoData.OnQuestInfo(this.gameObject);
        questInfoData.setQuestList(questName, questContents, questObjectives);
    }

    // 사냥 퀘스트 관련 -------------------------------
    public void HuntQuest(int maxKill)
    {
        questMaxCount = maxKill;
        questObjectives = $"처치 {QuestCount}/{questMaxCount} ";
        Debug.Log($"{QuestCount} 갱신");
        // 현재 퀘스트 진행 상황을 기준으로 클리어 여부를 확인합니다.
        if (QuestCount >= maxKill)
        {
            QuestClear();
        }
    }

    /// <summary>
    /// 퀘스트 패널UI를 업데이트 하는 함수
    /// </summary>
    void UpdateQuestProgress()
    {
        QuestCount++;
        questObjectives = $"처치 {QuestCount}/{questMaxCount} ";
        if (QuestCount == questMaxCount)
        {
            QuestClear();
        }
    }

    private void GetEnemyID()
    {
        test.EnemyQuestData[1] += (count) =>
        { 
                UpdateQuestProgress();
        };
    }

    // 아이템 기부 퀘스트 관련 -------------------------------

    public void GiveItemQuest()
    {
        // Give item quest logic
    }

    // 던전 클리어 퀘스트 관련 -------------------------------

    public void ClearDungeonQuest()
    {
        // Clear dungeon quest logic
    }

    /// <summary>
    /// 퀘스트 클리어시 실행될 함수
    /// </summary>
    private void QuestClear()
    {
        GameManager.Instance.QuestManager.clearQuestID.Add(questId);
        QuestClearId?.Invoke(questId);
        Debug.Log("클리어");
    }

}
