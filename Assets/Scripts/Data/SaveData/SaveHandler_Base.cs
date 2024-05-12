using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 플레이어의 정보를 저장하는 클래스
/// </summary>
public class SaveHandler_Base : MonoBehaviour
{
    // 컴포넌트 ================================================================
    CanvasGroup canvasGroup;

    /// <summary>
    /// 세이브 데이터 슬롯들
    /// </summary>
    protected SaveDataSlot[] saveSlots;

    /// <summary>
    /// 세이브 데이터 슬롯 접근 프로퍼티
    /// </summary>
    public SaveDataSlot[] SaveSlots => saveSlots;

    /// <summary>
    /// 세이브, 로드 확인창
    /// </summary>
    SaveCheckUI saveCheckUI;

    // 슬롯데이터 ===============================================================
    /// <summary>
    /// 씬 데이터
    /// </summary>
    protected int[] SceneDatas;

    /// <summary>
    /// 플레이어 데이터
    /// </summary>
    protected PlayerData[] playerDatas;

    protected Player player;

    /// <summary>
    /// 데이터 최대 사이즈
    /// </summary>
    const int DATA_SIZE = 5;

    /// <summary>
    /// 슬롯을 왼쪽 클릭했을 때 실행하는 델리게이트
    /// </summary>
    public Action<int> onClickSaveSlot;

    /// <summary>
    /// 슬롯을 오른쪽 클릭했을 때 실행하는 델리게이트
    /// </summary>
    public Action<int> onClickLoadSlot;

    protected virtual void Start()
    {
        SceneDatas = new int[DATA_SIZE];
        playerDatas = new PlayerData[DATA_SIZE];
        saveSlots = new SaveDataSlot[DATA_SIZE];

        Transform child = transform.GetChild(0); // slots
        for (int i = 0; i < saveSlots.Length; i++)
        {
            Transform slot = child.GetChild(i);
            saveSlots[i] = slot.GetComponent<SaveDataSlot>();
            saveSlots[i].InitializeComponent();
            SaveSlots[i].SlotInitialize(i);
        }

        RefreshSaveData();

        player = GameManager.Instance.Player;

        // Check UI =====================================
        child = transform.GetChild(1);
        saveCheckUI = child.GetComponent<SaveCheckUI>();

        saveCheckUI.onSave += SavePlayerData;
        saveCheckUI.onLoad += LoadPlayerData;

        onClickSaveSlot += saveCheckUI.ShowSaveCheck;
        onClickLoadSlot += saveCheckUI.ShowLoadCheck;
    }

    void OnEnable()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();        
    }

    void OnDestroy()
    {
        canvasGroup = null;
    }

    /// <summary>
    /// 세이브 데이터를 Default값으로 되돌리는 함수
    /// </summary>
    void SetDefaultData()
    {
        for (int i = 0; i < DATA_SIZE; i++)
        {
            SceneDatas[i] = 0;
            playerDatas[i] = new PlayerData(Vector3.zero, Vector3.zero, null);
        }
    }

    /// <summary>
    /// 저장된 데이터(Json파일)를 불러와서 갱신하는 함수 
    /// </summary>
    void RefreshSaveData()
    {
        // Json 파일 불러오기
        string path = $"{Application.dataPath}/Save/";
        if (System.IO.Directory.Exists(path))   // Save 디렉로티라 존재하면 
        {
            string fullPath = $"{path}Save.json";
            if (System.IO.File.Exists(fullPath))    // json 파일이 존재하면 불러오기
            {
                string json = System.IO.File.ReadAllText(fullPath);

                SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

                SceneDatas = loadedData.SceneNumber;
                playerDatas = loadedData.playerInfos;
            }
        }

        for(int i = 0; i < SaveSlots.Length; i++)
        {
            if (SceneDatas[i] == 0) // 씬 데이터가 없다 == 세이브 데이터가 존재하지 않는다.
            {
                SaveSlots[i].CheckSave(true, SceneDatas[i]);
            }
            else
            {
                SaveSlots[i].CheckSave(false, SceneDatas[i]);
            }
        }
    }

    /// <summary>
    /// 플레이어 데이터를 저장하는 함수
    /// </summary>
    /// <param name="saveIndex">세이브할 슬롯 인덱스</param>
    protected virtual void SavePlayerData(int saveIndex)
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
        if (!System.IO.Directory.Exists(path))
        {
            // path 폴더가 없다
            System.IO.Directory.CreateDirectory(path); // 폴더 생성
        }

        string fullPath = $"{path}Save.json";               // 전제 경로 만들기
        System.IO.File.WriteAllText(fullPath, jsonText);    // 파일로 저장

        RefreshSaveData();
        Debug.Log("Player Data convert complete");
    }


    /// <summary>
    /// 플레이어 데이터 로드
    /// </summary>
    /// <param name="loadIndex">로드할 파일 번호</param>
    /// <returns>로드에 성공했으면 true 아니면 false</returns>
    protected virtual void LoadPlayerData(int loadIndex)
    {
        // 저장한 데이터 불러오기   
        GameManager.Instance.spawnPoint = playerDatas[loadIndex].position; // 플레이어 위치 잡기
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
            }
        }

        //CloseSavePanel();

        // 씬 불러오기
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(SceneDatas[loadIndex])); // 저장한 씬 인덱스로 씬 저장
        GameManager.Instance.ChangeToTargetScene(sceneName, GameManager.Instance.Player.gameObject);
    }

    /// <summary>
    /// 패널을 보여주는 함수
    /// </summary>
    public void ShowSavePanel()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 패널을 숨기는 함수
    /// </summary>
    public void CloseSavePanel()
    {
        if (canvasGroup == null) Debug.Log($"접근하는 캠버스가 NULL입니다");
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
