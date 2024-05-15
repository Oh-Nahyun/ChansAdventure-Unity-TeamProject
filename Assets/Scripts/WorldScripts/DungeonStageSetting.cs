using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonStageSetting : MonoBehaviour
{
    public NPCBase dungeonNPC;

    public GameObject exitZone;

    public ParticleSystem[] winEffect;

    private void Start()
    {
        foreach (var item in winEffect)
        {
            item.Stop();
        }
    }

    private void Update()
    {
        if(dungeonNPC.id == 5001) // NPC의 대화를 맞췄으면 포탈 활성화
        {
            exitZone.SetActive(true);
            
            foreach(var item in winEffect)
            {
                item.Play();
            }
        }
    }
}