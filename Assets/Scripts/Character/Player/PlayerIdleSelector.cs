using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleSelector : StateMachineBehaviour
{
    public int idleCount = 4;
    public float basicIdlePercentage = 0.8f;
    readonly int Hash_SelctorIndex = Animator.StringToHash("IdleIndex");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float random = Random.value;
        int index = 0;
        if (random > basicIdlePercentage)
        {
            index = Random.Range(1, idleCount);
        }
        animator.SetInteger(Hash_SelctorIndex, index);
    }
}
