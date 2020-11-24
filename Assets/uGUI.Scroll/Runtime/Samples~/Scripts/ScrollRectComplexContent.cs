using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using uGUI.Scroll;
using UniRx;

public class ScrollRectComplexContent : MonoBehaviour
{
    [SerializeField] Button addButton = default;
    [SerializeField] GameObject itemPrefab = default;
    [SerializeField] VerticalScrollRectController verticalScrollRectController = default;

    int count = 0;
    void Start()
    {
        addButton.OnClickAsObservable().Subscribe(_ =>
        {
            var go = Instantiate(itemPrefab);
            go.name = $"Added: {count}";
            var content = go.GetComponent<IScrollRectContent>();
            verticalScrollRectController.AddContent(content);
            count++;
        }).AddTo(this);
    }
}
