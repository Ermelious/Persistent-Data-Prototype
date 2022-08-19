using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class OptionPanel : MonoBehaviour
{
    private static LinkedList<OptionPanel> inactivePool = new LinkedList<OptionPanel>();
    public static LinkedList<OptionPanel> activePool = new LinkedList<OptionPanel>();
    private LinkedListNode<OptionPanel> node;

    [SerializeField]
    private TextMeshProUGUI optionText;
    public TextMeshProUGUI OptionText
    {
        get { return optionText; }
        private set { optionText = value; }
    }
    [SerializeField]
    private Toggle toggle;
    public Toggle Toggle
    {
        get { return toggle; }
        private set { toggle = value; }
    }
    [SerializeField]
    private int index;
    public int Index
    {
        get { return index; }
        private set { index = value; }
    }

    private void OnValidate()
    {
        toggle = GetComponentInChildren<Toggle>();
    }

    public void Spawn(int index, string optionText, ToggleGroup toggleGroup, Transform parent)
    {
        LinkedListNode<OptionPanel> optionPanelNode = GetFromPool();
        optionPanelNode.Value.OptionText.text = optionText;
        optionPanelNode.Value.index = index;
        optionPanelNode.Value.node = optionPanelNode;
        optionPanelNode.Value.toggle.group = toggleGroup;
        optionPanelNode.Value.transform.SetParent(parent);
        optionPanelNode.Value.transform.localScale = Vector3.one;
        optionPanelNode.Value.transform.SetSiblingIndex(index);
    }

    private LinkedListNode<OptionPanel> GetFromPool()
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
