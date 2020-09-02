using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{
    [SerializeField] Text text;

    VerticalScrollRectController verticalScrollRectController;
    RectTransform rectTransform;

    public void Initialize(string name, VerticalScrollRectController scrollRectController)
    {
        text.text = name;
        this.verticalScrollRectController = scrollRectController;
        this.rectTransform = GetComponent<RectTransform>();
    }

    public void OnSelect()
    {
        verticalScrollRectController.Select(this.rectTransform);
    }
}
