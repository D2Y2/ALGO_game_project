using UnityEngine;
using UnityEngine.UI; // UI 요소를 사용하기 위해 추가
using TMPro; // TextMeshPro를 사용하기 위해 추가

public class MinigameManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform backgroundBar;     // 미니게임 배경 바 (전체 범위)
    public RectTransform movingElement;     // 움직이는 요소
    public RectTransform successZone;       // 성공 판정 영역
    public TextMeshProUGUI resultText;      // 결과 텍스트

    [Header("Movement Settings")]
    public float moveSpeed = 300f;          // 움직이는 요소의 이동 속도
    private int moveDirection = 1;          // 이동 방향 (1: 오른쪽, -1: 왼쪽)
    private float currentXPosition;         // 움직이는 요소의 현재 X 위치

    [Header("Game Settings")]
    public KeyCode actionKey = KeyCode.Space; // 플레이어 입력 키
    public float minigameDuration = 5f;     // 미니게임 총 진행 시간
    private float gameTimer;                // 게임 타이머
    private bool isGameActive = false;

    private float backgroundBarWidth;       // 배경 바의 실제 너비
    private float movingElementHalfWidth;   // 움직이는 요소의 절반 너비

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        // UI 요소의 실제 픽셀 너비를 계산
        backgroundBarWidth = backgroundBar.rect.width;
        movingElementHalfWidth = movingElement.rect.width / 2f;

        // 초기 위치 설정 (중앙)
        movingElement.anchoredPosition = Vector2.zero;
        currentXPosition = 0; // 시작 위치를 0으로 설정하여 중앙에서 시작

        // 초기 방향 설정 (랜덤)
        moveDirection = Random.Range(0, 2) * 2 - 1; // 1 또는 -1

        gameTimer = minigameDuration;
        resultText.text = "";
        isGameActive = true;

        Debug.Log("미니게임 시작! " + actionKey.ToString() + " 키를 눌러 타이밍을 맞춰보세요.");
    }

    void Update()
    {
        if (!isGameActive) return;

        HandleMovingElementMovement();
        HandlePlayerInput();
        UpdateGameTimer();
    }

    void HandleMovingElementMovement()
    {
        // 새 위치 계산
        currentXPosition += moveDirection * moveSpeed * Time.deltaTime;

        // 바 경계 제한 및 방향 전환
        float maxX = (backgroundBarWidth / 2f) - movingElementHalfWidth;
        float minX = -(backgroundBarWidth / 2f) + movingElementHalfWidth;

        if (currentXPosition >= maxX)
        {
            currentXPosition = maxX;
            moveDirection = -1; // 오른쪽 끝에 닿으면 왼쪽으로
        }
        else if (currentXPosition <= minX)
        {
            currentXPosition = minX;
            moveDirection = 1; // 왼쪽 끝에 닿으면 오른쪽으로
        }

        movingElement.anchoredPosition = new Vector2(currentXPosition, 0); // Y는 0으로 고정
    }

    void HandlePlayerInput()
    {
        // 지정된 키를 눌렀는지 확인
        if (Input.GetKeyDown(actionKey))
        {
            CheckSuccess();
        }
    }

    void CheckSuccess()
    {
        // 움직이는 요소의 현재 X 위치
        float currentElementX = movingElement.anchoredPosition.x;

        // 성공 영역의 X 위치 범위 계산
        float successZoneMinX = successZone.anchoredPosition.x - (successZone.rect.width / 2f);
        float successZoneMaxX = successZone.anchoredPosition.x + (successZone.rect.width / 2f);

        // 움직이는 요소의 중심이 성공 영역 안에 있는지 확인
        if (currentElementX >= successZoneMinX && currentElementX <= successZoneMaxX)
        {
            EndGame(true); // 성공
        }
        else
        {
            EndGame(false); // 실패
        }
    }

    void UpdateGameTimer()
    {
        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0)
        {
            gameTimer = 0;
            // 시간이 다 되었는데 성공하지 못했으면 실패 처리
            EndGame(false);
        }
    }

    void EndGame(bool success)
    {
        isGameActive = false;
        if (success)
        {
            resultText.color = Color.green;
            resultText.text = "Success!";
            Debug.Log("미니게임 성공!");
        }
        else
        {
            resultText.color = Color.red;
            resultText.text = "Fail!";
            Debug.Log("미니게임 실패!");
        }

        // 3초 후 게임 다시 시작
        Invoke("RestartGame", 3f);
    }

    void RestartGame()
    {
        // 게임 상태를 초기화하고 다시 시작
        InitializeGame();
    }
}