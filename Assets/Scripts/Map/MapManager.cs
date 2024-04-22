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
    public GameObject panelPrefab;

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
    MapPanelUI mapPanelUI;

    /// <summary>
    /// 맵 패널 UI를 접근하기 위한 프로퍼티
    /// </summary>
    public MapPanelUI MapPanelUI => mapPanelUI;

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

    /// <summary>
    /// 맵 카메라의 y 고정 좌표값
    /// </summary>
    const float mapCameraY = 100f;

    private void Awake()
    {
        Instance = this;

        InitalizeMapFunctions();
    }

    private void Start()
    {
        GenerateWorldMapUI(mapSizeX, mapSizeY);
    }

    /// <summary>
    /// Map에 관련된 초기화 함수를 모아둔 함수
    /// </summary>
    private void InitalizeMapFunctions()
    {
        InitalizeMapUI();
        InitializeMapColor();
    }

    private void InitalizeMapUI()
    {
        mapPanelUI = FindObjectOfType<MapPanelUI>();

        if(mapPanelUI == null)
        {
            Debug.LogWarning("[MapManager] : MapPanelUI가 존재하지 않습니다.");
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

    private void InitializeMapColor()
    {
        color = new Color[ColorCount];

        for (int i = 0; i < color.Length; i++)
        {
            float ratio = 1 / (float)ColorCount;    // 색깔개수 별로 색 비율 결정
            color[i] = Color.white * ratio * (i + 1);         // 색상 정하기
            color[i].a = 1f;                        // alpha값은 1로 다시 변경
        }
    }
    
    /// <summary>
    /// Map Object Panel을 생성하는 함수
    /// </summary>
    /// <param name="mapSizeX">최대 맵 사이즈 x</param>
    /// <param name="mapSizeY">최대 맵 사이즈 y</param>
    void GenerateWorldMapUI(float mapSizeX, float mapSizeY)
    {
        int panelCountX = Mathf.FloorToInt(mapSizeX / panelSize); // x좌표값의 패널 개수
        int panelCountY = Mathf.FloorToInt(mapSizeY / panelSize); // y좌표값의 패널 개수

        GameObject MapObj = new GameObject("MapObject");
        MapObj.transform.parent = transform;
        MapObj.transform.localPosition = new Vector3(0, 20f, 0);

        // panel 생성
        for(int y = 0; y < panelCountY; y++)
        {
            for (int x = 0; x < panelCountX; x++)
            {
                Vector3 objVector = new Vector3(x * panelSize, 0f, y * panelSize);
                GameObject panelobj = Instantiate(panelPrefab, Vector3.zero, Quaternion.Euler(90f,0f,0f), MapObj.transform);
                panelobj.transform.localPosition = objVector;
                panelobj.AddComponent<MapObject>().mapObject = panelobj;
                //panelobj.name = $"NO.{y * panelCountX + x} Panel";
                panelobj.name = $"[{x}]_[{y}] Panel";
            }
        }
    }

    /// <summary>
    /// 오브젝트의 색깔을 정해주는 함수
    /// </summary>
    /// <param name="yPosition">오브젝트의 y좌표 값 ( World )</param>
    /// <returns>yPosition / ColorCount의 결과 값 배열 색</returns>
    public Color SetColor(float yPosition)
    {
        Color resultColor = Color.white;

        int colorIndex = Mathf.FloorToInt(yPosition / (float)colorGap); // color 인덱스 값 설정
        if (colorIndex > ColorCount - 1) colorIndex = (int)ColorCount - 1;

        resultColor = color[colorIndex];

        return resultColor;
    }
    #endregion

    #region MapCameraSetting

    /// <summary>
    /// 카메라 위치를 조절하는 함수
    /// </summary>
    /// <param name="position"> 추가할 카메라 위치값 ( y좌표값은 100으로 고정 ) </param>
    public void SetCaemraPosition(Vector3 position)
    {
        Transform child = transform.GetChild(0); // MapObject

        float minX = transform.position.x; // MapManager는 맵의 좌측 하단에 있다.
        float minY = transform.position.z;
        float maxX = child.GetChild(child.childCount - 1).position.x; // 우측 상단의 Panel 좌표값
        float maxY = child.GetChild(child.childCount - 1).position.z;

        mapCamera.transform.position += position; // 카메라 위치 설정

        // 카메라 위치값 범위 설정
        mapCamera.transform.position = new Vector3
                (Mathf.Clamp(position.x, minX, maxX),
                mapCameraY,
                Mathf.Clamp(position.z, minY, maxY));
    }
    #endregion
}