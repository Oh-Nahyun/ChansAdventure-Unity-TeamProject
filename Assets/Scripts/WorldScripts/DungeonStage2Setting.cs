using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuestData;

public class DungeonStage2Setting : MonoBehaviour
{
    [Header("Room's Flag")]
    public bool isClear_Room1 = false;
    public bool isClear_Room2 = false;
    public bool isClear_Room3 = false;
    public bool isClear_Room4 = false;

    [Header("Room1")]
    public CinemachineVirtualCamera vcam_room1;
    public GameObject LeverObjectGroup_Room1;
    public GameObject[] Ballons;
    public int ballonsCount = 0;

    [Header("Room2")]
    public DungeonStage2_RoomTrigger Room2TriggerObject;

    [Header("Room3")]
    public CinemachineVirtualCamera vcam_room3;
    public GameObject LeverObjectGroup_Room3;
    public DungeonStage2_Scale ScaleTriggerObject;

    [Header("Room4")]
    public DungeonStage2_RoomTrigger Room4TriggerObject;
    public GameObject ExitPortal;

    private void Awake()
    {
        // room1
        LeverObjectGroup_Room1.transform.GetChild(0).gameObject.SetActive(false);

        for(int i = 0; i < Ballons.Length; i++)
        {
            Ballons[i].GetComponent<Balloon>().onExplosion = BallonExplosion;
        }

        // room3
        LeverObjectGroup_Room3.transform.GetChild(0).gameObject.SetActive(false);
        ScaleTriggerObject.gameObject.SetActive(false);
        ScaleTriggerObject.onTriggerEnter = OnTriggerScale;
        Room2TriggerObject.onTriggerEnter = OnTriggerEnterRoom2;

        // room4
        Room4TriggerObject.onTriggerEnter = OnTriggerEnterRoom4;
        ExitPortal.SetActive(false);
    }

    private void OnTriggerEnterRoom4()
    {
        isClear_Room4 = true;
        ExitPortal.SetActive(true);
        QuestManager.Instance.GetQuestTalkIndex((int)QuestType.ClearDungeon2 * 10, true, true);
        QuestManager.Instance.checkClearQuests[(int)QuestType.ClearDungeon2] = true;
    }

    private void LateUpdate()
    {
        CheckRoom1();
        CheckRoom2();
        CheckRoom3();
        CheckRoom4();
    }

    // room check fucntions ===========================================

    void CheckRoom1()
    {

    }

    void CheckRoom2()
    {

    }

    void CheckRoom3()
    {
        if(isClear_Room1)
            ScaleTriggerObject.gameObject.SetActive(true);
    }

    void CheckRoom4()
    {

    }

    // trigger functions ===========================================

    /// <summary>
    /// 풍선이 폭발할 때 실행하는 함수
    /// </summary>
    private void BallonExplosion()
    {
        ballonsCount++;

        if (ballonsCount == Ballons.Length && !isClear_Room1)
        {
            LeverObjectGroup_Room1.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(ActiveRoom1Camera(vcam_room1));
            isClear_Room1 = true;
        }
    }
    
    /// <summary>
    /// 첫번째 방 카메라 활성화 코루틴
    /// </summary>
    IEnumerator ActiveRoom1Camera(CinemachineVirtualCamera vcam)
    {
        float timeElapsed = 0f;
        while(timeElapsed < 2.5f)
        {
            timeElapsed += Time.deltaTime;
            vcam.m_Priority = 100;
            yield return null;
        }

        vcam.Priority = 0;
    }

    /// <summary>
    /// 저울이 일정이상 위치에 도달 했을 때 실행하는 함수
    /// </summary>
    void OnTriggerScale()
    {
        LeverObjectGroup_Room3.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(ActiveRoom1Camera(vcam_room3));
        isClear_Room3 = true;
    }

    /// <summary>
    /// 두번째 방에 벗어날 때 실행되는 함수
    /// </summary>
    void OnTriggerEnterRoom2()
    {
        isClear_Room2 = true;
    }
}