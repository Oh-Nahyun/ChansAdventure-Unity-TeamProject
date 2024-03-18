using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : MonoBehaviour
{
    public int id = 0;
    public int idNext = 0;
    public string nameNPC = "";

    private void Awake()
    {
        name = nameNPC;
    }

    private void Start()
    {
        GameManager.Instance.onNextTalk += () =>
        {
            TalkNext();
            id = id + idNext;
        };
    }

    public void TalkNext()
    {
        idNext += 10;
    }

}
