using UnityEngine;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private MinigameManager minigame;

    void Reset()
    {
        if (startButton == null) startButton = GetComponent<Button>();
    }

    void Awake()
    {
        if (startButton == null) startButton = GetComponent<Button>();
        if (startButton != null) startButton.onClick.AddListener(OnClickStart);
    }

    void OnDestroy()
    {
        if (startButton != null) startButton.onClick.RemoveListener(OnClickStart);
    }

    void OnClickStart()
    {
        if (minigame != null) minigame.InitializeGame();
    }
}
