using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class TestCanvasView : MonoBehaviour
{
    public Toggle KeepLatestToggle;
    public Button DebugButton;
    public Button ClearButton;
    public ScrollableTextPanelView panelView;
    public InputField input;

    void log(string msg)
    {
        panelView.WriteLine(msg);
    }

    void Start()
    {
        // Debug
        int count = 0;
        GameObject.Find("Button")?.GetComponent<Button>()?.OnClickAsObservable()
            .Subscribe(_ => { log($"button-{count}"); count++; });

        KeepLatestToggle.OnValueChangedAsObservable()
            .Subscribe(v => panelView.keepLatest.Value = v);

        DebugButton.OnClickAsObservable()
            .Subscribe(_ => log("Debug"));

        ClearButton.OnClickAsObservable()
            .Subscribe(_ => panelView.Clear());

        input.OnEndEditAsObservable()
            .Subscribe(txt => {
                log($"> {txt}");
                input.text = "";
                input.ActivateInputField();
            });
    }
}
