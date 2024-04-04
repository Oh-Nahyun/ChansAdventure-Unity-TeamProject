using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    readonly int isPatrolling_Hash = Animator.StringToHash("IsPatrolling");
    readonly int isChasing_Hash = Animator.StringToHash("IsChasing");

    float timer;

    float startChasingRange = 8.0f;

    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0.0f;
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 태그를 가진 오브젝트 찾기
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > 5) // 시간 지나고 걷는 애니메이션 실행
        {
            animator.SetBool(isPatrolling_Hash, true);
        }

        float distance = Vector3.Distance(player.position, animator.transform.position); // 자신과 플레이어의 거리 구하기
        if(distance < startChasingRange) // 자신과 플레이어의 거리가 일정거리 이하이면
        {
            animator.SetBool(isChasing_Hash, true); // 달리기 애니메이션 실행
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
