using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBase : MonoBehaviour
{
    Animator animator;

    readonly int IsOpenHash = Animator.StringToHash("Open");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {

        GameManager.Instance.openChase += () =>
        {
            OpenChest();
        };
    }

    public void OpenChest()
    {
        animator.SetBool(IsOpenHash, true);
    }

}
