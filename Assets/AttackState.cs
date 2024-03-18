using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    float endChasingRange = 3.5f;


    readonly int isAttacking_Hash = Animator.StringToHash("IsAttacking");

    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 태그를 가진 오브젝트 찾기
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(player);

        float distance = Vector3.Distance(player.position, animator.transform.position); // 자신과 플레이어의 거리 구하기
        if (distance > endChasingRange) // 자신과 플레이어의 거리가 일정거리 이상이면
        {
            animator.SetBool(isAttacking_Hash, false); // 달리기 애니메이션 설정
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
