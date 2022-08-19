using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class NewQuizUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField quizNameInputText;
    [SerializeField]
    private Button saveButton;

    public delegate void CreateNewQuizDelegate(string quizTitle);
    public CreateNewQuizDelegate CreateNewQuiz;

    private void OnValidate()
    {
        saveButton = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        quizNameInputText.text = "";
    }

    private void Awake()
    {
        saveButton.onClick.AddListener(() =>
        { CreateNewQuiz?.Invoke(quizNameInputText.text); });
    }

    public void Initialize(CreateNewQuizDelegate createNewQuizAction)
    {
        CreateNewQuiz = createNewQuizAction;
    }
}
