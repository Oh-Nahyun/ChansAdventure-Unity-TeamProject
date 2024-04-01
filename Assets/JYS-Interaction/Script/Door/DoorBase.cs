using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : NPCBase
{

    readonly int IsOpenHash = Animator.StringToHash("Open");
    public bool open = false;
    public bool isLock = false;

    protected override void Awake()
    {
        base.Awake();
        isNPC = false;
        otherObject = true;
        id = 299;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        OpenDoor(isTalk);
    }

    private void OpenDoor(bool isTalk)
    {
        if (isTalk)
        {
            if (!isLock)
            {
                otherObject = true;
                open = !open;
                Debug.Log($"¹®¿­¸²{open}");
                //animator.SetBool(IsOpenHash, open);
            }
            else
            {
                otherObject = false;
            }
        }
    }



}
