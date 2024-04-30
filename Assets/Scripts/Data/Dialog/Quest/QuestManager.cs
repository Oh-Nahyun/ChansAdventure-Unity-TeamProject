using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;

    Dictionary<int, QuestData> questList;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerteData();
    }

    private void GenerteData()
    {
        //questList.Add(10, new QuestData("퀘스트 이름", new int[] {1000, 2000}));  //퀘스트 ID / 퀘스트 이름 / 관련 NPC ID 입력
    }

    public int GetQusetTalkIndex(int id)
    {
        return questList.Count;
    }
}
