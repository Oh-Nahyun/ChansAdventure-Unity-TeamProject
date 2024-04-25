using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;

    Weapon weapon;
    public Weapon Weapon => weapon;

    CameraManager cameraManager;

    // ItemData
    ItemDataManager itemDataManager;

    /// <summary>
    /// 아이템 데이터 클래스 접근을 위한 프로퍼티
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

    public bool isLoading;

    string targetSceneName = null;

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

    public GameObject loadPlayerGameObject;

    /// <summary>
    /// 씬을 변경할 때 실행하는 함수
    /// </summary>
    /// <param name="SceneName"> 변경할 씬 이름</param>
    public void ChangeToTargetScene(string SceneName, GameObject player)
    {
        Instantiate(player, loadPlayerGameObject.transform);
        loadPlayerGameObject.SetActive(false);

        TargetSceneName = SceneName;
    }

    public void SpawnPlayerAfterLoadScene()
    {
        if (loadPlayerGameObject.transform.childCount < 1)
            return;

        if(!isLoading)
        {
            loadPlayerGameObject.SetActive(true);
            GameObject player = Instantiate(loadPlayerGameObject.transform.GetChild(0).gameObject);
            Debug.Log($"{player.gameObject.name}");
            Destroy(loadPlayerGameObject);
        }
    }

    void ChangeToLoadingScene()
    {
        SceneManager.LoadScene("02_LoadingScene");
        isLoading = true;
    }

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();

        loadPlayerGameObject = new GameObject();
        DontDestroyOnLoad(loadPlayerGameObject);
    }

    protected override void OnInitialize()
    {
        SpawnPlayerAfterLoadScene();

        player = FindAnyObjectByType<Player>();
        weapon = FindAnyObjectByType<Weapon>();
        cameraManager = GetComponent<CameraManager>();
        itemDataManager = GetComponent<ItemDataManager>();
        mapManager = GetComponent<MapManager>();

        itemDataManager.InitializeItemDataUI();

        mapManager.InitalizeMapUI();
    }

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
