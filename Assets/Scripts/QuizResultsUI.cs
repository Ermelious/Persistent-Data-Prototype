using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizResultsUI : MonoBehaviour
{
    [SerializeField]
    private Button backButton;
    public Button BackButton
    {
        get { return backButton; }
        private set { backButton = value; }
    }
    [SerializeField]
    private TextMeshProUGUI scoreTextMeshProUGUI;
    public TextMeshProUGUI ScoreTextMeshProUGUI
    {
        get { return scoreTextMeshProUGUI; }
        private set { scoreTextMeshProUGUI = value; }
    }

    private void OnValidate()
    {
        BackButton = GetComponentInChildren<Button>();
    }

    public void ShowScore(int score, int maxScore)
    {
        ScoreTextMeshProUGUI.text = "Your Score: " + score + " / " + maxScore;
    }
}
