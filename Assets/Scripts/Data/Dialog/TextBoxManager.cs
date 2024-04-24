using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxManager : MonoBehaviour
{
    /// <summary>
    /// 대사 관리를 위한 딕셔너리
    /// </summary>
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    /// <summary>
    /// 대사를 관리하는 함수
    /// </summary>
    void GenerateData()
    {
        talkData.Add(0, new string[] { "초기값" });
        // 물체 오브젝트
        talkData.Add(100, new string[] { "녹색으로 반짝반짝 빛나는 루피" });
        talkData.Add(199, new string[] { "이미 아이템을 획득한 상자이다" });
        talkData.Add(298, new string[] { "문이 열렸다" });
        talkData.Add(299, new string[] { "잠겨있는 문이다" });

        // NPC
        // 노인
        talkData.Add(1000, new string[] { "애국가는 말 그대로 '나라를 사랑하는 노래'를 뜻한다.", "1896년 '독립신문' 창간을 계기로 여러 가지의 애국가 가사가 신문에 게재되기 시작했는데", "이 노래들을 어떤 곡조로 불렀는가는 명확하지 않다.", "다만 대한제국이 서구식 군악대를 조직해 1902년 '대한제국 애국가'라는 이름의 국가를 만들어", " 나라의 주요 행사에 사용했다는 기록은 지금도 남아 있다." });
        talkData.Add(1010, new string[] { "다음대사" });
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

        // 시민
        talkData.Add(2000, new string[] { "가나다라마바사  아자차카타파하  가나다라마바사  아자차카타파하  가나다라마바사  아자차카타파하" });

        // 시민
        talkData.Add(3000, new string[] { "날씨가 참 좋구나..." });

        // 상인
        talkData.Add(4000, new string[] { "어서옵쇼!!" });
        talkData.Add(4010, new string[] { "어서옵쇼!!" });
        talkData.Add(4011, new string[] { "구매"});
        talkData.Add(4012, new string[] { "판매"});
        talkData.Add(4013, new string[] { "안녕히 가십쇼!!" });

        talkData.Add(4014, new string[] { "구매하기", "판매하기","나가기" });

        // 도사
        talkData.Add(5000, new string[] { "....." });
    }

    /// <summary>
    /// 각 id에 해당하는 대화 내용 가져오는 함수
    /// </summary>
    /// <param name="id">해당 오브젝트의 Id 키값</param>
    /// <returns></returns>
    public string[] GetTalkData(int id)
    {
        if (talkData.ContainsKey(id))
            return talkData[id];
        else
        {
            Debug.LogError("해당 ID에 대한 대화 데이터를 찾을 수 없습니다: " + id);
            return null;
        }
    }

}