using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiStageList : MonoBehaviour
{
    MultiStageListManager manager;

    void Awake()
    {
        manager = GetComponentInParent<MultiStageListManager>();
    }

    public void Select(MultiStageListItem item)
    {
        manager.Select(this);
    }
}
