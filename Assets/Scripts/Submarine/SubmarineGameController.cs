using UnityEngine;

// Submarine mini oyunundaki sandık toplama,
// quiz akışı ve genel oyun durumunu yöneten controller.

public class SubmarineGameController : MonoBehaviour
{
    public static SubmarineGameController Instance;

    [Header("Chest Settings")]
    public int totalChests = 5;
    private int collectedChests = 0;

    [Header("Quiz & Result")]
    public GameObject quizPanel;
    public QuizResultPanel resultPanel;

    // SubmarineGameController instance’ını ayarlar
    private void Awake()
    {
        Instance = this;
    }

    // Oyun başladığında ilk UI durumunu günceller
    private void Start()
    {
        UpdateUI();
    }

    // Bir sandık toplandığında çağrılır
    public void CollectChest()
    {
        collectedChests++;

        UpdateUI();

        if (collectedChests >= totalChests)
        {
            AllChestsCollected();
        }
    }

    // Sandık sayısını UI üzerinde günceller
    private void UpdateUI()
    {
        SubmarineUI.Instance.UpdateChestUI(collectedChests, totalChests);
    }

    // Tüm sandıklar toplandığında quiz ekranını açar
    private void AllChestsCollected()
    {
        SubmarineUI.Instance.OpenQuizPanel();
    }

    // Quiz tamamlandığında QuizManager tarafından çağrılır
    public void OnQuizFinished(int correctCount, int totalQuestions)
    {
        quizPanel.SetActive(false);
        resultPanel.ShowResult(correctCount, totalQuestions);
    }

    // Submarine mini oyununu baştan başlatır
    public void RestartSubmarine()
    {
        MiniGameManager.Instance.RestartCurrentGame();
    }

    // Oyuncuyu hub sahnesine geri döndürür
    public void ReturnToHub()
    {
        MiniGameManager.Instance.ReturnToHub();
    }
}
