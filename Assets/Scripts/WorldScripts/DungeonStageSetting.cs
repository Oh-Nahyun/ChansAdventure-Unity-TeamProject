using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonStageSetting : MonoBehaviour
{
    public Transform dropposition;
    void Start()
    {
        Factory.Instance.GetItemObject(GameManager.Instance.ItemDataManager[4], dropposition.position);
        Factory.Instance.GetItemObject(GameManager.Instance.ItemDataManager[8], dropposition.position);
        Factory.Instance.GetItemObjects(GameManager.Instance.ItemDataManager[9], 5, dropposition.position);
    }
}
