using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace uGUI.Scroll
{
    [RequireComponent(typeof(Selectable))]
    [RequireComponent(typeof(EventTrigger))]
    public class VerticalScrollRectContentController : MonoBehaviour
    {
        [SerializeField] Text text;

        VerticalScrollRectController verticalScrollRectController;
        RectTransform rectTransform;

        void Start()
        {
            this.verticalScrollRectController = GetComponentInParent<VerticalScrollRectController>();
            this.rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(string name, VerticalScrollRectController scrollRectController)
        {
            text.text = name;
            this.verticalScrollRectController = scrollRectController;
        }

        public void OnSelect()
        {
            if (verticalScrollRectController)
            {
                verticalScrollRectController.Select(this.rectTransform);
            }
        }
    }
}
