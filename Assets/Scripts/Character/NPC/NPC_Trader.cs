using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Trader : NPCBase
{
    ShopInfo shop;

    public string selectButtonText1_1;
    public string selectButtonText1_2;
    public string selectButtonText1_3;

    protected override void Awake()
    {
        shop = FindAnyObjectByType<ShopInfo>();

        base.Awake();
        isNPC = true;    
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        openShopinfo();
    }

    public void openShopinfo()
    {
        if(id == 4011)
        {
            shop.gameObject.SetActive(true);
            id = 4010;
        }
        else
        {
            id = 4010;
        }
    }


}
