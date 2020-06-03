using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

public enum InputEventType
{
    Begin = 1,
    Move = 2,
    End = 4,
    Other = 8
}

public interface IInputEvent
{
    InputEventType Type { get; }
    Vector2 Position { get; }
    bool Ignored { get; }
}

public interface IInput
{
    bool IsEnable { get; }
    IInputEvent Current { get; }
}

public class UnityTouchInput : IInput
{
    class UnityInputEvent : IInputEvent
    {
        private Touch touch;

        public InputEventType Type
        {
            get
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        return InputEventType.Begin;
                    case TouchPhase.Moved:
                        return InputEventType.Move;
                    case TouchPhase.Ended:
                        return InputEventType.End;
                    default:
                        return InputEventType.Other;
                }
            }
        }

        public Vector2 Position { get => touch.position; }

        public bool Ignored { get => EventSystem.current.IsPointerOverGameObject(touch.fingerId); }

        public UnityInputEvent(Touch touch)
        {
            this.touch = touch;
        }
    }

    public bool IsEnable { get => Input.touchCount > 0; }
    public IInputEvent Current { get => new UnityInputEvent(Input.GetTouch(0)); }
}

public class UnityMouseInput : IInput
{
    class UnityMouseInputEvent : IInputEvent
    {
        InputEventType type;
        Vector2 position;
        public InputEventType Type { get => type; }
        public Vector2 Position { get => position; }
        public bool Ignored { get => EventSystem.current.IsPointerOverGameObject(); }
        public UnityMouseInputEvent(int id)
        {
            if (Input.GetMouseButtonDown(id))
                type = InputEventType.Begin;
            else if (Input.GetMouseButtonUp(id))
                type = InputEventType.End;
            else
                type = InputEventType.Move;
            position = Input.mousePosition;
        }
    }
    private int id;
    public bool IsEnable { get => true; }
    public IInputEvent Current { get => new UnityMouseInputEvent(id); }

    public UnityMouseInput(int id)
    {
        this.id = id;
    }
}


public class Swipe : MonoBehaviour
{
    public Toggle KeepLatestToggle;
    public Button DebugButton;
    public Button ClearButton;
    public ScrollableTextPanelView logPanel;

    IInput input;

    void log(string msg)
    {
        logPanel.WriteLine(msg);
    }

    void Start()
    {
#if UNITY_EDITOR
        input = new UnityMouseInput(0);
#elif UNITY_ANDROID || UNITY_IOS
        input = new UnityTouchInput();
#endif

        bool begin = false;
        var beginPos = Vector2.zero;
        var beginTime = DateTime.Now;
        var points = 0;

        this.UpdateAsObservable()
            .Where(_ => input.IsEnable)
            .Select(_ => input.Current)
            .Where(e => e.Type == InputEventType.Begin)
            .Where(e => !e.Ignored) // ignore on UI
            .Subscribe(e => {
                begin = true;
                beginPos = e.Position;
                beginTime = DateTime.Now;
                log($"<color=red>[0] {beginTime} {e.Type}: {e.Position}</color>");
                points = 1;
            });

        this.UpdateAsObservable()
            .Where(_ => begin && input.IsEnable)
            .Select(_ => input.Current)
            .Where(e => (e.Type != InputEventType.Begin))
            .Subscribe(e =>
            {
                if (e.Type == InputEventType.End || e.Type == InputEventType.Other)
                {
                    begin = false; // clear flag
                }
                log($"[{points}] {DateTime.Now} {e.Type}: {e.Position}");
                points++;
                if (e.Type == InputEventType.End)
                {
                    var vec = e.Position - beginPos;
                    log($"<color=red>Swpie: vec={vec}, {points} points, {(DateTime.Now - beginTime).Milliseconds} ms</color>");
                }
            });

        // Debug
        int count = 0;
        GameObject.Find("Button")?.GetComponent<Button>()?.OnClickAsObservable()
            .Subscribe(_ => { log($"button-{count}"); count++; });

        KeepLatestToggle.OnValueChangedAsObservable()
            .Subscribe(v => logPanel.keepLatest.Value = v);

        DebugButton.OnClickAsObservable()
            .Subscribe(_ => log("Debug"));

        ClearButton.OnClickAsObservable()
            .Subscribe(_ => logPanel.Clear());
    }
}
