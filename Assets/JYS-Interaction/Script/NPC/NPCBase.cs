using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : MonoBehaviour
{
    public int id = 0;
    public string nameNPC = "";

    private void Awake()
    {
        name = nameNPC;
    }
}
