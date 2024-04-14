

using UnityEngine;

public interface ILifter
{
    /// <summary>
    /// 물체가 들릴 위치
    /// (생명체의 경우 손)
    /// </summary>
    Transform Hand { get; }
    /// <summary>
    /// 물체 들기
    /// </summary>
    void PickUp(ReactionObject pickUpObject);

    /// <summary>
    /// 물체 내려놓기
    /// </summary>
    void Drop();
}
