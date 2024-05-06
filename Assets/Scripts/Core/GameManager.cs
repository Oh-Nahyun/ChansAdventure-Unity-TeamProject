using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player
    {
        get
        {
            if (player == null)
            {
                player = FindAnyObjectByType<Player>();
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
    /// 아이템 데이터 클래스 접근을 하기위한 프로퍼티
    /// </summary>
    public ItemDataManager ItemDataManager => itemDataManager;

    MapManager mapManager;

    /// <summary>
    /// mapManager 접근을 위한 프로퍼티
    /// </summary>
    public MapManager MapManager => mapManager;

    public CameraManager Cam
    {
        get
        {
            if (cameraManager == null)
                cameraManager = GetComponent<CameraManager>();
            return cameraManager;
        }
    }

    /// <summary>
    /// 로딩 중인지 확인하는 bool값
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

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();

        loadPlayerGameObject = new GameObject();
        DontDestroyOnLoad(loadPlayerGameObject);
    }

    protected override void OnInitialize()
    {
        if (isLoading)
            return;

        if(player == null) player = FindAnyObjectByType<Player>();
        weapon = FindAnyObjectByType<Weapon>();
        cameraManager = GetComponent<CameraManager>();
        itemDataManager = GetComponent<ItemDataManager>();
        mapManager = GetComponent<MapManager>();

        itemDataManager.InitializeItemDataUI();

        mapManager.InitalizeMapUI();

        if (savedInventory != null)
        {
            ItemDataManager.InventoryUI.InitializeInventoryUI(savedInventory);  // 플레이어 인벤토리 UI 초기화
            savedInventory = null;                                              // 저장한 인벤토리 데이터 제거
            player.Inventory.SetOwner(player.gameObject);                       // 인벤토리 오너값 초기화
            player.EquipPart = savedEquipParts;                                 // 장착부위 정보 저장
            savedEquipParts = null;                                             // 장착부위 정보 제거
        }
    }

    protected override void OnAdditiveInitiallize()
    {
        SpawnPlayerAfterLoadScene();

        weapon = FindAnyObjectByType<Weapon>();
        cameraManager = GetComponent<CameraManager>();
        itemDataManager = GetComponent<ItemDataManager>();
        mapManager = GetComponent<MapManager>();

        itemDataManager.InitializeItemDataUI();

        mapManager.InitalizeMapUI();
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
    /// 씬 로딩이 끝난 후 플레이어 스폰을 실행하는 함수
    /// </summary>
    public void SpawnPlayerAfterLoadScene()
    {
        if (loadPlayerGameObject.transform.childCount < 1)
            return;

        if (!isLoading)
        {
            loadPlayerGameObject.SetActive(true);
            GameObject loadingPlayer = Instantiate(loadPlayerGameObject.transform.GetChild(0).gameObject);  // 새로운 씬에 플레이어 생성
            loadingPlayer.name = "Player";

            loadingPlayer.transform.position = Vector3.zero;

            Destroy(loadPlayerGameObject.transform.GetChild(0).gameObject); // 저장된 플레이어 오브젝트 제거

            player = loadingPlayer.GetComponent<Player>();  // 플레이어 초기화
            player.GetInventoryData(savedInventory);        // 플레이어 인벤토리 데이터 받기
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
