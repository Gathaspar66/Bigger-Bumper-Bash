using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlsOnPointerexit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image img;
    public ControlHandler handler;

    public SteeringDirection direction;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        img.color = new Color(1f, 1f, 1f, 1f);
        handler.SetInput(direction);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.color = new Color(1f, 1f, 1f, 0.3f);
        if (direction == SteeringDirection.LEFT || direction == SteeringDirection.RIGHT)
        {
            handler.SetInput(SteeringDirection.FORWARD);
        }
        else
        {
            handler.SetInput(SteeringDirection.NEUTRAL);
        }
    }
}
