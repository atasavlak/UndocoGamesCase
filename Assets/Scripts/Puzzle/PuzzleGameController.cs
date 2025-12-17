using UnityEngine;

// Puzzle mini oyunundaki genel akışı, parça kontrolünü
// ve tamamlanma durumunu yöneten controller.

public class PuzzleGameController : MonoBehaviour
{
    public static PuzzleGameController Instance;

    public int totalPieces = 4;
    private int correctPlacedCount = 0;

    public GameObject completePanel;

    public AudioSource sfxSource;
    public AudioClip correctClip;
    public float completeDelay = 0.8f;
    
    // Puzzle sahnesi açıldığında controller instance’ını ayarlar
    private void Awake()
    {
        Instance = this;
    }

    // Bir parça doğru yerleştirildiğinde çağrılır
    // Sayıyı artırır ve puzzle bitişini kontrol eder
    public void NotifyCorrectPlacement()
    {
        correctPlacedCount++;

        if (sfxSource != null && correctClip != null)
            sfxSource.PlayOneShot(correctClip);

        if (correctPlacedCount >= totalPieces)
            Invoke(nameof(ShowCompletePanel), completeDelay);
    }

    // Puzzle tamamlandığında bitiş panelini açar
    private void ShowCompletePanel()
    {
        if (completePanel != null)
            completePanel.SetActive(true);
    }

    // Puzzle sahnesini baştan başlatır
    public void RestartPuzzle()
    {
        MiniGameManager.Instance.RestartCurrentGame();
    }

    // Oyuncuyu hub sahnesine geri döndürür
    public void ReturnToHub()
    {
        MiniGameManager.Instance.ReturnToHub();
    }

    // Doğru yerleştirme sesini manuel olarak çalar
    public void PlayCorrectSfx()
    {
        if (sfxSource != null && correctClip != null)
            sfxSource.PlayOneShot(correctClip);
    }
}
