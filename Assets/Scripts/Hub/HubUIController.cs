using UnityEngine;

// Hub sahnesindeki tüm menü akışını yöneten controller
public class HubUIController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject playSelectPanel;

    private void Start()
    {
        ShowMainMenu();
    }

    // Ana menüyü gösterir
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        playSelectPanel.SetActive(false);
    }

    // Play'e basıldığında oyun seçme ekranını açar
    public void OpenPlayMenu()
    {
        mainMenuPanel.SetActive(false);
        playSelectPanel.SetActive(true);
    }

    // Puzzle mini oyunu başlatır
    public void OpenPuzzleGame()
    {
        MiniGameManager.Instance.LoadMiniGame(MiniGameType.Puzzle);
    }

    // Submarine mini oyunu başlatır
    public void OpenSubmarineGame()
    {
        MiniGameManager.Instance.LoadMiniGame(MiniGameType.Submarine);
    }

    // Oyundan çıkış
    public void QuitGame()
    {
        Application.Quit();
    }
}
