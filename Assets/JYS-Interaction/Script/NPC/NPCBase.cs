using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NPCBase : MonoBehaviour
{
    public TextBoxManager textBoxManager;
    TextMeshPro textViweName;

    public int id = 0;
    public string nameNPC = "";
    public bool selectId = false;
    public bool nextTaklSelect = false;
    public bool isTalk = false;
    public bool isNPC;
    public bool isItemChest;
    public bool isWarp;

    protected virtual void Awake()
    {
        name = nameNPC;
        textBoxManager = FindObjectOfType<TextBoxManager>();
        textViweName = GetComponentInChildren<TextMeshPro>(true);
    }

    protected virtual void Start()
    {
        if (isNPC)
        {
            textViweName.gameObject.SetActive(false);
        }
        GameManager.Instance.onNextTalk += () =>
        {
            TalkNext();
        };
    }

    protected virtual void Update()
    {
        SelectId();
        ViewName();
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

    public void ViewName()
    {
        if (isNPC)
        {
            if (name != null)
            {
                textViweName.text = name;
            }

            Vector3 cameraToNpc = transform.position - Camera.main.transform.position;

            float angle = Vector3.Angle(transform.forward, cameraToNpc);
            if (angle > 90.0f)
            {
                textViweName.transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
            }
            else
            {
                textViweName.transform.rotation = transform.rotation;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textViweName.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textViweName.gameObject.SetActive(false);
        }
    }
}
