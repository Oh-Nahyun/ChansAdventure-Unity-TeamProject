using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleSelector : StateMachineBehaviour
{
    /// <summary>
    /// Idle 모드
    /// </summary>
    int preSelect = 0;

    // 애니메이션용 해시값
    readonly int IdleModeHash = Animator.StringToHash("IdleMode");

    // 플레이어
    Player player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
        {
            player = GameManager.Instance.Player;
        }

        if (player.currentSpeed == 0.0f) // 정지하고 있을 때
        {
            animator.SetInteger(IdleModeHash, RandomSelect());
        }
    }

    /// <summary>
    /// 랜덤으로 Idle 애니메이션을 골라주는 함수
    /// </summary>
    /// <returns>select(0 ~ 3)</returns>
    int RandomSelect()
    {
        int select = 0;         // 70%

        if (preSelect == 0)
        {
            float num = Random.value;

            if (num < 0.10f)
            {
                select = 3;     // 10%
            }
            else if (num < 0.20f)
            {
                select = 2;     // 10%
            }
            else if (num < 0.30f)
            {
                select = 1;     // 10%
            }
        }

        preSelect = select;
        return select;
    }
}
