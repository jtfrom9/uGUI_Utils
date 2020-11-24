using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using uGUI.Scroll;

public class ScrollRectContent : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] VerticalScrollRectController verticalScrollRectController;

    int count = 0;

    public void Add()
    {
        var go = Instantiate(itemPrefab);
        go.name = $"item {count}";
        var content = go.GetComponent<VerticalScrollRectContent>();
        content.AddTrigger(EventTriggerType.Select, () =>
        {
            Debug.Log($"selected. {content.gameObject.name}");
        });
        content.AddTrigger(EventTriggerType.Deselect, () =>
        {
            Debug.Log($"De-selected. {content.gameObject.name}");
        });
        verticalScrollRectController.AddContent(content);
        count++;
    }

    public void Remove()
    {
        verticalScrollRectController.RemoveContent();
    }
}
