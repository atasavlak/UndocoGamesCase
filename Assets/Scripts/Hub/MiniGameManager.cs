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

    // Singleton yapısını kurar ve sahneler arası kalıcı olmasını sağlar
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

    // Seçilen mini oyunun sahnesini yükler
    public void LoadMiniGame(MiniGameType gameType)
    {
        SceneManager.LoadScene(gameType.ToString() + "Scene");
    }

    // Aktif sahneyi yeniden yükleyerek mini oyunu baştan başlatır
    public void RestartCurrentGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Oyuncuyu ana hub sahnesine geri döndürür
    public void ReturnToHub()
    {
        SceneManager.LoadScene("HubScene");
    }

    // Mini oyun tamamlandığında çağrılır
    // Event'i tetikler ve oyuncuyu hub'a geri gönderir
    public void CompleteMiniGame(MiniGameType gameType)
    {
        OnMiniGameCompleted?.Invoke(gameType);
        ReturnToHub();
    }
}
