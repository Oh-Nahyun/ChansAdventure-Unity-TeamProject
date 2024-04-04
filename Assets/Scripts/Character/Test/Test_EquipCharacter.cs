using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_EquipCharacter : MonoBehaviour, IEquipTarget
{
    public Transform equipmentPostion_R;
    GameObject Equip_R;

    InventorySlot[] equipPart;

    public InventorySlot[] EquipPart
    {
        get => equipPart;        
    }

    void Awake()
    {
        equipPart = new InventorySlot[2];
    }

    public void CharacterEquipItem(GameObject equipment)
    {
        Equip_R = Instantiate(equipment, equipmentPostion_R);
    }

    public void CharacterUnequipItem()
    {
        Destroy(Equip_R);        
    }
}
