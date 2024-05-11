using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAttackSelector : StateMachineBehaviour
{
    /// <summary>
    /// Idle 모드
    /// </summary>
    int preSelect = 0;

    // 애니메이션용 해시값
    public int AttackModeHash = Animator.StringToHash("AttackMode");

    // 무기
    Weapon weapon;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (weapon == null)
        {
            weapon = GameManager.Instance.Weapon;
        }

        if (weapon.CheckWeaponMode() == 1) // 무기 모드가 칼일 때
        {
            animator.SetInteger(AttackModeHash, RandomSelect());
        }
    }

    /// <summary>
    /// 랜덤으로 Attack 애니메이션을 골라주는 함수
    /// </summary>
    /// <returns>select(0 ~ 1)</returns>
    int RandomSelect()
    {
        int select = 0;         // 80%

        if (preSelect == 0)
        {
            float num = Random.value;

            if (num < 0.20f)
            {
                select = 1;     // 20%
            }
        }

        preSelect = select;
        return select;
    }
}
