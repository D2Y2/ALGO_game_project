using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private GameObject interactText;
    [SerializeField] private string sceneToLoad = "";

    private bool isPlayerNear = false;

    void Start()
    {
        if (interactText != null) interactText.SetActive(false);
    }

    void Update()
    {
        if (!isPlayerNear) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!string.IsNullOrEmpty(sceneToLoad))
                SceneManager.LoadScene(sceneToLoad);
            else
                Debug.Log("[NPCInteraction] 대화/상호작용 트리거 (씬 미지정).");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        isPlayerNear = true;
        if (interactText != null) interactText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        isPlayerNear = false;
        if (interactText != null) interactText.SetActive(false);
    }
}
