using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace uGUI.Scroll
{
    public class NameBind : MonoBehaviour
    {
        void Start()
        {
            var textUI = GetComponentInChildren<Text>();
            if(textUI) {
                textUI.text = gameObject.name;
            }
        }
    }
}
