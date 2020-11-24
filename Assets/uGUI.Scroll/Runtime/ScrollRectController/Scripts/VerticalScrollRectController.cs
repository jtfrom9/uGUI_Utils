using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace uGUI.Scroll
{
    public interface IScrollRectController
    {
        bool IsSelected(IScrollRectContent content);
        IScrollRectContent SelectedContent { get; }
        void AddContent(IScrollRectContent content);
        void RemoveContent();
    }

    public static class ScrollRectControllerExtension
    {
        public static bool AnySelected(this IScrollRectController c) => c.SelectedContent != null;
    }

    public class VerticalScrollRectController : MonoBehaviour, IScrollRectController
    {
        ScrollRect scrollRect = null;
        IScrollRectContent selectedContent = null;

        void Start()
        {
            scrollRect = GetComponent<ScrollRect>();
            // scrollRect.onValueChanged.AddListener((pos) => {
            //     var anchorPos = scrollRect.content.anchoredPosition;
            //     Debug.Log($"scrollRect: {pos}, content.anchoredPosision: {anchorPos}");
            // });
        }

        internal void Select(IScrollRectContent selectedContent)
        {
            var selectedRect = selectedContent.rectTransform;
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
            this.selectedContent = selectedContent;
        }

        internal void Deselect()
        {
            this.selectedContent = null;
        }

        public void AddContent(IScrollRectContent content)
        {
            var contentHolder = scrollRect.content.transform;
            content.rectTransform.transform.SetParent(contentHolder, false);
        }

        public void RemoveContent()
        {
            if (selectedContent != null)
            {
                var go = selectedContent.rectTransform.gameObject;
                go.transform.SetParent(null);
                Destroy(go);

                selectedContent = null;
            }
        }

        public bool IsSelected(IScrollRectContent content)
        {
            return (this.selectedContent == content);
        }

        public IScrollRectContent SelectedContent { get => this.selectedContent; }
    }
}
