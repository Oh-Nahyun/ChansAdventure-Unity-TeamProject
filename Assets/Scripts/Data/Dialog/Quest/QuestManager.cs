using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    private Dictionary<int, QuestData> questList = new Dictionary<int, QuestData>();

    private QuestMessage questMessage;

    /// <summary>
    /// QuestMessage 초기화용 접근 프로퍼티
    /// </summary>
    private QuestMessage QuestMessage
    {
        get
        {
            if(questMessage == null)
            {
                questMessage = FindObjectOfType<QuestMessage>();
            }
            return questMessage;
        }
        set => questMessage = value;
    }

    private List<QuestInfoPanel> questInfoPanels = new List<QuestInfoPanel>();

    public GameObject questInfoPanelPrefab; // QuestInfoPanel 프리팹
    public Transform questInfoPanelParent;  // QuestInfoPanel이 생성될 부모 Transform

    public QuestInfo questInfo;

    /// <summary>
    /// QuestInfo 초기화용 프로퍼티
    /// </summary>
    public QuestInfo QuestInfo
    {
        get
        {
            if(questInfo == null)
            {
                questInfo = FindObjectOfType<QuestInfo>();
            }
            return questInfo;
        }
        set => questInfo = value;
    }

    public List<int> onQuestID;
    public List<int> clearQuestID;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        GenerateData();
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        QuestMessage = FindObjectOfType<QuestMessage>();
        QuestInfo = FindObjectOfType<QuestInfo>();
        questInfoPanelParent = questInfo.transform.GetChild(2);
    }

    /// <summary>
    /// Dictionary questList에 퀘스트 Data를 추가하는 함수 (키값 / QuestData(퀘스트 종류, 이름, 내용, 목표 ID))
    /// </summary>
    private void GenerateData()
    {
        questList.Add(0, new QuestData(QuestData.QuestType.None, "퀘스트 이름", "퀘스트 내용", "퀘스트 목표", 1, 0));
        questList.Add(10, new QuestData(QuestData.QuestType.Hunt, "퀘스트 사냥", "퀘스트 내용 사냥", "퀘스트 목표 10마리", 10, 1));
        questList.Add(20, new QuestData(QuestData.QuestType.GiveItem, "퀘스트 아이템 기부", "퀘스트 내용 아이템 기부", "퀘스트 목표 10개", 10, 100));
        questList.Add(30, new QuestData(QuestData.QuestType.ClearDungeon, "사당 돌파하기", "사당을 끝까지 돌파하라", "사당 클리어", 1, 0));
    }

    /// <summary>
    /// Quest관련 대화가 진행되었을때 실행되는 함수
    /// </summary>
    /// <param name="id"></param>
    /// <param name="complete"></param>
    public void GetQuestTalkIndex(int id, bool complete)
    {
        if (questList.ContainsKey(id))
        {
            QuestData questData = questList[id];
            QuestMessage.OnQuestMessage(questData.questName, complete);
          
            if (!complete)
            {

                // 퀘스트 시작일 때
                // 해당 퀘스트에 대한 QuestInfoPanel이 이미 생성되었는지 확인
                QuestInfoPanel existingPanel = questInfoPanels.Find(panel => panel.questId == id);
                if (existingPanel == null)
                {
                    // QuestInfoPanel 동적 생성 및 초기화
                    QuestInfoPanel newQuestInfoPanel = CreateQuestInfoPanel();
                    newQuestInfoPanel.Initialize(questData.questType , id, questData.questName, questData.questContents, questData.questObjectivesText, questData.questObjectivesCount, questData.questObjectID);

                    // 생성된 QuestInfoPanel을 리스트에 추가
                    questInfoPanels.Add(newQuestInfoPanel);
                }
            }
            else
            {
                // 퀘스트 완료일 때
                // id에 해당하는 QuestInfoPanel 찾아서 제거
                QuestInfoPanel panelToRemove = questInfoPanels.Find(panel => panel.questId == id);
                if (panelToRemove != null)
                {
                    DestroyQuestInfoPanel(panelToRemove);
                }
            }

        }
    }

    /// <summary>
    /// QuestInfoPanel생성을 위한 함수
    /// </summary>
    /// <returns></returns>
    private QuestInfoPanel CreateQuestInfoPanel()
    {
        // QuestInfoPanel 프리팹을 Instantiate하여 생성
        GameObject newPanelObject = Instantiate(questInfoPanelPrefab, questInfoPanelParent);
        QuestInfoPanel newQuestInfoPanel = newPanelObject.GetComponent<QuestInfoPanel>();

        return newQuestInfoPanel;
    }

    /// <summary>
    /// QuestInfoPanel삭제를 위한 함수
    /// </summary>
    /// <param name="panel"></param>
    private void DestroyQuestInfoPanel(QuestInfoPanel panel)
    {
        // 리스트에서 제거하고 GameObject를 파괴
        questInfoPanels.Remove(panel);
        Destroy(panel.gameObject);
    }

    public void OpenQuest()
    {
        QuestInfo.gameObject.SetActive(true);
        QuestInfo.OnQuestInfo();
    }
}
