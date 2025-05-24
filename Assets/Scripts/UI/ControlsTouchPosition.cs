using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsTouchPosition : MonoBehaviour
{
    public Image l;
    public Image r;
    public Image u;
    public Image d;

    public Image marker;

    public ControlHandler handler;

    Bounds lb;
    Bounds rb;
    Bounds ub;
    Bounds db;

    private void Start()
    {
        CalculateControlsBounds();
    }

    private void Update()
    {
        CheckTouch();
        CheckMouseInput();
    }

    private void CalculateControlsBounds()
    {
        lb = new Bounds(l.rectTransform.position, l.rectTransform.sizeDelta);
        rb = new Bounds(r.rectTransform.position, r.rectTransform.sizeDelta);
        ub = new Bounds(u.rectTransform.position, u.rectTransform.sizeDelta);
        db = new Bounds(d.rectTransform.position, d.rectTransform.sizeDelta);
    }

    private void CheckTouch()
    {
        SetAllToNeutral();
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (lb.Contains(touch.position))
                {
                    l.color = new Color(1f, 1f, 1f, 1f);
                    handler.SetInput(SteeringDirection.LEFT);
                }

                if (rb.Contains(touch.position))
                {
                    r.color = new Color(1f, 1f, 1f, 1f);
                    handler.SetInput(SteeringDirection.RIGHT);
                }

                if (ub.Contains(touch.position))
                {
                    u.color = new Color(1f, 1f, 1f, 1f);
                    handler.SetInput(SteeringDirection.ACCELERATE);
                }

                if (db.Contains(touch.position))
                {
                    d.color = new Color(1f, 1f, 1f, 1f);
                    handler.SetInput(SteeringDirection.BRAKE);
                }
                marker.rectTransform.position = touch.position;
            }

        }
    }

    private void CheckMouseInput()
    {
        SetAllToNeutral();
        Vector2 mouse = Input.mousePosition;
        if (lb.Contains(mouse))
        {
            l.color = new Color(1f, 1f, 1f, 1f);
            handler.SetInput(SteeringDirection.LEFT);
        }

        if (rb.Contains(mouse))
        {
            r.color = new Color(1f, 1f, 1f, 1f);
            handler.SetInput(SteeringDirection.RIGHT);
        }

        if (ub.Contains(mouse))
        {
            u.color = new Color(1f, 1f, 1f, 1f);
            handler.SetInput(SteeringDirection.ACCELERATE);
        }

        if (db.Contains(mouse))
        {
            d.color = new Color(1f, 1f, 1f, 1f);
            handler.SetInput(SteeringDirection.BRAKE);
        }
    }

    void SetAllToNeutral()
    {
        handler.SetInput(SteeringDirection.NEUTRAL);
        handler.SetInput(SteeringDirection.FORWARD);
        l.color = new Color(1f, 1f, 1f, 0.5f);
        r.color = new Color(1f, 1f, 1f, 0.5f);
        u.color = new Color(1f, 1f, 1f, 0.5f);
        d.color = new Color(1f, 1f, 1f, 0.5f);
        marker.rectTransform.position = new Vector2(Screen.width/2, Screen.height/2);
    }
}
