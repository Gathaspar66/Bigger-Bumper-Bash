using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class testpointerhandler : MonoBehaviour
{
    public Image img;
    public Image marker;

    public Image l;
    public Image r;
    public Image u;
    public Image d;


    Bounds lb; //= new Bounds(l.rectTransform.position, l.rectTransform.sizeDelta);
    Bounds rb; //= new Bounds(r.rectTransform.position, r.rectTransform.sizeDelta);
    Bounds ub; //= new Bounds(u.rectTransform.position, u.rectTransform.sizeDelta);
    Bounds db;// = new Bounds(d.rectTransform.position, d.rectTransform.sizeDelta);

    private void Start()
    {
        lb = new Bounds(l.rectTransform.position, l.rectTransform.sizeDelta);
        rb = new Bounds(r.rectTransform.position, r.rectTransform.sizeDelta);
        ub = new Bounds(u.rectTransform.position, u.rectTransform.sizeDelta);
        db = new Bounds(d.rectTransform.position, d.rectTransform.sizeDelta);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            marker.rectTransform.position = touch.position;
            if (lb.Contains(touch.position))
            {
                l.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                l.color = new Color(1f, 1f, 1f, 0.5f);
            }

            if (rb.Contains(touch.position))
            {
                r.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                r.color = new Color(1f, 1f, 1f, 0.5f);
            }

            if (ub.Contains(touch.position))
            {
                u.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                u.color = new Color(1f, 1f, 1f, 0.5f);
            }

            if (db.Contains(touch.position))
            {
                d.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                d.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
        else
        {
            Debug.Log("No touch contacts");
        }
    }
}
