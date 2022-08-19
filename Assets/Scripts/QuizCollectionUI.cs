using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizCollectionUI : MonoBehaviour
{
    [SerializeField]
    private Button createQuizButton;
    public Button CreateQuizButton
    {
        get { return createQuizButton; }
        private set { createQuizButton = value; }
    }
    [SerializeField]
    private QuizInfoPanel quizInfoPanelPrefab;
    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;

    private void OnValidate()
    {
        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
    }

    public void Populate(Quizes quizes)
    {
        int index = 0;
        //quizInfoPanelPrefab.DespawnAll();

        foreach (QuizData quizData in quizes.data)
        {
            quizInfoPanelPrefab.Spawn(index, quizData.title, gridLayoutGroup.transform);
            index++;
        }
    }
}
