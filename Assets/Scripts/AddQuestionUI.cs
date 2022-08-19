using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddQuestionUI : MonoBehaviour
{
    [SerializeField]
    private QuizData quizData;
    [SerializeField]
    private int amountOfOptions = 3;
    [SerializeField]
    private OptionEditPanel optionEditPanelPrefab;
    [SerializeField]
    private GridLayoutGroup gridLayout;
    [SerializeField]
    private ToggleGroup toggleGroup;
    [SerializeField]
    private Button saveButton;
    [SerializeField]
    private TMP_InputField questionInputTextMeshProUGUI;
    public Button SaveButton
    {
        get { return saveButton; }
        private set { saveButton = value; }
    }

    private void OnValidate()
    {
        gridLayout = GetComponentInChildren<GridLayoutGroup>();
        toggleGroup = GetComponentInChildren<ToggleGroup>();
        saveButton = GetComponentInChildren<Button>();
    }

    public void Launch(QuizData quizData)
    {
        this.quizData = quizData;

        questionInputTextMeshProUGUI.text = "";
        for (int index = 0; index < amountOfOptions; index++)
            optionEditPanelPrefab.Spawn(index, gridLayout.transform, toggleGroup);
    }

    public void WriteToQuizData()
    {
        QuizQuestion question = new QuizQuestion();
        question.question = questionInputTextMeshProUGUI.text;
        int index = amountOfOptions-1;
        foreach (OptionEditPanel optionEditPanel in OptionEditPanel.activePool)
        {
            if (question.options == null)
                question.options = new LinkedList<string>();
            question.options.AddFirst(optionEditPanel.OptionText.text);
            if (optionEditPanel.Toggle.isOn)
                question.answerIndex = index;
            index--;
        }

        if (quizData.questionList == null)
            quizData.questionList = new LinkedList<QuizQuestion>();
        quizData.questionList.AddLast(question);
    }

    private void OnDisable()
    {
        optionEditPanelPrefab.DespawnAll();
    }
}
