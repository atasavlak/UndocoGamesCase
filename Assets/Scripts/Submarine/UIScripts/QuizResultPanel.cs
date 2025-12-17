using UnityEngine;
using TMPro;

// Quiz tamamlandıktan sonra sonuç ve skor bilgisini
// ekranda gösteren sonuç paneli scripti.

public class QuizResultPanel : MonoBehaviour
{
    public TMP_Text resultText;
    public TMP_Text scoreText;

    // Quiz sonucu hesaplandıktan sonra paneli açar
    // ve skor ile sonuç mesajını günceller
    public void ShowResult(int correctCount, int totalQuestions)
    {
        gameObject.SetActive(true);

        scoreText.text = $"Score: {correctCount} / {totalQuestions}";

        if (correctCount >= 4)
            resultText.text = "Perfect!";
        else if (correctCount >= 2)
            resultText.text = "Well done!";
        else
            resultText.text = "Good!";
    }
}
