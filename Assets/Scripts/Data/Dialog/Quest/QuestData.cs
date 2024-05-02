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
    /// 퀘스트 목표 내용
    /// </summary>
    public string questObjectivesText;

    /// <summary>
    /// 퀘스트 목표 수행 횟수
    /// </summary>
    public int questObjectivesCount;

    /// <summary>
    /// 퀘스트 목표 게임 오브젝트
    /// </summary>
    public GameObject questObject;

    public QuestData(QuestType type, string name, string contents, string objectives, int count, GameObject gameObject)
    {
        QuestInfo(type, name, contents, objectives, count, gameObject);
    }

    public void QuestInfo(QuestType type, string name, string contents, string objectives, int count, GameObject gameObject)
    {
        questName = name;
        questContents = contents;
        questObjectivesText = objectives;
        questType = type;
        questObjectivesCount = count;
        questObject = gameObject;
    }

  
}
