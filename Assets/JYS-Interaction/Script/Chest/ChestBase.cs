using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBase : NPCBase
{
    Animator animator;

    readonly int IsOpenHash = Animator.StringToHash("Open");

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        isItemChest = true;
        isNPC = false;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        OpenChest(isTalk);
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
