using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FakeJoystickSteering : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform Background;
    public RectTransform Handle;
    public RectTransform center;
    bool left = false;
    bool right = false;


    public void OnPointerDown(PointerEventData eventData)
    {
       
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchLocation = eventData.position;
        if (touchLocation.x > center.position.x)
        {
            left = false;
            right = true;
        }
        else
        {
            left = true;
            right = false;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        left = false;
        right = false;
    }

    public int GetInput()
    {
        int result = 0;
        if (left) result = -1;
        if (right) result = 1;
        return result;
    }
}
