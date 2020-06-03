using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

public class Swipe : MonoBehaviour
{
    public Toggle KeepLatestToggle;
    public Button DebugButton;
    public Button ClearButton;
    public ScrollableTextPanelView logPanel;

    void log(string msg)
    {
        logPanel.WriteLine(msg);
    }

    void Start()
    {
        bool begin = false;
        var beginPos = Vector2.zero;
        var beginTime = DateTime.Now;
        var points = 0;

        this.UpdateAsObservable()
            .Where(_ => Input.touchCount > 0)
            .Select(_ => Input.GetTouch(0))
            .Where(touch => touch.phase == TouchPhase.Began)
            .Where(touch => !EventSystem.current.IsPointerOverGameObject(touch.fingerId)) // ignore on UI
            .Subscribe(touch => {
                begin = true;
                beginPos = touch.position;
                beginTime = DateTime.Now;
                log($"<color=red>[0] {beginTime} {touch.phase}: {touch.position}</color>");
                points = 1;
            });

        this.UpdateAsObservable()
            .Where(_ => begin && Input.touchCount > 0)
            .Select(_ => Input.GetTouch(0))
            .Where(touch => (touch.phase != TouchPhase.Began))
            .Subscribe(touch =>
            {
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    begin = false; // clear flag
                }
                log($"[{points}] {DateTime.Now} {touch.phase}: {touch.position}");
                points++;
                if (touch.phase == TouchPhase.Ended)
                {
                    var vec = touch.position - beginPos;
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
