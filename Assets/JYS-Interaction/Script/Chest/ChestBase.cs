using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBase : NPCBase
{
    Animator animator;

    readonly int IsOpenHash = Animator.StringToHash("Open");

    protected override void Awake()
    {
        isNPC = false;
        base.Awake();
    }

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        base.Start();
    }

    protected override void Update()
    {
        OpenChest(isTalk);
        base.Update();
    }

    private void OpenChest(bool isOpen)
    {
        if (isOpen)
        {
            animator.SetBool(IsOpenHash, true);
            id = 199;
        }
    }

}
