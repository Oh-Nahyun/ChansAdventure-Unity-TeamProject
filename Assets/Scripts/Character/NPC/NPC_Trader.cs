using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Trader : NPCBase
{
    ShopInfo shop;
    TextBox textBox;

    public string selectButtonText1_1;
    public string selectButtonText1_2;
    public string selectButtonText1_3;

    protected override void Awake()
    {
        shop = FindAnyObjectByType<ShopInfo>();
        textBox = FindAnyObjectByType<TextBox>();

        base.Awake();
        isNPC = true;    
    }

    protected override void Start()
    {
        base.Start();
        //shop.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        openShopinfo();
    }

    /// <summary>
    /// 상인과 대화시 실행될 함수
    /// </summary>
    public void openShopinfo()
    {
        if(id == 4011)
        {
            // 구매 열기
            shop.gameObject.SetActive(true);
            shop.CanvasGroup.alpha = 1;
            if (!textBox.TalkingEnd)
            {
                id = 4010;
            }
        }else if (id == 4012)
        {
            // 판매 열기
            if (!textBox.TalkingEnd)
            {
                id = 4010;
            }
        }
        else
        {
            // 나가기
            if (!textBox.TalkingEnd)
            {
                id = 4010;
            }
        }
    }


}
