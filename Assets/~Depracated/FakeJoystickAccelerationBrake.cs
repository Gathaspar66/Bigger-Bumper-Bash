using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FakeJoystickAccelerationBrake : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform Background;
    public RectTransform Handle;
    public RectTransform center;
    bool accelerate = false;
    bool brake = false;

    public FakeJoystickSteering steering;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchLocation = eventData.position;
        if (touchLocation.x > center.position.x)
        {
            accelerate = false;
            brake = true;
        }
        else
        {
            accelerate = true;
            brake = false;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        accelerate = false;
        brake = false;
    }

    public Vector2 GetInput()
    {
        Vector2 inputVector = Vector2.zero;
        if (accelerate) inputVector.y = 1;
        if (brake) inputVector.y = -1;
        inputVector.x = steering.GetInput();
        return inputVector;
    }
}
