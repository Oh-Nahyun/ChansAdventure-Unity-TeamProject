using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPlace : MonoBehaviour
{
    public int qusetId;
    
    QuestManager questManager;

    private void Awake()
    {
        questManager = FindObjectOfType<QuestManager>();
    }

    private void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
    }

    private void OnTriggerExit(Collider other)
    {
         if (other.CompareTag("Player"))
        {
            questManager.GetQuestTalkIndex(qusetId, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questManager.GetQuestTalkIndex(qusetId, false);
        }
    }

}
