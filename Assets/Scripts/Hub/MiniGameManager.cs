using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// Projedeki tüm mini oyunların sahne geçişlerini ve
// genel oyun akışını yöneten merkezi sınıf.

public enum MiniGameType
{
    Hub,
    Puzzle,
    Submarine
}

public class MiniGameManager : MonoBehaviour
{
    // Proje genelinde tek bir MiniGameManager olmasını sağlar
    public static MiniGameManager Instance;

    // Mini oyunlar tamamlandığında tetiklenen event
    public static Action<MiniGameType> OnMiniGameCompleted;

    // Singleton kontrolünü yapar ve objeyi sahneler arası kalıcı kılar
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Verilen mini oyuna ait sahneyi yükler
    public void LoadMiniGame(MiniGameType gameType)
    {
        SceneManager.LoadScene(gameType.ToString() + "Scene");
    }

    // O anki sahneyi yeniden yükleyerek oyunu sıfırlar
    public void RestartCurrentGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Oyuncuyu ana hub sahnesine geri gönderir
    public void ReturnToHub()
    {
        SceneManager.LoadScene("HubScene");
    }

    // Mini oyun bitince çağrılır, event'i tetikler ve hub'a döner
    public void CompleteMiniGame(MiniGameType gameType)
    {
        OnMiniGameCompleted?.Invoke(gameType);
        ReturnToHub();
    }
}
