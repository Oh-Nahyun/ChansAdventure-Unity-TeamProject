using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(0, new string[] { "초기값" });
        talkData.Add(1000, new string[] { "애국가(愛國歌)는 말 그대로 '나라를 사랑하는 노래'를 뜻한다.", "1896년 '독립신문' 창간을 계기로 여러 가지의 애국가 가사가 신문에 게재되기 시작했는데", "이 노래들을 어떤 곡조로 불렀는가는 명확하지 않다.", "다만 대한제국(大韓帝國)이 서구식 군악대를 조직해 1902년 '대한제국 애국가'라는 이름의 국가를 만들어 나라의 주요 행사에 사용했다는 기록은 지금도 남아 있다." });
        talkData.Add(2000, new string[] { "가나다라마바사  아자차카타파하  가나다라마바사  아자차카타파하  가나다라마바사  아자차카타파하" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        return talkData[id][talkIndex];
    }

}
