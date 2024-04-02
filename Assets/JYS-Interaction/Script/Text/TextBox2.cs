using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBox2 : MonoBehaviour
{
    
    Dictionary<int, string[]> talkData;

    public float alphaChangeSpeed = 5.0f;

    TextMeshProUGUI talkText;
    TextMeshProUGUI nameText;
    CanvasGroup canvasGroup;
    Image endImage;
    public GameObject scanObject;

    TextSelect textSelet;
    Interaction interaction;

    public string talkString;
    public int talkIndex = 0;
    public float charPerSeconds = 0.05f;

    private bool talkingEnd;
    private bool talking;
    private bool typingTalk;
    private bool typingStop;
    public bool onScanObject;

    //private bool isNpc;

    public NPCBase NPCdata;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(1);
        talkText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(2);
        nameText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(3);
        endImage = child.GetComponent<Image>();

        child = transform.GetChild(4);
        textSelet = child.GetComponent<TextSelect>();

        interaction = FindObjectOfType<Interaction>();

        talkData = new Dictionary<int, string[]>();
        StopAllCoroutines();
        GenerateData();
    }

    private void Start()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if(scanObject != null) 
        {
            GameManager_JYS.Instance.onTalkNPC += () =>
            {      
                Action();
            };
        }

        /*
        GameManager_JYS.Instance.onTalkObj += () =>
        {
            //isNpc = false;
            ObjAction();
        };*/
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
        //scanObject = gameObject;
        if (typingTalk == false)
        {
            endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 0f);
            StartCoroutine(TalkStart());
        }
        else
        {
            typingStop = true;
            //StopCoroutine(TypingText(talkText.text));
            NPCdata = scanObject.GetComponent<NPCBase>();
            if (!talkingEnd)
            {
                talkIndex--;
            }
            Talk(NPCdata.id);
            nameText.text = $"{NPCdata.nameNPC}";
            //talkText.text = $"{talkString}";
            endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 1f);
            typingTalk = false;
        }
    }

    public void ObjAction()
    {
        
        talkText.text = "";
        nameText.text = "";
        if (typingTalk == false)
        {
            StartCoroutine(TalkStart());
        }
        else
        {
            typingStop = true;
            NPCdata = scanObject.GetComponent<NPCBase>();
            if (!talkingEnd)
            {
                talkIndex--;
            }
            Talk(NPCdata.id);
            typingTalk = false;
        }
    }

    IEnumerator TalkStart()
    {

        if (!talking && !talkingEnd)
        {
            while (canvasGroup.alpha < 1.0f)
            {
                canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            NPCdata = scanObject.GetComponent<NPCBase>();
            Talk(NPCdata.id);

            nameText.text = $"{NPCdata.nameNPC}";
            talkText.text = $"{talkString}";

            if (NPCdata.selectId)
            {
                textSelet.onSeletStart();
            }
            else
            {
                textSelet.onSeletEnd();
            }


            StartCoroutine(TypingText(talkText.text));

        }
        else if (talking && !talkingEnd) 
        {
            Talk(NPCdata.id);

            nameText.text = $"{NPCdata.nameNPC}";
            talkText.text = $"{talkString}";

            if (NPCdata.selectId)
            {
                textSelet.onSeletStart();
            }
            else
            {
                textSelet.onSeletEnd();
            }

            StartCoroutine(TypingText(talkText.text));
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
            talking = false;
            talkingEnd = false;

        }
    }

    /// <summary>
    /// 텍스트 타이핑 효과
    /// </summary>
    /// <param name="text"></param>
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
                endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 1f);
                typingTalk = false;
            }
        }
    }

    void Talk(int id)
    {
        if ((talkIndex + 1) == talkData[id].Length)
        {
            talkString = talkData[id][talkIndex];
            talkingEnd = true;
            return;
        }
        talkString = talkData[id][talkIndex];
        talking = true;
        talkIndex++;
    }

    void GenerateData()
    {
        talkData.Add(0, new string[] { "초기값" });
        talkData.Add(100, new string[] { "아이템을 획득했다." });
        talkData.Add(110, new string[] { "이미 아이템을 획득한 상자이다." });

        talkData.Add(1000, new string[] { "애국가는 말 그대로 '나라를 사랑하는 노래'를 뜻한다.", "1896년 '독립신문' 창간을 계기로 여러 가지의 애국가 가사가 신문에 게재되기 시작했는데", "이 노래들을 어떤 곡조로 불렀는가는 명확하지 않다.", "다만 대한제국이 서구식 군악대를 조직해 1902년 '대한제국 애국가'라는 이름의 국가를 만들어" ," 나라의 주요 행사에 사용했다는 기록은 지금도 남아 있다." });
        talkData.Add(1010, new string[] { "다음대사"});
        talkData.Add(1011, new string[] { "선택지 11 선택완료", "AAAAA" });
        talkData.Add(1012, new string[] { "선택지 12 선택완료", "BBBBB" });
        talkData.Add(1013, new string[] { "선택지 13 선택완료", "CCCCC" });

        talkData.Add(1020, new string[] { "다다음대사" });
        talkData.Add(1021, new string[] { "선택지 21 선택완료", "AAAAA" });
        talkData.Add(1022, new string[] { "선택지 22 선택완료", "BBBBB" });
        talkData.Add(1023, new string[] { "선택지 23 선택완료", "CCCCC" });

        talkData.Add(1100, new string[] { "선택지 없는 다음대사" });
        talkData.Add(1110, new string[] { "선택지 있는 다다음대사" });
        talkData.Add(1111, new string[] { "선택지 111 선택완료", "AAAAA" });
        talkData.Add(1112, new string[] { "선택지 112 선택완료", "BBBBB" });
        talkData.Add(1113, new string[] { "선택지 113 선택완료", "CCCCC" });
        talkData.Add(1200, new string[] { "선택지 없는 다다음대사" });

        talkData.Add(2000, new string[] { "가나다라마바사  아자차카타파하  가나다라마바사  아자차카타파하  가나다라마바사  아자차카타파하" });
    }

    public void OnSelect(int selectId)
    {
        NPCdata.id += selectId;
        talkingEnd = false;
        Action();
        textSelet.onSeletEnd();
    }


   
} 
