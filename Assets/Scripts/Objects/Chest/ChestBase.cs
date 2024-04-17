using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBase : NPCBase
{
    readonly int IsOpenHash = Animator.StringToHash("Open");
    public ParticleSystem lightParticle;

    // Scriptable Object를 저장할 필드
    public ItemData scriptableObject;

    /// <summary>
    /// 상자안의 아이템 코드
    /// </summary>
    [Tooltip("아이템 코드 입력")]
    public uint itemCode;

    /// <summary>
    /// 상자안의 아이템 개수
    /// </summary>
    [Tooltip("아이템 개수 입력")]
    [Range(1, 10)]
    public int itemCount = 1;

    protected override void Awake()
    {
        base.Awake();
        isTextObject = true;
        isNPC = false;
        itemCode = (uint)scriptableObject.itemCode;
    }

    protected override void Start()
    {
        base.Start();
        lightParticle = GetComponentInChildren<ParticleSystem>();
    }

    protected override void Update()
    {
        OpenChest(isTalk);
    }

    /// <summary>
    /// 상자를 열었을 때 처리하는 함수
    /// </summary>
    /// <param name="isOpen">상자 상태</param>
    private void OpenChest(bool isOpen)
    {
        if (isOpen)
        {
            animator.SetBool(IsOpenHash, true);
            id = 199;
        }
    }


}