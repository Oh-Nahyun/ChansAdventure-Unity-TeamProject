using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCBase : MonoBehaviour
{
    public TextBoxManager textBoxManager;

    public int id = 0;
    public string nameNPC = "";
    public bool selectId = false;
    public bool nextTaklSelect = false;
    public bool isTalk = false;
    public bool isNPC;


    protected virtual void Awake()
    {
        name = nameNPC;
        textBoxManager = FindObjectOfType<TextBoxManager>();
    }

    protected virtual void Start()
    {
        GameManager.Instance.onNextTalk += () =>
        {
            TalkNext();
        };
    }

    protected virtual void Update()
    {
        SelectId();
    }

    public void TalkNext()
    {
        int ones = id % 10; // 1의 자리
        int tens = (id / 10) % 10; // 10의 자리

        if (ones != 0)
        {
            id = id / 10;
            id = id * 10;
        }

        if (nextTaklSelect)
        {
            id = id + 10;
        }
        else
        {
            if (tens != 0)
            {
                id = id / 100;
                id = id * 100;
            }
            id = id + 100;
        }
    }

    public void SelectId()
    {
        int tens = (id / 10) % 10; // 10의 자리
        int ones = id % 10; // 1의 자리
        if (tens != 0 && ones == 0)
        {
            selectId = true;
        }
        else
        {
            selectId = false;
        }
    }

}
