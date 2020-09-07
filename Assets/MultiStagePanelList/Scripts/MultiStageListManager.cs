using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiStageListManager : MonoBehaviour
{
    public List<MultiStageList> lists = new List<MultiStageList>();

    void Start()
    {
        if (lists.Count > 0)
        {
            Select(lists[lists.Count - 1]);
        }
    }

    public void Select(MultiStageList list)
    {
        var i = lists.FindIndex(x => x == list);
        Debug.Log($"index={i}");
        lists[i].gameObject.SetActive(false);
        if (i + 1 < lists.Count)
        {
            lists[i + 1].gameObject.SetActive(true);
            Debug.Log($"enable {lists[i + 1].name}");
        }
        else
        {
            lists[0].gameObject.SetActive(true);
            Debug.Log($"enable {lists[0].name}");
        }
    }
}
