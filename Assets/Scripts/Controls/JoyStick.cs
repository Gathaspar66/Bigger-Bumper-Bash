using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform Background; 
    public RectTransform Handle; 
    private Vector2 inputVector = Vector2.zero; 

    private Vector2 initialTouchPosition = Vector2.zero; 

    public static JoyStick Instance { get; private set; } 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Background.gameObject.SetActive(true); 
        initialTouchPosition = eventData.position; 
        Background.position = eventData.position; 
        Handle.anchoredPosition = Vector2.zero; 
        OnDrag(eventData); 
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchDirection = eventData.position - initialTouchPosition; 
        float joystickRadius = Background.sizeDelta.x / 2f; 

        // Calculate the input vector
        inputVector = (touchDirection.magnitude > joystickRadius)
            ? touchDirection.normalized
            : touchDirection / joystickRadius;

        Handle.anchoredPosition = inputVector * joystickRadius; 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Background.gameObject.SetActive(false); 
        inputVector = Vector2.zero; 
        Handle.anchoredPosition = Vector2.zero; 
    }

  
    public float Horizontal
    {
        get { return inputVector.x; }
    }

    public float Vertical
    {
        get { return inputVector.y; }
    }


    public Vector2 GetInput()
    {
        return inputVector;
    }
}
