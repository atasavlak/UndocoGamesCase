using UnityEngine;

// Puzzle oyununun tamamlanma durumunu takip eder
public class PuzzleGameController : MonoBehaviour
{
    public static PuzzleGameController Instance;

    [Header("Puzzle Settings")]
    public int totalPieces = 4;
    public AudioSource sfxSource;
    public AudioClip correctClip;


    private int correctPlacedCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    // Doğru parça yerleştiğinde çağrılır
    public void NotifyCorrectPlacement()
    {
        correctPlacedCount++;

        if (sfxSource != null && correctClip != null)
        sfxSource.PlayOneShot(correctClip);


        if (correctPlacedCount >= totalPieces)
        {
            
        }
    }

        public void PlayCorrectSfx()
    {
        if (sfxSource != null && correctClip != null)
            sfxSource.PlayOneShot(correctClip);
    }

}
