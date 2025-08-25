using UnityEngine;
using TMPro;

public class MinigameManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform backgroundBar;
    public RectTransform movingElement;
    public RectTransform successZone;
    public TextMeshProUGUI resultText;

    [Header("Movement Settings")]
    public float moveSpeed = 300f;     // px/sec
    private int moveDirection = 1;
    private float currentXPosition;

    [Header("Game Settings")]
    public KeyCode actionKey = KeyCode.Space;
    public float successZoneWidth = 100f;
    public int maxTraversals = 4;
    private int currentTraversals;

    private bool isGameActive = false;

    private float backgroundBarWidth;
    private float movingElementHalfWidth;

    void Start()
    {
        isGameActive = false;
        if (resultText != null) resultText.text = "";
    }

    void Update()
    {
        if (!isGameActive) return;
        HandleMovingElementMovement();
        if (Input.GetKeyDown(actionKey)) CheckSuccess();
    }

    public void InitializeGame()
    {
        if (backgroundBar == null || movingElement == null || successZone == null) {
            Debug.LogError("[MinigameManager] UI references not assigned.");
            return;
        }

        Canvas.ForceUpdateCanvases();
        backgroundBarWidth = backgroundBar.rect.width;
        movingElementHalfWidth = movingElement.rect.width * 0.5f;

        successZone.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, successZoneWidth);
        successZone.anchoredPosition = new Vector2(0f, successZone.anchoredPosition.y);

        currentXPosition = -backgroundBarWidth * 0.5f + movingElementHalfWidth;
        movingElement.anchoredPosition = new Vector2(currentXPosition, 0f);

        moveDirection = 1;
        currentTraversals = 0;
        if (resultText) { resultText.color = Color.white; resultText.text = ""; }
        isGameActive = true;
    }

    void HandleMovingElementMovement()
    {
        currentXPosition += moveDirection * moveSpeed * Time.deltaTime;

        float maxX = (backgroundBarWidth * 0.5f) - movingElementHalfWidth;
        float minX = -(backgroundBarWidth * 0.5f) + movingElementHalfWidth;

        bool bounced = false;
        if (currentXPosition >= maxX) { currentXPosition = maxX; moveDirection = -1; bounced = true; }
        if (currentXPosition <= minX) { currentXPosition = minX; moveDirection =  1; bounced = true; }

        if (bounced) {
            currentTraversals++;
            if (currentTraversals >= maxTraversals) {
                EndGame(false);
                return;
            }
        }

        movingElement.anchoredPosition = new Vector2(currentXPosition, 0f);
    }

    public bool IsInSuccessZone()
    {
        float x = movingElement.anchoredPosition.x;
        float minX = successZone.anchoredPosition.x - (successZone.rect.width * 0.5f);
        float maxX = successZone.anchoredPosition.x + (successZone.rect.width * 0.5f);
        return x >= minX && x <= maxX;
    }

    void CheckSuccess() => EndGame(IsInSuccessZone());

    void EndGame(bool success)
    {
        isGameActive = false;
        if (resultText != null) {
            resultText.color = success ? Color.green : Color.red;
            resultText.text = success ? "Success!" : "Fail!";
        }
        Invoke(nameof(RestartGame), 1.5f);
    }

    void RestartGame()
    {
        isGameActive = false;
        if (resultText != null) resultText.text = "";
        if (movingElement != null) movingElement.anchoredPosition = Vector2.zero;
    }
}
