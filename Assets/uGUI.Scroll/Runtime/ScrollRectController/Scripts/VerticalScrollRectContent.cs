using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace uGUI.Scroll
{
    public interface IScrollRectContent
    {
        void AddTrigger(EventTriggerType eventID, System.Action callback);
        bool Selected { get; }
        RectTransform rectTransform { get; }
    }

    [RequireComponent(typeof(EventTrigger))]
    [RequireComponent(typeof(RectTransform))]
    public class VerticalScrollRectContent : MonoBehaviour, IScrollRectContent
    {
        VerticalScrollRectController verticalScrollRectController;
        RectTransform _rectTransform;
        EventTrigger eventTrigger;

        List<EventTrigger.Entry> entries = new List<EventTrigger.Entry>();

        void Awake()
        {
            this._rectTransform = GetComponent<RectTransform>();
            this.eventTrigger = GetComponent<EventTrigger>();
        }

        void Start()
        {
            this.verticalScrollRectController = GetComponentInParent<VerticalScrollRectController>();

            AddTrigger(EventTriggerType.Select, () => {
                if (verticalScrollRectController)
                {
                    verticalScrollRectController.Select(this);
                }
            });

            // setup trigger
            foreach(var entry in entries) {
                this.eventTrigger.triggers.Add(entry);
            }
        }

        public void AddTrigger(EventTriggerType eventID, System.Action callback)
        {
            var cb = new EventTrigger.TriggerEvent();
            cb.AddListener((e) => callback());
            this.entries.Add(new EventTrigger.Entry
            {
                eventID = eventID,
                callback = cb
            });
        }

        public bool Selected { get => this.verticalScrollRectController.IsSelected(this); }
        public RectTransform rectTransform { get => this._rectTransform; }
    }
}
