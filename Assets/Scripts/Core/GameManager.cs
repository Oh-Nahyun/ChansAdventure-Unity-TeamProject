using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// 게임 상태 enum
/// </summary>
public enum GameState
{
    NotStart = 0,
    Started = 1,
}


[RequireComponent(typeof(CameraManager))]
[RequireComponent(typeof(ItemDataManager))]
[RequireComponent(typeof(MapManager))]
[RequireComponent(typeof(SkillManager))]
public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 현재 게임 상태
    /// </summary>
    public GameState gameState = GameState.NotStart;

    /// <summary>
    /// 현재 게임 상태 접근 프로퍼티
    /// </summary>
    public GameState CurrnetGameState
    {
        get => gameState;
        set
        {
            if (gameState != value)
            {
                gameState = value;
            }
        }
    }

    Player player;
    
    public Player Player
    {
        get
        {
            if (player == null)
            {
                player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);
            }
            return player;
        }
    }

    Weapon weapon;
    public Weapon Weapon => weapon;

    CameraManager cameraManager;

    // ItemData
    ItemDataManager itemDataManager;

    /// <summary>
    /// 아이템 데이터 매니저에 접근하는 프로퍼티
    /// </summary>
    public ItemDataManager ItemDataManager => itemDataManager;

    MapManager mapManager;

    /// <summary>
    /// mapManager에 접근하는 프로퍼티
    /// </summary>
    public MapManager MapManager => mapManager;

    /// <summary>
    /// questManager ������ ���� ������Ƽ
    /// </summary>
    QuestManager questManager;
    public QuestManager QuestManager => questManager;

    TextBoxManager textBoxManager;
    public TextBoxManager TextBoxManager => textBoxManager;

    public CameraManager Cam
    {
        get
        {
            if (cameraManager == null)
                cameraManager = GetComponent<CameraManager>();
            return cameraManager;
        }
    }

    SkillManager skillManager;
    public SkillManager Skill => skillManager;

    public List<int> clearedQuests = new List<int>();

    /// <summary>
    /// 스폰 위지 트랜스폼
    /// </summary>
    public Vector3 spawnPoint = Vector3.zero;

    /// <summary>
    /// 로딩하는 중인지 확인하는 bool값
    /// </summary>
    public bool isLoading;

    /// <summary>
    /// 이동할 씬의 이름
    /// </summary>
    string targetSceneName = null;

    /// <summary>
    /// 이동할 씬의 이름을 접근 및 수정하기 위한 프로퍼티 ( 이름이 바뀌면 해당 씬이 TragetScene이 되고 로딩씬을 호출한다. )
    /// </summary>
    public string TargetSceneName
    {
        get => targetSceneName;
        set
        {
            if(targetSceneName != value)
            {
                targetSceneName = value;
                ChangeToLoadingScene();
            }
            else
            {
                ChangeToLoadingScene();
            }
        }
    }

    /// <summary>
    /// 플레이어 오브젝트를 저장할 파괴 불가능한 오브젝트 ( 씬 이동용 )
    /// </summary>
    public GameObject loadPlayerGameObject;

    /// <summary>
    /// 플레이어 저장용 인벤토리 클래스
    /// </summary>
    Inventory savedInventory;

    InventorySlot[] savedEquipParts;

    /// <summary>
    /// 플레이어 프리팹
    /// </summary>
    public GameObject playerPrefab;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();

        loadPlayerGameObject = new GameObject();
        DontDestroyOnLoad(loadPlayerGameObject);

        GameObject playerObj = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        player = playerObj.GetComponent<Player>();

        cameraManager = GetComponent<CameraManager>();
        itemDataManager = GetComponent<ItemDataManager>();
        mapManager = GetComponent<MapManager>();
    }

    protected override void OnInitialize()
    {
        if (gameState == GameState.NotStart)
            return;

        if (isLoading) // 로딩중일 때 실행
        {
            OnLoadInitiallize();
            return;
        }

        if (player == null) player = FindAnyObjectByType<Player>();
        weapon = FindAnyObjectByType<Weapon>();
        cameraManager = GetComponent<CameraManager>();
        itemDataManager = GetComponent<ItemDataManager>();
        mapManager = GetComponent<MapManager>();
        skillManager = GetComponent<SkillManager>();
        skillManager.Initialize();

        itemDataManager.InitializeItemDataUI();

        mapManager.InitalizeMapUI();
        questManager = FindAnyObjectByType<QuestManager>();
        textBoxManager = FindAnyObjectByType<TextBoxManager>();
        
        SpawnPlayerAfterLoadScene();
    }

    #region Loading Function
    /// <summary>
    /// 씬을 변경할 때 실행하는 함수
    /// </summary>
    /// <param name="SceneName"> 변경할 씬 이름</param>
    public void ChangeToTargetScene(string SceneName, GameObject playerObject)
    {
        GameObject obj = Instantiate(playerObject, loadPlayerGameObject.transform); // 플레이어를 로딩 오브젝트에 복제
        obj.transform.position = Vector3.zero;                                      // 오브젝트 위치 초기화
        savedInventory = playerObject.GetComponent<Player>().Inventory;             // 인벤토리 저장
        savedEquipParts = playerObject.GetComponent<Player>().EquipPart;            // 장착부위 정보 저장

        loadPlayerGameObject.SetActive(false);

        TargetSceneName = SceneName;
    }

    /// <summary>
    /// 로딩할 때 실행하는 초기화 함수
    /// </summary>
    protected void OnLoadInitiallize()
    {
        weapon = FindAnyObjectByType<Weapon>();
        cameraManager = GetComponent<CameraManager>();
        itemDataManager = GetComponent<ItemDataManager>();
        mapManager = GetComponent<MapManager>();

        itemDataManager.InitializeItemDataUI();

        mapManager.InitalizeMapUI();
    }

    /// <summary>
    /// 씬 로딩이 끝난 후 플레이어 스폰을 실행하는 함수
    /// </summary>
    public void SpawnPlayerAfterLoadScene()
    {
        if (loadPlayerGameObject.transform.childCount < 1)
            return;

        if (!isLoading)
        {
            loadPlayerGameObject.SetActive(true);
            GameObject loadingPlayer = Instantiate(loadPlayerGameObject.transform.GetChild(0).gameObject,
                                                    spawnPoint,
                                                    Quaternion.identity);   // 새로운 씬에 플레이어 생성
            loadingPlayer.name = "Player";

            //loadingPlayer.transform.position = Vector3.zero;
            loadingPlayer.SetActive(true);

            Destroy(loadPlayerGameObject.transform.GetChild(0).gameObject); // 저장된 플레이어 오브젝트 제거

            player = loadingPlayer.GetComponent<Player>();  // 플레이어 초기화
            player.GetInventoryData(savedInventory);        // 플레이어 인벤토리 데이터 받기

            itemDataManager.InitializeItemDataUI();         // 아이템 데이터 매니저 초기화 후

            player.Inventory.SetOwner(player.gameObject);   // 인벤토리 주인 갱신
            ItemDataManager.InventoryUI.InitializeInventoryUI(player.Inventory); // 로딩 후 인벤 UI 초기화

            mapManager.InitalizeMapUI();                    // 맵 매니저 초기화
        }
    }

    /// <summary>
    /// 맵을 이동할 때 호출되는 함수 ( 로딩씬으로 이동 )
    /// </summary>
    void ChangeToLoadingScene()
    {
        SceneManager.LoadScene("02_LoadingScene");
        isLoading = true;
    }
    #endregion

#if UNITY_EDITOR
    public bool isNPC = false;
    public Action onTalkNPC;
    public Action onTalkObj;
    public void StartTalk()
    {
        //onTalk?.Invoke();

        if (!isNPC)
        {
            onTalkNPC?.Invoke();
            Debug.Log("상호작용 키 누름");
        }
        else
        {
            onTalkObj?.Invoke();
            Debug.Log("오브젝트와 대화");
        }
    }

    public Action onNextTalk;
    public void NextTalk()
    {
        onNextTalk?.Invoke();
    }

    public void IsNPCObj()
    {
        isNPC = !isNPC;
    }

    public Action openChase;
    public void OpenChest()
    {
        openChase?.Invoke();
    }
#endif
}