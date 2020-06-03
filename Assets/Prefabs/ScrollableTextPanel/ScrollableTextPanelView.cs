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
        text.text += $"{msg}\n";
    }

    public void Clear()
    {
        text.text = "";
        text.SetAllDirty();
    }
}
