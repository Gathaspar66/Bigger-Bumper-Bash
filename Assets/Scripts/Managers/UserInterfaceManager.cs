using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    public GameObject fakeButtonsJoystick;

    void Start()
    {
        Instantiate(fakeButtonsJoystick, new Vector3(0, 0, 0), Quaternion.identity);
    }
}