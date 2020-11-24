using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace uGUI.Scroll
{
    public class VerticalScrollRectController : MonoBehaviour
    {
        ScrollRect scrollRect;

        void Start()
        {
            scrollRect = GetComponent<ScrollRect>();
            // scrollRect.onValueChanged.AddListener((pos) => {
            //     var anchorPos = scrollRect.content.anchoredPosition;
            //     Debug.Log($"scrollRect: {pos}, content.anchoredPosision: {anchorPos}");
            // });
        }

        public void Select(RectTransform selectedRect)
        {
            var viewPortHeight = scrollRect.viewport.rect.height;
            var contentPosY = scrollRect.content.anchoredPosition.y;

            var refY = -selectedRect.offsetMax.y;
            var visible = (contentPosY <= refY && (refY + selectedRect.rect.height) <= contentPosY + viewPortHeight);
            // Debug.Log($"contentPosY({contentPosY}) <= refY({refY}) is {contentPosY <= refY}");
            // Debug.Log($"refY({refY}) + rect.height({selectedRect.rect.height}) <= contentPosY({contentPosY}) + viewPortHeight({viewPortHeight}) is {(refY + selectedRect.rect.height) <= contentPosY + viewPortHeight}");

            if (!visible)
            {
                var newY = refY + selectedRect.rect.height - viewPortHeight;
                // Debug.Log($"newY = refY({refY}) + rect.height({selectedRect.rect.height}) - viewPortHeight({viewPortHeight}) = {newY}");
                scrollRect.content.anchoredPosition = new Vector2
                {
                    x = scrollRect.content.anchoredPosition.x,
                    y = (newY < 0) ? refY : newY
                };
            }
        }
    }
}
