using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class QuizInfoPanel : MonoBehaviour
{
    private static LinkedList<QuizInfoPanel> inactivePool = new LinkedList<QuizInfoPanel>();
    private static LinkedList<QuizInfoPanel> activePool = new LinkedList<QuizInfoPanel>();
    private LinkedListNode<QuizInfoPanel> node;

    [SerializeField]
    private TextMeshProUGUI quizIndexText;
    public TextMeshProUGUI QuizIndexText
    {
        get { return quizIndexText; }
        private set { quizIndexText = value; }
    }
    [SerializeField]
    private TextMeshProUGUI quizTitleText;
    public TextMeshProUGUI QuizTitleText
    {
        get { return quizTitleText; }
        private set { quizTitleText = value; }
    }

    public void Spawn(int index, string quizTitle, Transform parent)
    {
        LinkedListNode<QuizInfoPanel> quizInfoPanelNode = GetFromPool();
        quizInfoPanelNode.Value.gameObject.SetActive(true);
        quizInfoPanelNode.Value.node = quizInfoPanelNode;
        quizInfoPanelNode.Value.quizIndexText.text = index.ToString();
        quizInfoPanelNode.Value.quizTitleText.text = quizTitle;
        quizInfoPanelNode.Value.transform.SetParent(parent);
        quizInfoPanelNode.Value.transform.localScale = Vector3.one;
    }

    private LinkedListNode<QuizInfoPanel> GetFromPool()
    {
        if (inactivePool.Count != 0)
        {
            activePool.AddFirst(inactivePool.First.Value);
            inactivePool.RemoveFirst();
        }
        else
        {
            activePool.AddFirst(Instantiate(this));
        }

        return activePool.First;
    }

    private void Despawn()
    {
        activePool.Remove(node);
        inactivePool.AddLast(this);
    }

    public void DespawnAll()
    {
        while (activePool.Count != 0)
            activePool.First.Value.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
        Despawn();
    }
}
