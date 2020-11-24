using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using uGUI.Scroll;

public class ComplexContent : MonoBehaviour
{
    IScrollRectContent content = null;

    [SerializeField] Toggle toggle = default;

    void Start()
    {
        content = GetComponent<IScrollRectContent>();

        toggle.OnValueChangedAsObservable().Subscribe(v => {
            Debug.Log($"{this.name} checked. {v}");
        }).AddTo(this);
    }
}
