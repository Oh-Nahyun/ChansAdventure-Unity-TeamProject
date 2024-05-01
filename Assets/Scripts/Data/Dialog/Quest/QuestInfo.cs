using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestInfo : MonoBehaviour
{
    TextMeshProUGUI qusetName;
    TextMeshProUGUI questContents;
    TextMeshProUGUI questObjectives;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        child = child.GetChild(0);
        qusetName = child.GetComponent<TextMeshProUGUI>();
        child = child.GetChild(1);
        questContents = child.GetComponent<TextMeshProUGUI>();
        child = child.GetChild(2);
        questObjectives = child.GetComponent<TextMeshProUGUI>();
    }

}
