using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_01_Save : TestBase
{
    // 1. 버튼을 누르면 현재 정보 저장 파일 생성
    // 2. 3번버튼을 누르면 현재 저장된 파일 Load
    // 2.1 인벤토리가 1칸인건 Emtpy 파일
    int[] SceneDatas;
    PlayerData[] playerDatas;

    public Player player;

    /// <summary>
    /// 데이터 최대 사이즈
    /// </summary>
    const int DATA_SIZE = 5;

    /// <summary>
    /// 저장할 세이브 칸 인덱스 번호
    /// </summary>
    public int saveIndex = 0;

    /// <summary>
    /// 로드할 세이브 칸 인덱스번호
    /// </summary>
    public int loadIndex = 0;

    public Transform traget;

    private void Start()
    {
        SceneDatas = new int[DATA_SIZE];
        playerDatas = new PlayerData[DATA_SIZE];

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

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        LoadPlayerData(loadIndex);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Factory.Instance.GetItemObject(GameManager.Instance.ItemDataManager[4]);
        Factory.Instance.GetItemObject(GameManager.Instance.ItemDataManager[8]);
        Factory.Instance.GetItemObjects(GameManager.Instance.ItemDataManager[9], 3, traget.position, true);
    }

    // Save Scripts

    void SetDefaultData()
    {
        for(int i = 0; i < DATA_SIZE; i++)
        {
            SceneDatas[i] = 0;
            playerDatas[i] = new PlayerData(Vector3.zero, Vector3.zero, null);
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
        Inventory curInven = player.Inventory;

        //data.playerInfos = new List<PlayerData>[DATA_SIZE]; // 저장할 데이터 초기화
        data.playerInfos = new PlayerData[DATA_SIZE]; // 저장할 데이터 초기화
        PlayerData playerData = new PlayerData(curPos, curRot, curInven); // 저장할 데이터값
        playerDatas[saveIndex] = playerData;    // 플레이어 데이터값 저장

        // 저장용 클래스 인스턴스에 현재 저장된 값 갱신
        for (int i = 0; i < DATA_SIZE; i++)
        {
            data.playerInfos[i] = playerDatas[i];
        }

        //data.playerInfos[saveIndex].Insert(saveIndex, playerDatas[saveIndex]); // SaveData 클래스에 저장

        // save Data file
        string jsonText = JsonUtility.ToJson(data, true); // json 형식 문자열로 변경
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


    /// <summary>
    /// 플레이어 데이터 로드
    /// </summary>
    /// <param name="loadIndex">로드할 파일 번호</param>
    /// <returns>로드에 성공했으면 true 아니면 false</returns>
    bool LoadPlayerData(int loadIndex)
    {
        bool result = false;

        // Json 파일 불러오기
        string path = $"{Application.dataPath}/Save/";
        if(System.IO.Directory.Exists(path))
        {
            string fullPath = $"{path}Save.json";
            if(System.IO.File.Exists(fullPath))
            {
                string json = System.IO.File.ReadAllText(fullPath);

                SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

                SceneDatas = loadedData.SceneNumber;
                playerDatas = loadedData.playerInfos;

                result = true;
            }
        }

        // 저장한 데이터 불러오기   
        player.transform.position = playerDatas[loadIndex].position;                // 플레이어 위치 잡기
        player.transform.rotation = Quaternion.Euler(playerDatas[loadIndex].rotation);

        Inventory inventory = player.Inventory; // 저장할 플레이어 인벤토리 불러오기
        for (int i = 0; i < inventory.SlotSize; i++)
        {
            if (playerDatas[loadIndex].itemDataClass[i].count == 0) // 아이템 개수가 없으면 무시
            {
                continue;
            }
            else // 아이템이 존재하면 아이템 추가
            {
                uint itemCode = (uint)playerDatas[loadIndex].itemDataClass[i].itemCode; // 아이템 코드
                int itemCount = playerDatas[loadIndex].itemDataClass[i].count;            // 아이템 개수

                player.Inventory.AddSlotItem(itemCode, itemCount, (uint)i);
                //inventory[(uint)i].AssignItem(itemCode, itemCount, out int over);
            }
        }

        // 씬 불러오기
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(SceneDatas[loadIndex])); // 저장한 씬 인덱스로 씬 저장
        GameManager.Instance.ChangeToTargetScene(sceneName, player.gameObject);

        return result;
    }
}