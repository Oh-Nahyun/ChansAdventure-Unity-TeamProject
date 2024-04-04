using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Spawn : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            Factory.Instance.GetEnemy();
        }
    }
}
