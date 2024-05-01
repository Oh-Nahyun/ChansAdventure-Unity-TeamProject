using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<int, QuestData> questList = new Dictionary<int, QuestData>();

    private QuestMessage questMessage;

    private void Awake()
    {
        questMessage = FindObjectOfType<QuestMessage>();
        GenerateData();
    }

    private void GenerateData()
    {
        questList.Add(0, new QuestData(QuestData.QuestType.None, "퀘스트 이름", "퀘스트 내용", "퀘스트 목표"));
        questList.Add(10, new QuestData(QuestData.QuestType.Hunt, "퀘스트 사냥", "퀘스트 내용 사냥", "퀘스트 목표 10마리"));
        questList.Add(20, new QuestData(QuestData.QuestType.GiveItem, "퀘스트 아이템 기부", "퀘스트 내용 아이템 기부", "퀘스트 목표 10개"));
        questList.Add(30, new QuestData(QuestData.QuestType.ClearDungeon, "퀘스트 던전 클리어", "퀘스트 내용 던전 클리어", "퀘스트 목표 던전"));
    }

    public void GetQuestTalkIndex(int id, bool complete)
    {
        if (questList.ContainsKey(id))
        {
            string questName = questList[id].questName;
            questMessage.OnQuestMessage(questName, complete);
        }
    }
}
