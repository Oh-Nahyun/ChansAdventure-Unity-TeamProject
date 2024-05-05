using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_01_Save : TestBase
{
    // 1. 버튼을 누르면 현재 정보 저장 파일 생성
    int[] SceneDatas;
    //PlayerData[] playerDatas;
    List<PlayerData> playerDatas;

    Player player;

    const int DATA_SIZE = 5;

    public int saveIndex;

    private void Start()
    {
        SceneDatas = new int[DATA_SIZE];
        playerDatas = new List<PlayerData>(DATA_SIZE);

        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        SetDefaultData();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        SavePlayerData();
    }


    // Save Scripts

    void SetDefaultData()
    {
        for(int i = 0; i < DATA_SIZE; i++)
        {
            SceneDatas[i] = 0;
            //playerDatas[i] = new PlayerData(Vector3.zero, Vector3.zero, null);
        }
    }

    void SavePlayerData()
    {
        SaveData data = new SaveData(); // 저장용 클래스 인스턴스 생성
        // 저장용 객체에 데이터 저장
        // Scene 번호 저장
        SceneDatas[saveIndex] = SceneManager.GetActiveScene().buildIndex;
        data.SceneNumber = SceneDatas;

        // Player 정보 저장
        Vector3 curPos = player.gameObject.transform.position;
        Vector3 curRot = player.gameObject.transform.eulerAngles;
        Inventory curInven = player.PlayerInventory;

        data.playerInfos = new List<PlayerData>(DATA_SIZE);         // SaveData 클래스의 LIst 초기화
        SetSaveData(saveIndex, curPos, curRot, curInven);       
        data.playerInfos.Insert(saveIndex, playerDatas[saveIndex]); // SaveData 클래스에 저장

        // save Data file
        string jsonText = JsonUtility.ToJson(data); // json 형식 문자열로 변경
        string path = $"{Application.dataPath}/Save/";
        if(!System.IO.Directory.Exists(path))
        {
            // path 폴더가 없다
            System.IO.Directory.CreateDirectory(path); // 폴더 생성
        }

        string fullPath = $"{path}Save.json";               // 전제 경로 만들기
        System.IO.File.WriteAllText(fullPath, jsonText);    // 파일로 저장

        Debug.Log("Player Data convert complete");
    }

    bool LoadPlayerData()
    {
        return false;
    }

    void UpdateData()
    {

    }

    /// <summary>
    /// 플레이어 데이터를 설정하고 저장하는 함수
    /// </summary>
    /// <param name="dataIndex">데이터 인덱스</param>
    /// <param name="pos">플레이어 위치</param>
    /// <param name="rot">플레이어 회전값 ( 오일러 )</param>
    /// <param name="inven">플레이어 인벤토리</param>
    void SetSaveData(int dataIndex, Vector3 pos, Vector3 rot, Inventory inven)
    {
        PlayerData data = new PlayerData(pos, rot, inven);  // 저장할 플레이어 데이터 생성 
        playerDatas.Insert(dataIndex, data);                // 데이터 저장

        Debug.Log("플레이어 데이터 저장 완료");
    }
}