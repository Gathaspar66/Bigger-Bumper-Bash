using UnityEngine;
using UnityEngine.UI;

public class ControlsTouchPosition : MonoBehaviour
{
    public Image l;
    public Image r;
    public Image u;
    public Image d;

    public ControlHandler handler;

    private Bounds lb;
    private Bounds rb;
    private Bounds ub;
    private Bounds db;
    private void Start()
    {
        CalculateControlsBounds();
    }

    private void Update()
    {
        CheckTouch();
        KeyboardInput();
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

                if (RectTransformUtility.RectangleContainsScreenPoint(l.rectTransform, touch.position, null))
                {
                    l.color = new Color(1f, 1f, 1f, 1f);
                    handler.SetInput(SteeringDirection.LEFT);
                }

                if (RectTransformUtility.RectangleContainsScreenPoint(r.rectTransform, touch.position, null))
                {
                    r.color = new Color(1f, 1f, 1f, 1f);
                    handler.SetInput(SteeringDirection.RIGHT);
                }

                if (RectTransformUtility.RectangleContainsScreenPoint(u.rectTransform, touch.position, null))
                {
                    u.color = new Color(1f, 1f, 1f, 1f);
                    handler.SetInput(SteeringDirection.ACCELERATE);
                }

                if (RectTransformUtility.RectangleContainsScreenPoint(d.rectTransform, touch.position, null))
                {
                    d.color = new Color(1f, 1f, 1f, 1f);
                    handler.SetInput(SteeringDirection.BRAKE);
                }
            }
        }
    }

    private void KeyboardInput()
    {
        if (Input.GetKey(KeyCode.A))
        {

            l.color = new Color(1f, 1f, 1f, 1f);
            handler.SetInput(SteeringDirection.LEFT);
        }



        if (Input.GetKey(KeyCode.D))
        {
            r.color = new Color(1f, 1f, 1f, 1f);
            handler.SetInput(SteeringDirection.RIGHT);
        }

        if (Input.GetKey(KeyCode.W))
        {
            u.color = new Color(1f, 1f, 1f, 1f);
            handler.SetInput(SteeringDirection.ACCELERATE);
        }

        if (Input.GetKey(KeyCode.S))
        {
            d.color = new Color(1f, 1f, 1f, 1f);
            handler.SetInput(SteeringDirection.BRAKE);
        }
    }


    private void SetAllToNeutral()

    {
        handler.SetInput(SteeringDirection.NEUTRAL);
        handler.SetInput(SteeringDirection.FORWARD);
        l.color = new Color(1f, 1f, 1f, 0.5f);
        r.color = new Color(1f, 1f, 1f, 0.5f);
        u.color = new Color(1f, 1f, 1f, 0.5f);
        d.color = new Color(1f, 1f, 1f, 0.5f);
    }
}

