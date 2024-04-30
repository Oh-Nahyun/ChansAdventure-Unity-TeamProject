using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData : MonoBehaviour
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

        switch (type)
        {
            case QuestType.None:
                break;
            case QuestType.Hunt:
                HuntQuest(1, 10); // 예시로 최소 1 마리부터 최대 10 마리를 사냥해야 하는 퀘스트로 설정
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
    }

    public void HuntQuest(int currentKill, int maxKill)
    {
        int killCount = maxKill - currentKill;
        Debug.Log("Remaining enemies to kill: " + killCount);
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
