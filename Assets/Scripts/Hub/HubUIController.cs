using UnityEngine;

// Hub sahnesindeki tüm menü akışını yöneten controller
public class HubUIController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject playSelectPanel;

    // Sahne açıldığında varsayılan olarak ana menüyü gösterir
    private void Start()
    {
        ShowMainMenu();
    }

    // Ana menü panelini açar, diğer menüleri kapatır
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        playSelectPanel.SetActive(false);
    }

    // Play butonuna basıldığında oyun seçim ekranını açar
    public void OpenPlayMenu()
    {
        mainMenuPanel.SetActive(false);
        playSelectPanel.SetActive(true);
    }

    // Puzzle mini oyununu yükler
    public void OpenPuzzleGame()
    {
        MiniGameManager.Instance.LoadMiniGame(MiniGameType.Puzzle);
    }

    // Submarine mini oyununu yükler
    public void OpenSubmarineGame()
    {
        MiniGameManager.Instance.LoadMiniGame(MiniGameType.Submarine);
    }

    // Oyunu tamamen kapatır
    public void QuitGame()
    {
        Application.Quit();
    }
}
