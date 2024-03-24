using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxItem : MonoBehaviour
{
    public float alphaChangeSpeed = 5.0f;

    TextMeshProUGUI talkText;
    TextMeshProUGUI nameText;
    CanvasGroup canvasGroup;
    Image endImage;
    public GameObject scanObject;
    Animator animator;
    Animator endImageAnimator;

    Interaction interaction;

    public string talkString;
    public int talkIndex = 0;
    public float charPerSeconds = 0.05f;

    private bool talking;

    public NPCBase NPCdata;
    TextBoxManager textBoxManager; // TextBoxManager에 대한 참조

    readonly int IsOnTextBoxItemHash = Animator.StringToHash("OnTextBoxItem");

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();

        Transform child = transform.GetChild(2);
        talkText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(3);
        nameText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(4);
        endImage = child.GetComponent<Image>();
        endImageAnimator = child.GetComponent<Animator>();

        interaction = FindObjectOfType<Interaction>();

        // TextBoxManager에 대한 참조 가져오기
        textBoxManager = FindObjectOfType<TextBoxManager>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        endImageAnimator.speed = 0.0f;

        GameManager.Instance.onTalkNPC += () =>
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

        if (NPCdata != null && NPCdata.isItemChest)
        {
            StartCoroutine(TalkStart());
        }
    }

    IEnumerator TalkStart()
    {
        if (!talking)
        {            
            animator.SetBool(IsOnTextBoxItemHash, true);
         
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            endImageAnimator.speed = 1.0f;
            endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 1f);

            Talk(NPCdata.id);

            nameText.text = $"{NPCdata.nameNPC}";
            talkText.text = $"{talkString}";

            while (canvasGroup.alpha < 1.0f)
            {
                canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }

            NPCdata.isTalk = true;

        }
        else
        {
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
            endImageAnimator.speed = 0.0f;
            endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 0f);
            talking = false;
            NPCdata.isTalk = false;
            NPCdata.isItemChest = false;
            animator.SetBool(IsOnTextBoxItemHash, false);
        }
    }

    void Talk(int id)
    {
        talkString = textBoxManager.GetTalkData(id)[talkIndex];
        talking = true;
    }


}
