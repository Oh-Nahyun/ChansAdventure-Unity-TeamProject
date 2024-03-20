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
        // 사거리 잡아서 거리가 가까워지면 플레이어 바라보게 설정
        float distance = Vector3.Distance(player.position, animator.transform.position); // 자신과 플레이어의 거리 구하기
        if(distance > 0.1f)
        {
            animator.transform.LookAt(player);
        }
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
