using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using System;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    private enum UIType { QuizCollectionUI, QuizCreationUI, NewQuizUI, AddQuestionUI, QuizPreviewUI, QuizResultsUI }

    [SerializeField]
    private string jsonTextFileName;
    [SerializeField]
    private Quizes quizes;
    [SerializeField]
    private UserInterface userInterface;
    [SerializeField]
    private QuizInstanceData quizInstanceData = new QuizInstanceData();

    private QuizData selectedQuizData;

    private void OnValidate()
    {
#if UNITY_EDITOR
        userInterface.quizCollectionUI = GetComponentInChildren<QuizCollectionUI>();
        userInterface.quizCreationUI = GetComponentInChildren<QuizCreationUI>();
        userInterface.newQuizUI = GetComponentInChildren<NewQuizUI>();
        userInterface.addQuestionUI = GetComponentInChildren<AddQuestionUI>();
        userInterface.quizPreviewUI = GetComponentInChildren<QuizPreviewUI>();
        userInterface.quizResultsUI = GetComponentInChildren<QuizResultsUI>();
#endif
    }

    /// <summary>
    /// Activates the specified UI and Deactivates the rest. Only one UI will be active at a time.
    /// </summary>
    /// <param name="uiType"></param>
    private void SwitchUI(UIType uiType)
    {
        userInterface.quizCollectionUI.gameObject.SetActive(uiType == UIType.QuizCollectionUI);
        userInterface.quizCreationUI.gameObject.SetActive(uiType == UIType.QuizCreationUI);
        userInterface.newQuizUI.gameObject.SetActive(uiType == UIType.NewQuizUI);
        userInterface.addQuestionUI.gameObject.SetActive(uiType == UIType.AddQuestionUI);
        userInterface.quizPreviewUI.gameObject.SetActive(uiType == UIType.QuizPreviewUI);
        userInterface.quizResultsUI.gameObject.SetActive(uiType == UIType.QuizResultsUI);
    }

    private void Awake()
    {
        SwitchUI(UIType.QuizCollectionUI);

        using (var reader = new StreamReader(Application.streamingAssetsPath + "/Data/" + jsonTextFileName))
        {
            quizes = JsonConvert.DeserializeObject<Quizes>(reader.ReadToEnd());
            reader.Close();
        }
        if (null != quizes)
            userInterface.quizCollectionUI.Populate(quizes);

        userInterface.quizCollectionUI.CreateQuizButton.onClick.AddListener(() =>
        {
            SwitchUI(UIType.NewQuizUI);
        });

        userInterface.quizCreationUI.AddQuestionButton.onClick.AddListener(() =>
        {
            SwitchUI(UIType.AddQuestionUI);
            userInterface.addQuestionUI.Launch(selectedQuizData);
        });

        userInterface.addQuestionUI.SaveButton.onClick.AddListener(() =>
        {
            userInterface.addQuestionUI.WriteToQuizData();
            Save();
            SwitchUI(UIType.QuizCreationUI);
            userInterface.quizCreationUI.OpenQuiz(selectedQuizData);
        });

        userInterface.quizCreationUI.PreviewQuizButton.onClick.AddListener(() =>
        {
            SwitchUI(UIType.QuizPreviewUI);
            userInterface.quizPreviewUI.PreviewQuiz(selectedQuizData.questionList.First);
            quizInstanceData.score = 0;
            quizInstanceData.questionIndex = 1;
            userInterface.quizPreviewUI.QuizQuestionCountTextMeshProUGUI.text = "Question (" + quizInstanceData.questionIndex + " / " + selectedQuizData.questionList.Count + ")";
        });

        userInterface.quizResultsUI.BackButton.onClick.AddListener(() =>
        {
            SwitchUI(UIType.QuizCreationUI);
            userInterface.quizCreationUI.OpenQuiz(selectedQuizData);
        });

        userInterface.quizCreationUI.BackButton.onClick.AddListener(() =>
        {
            SwitchUI(UIType.QuizCollectionUI);
            userInterface.quizCollectionUI.Populate(quizes);
        });

        userInterface.newQuizUI.Initialize(CreateNewQuiz);
        userInterface.quizPreviewUI.OnSubmitAnswer += OnSubmitAnswer;
        userInterface.quizPreviewUI.OnEndOfQuiz += OnEndOfQuiz;
    }

    /// <summary>
    /// This method creates a new Quiz and adds it to Quizes data list. 
    /// The Save function can be called soon after this function.
    /// </summary>
    /// <param name="quizTitle"></param>
    private void CreateNewQuiz(string quizTitle)
    {
        if (quizes == null)
            quizes = new Quizes();
        if (quizes.data == null)
            quizes.data = new LinkedList<QuizData>();

        quizes.data.AddFirst(new QuizData(quizTitle));
        Save();
        userInterface.newQuizUI.gameObject.SetActive(false);
        userInterface.quizCreationUI.gameObject.SetActive(true);
        userInterface.quizCreationUI.OpenQuiz(quizes.data.First.Value);
        selectedQuizData = quizes.data.First.Value;
    }

    /// <summary>
    /// This function Serializes the Quizes object into JSON format and writes it to the jsonStorage.txt file.
    /// </summary>
    private void Save()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/Data/" + jsonTextFileName))
            {
                writer.Write(JsonConvert.SerializeObject(quizes));
                writer.Flush();
                writer.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    /// <summary>
    /// This function triggers when the user answers a question by clicking on an option and hitting the next button.
    /// </summary>
    /// <param name="optionIndex"></param>
    /// <param name="quizQuestion"></param>
    /// <param name="quizInstanceData"></param>
    private void OnSubmitAnswer(int optionIndex, QuizQuestion quizQuestion)
    {
        quizInstanceData.questionIndex++;
        userInterface.quizPreviewUI.QuizQuestionCountTextMeshProUGUI.text = "Question (" + quizInstanceData.questionIndex + " / " + selectedQuizData.questionList.Count + ")";

        if (quizQuestion.answerIndex == optionIndex)
        {
            quizInstanceData.score++;
        }
    }

    /// <summary>
    /// This function triggers when the user has no questions left to answer.
    /// </summary>
    private void OnEndOfQuiz()
    {
        SwitchUI(UIType.QuizResultsUI);
        userInterface.quizResultsUI.ShowScore(quizInstanceData.score, selectedQuizData.questionList.Count);
    }
}

[System.Serializable]
public class Quizes
{
    public LinkedList<QuizData> data;
}

[System.Serializable]
public class QuizData
{
    public string title;
    public LinkedList<QuizQuestion> questionList;

    public QuizData(string title)
    {
        this.title = title;
    }
}

[System.Serializable]
public class QuizQuestion
{
    public string question;
    public LinkedList<string> options;
    public int answerIndex;
}

[System.Serializable]
public class UserInterface {
    public QuizCollectionUI quizCollectionUI;
    public QuizCreationUI quizCreationUI;
    public NewQuizUI newQuizUI;
    public AddQuestionUI addQuestionUI;
    public QuizPreviewUI quizPreviewUI;
    public QuizResultsUI quizResultsUI;
}

[System.Serializable]
public class QuizInstanceData
{
    public int score;
    public int questionIndex;
}
