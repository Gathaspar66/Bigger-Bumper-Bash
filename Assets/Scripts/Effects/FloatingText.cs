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
        RotateTowardsCamera();
    }

    private void RotateTowardsCamera()
    {
        transform.rotation = Camera.main.transform.rotation;
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
        SetFloatingText(message);
        SetFloatingText(target);
        SetFloatingText(offset);
    }

    public void SetFloatingText(string message)
    {
        text.text = message;
    }

    public void SetFloatingText(GameObject target)
    {
        targetFollow = target;
    }

    public void SetFloatingText(Vector3 offset)
    {
        this.offset = offset;
    }
}
