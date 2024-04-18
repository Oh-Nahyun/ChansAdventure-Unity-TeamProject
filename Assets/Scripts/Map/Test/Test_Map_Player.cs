using System;
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

    private void OnOpenMap(InputAction.CallbackContext context)
    {
        // 임시 온오프
        if(context.performed)
        {
            if(map_CanvasGroup.alpha == 1f)
            {
                map_CanvasGroup.alpha = 0f;
            }
            else
            {
                map_CanvasGroup.alpha = 1f;
            }
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
            AddLine(playerPos);
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
                    AddLine(playerPos);
                    ResetLines(playerLineRenderer.positionCount);
                }

                AddLine(playerPos);
                prePos = playerPos; // 이전 위치값 저장

            }
        }
    }

    /// <summary>
    /// 라인을 추가 하는 함수
    /// </summary>
    /// <param name="linePosition">추가할 라인 위치</param>
    void AddLine(Vector3 linePosition)
    {
        playerLineRenderer.positionCount++;
        playerLineRenderer.SetPosition(playerLineRenderer.positionCount - 1, linePosition);    // 새로운 LineRenderer 위치 설정
    }

    /// <summary>
    /// 라인 개수가 최대 개수(lineMaxCount)에 도달하면 초기화 하는 함수
    /// </summary>
    /// <param name="lineCount">체크할 라인 수</param>
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