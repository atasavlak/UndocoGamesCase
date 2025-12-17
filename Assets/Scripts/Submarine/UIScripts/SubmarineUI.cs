using UnityEngine;
using TMPro;
using System.Collections;

// Submarine mini oyunundaki UI akışını,
// sandık sayacı ve quiz paneli geçişlerini yöneten sınıf.

public class SubmarineUI : MonoBehaviour
{
    public static SubmarineUI Instance;

    public TMP_Text chestText;

    [Header("Panels")]
    public GameObject scoreUI;
    public GameObject quizPanel;

    // SubmarineUI instance’ını ayarlar
    private void Awake()
    {
        Instance = this;
    }

    // Toplanan sandık sayısını UI üzerinde günceller
    public void UpdateChestUI(int collected, int total)
    {
        chestText.text = collected + " / " + total;
    }

    // Quiz panelini açma sürecini başlatır
    public void OpenQuizPanel()
    {
        StartCoroutine(OpenQuizFlow());
    }

    // Score UI'yi kapatıp quiz panelini açar
    // Kısa bir gecikme ile geçişi daha yumuşak yapar
    private IEnumerator OpenQuizFlow()
    {
        yield return new WaitForSeconds(0.3f);

        if (scoreUI != null)
            scoreUI.SetActive(false);

        if (quizPanel != null)
            quizPanel.SetActive(true);
    }
}
