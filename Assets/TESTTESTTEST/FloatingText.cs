using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FloatingText : MonoBehaviour
{
    public TMP_Text text;
    public Vector3 startLocation;
    public Vector3 targetLocation;
    public Vector3 offset;
    public GameObject targetFollow;
    public float speed = 1;
    public float lerpTracker = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        FloatText();
    }

    private void FloatText()
    {
        if (lerpTracker > 1) Destroy(gameObject);
        lerpTracker += Time.deltaTime * speed;
        targetLocation = Vector3.Lerp(Vector3.zero, Vector3.up, lerpTracker);
        targetLocation += targetFollow.transform.position + offset;
        transform.position = targetLocation;
    }

    public void SetFloatingText(string message, GameObject target, Vector3 offset)
    {
        text.text = message;
        targetFollow = target;
        this.offset = offset;
    }
}
