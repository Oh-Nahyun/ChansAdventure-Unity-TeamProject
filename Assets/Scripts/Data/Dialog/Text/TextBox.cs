using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;


public class TextBox : MonoBehaviour
{
    public float alphaChangeSpeed = 5.0f;

    TextMeshProUGUI talkText;
    TextMeshProUGUI nameText;
    CanvasGroup canvasGroup;
    Image endImage;
    public GameObject scanObject;
    Animator endImageAnimator;
    WarpBase warpBase;

    TextSelect textSelet;
    Interaction interaction;
    PlayerController controller;

    public string talkString;
    public int talkIndex = 0;
    public float charPerSeconds = 0.05f;

    private bool talkingEnd;
    private bool talking;
    public bool TalkingEnd => talkingEnd;

    private bool typingTalk;
    private bool typingStop;

    public NPCBase NPCdata;

    TextBoxManager textBoxManager; // TextBoxManager에 대한 참조
    QuestManager questManager;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(1);
        talkText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(2);
        nameText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(3);
        endImage = child.GetComponent<Image>();
        endImageAnimator = child.GetComponent<Animator>();

        child = transform.GetChild(4);
        textSelet = child.GetComponent<TextSelect>();

        interaction = FindObjectOfType<Interaction>();

        textBoxManager = FindObjectOfType<TextBoxManager>();
        questManager = FindObjectOfType<QuestManager>();
        warpBase = FindObjectOfType<WarpBase>();

        controller = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Include);
    }

    private void Start()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        endImageAnimator.speed = 0.0f;

        controller.onInteraction += () =>
        {
            if (scanObject != null)
            {
                Action();
            }
        };
    }

    private void Update()
    {
        if (interaction != null)
        {
            scanObject = interaction.scanIbgect; // scanIbgect 값을 가져옴
        }
    }

    /// <summary>
    /// 상호작용 입력시 대상 분별 함수
    /// </summary>
    public void Action()
    {
        
        talkText.text = "";
        nameText.text = "";
        if (scanObject != null)
        {
            NPCdata = scanObject.GetComponent<NPCBase>();
        }
        else
        {
            NPCdata = null;
        }


        if (typingTalk == false && NPCdata != null && !NPCdata.isTextObject && !NPCdata.otherObject)
        {
            endImageAnimator.speed = 0.0f;
            endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 0f);
            StartCoroutine(TalkStart());
        }
        else if (typingTalk == true && NPCdata != null && !NPCdata.isTextObject && !NPCdata.otherObject)
        {
            typingStop = true;
            NPCdata = scanObject.GetComponent<NPCBase>();
            if (!talkingEnd)
            {
                talkIndex--;
            }
            Talk(NPCdata.id);
            if (NPCdata.isNPC)
            {
                nameText.text = $"{NPCdata.nameNPC}";
            }
            endImageAnimator.speed = 1.0f;
            endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 1f);
            typingTalk = false;
        }
        else if (NPCdata != null && NPCdata.otherObject)
        {
            isOtherObject();
        }
        else
        {
            Debug.Log("대상이 없음");
        }
    }

    /// <summary>
    /// 대화 시작, 진행, 종료 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator TalkStart()
    {
        if (!talking && !talkingEnd)
        {

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            Talk(NPCdata.id);

            SetTalkText();

            NPCdata.isTalk = true;

            while (canvasGroup.alpha < 1.0f)
            {
                canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
        }
        else if (talking && !talkingEnd)
        {
            Talk(NPCdata.id);

            SetTalkText();
        }
        else
        {
            talkingEnd = false;
            while (canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            talkText.text = "";
            nameText.text = "";
            talkIndex = 0;
            talking = false;
            NPCdata.isTalk = false;
        }
    }

    /// <summary>
    /// 대사 타이핑 효과 코루틴
    /// </summary>
    /// <param name="text">타이핑 효과를 줄 텍스트</param>
    /// <returns></returns>
    IEnumerator TypingText(string text)
    {
        typingStop = false;
        typingTalk = true;
        talkText.text = null;
        for (int i = 0; i < text.Length; i++)
        {
            if (typingStop)
            {
                talkText.text = $"{talkString}";
                break;
            }
            talkText.text += text[i];
            yield return new WaitForSeconds(charPerSeconds);
            if (i + 2 > text.Length)
            {
                endImageAnimator.speed = 1.0f;
                endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 1f);
                typingTalk = false;
            }
        }
    }

    /// <summary>
    /// 이름 및 대사 출력 함수
    /// </summary>
    void SetTalkText()
    {
        if (talkText != null)
        {
            talkText.text = $"{talkString}";
            if (NPCdata.isNPC)
            {
                nameText.text = $"{NPCdata.nameNPC}";
                StartCoroutine(TypingText(talkText.text));
            }
            else
            {
                endImageAnimator.speed = 1.0f;
                endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 1f);
            }


            if (textBoxManager.GetTalkData(NPCdata.id + 4) != null)
            {
                string buttonText0 = textBoxManager.GetTalkData(NPCdata.id + 4)[0];
                string buttonText1 = textBoxManager.GetTalkData(NPCdata.id + 4)[1];
                string buttonText2 = textBoxManager.GetTalkData(NPCdata.id + 4)[2];

                textSelet.setButtonText(buttonText0, buttonText1, buttonText2);
                textSelet.onSeletStart();

            }
            else
            {
                textSelet.onSeletEnd();
            }


        }
    }

    /// <summary>
    /// 다음 대화 내용 불러오는 함수
    /// </summary>
    /// <param name="id">대화 대상의 ID</param>
    void Talk(int id)
    {
        if ((talkIndex + 1) == textBoxManager.GetTalkData(id).Length)
        {
            talkString = textBoxManager.GetTalkData(id)[talkIndex];
            talkingEnd = true;
            return;
        }
        talkString = textBoxManager.GetTalkData(id)[talkIndex];
        talking = true;
        talkIndex++;
    }

    /// <summary>
    /// 선택지 받아오는 함수
    /// </summary>
    /// <param name="selectId">받아온 선택지</param>
    public void OnSelect(int selectId)
    {
        NPCdata.id += selectId; // 받아온 선택지에 따라 Id값을 증가시켜 다음 대사로 진행
        talkingEnd = false;
        Action();
        textSelet.onSeletEnd();
    }

    /// <summary>
    /// 대사창을 출력하지 않는 오브젝트 처리 함수
    /// </summary>
    void isOtherObject()
    {
        warpBase = scanObject.GetComponent<WarpBase>();
        DoorBase door = scanObject.GetComponent<DoorBase>();
        Lever lever = scanObject.GetComponent<Lever>();
        if (warpBase != null)
        {
            Debug.Log("워프");
            warpBase.WarpToWarpPoint();
        }

        if (door != null)
        {
            door.OpenDoor();
        }

        if (lever != null)
        {
            lever.Use();
        }
    }

}
