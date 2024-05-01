using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempTest : TestBase
{
    public Transform test;
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //Debug.Log($"로컬좌표: {test.localPosition} 로컬좌표변환: {test.TransformPoint(new Vector3(0,0,0))} / 월드좌표: {test.position}");
        Debug.Log($"월드좌표: {test.position} 월드좌표변환: {test.InverseTransformPoint(new Vector3(0,0,0))} / 로컬좌표: {test.localPosition}");
    }
}
