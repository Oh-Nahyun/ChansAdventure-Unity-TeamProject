using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCBase : MonoBehaviour
{
    GameManager gameManager;
    protected TextBoxManager textBoxManager;
    protected QuestManager questManager;
    protected TextBox textbox;
    TextMeshPro textViweName;
    protected readonly int Talk_Hash = Animator.StringToHash("IsTalk");

    /// <summary>
    /// 퀘스트 수락을 알리는 델리게이트
    /// </summary>
    public Action onQuestAccept;

    /// <summary>
    /// 퀘스트 완료를 알리는 델리게이트
    /// </summary>
    public Action onQuestCompleted;

    protected Inventory inventory;
    protected TextBoxItem boxItem;
    private QuestInfoPanel questInfoPanel;
    private HeartCheckUI heartCheckUI;


    public int id = 0;
    public string nameNPC = "";
    public bool selectId = false;
    protected bool nextTaklSelect = false;
    public bool isTalk = false;
    public bool isNPC;
    public bool isTextObject;
    public bool otherObject;
    protected Animator animator;

    protected virtual void Awake()
    { 
        textViweName = GetComponentInChildren<TextMeshPro>(true);
        questInfoPanel = FindObjectOfType<QuestInfoPanel>();
        textbox = FindObjectOfType<TextBox>();
        boxItem = FindAnyObjectByType<TextBoxItem>();
        heartCheckUI = FindAnyObjectByType<HeartCheckUI>();
    }

    protected virtual void Start()
    {
        gameManager = GameManager.Instance;

        textBoxManager = FindObjectOfType<TextBoxManager>();
        questManager = FindObjectOfType<QuestManager>();

        if (isNPC)
        {
            textViweName.gameObject.SetActive(false);
            StartCoroutine(ViewName());
        }
        // questInfoPanel.QuestClearId += (id) => IsQusetClear(id);

        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        SelectId();
        if (isNPC)
        {
            SetAnimation();
        }
        TalkData();
    }

    void SetAnimation()
    {
        animator.SetBool(Talk_Hash, isTalk);
    }

    /// <summary>
    /// 다음 id의 대사를 가져오는 함수
    /// </summary>
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

    /// <summary>
    /// 각 선택지의 해당하는 다음 대사를 가져오는 함수
    /// </summary>
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

    /// <summary>
    /// 오브젝트 위에 이름을 표기하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator ViewName()
    {
        if (isNPC)
        {
            if (name != null)
            {
                textViweName.text = nameNPC;

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
                yield return null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(textViweName != null)
            {
                textViweName.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (textViweName != null)
            {
                textViweName.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 각종 대화를 관리해줄 함수
    /// </summary>
    private void TalkData()
    {
        if (id == 5000)
        {
            if (isTalk)
            {
                questManager.GetQuestTalkIndex(30, true);
                heartCheckUI.PlusHeart();
                id = 5001;
            }
        }
    }
}
