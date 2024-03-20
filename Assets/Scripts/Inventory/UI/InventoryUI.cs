using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;

    void Awake()
    {
        inventory = GetComponent<Inventory>();    
    }

    void Start()
    {
        inventory = new Inventory();
    }
}
