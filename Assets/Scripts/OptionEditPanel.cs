using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionEditPanel : MonoBehaviour
{
    private static LinkedList<OptionEditPanel> inactivePool = new LinkedList<OptionEditPanel>();
    public static LinkedList<OptionEditPanel> activePool = new LinkedList<OptionEditPanel>();
    private LinkedListNode<OptionEditPanel> node;

    [SerializeField]
    private TextMeshProUGUI optionIndexText;
    public TextMeshProUGUI OptionIndexText
    {
        get { return optionIndexText; }
        private set { optionIndexText = value; }
    }
    [SerializeField]
    private TMP_InputField optionText;
    public TMP_InputField OptionText
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

    public void Spawn(int index, Transform parent, ToggleGroup toggleGroup)
    {
        LinkedListNode<OptionEditPanel> optionPanelNode = GetFromPool();
        optionPanelNode.Value.gameObject.SetActive(true);
        optionPanelNode.Value.OptionIndexText.text = index.ToString();
        optionPanelNode.Value.transform.SetParent(parent);
        optionPanelNode.Value.transform.localScale = Vector3.one;
        optionPanelNode.Value.toggle.group = toggleGroup;
        optionPanelNode.Value.node = optionPanelNode;
        optionPanelNode.Value.optionText.text = "";
    }

    private LinkedListNode<OptionEditPanel> GetFromPool()
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

        gameObject.SetActive(false);
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
