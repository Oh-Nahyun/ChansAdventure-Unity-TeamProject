using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Map_Player : MonoBehaviour
{
    PlayerinputActions inputActions;
    public CanvasGroup map_CanvasGroup;
    public LineRenderer playerLineRenderer;
    public Camera mapCamera;

    public float speed = 5f;
    public float rotatePower = 90f;

    float moveInput = 0f;
    float rotateInput = 0f;

    public int lineMaxCount = 10;

    void Awake()
    {
        inputActions = new PlayerinputActions();

        InitLine();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Open_Map.performed += OnOpenMap;
        inputActions.Player.Open_Map.canceled += OnOpenMap;
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Open_Map.canceled -= OnOpenMap;
        inputActions.Player.Open_Map.performed -= OnOpenMap;
        inputActions.Player.Disable();
        
    }

    void Update()
    {
        transform.position += transform.forward * moveInput * Time.deltaTime;
        transform.Rotate(Vector3.up * rotateInput * Time.deltaTime);

        DrawLine();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();

        moveInput = inputVector.y * speed;
        rotateInput = inputVector.x * rotatePower;
    }

    private void OnOpenMap(InputAction.CallbackContext obj)
    {
        // 임시 온오프
        if(map_CanvasGroup.alpha == 1f)
        {
            map_CanvasGroup.alpha = 0f;
        }
        else
        {
            map_CanvasGroup.alpha = 1f;
        }
    }

    /// <summary>
    /// LineRenderer 초기화
    /// </summary>
    void InitLine()
    {
        // 사이즈 초기화
        playerLineRenderer.positionCount = 0;

        // LineRenderer 넓이 설정
        playerLineRenderer.startWidth = 5f;
        playerLineRenderer.endWidth = 5f;
    }

    /// <summary>
    /// LineRenderer을 위치설정을 하기위한 플레이어 위치 벡터
    /// </summary>
    public Vector3 playerPos;

    /// <summary>
    /// LineRenderer의 이전 위치 값
    /// </summary>
    public Vector3 prePos;

    void DrawLine()
    {
        playerPos = new Vector3(Mathf.FloorToInt(transform.position.x), 10f, Mathf.FloorToInt(transform.position.z));   // Line Position 위치

        if(playerLineRenderer.positionCount == 0) // 최초 지점 ( 거리를 측정할 이전 값이 없기 때문에 )
        {
            playerLineRenderer.positionCount++;                                         // size 증가
            playerLineRenderer.SetPosition(playerLineRenderer.positionCount - 1, playerPos);    // Line Position 위치 설정
            prePos = playerPos;                                                 // 이전 위치값 저장

            //linePrefab.positionCount++;                                         // size 증가
        }
        else
        {
            float betweenVertex = (playerPos - prePos).sqrMagnitude;    // 거리
            float maxLength = 5f;                                       // 각 Vertex의 최대 거리
            if(betweenVertex >= maxLength * maxLength)                  // betweenVertex보다 거리가 크다
            {
                if (playerLineRenderer.positionCount > 10)
                {
                    playerLineRenderer.positionCount++; // size 증가
                    playerLineRenderer.SetPosition(playerLineRenderer.positionCount - 1, playerPos);    // 새로운 LineRenderer 위치 설정

                    ResetLines(playerLineRenderer.positionCount);
                }

                playerLineRenderer.positionCount++; // size 증가
                playerLineRenderer.SetPosition(playerLineRenderer.positionCount - 1, playerPos);    // 새로운 LineRenderer 위치 설정
                prePos = playerPos; // 이전 위치값 저장

            }
        }
    }

    void ResetLines(int lineCount)
    {
        if(lineCount > lineMaxCount)
        {

            playerLineRenderer.positionCount = 2;
            playerLineRenderer.SetPosition(0, prePos);
            playerLineRenderer.SetPosition(playerLineRenderer.positionCount - 1, playerPos);
        }
    }
}