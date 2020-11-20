using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using uGUI.Scroll;

public class ButtonControl : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform listPanel;
    [SerializeField] VerticalScrollRectController verticalScrollRectController;

    int count = 0;
    void Start()
    {
        button.OnClickAsObservable().Subscribe(_ => {
            var go = Instantiate(itemPrefab);
            go.transform.SetParent(listPanel, false);
            go.GetComponent<VerticalScrollRectContentController>()?.Initialize($"item {count}", verticalScrollRectController);
            count++;
        }).AddTo(this);
    }
}
