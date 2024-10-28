using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : MonoBehaviour
{
    public static Printer instance;

    int printCounter = 0;

    private void Awake()
    {
        instance = this;
    }

    public void prt(string value)
    {
        printCounter++;
        print(printCounter + " " + value);
    }
}
