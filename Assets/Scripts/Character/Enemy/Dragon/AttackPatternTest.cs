using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatternTest : MonoBehaviour
{
    // BasicAttack, HornAttack, ClawAttack 애니메이션 이름
    // AttackBasic, nAttackHor, AttackClaw 이름의 Trigger형으로 각각 연결되어 있음
    // // BasicAttack, HornAttack, ClawAttack 애니메이션 시간 각각 1.2f, 2.167f, 3.333f

    // 기본적으로는 3개 모션을 1개만 랜덤재생
    // 확률로 연속 공격
    // 연속 3번 or 2번 결정해야됨

    // 각각 25퍼 패턴(4개) 6.25퍼
    // 각각 20퍼 패턴(4개) 10퍼
    // 각각 25퍼 패턴(3개) 약8.3퍼
    // 각각 20퍼 패턴(3개) 약13.3퍼

    // 2번
    // 물기 박치기
    // 박치기 휘두르기
    // 물기 휘두르기
    // 더 추가 하던가 말던가

    // 3번
    // 물기 물기 휘두르기
    // 박치기 박치기 휘두르기
    // 박치기 물기 휘두르기
    // 더 추가 하던가 말던가

    // 스위치문으로 만들기
    // 단일 공격은 그냥 애니메이터 사용
    // 연속 공격은 코루틴으로 사용?
    // 아니면 리스트에 각각 설정하고 각각의 공격 함수를 생성 후 패턴 조합으로 사용할 코루틴을 생성
}
