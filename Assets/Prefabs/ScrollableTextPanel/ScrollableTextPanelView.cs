using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ScrollableTextPanelView : MonoBehaviour
{
    [SerializeField]
    public BoolReactiveProperty keepLatest = new BoolReactiveProperty();

    ScrollRect scrollRect;
    Scrollbar scrollbar;
    Text text;
    StringBuilder stringBuilder = new StringBuilder(4096);

    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollbar = scrollRect.verticalScrollbar;
        text = GetComponentInChildren<Text>();
    }

    void Start()
    {
        scrollRect.OnValueChangedAsObservable()
            .Where(_ => keepLatest.Value)
            .Subscribe(_ => {
                scrollRect.verticalNormalizedPosition = 0;
            }).AddTo(this);

        keepLatest.SkipLatestValueOnSubscribe().Subscribe(v => {
            scrollbar.interactable = !v;
            if(v) {
                scrollRect.verticalNormalizedPosition = 0;
            }
        });
    }

    public void WriteLine(string msg) 
    {
        // text.text += $"{msg}\n";
        stringBuilder.Append(msg);
        stringBuilder.Append("\n");
        text.text = stringBuilder.ToString();
    }

    public void Clear()
    {
        text.text = "";
        stringBuilder.Clear();
        text.SetAllDirty();
    }
}
