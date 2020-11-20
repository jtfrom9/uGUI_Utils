using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiStageListItem : MonoBehaviour
{
    MultiStageList parentList;
    CanvasRenderer _renderer;

    void Awake()
    {
        parentList = GetComponentInParent<MultiStageList>();
    }

    public void OnSelect()
    {
        Debug.Log($"{name}");
        parentList.Select(this);
    }
}
