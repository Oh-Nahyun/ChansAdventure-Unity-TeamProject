using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPlace : MonoBehaviour
{
    public int qusetId;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager.Instance.GetQuestTalkIndex(qusetId, false);
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager.Instance.GetQuestTalkIndex(qusetId, false);
        }
        gameObject.SetActive(false);
    }

}
