using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{

    /// <summary>
    /// 퀘스트 종류
    /// </summary>
    public enum QuestType
    {
        None = 0,
        Hunt,
        GiveItem,
        ClearDungeon
    }

    public QuestType questType;

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
    public int[] npcId;

    public QuestData(QuestType type, string name, string contents, string objectives)
    {
        QuestInfo(type, name, contents, objectives);
    }

    public void QuestInfo(QuestType type, string name, string contents, string objectives)
    {
        questName = name;
        questContents = contents;
        questObjectives = objectives;

        questType = type;
    }

  
}
