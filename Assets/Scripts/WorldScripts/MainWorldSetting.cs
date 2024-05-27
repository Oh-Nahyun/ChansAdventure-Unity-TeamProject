using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldSetting : MonoBehaviour
{
    Vector3 itemDropVector;
    Vector3 enemySpawnVector;

    private void Awake()
    {
        itemDropVector = new Vector3(16.49f, -11.8f, -7.56f);
        enemySpawnVector = new Vector3(-69.3f, -12.7f, 1.66f);
    }

    private void Start()
    {
        // 아이템 소환
        ItemDataManager dataManager = GameManager.Instance.ItemDataManager;

        Factory.Instance.GetItemObject(dataManager[ItemCode.Coin], itemDropVector);
        Factory.Instance.GetItemObjects(dataManager[ItemCode.Arrow], 10, itemDropVector, true);
    }
}
