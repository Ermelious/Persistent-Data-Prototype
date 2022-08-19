using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionPanel : MonoBehaviour
{
    private static LinkedList<QuestionPanel> inactivePool = new LinkedList<QuestionPanel>();
    private static LinkedList<QuestionPanel> activePool = new LinkedList<QuestionPanel>();
    private LinkedListNode<QuestionPanel> node;

    [SerializeField]
    private TextMeshProUGUI questionIndexText;
    public TextMeshProUGUI QuestionIndexText
    {
        get { return questionIndexText; }
        private set { questionIndexText = value; }
    }

    [SerializeField]
    private TextMeshProUGUI questionText;
    public TextMeshProUGUI QuestionText
    {
        get { return questionText; }
        private set { questionText = value; }
    }

    public void Spawn(int index, string quizTitle, Transform transform)
    {
        LinkedListNode<QuestionPanel> questionPanelNode = GetFromPool();
        questionPanelNode.Value.gameObject.SetActive(true);
        questionPanelNode.Value.QuestionIndexText.text = index.ToString();
        questionPanelNode.Value.QuestionText.text = quizTitle;
        questionPanelNode.Value.node = questionPanelNode;
        questionPanelNode.Value.transform.SetParent(transform);
        questionPanelNode.Value.transform.localScale = Vector3.one;
    }

    private LinkedListNode<QuestionPanel> GetFromPool()
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
        gameObject.SetActive(false);
        if (null == node)
            return;

        activePool.Remove(node);
        inactivePool.AddLast(this);
        node = null;
    }

    public void DespawnAll()
    {
        while (activePool.Count != 0)
            activePool.First.Value.Despawn();
    }

    private void OnDisable()
    {
        Despawn();
    }
}
