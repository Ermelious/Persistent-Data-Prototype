using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizCreationUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI quizTitleText;
    public TextMeshProUGUI QuizTitleText
    {
        get { return quizTitleText; }
        private set { quizTitleText = value; }
    }
    [SerializeField]
    private Button previewQuizButton, addQuestionButton, backButton;
    public Button AddQuestionButton
    {
        get { return addQuestionButton; }
        private set { addQuestionButton = value; }
    }
    public Button PreviewQuizButton { 
        get { return previewQuizButton; } 
        private set { previewQuizButton = value; }
    }
    public Button BackButton
    {
        get { return backButton; }
        private set { backButton = value; }
    }

    [SerializeField]
    private QuestionPanel questionPanelPrefab;
    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;

    private void OnValidate()
    {
        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
    }

    public void OpenQuiz(QuizData quizData)
    {
        questionPanelPrefab.DespawnAll();
        previewQuizButton.gameObject.SetActive(quizData.questionList != null);
        quizTitleText.text = quizData.title;

        if (quizData.questionList != null)
        {
            int index = 0;
            foreach(QuizQuestion question in quizData.questionList)
            {
                questionPanelPrefab.Spawn(index, question.question, gridLayoutGroup.transform);
                index++;
            }
        }
    }
}
