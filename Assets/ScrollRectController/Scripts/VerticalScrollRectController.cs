using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalScrollRectController : MonoBehaviour
{
    ScrollRect scrollRect;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void Select(RectTransform selectedRect)
    {
        var viewPortHeight = scrollRect.viewport.rect.height;
        var contentPosY = scrollRect.content.anchoredPosition.y;

        var refY = -selectedRect.offsetMax.y;
        var visible = (contentPosY <= refY && (refY + selectedRect.rect.height) <= contentPosY + viewPortHeight);

        if (!visible)
        {
            var newY = refY + selectedRect.rect.height - viewPortHeight;
            scrollRect.content.anchoredPosition = new Vector2
            {
                x = scrollRect.content.anchoredPosition.x,
                y = (newY < 0) ? refY : newY
            };
        }
    }
}
