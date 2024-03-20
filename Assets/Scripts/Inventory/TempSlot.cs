using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 임시 슬롯 클래스
/// </summary>
public class TempSlot : InventorySlot
{
    /// <summary>
    /// 설정 안되있으면 설정되는 인덱스 번호
    /// </summary>
    const uint notSet = uint.MaxValue;

    /// <summary>
    /// 가져온 인덱스 저장 변수
    /// </summary>
    uint fromIndex = notSet;

    /// <summary>
    /// 임시 슬롯 생성자
    /// </summary>
    /// <param name="index">인덱스 값</param>
    public TempSlot(uint index) : base(index)
    {
        fromIndex = index;
    }

    /// <summary>
    /// 임시 슬롯 아이템 추가
    /// </summary>
    /// <param name="code">아이템 코드</param>
    /// <param name="count">아이템 개수</param>
    /// <param name="over">사용 안함</param>
    public override void AssignItem(uint code, int count, out int _)
    {
        base.AssignItem(code, count, out _);
    }

    /// <summary>
    /// 임시 슬롯 클리어
    /// </summary>
    public override void ClearItem()
    {
        base.ClearItem();
        fromIndex = notSet;
    }
}
