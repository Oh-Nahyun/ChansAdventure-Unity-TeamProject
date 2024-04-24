using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Map UI의 각종 값을 다루는 Manager 클래스 ( 위치값 맵의 왼쪽 밑에 고정 )
/// </summary>
public class MapManager : MonoBehaviour
{
    /// <summary>
    /// 임시 Map Singleton
    /// </summary>
    public static MapManager Instance;

    [Header("Currnet Map Size")]
    public float mapSizeX = 300f;
    public float mapSizeY = 300f;
    public float panelSize = 10f;

    [Header("Map Object Info")]
    /// <summary>
    /// 맵의 등고선 색 ( Color Gap마다 다른 색으로 표시 )
    /// </summary>
    public Color[] color;

    /// <summary>
    /// 등고선 색 개수
    /// </summary>
    public uint ColorCount;

    /// <summary>
    /// 색깔별 Object y값 차이
    /// </summary>
    public float colorGap = 5f;

    /// <summary>
    /// 맵 패널 UI
    /// </summary>
    MapPanelUI largeMapPanelUI;

    /// <summary>
    /// 맵 패널 UI를 접근하기 위한 프로퍼티
    /// </summary>
    public MapPanelUI LargeMapPanelUI => largeMapPanelUI;

    /// <summary>
    /// largeMap panel canvasGruop
    /// </summary>
    CanvasGroup largeMapCanvasGroup;

    /// <summary>
    /// 미니맵 패널
    /// </summary>
    CanvasGroup miniMapPanelUI;

    /// <summary>
    /// 미니맵 패널 접근을 위한 프로퍼티
    /// </summary>
    public CanvasGroup MiniMapPanelUI => miniMapPanelUI;

    /// <summary>
    /// Map상에 플레이어 이동흔적을 표시할 Linerenderer
    /// </summary>
    LineRenderer playerLineRenderer;

    /// <summary>
    /// playerLineRenderer을 접근하기 위한 프로퍼티
    /// </summary>
    public LineRenderer PlayerLineRendere => playerLineRenderer;

    /// <summary>
    /// Map UI용 카메라
    /// </summary>
    Camera mapCamera;

    /// <summary>
    /// mapCamera를 접근하기 위한 프로퍼티
    /// </summary>
    public Camera MapCamera => mapCamera;

    public GameObject playerMark;

    /// <summary>
    /// 맵 카메라의 y 고정 좌표값
    /// </summary>
    const float mapCameraY = 100f;

    private void Awake()
    {
        Instance = this;

        InitalizeMapFunctions();
    }

    /// <summary>
    /// Map에 관련된 초기화 함수를 모아둔 함수
    /// </summary>
    private void InitalizeMapFunctions()
    {
        InitalizeMapUI();
    }

    private void InitalizeMapUI()
    {
        largeMapPanelUI = FindObjectOfType<MapPanelUI>();

        if(largeMapPanelUI == null)
        {
            Debug.LogWarning("[MapManager] : MapPanelUI가 존재하지 않습니다.");
        }
        else
        {
            largeMapCanvasGroup = LargeMapPanelUI.GetComponent<CanvasGroup>();
        }

        miniMapPanelUI = GameObject.Find("MiniMapPanel")?.GetComponent<CanvasGroup>();

        if (miniMapPanelUI == null)
        {
            Debug.LogWarning("[MapManager] : miniMapPanelUI 존재하지 않습니다." +
                "/ 오브젝트 이름을 확인해주세요 (MiniMapPanel)");
        }

        playerLineRenderer = GameObject.Find("PlayerFollowLine")?.GetComponent<LineRenderer>();

        if(playerLineRenderer == null)
        {
            Debug.LogWarning("[MapManager] : playerlineRenderer가 존재 하지않습니다. " +
                "/ 오브젝트 이름을 확인해주세요 (PlayerFollowLine)");            
        }

        mapCamera = GameObject.Find("MapCamera")?.GetComponent<Camera>();
        if (playerLineRenderer == null)
        {
            Debug.LogWarning("[MapManager] : mapCamera 존재 하지않습니다. " +
                "/ 오브젝트 이름을 확인해주세요 (MapCamera)");
        }
    }

    #region MapPanelMethods

    /// <summary>
    /// large 맵을 키는 함수 ( largeMap 켜짐, miniMap 꺼짐 )
    /// </summary>
    public void OpenMapUI()
    {
        largeMapCanvasGroup.alpha = 1.0f;
        largeMapCanvasGroup.interactable = true;
        largeMapCanvasGroup.blocksRaycasts = true;
        miniMapPanelUI.alpha = 0.0f;

        MapCamera.orthographicSize = 50f;
    }

    /// <summary>
    /// large 맵을 끄는 함수 ( largeMap 꺼짐, miniMap 켜짐 )
    /// </summary>
    public void CloseMapUI()
    {
        largeMapCanvasGroup.alpha = 0.0f;
        largeMapCanvasGroup.interactable = false;
        largeMapCanvasGroup.blocksRaycasts = false;
        miniMapPanelUI.alpha = 1.0f;

        MapCamera.orthographicSize = 20f;
    }

    /// <summary>
    /// 미니맵 열 때 실행하는 함수
    /// </summary>
    public void OpenMiniMapUI()
    {
        miniMapPanelUI.alpha = 1.0f;
    }

    /// <summary>
    /// 미니맵 끌 때 실행하는 함수
    /// </summary>
    public void CloseMiniMapUI()
    {
        miniMapPanelUI.alpha = 0.0f;        
    }

    #endregion

    #region ObjectSetting

    /// <summary>
    /// 카메라 위치를 조절하는 함수
    /// </summary>
    /// <param name="position"> 추가할 카메라 위치값 ( y좌표값은 100으로 고정 ) </param>
    public void SetCameraPosition(Vector3 position)
    {
        //Transform child = transform.GetChild(0); // MapObject

        float minX = transform.position.x; // MapManager는 맵의 좌측 하단에 있다.
        float minY = transform.position.z;
        float maxX = mapSizeX * 0.5f; // 우측 상단의 Panel 좌표값
        float maxY = mapSizeY * 0.5f;
        //float maxX = child.GetChild(child.childCount - 1).position.x; // 우측 상단의 Panel 좌표값
        //float maxY = child.GetChild(child.childCount - 1).position.z;
        
        mapCamera.transform.position += position; // 카메라 위치 설정
        
        // 카메라 위치값 범위 설정
        mapCamera.transform.position = new Vector3
                (Mathf.Clamp(position.x, minX, maxX),
                mapCameraY,
                Mathf.Clamp(position.z, minY, maxY));
    }

    /// <summary>
    /// Player Mark 위치를 설정하는 함수
    /// </summary>
    /// <param name="position">설정할 위치</param>
    public void SetPlayerMarkPosition(Vector3 position)
    {
        playerMark.transform.position = position;
    }
    #endregion
}