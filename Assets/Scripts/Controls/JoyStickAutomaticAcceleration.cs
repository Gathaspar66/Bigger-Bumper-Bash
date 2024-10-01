using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickAutomaticAcceleration : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform Background;
    public RectTransform Handle;
    private Vector2 inputVector = Vector2.zero;
    private Vector2 initialTouchPosition = Vector2.zero;
    public bool isTouching = false;

    public static JoyStickAutomaticAcceleration Instance { get; private set; }

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
        isTouching = true;
        Background.gameObject.SetActive(true);
        initialTouchPosition = eventData.position;
        Background.position = eventData.position;
        Handle.anchoredPosition = Vector2.zero;
        OnDrag(eventData);

        inputVector.y = 1;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        isTouching = false;
        Background.gameObject.SetActive(false);


        inputVector.y = -1;


        Handle.anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchDirection = eventData.position - initialTouchPosition;
        float joystickRadius = Background.sizeDelta.x / 2f;


        float inputX = touchDirection.x;
        inputVector = new Vector2(
            (Mathf.Abs(inputX) > joystickRadius) ? Mathf.Sign(inputX) : (inputX / joystickRadius),
            0f
        );


        Handle.anchoredPosition = inputVector * joystickRadius;
        inputVector.y = 1;
    }

    public Vector2 GetInput()
    {
        return inputVector;
    }
}