using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatternTest : MonoBehaviour
{
    // 기본적으로는 3개 모션을 1개만 랜덤재생
    // 확률로 연속 공격
    // 연속 3번 or 2번 결정해야됨

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

    // 
    // 스위치문으로 만들기
    // 단일 공격은 그냥 애니메이터 사용
    // 연속 공격은 코루틴으로 사용?
}
