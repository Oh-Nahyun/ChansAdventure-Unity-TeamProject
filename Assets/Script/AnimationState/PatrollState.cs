using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollState : StateMachineBehaviour
{
    readonly int isPatrolling_Hash = Animator.StringToHash("IsPatrolling");
    readonly int isChasing_Hash = Animator.StringToHash("IsChasing");

    float timer;

    float startChasingRange = 8.0f;

    List<Transform> wayPoints = new List<Transform>();

    NavMeshAgent agent;
    Transform player;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 태그를 가진 오브젝트 찾기
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = 1.5f;

        timer = 0.0f;
        GameObject go = GameObject.FindGameObjectWithTag("WayPoints");

        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t); // 웨이포인트들 리스트에 저장
        }

        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position); // 다음 웨이포인트 설정
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position); // 다음 웨이포인트에 가까워지면 그 다음 웨이포인트 설정
        }

        timer += Time.deltaTime;
        if (timer > 10)
        {
            animator.SetBool(isPatrolling_Hash, false); // 일정 시간 후에 걷는 애니메이션 정지
        }
        float distance = Vector3.Distance(player.position, animator.transform.position); // 자신과 플레이어 태그를 가진 오브젝트 찾기
        if (distance < startChasingRange)
        {
            // // 자신과 플레이어의 거리가 일정거리 이하이면
            animator.SetBool(isChasing_Hash, true); // 달리기 애니메이션 실행
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position); // 웨이포인트 설정
    }

}
