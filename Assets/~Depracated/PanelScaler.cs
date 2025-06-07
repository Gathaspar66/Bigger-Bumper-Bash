using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelScaler : MonoBehaviour
{
    RectTransform panel;


    // Start is called before the first frame update
    void Start()
    {
        panel = gameObject.GetComponent<RectTransform>();
        float scale = Screen.height /2.0f / 300.0f;
        panel.localScale = scale * Vector3.one;
        panel.anchoredPosition = panel.anchoredPosition * scale;
    }
}
