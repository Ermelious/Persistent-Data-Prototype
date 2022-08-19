using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizPreviewUI : MonoBehaviour
{
    [SerializeField]
    private OptionPanel optionPanelPrefab;
    [SerializeField]
    private TextMeshProUGUI questionTextMeshProUGUI, quizQuestionCountTextMeshProUGUI;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private LinkedListNode<QuizQuestion> currentQuestionNode;
    [SerializeField]
    private ToggleGroup toggleGroup;
    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;
    [SerializeField]
    private LinkedListNode<QuizQuestion> selectedQuizQuestionNode;
    public delegate void SubmitAnswerDelegate(int optionIndex, QuizQuestion quizQuestion);
    public SubmitAnswerDelegate OnSubmitAnswer;
    public System.Action OnEndOfQuiz;
    public TextMeshProUGUI QuizQuestionCountTextMeshProUGUI
    {
        get { return quizQuestionCountTextMeshProUGUI; }
        private set { quizQuestionCountTextMeshProUGUI = value; }
    }

    public Button NextButton
    {
        get { return nextButton; }
        private set { nextButton = value; }
    }

    private void OnValidate()
    {
        nextButton = GetComponentInChildren<Button>();
        toggleGroup = GetComponentInChildren<ToggleGroup>();
        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
    }

    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            foreach (OptionPanel optionPanel in OptionPanel.activePool)
                if (optionPanel.Toggle.isOn)
                {
                    OnSubmitAnswer?.Invoke(optionPanel.Index, selectedQuizQuestionNode.Value);
                    break;
                }

            selectedQuizQuestionNode = selectedQuizQuestionNode.Next;

            if (selectedQuizQuestionNode == null)
            {
                optionPanelPrefab.DespawnAll();
                OnEndOfQuiz?.Invoke();
                return;
            } 
            else
                PreviewQuiz(selectedQuizQuestionNode);
        });
    }

    public void PreviewQuiz(LinkedListNode<QuizQuestion> quizQuestionNode)
    {
        questionTextMeshProUGUI.text = quizQuestionNode.Value.question;
        int index = 0;
        optionPanelPrefab.DespawnAll();
        foreach (string options in quizQuestionNode.Value.options)
        {
            optionPanelPrefab.Spawn(index, options, toggleGroup, gridLayoutGroup.transform);
            index++;
        }
        selectedQuizQuestionNode = quizQuestionNode;
    }
}
