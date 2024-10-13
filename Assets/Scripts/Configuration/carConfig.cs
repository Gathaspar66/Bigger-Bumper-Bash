using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carConfig : MonoBehaviour
{
    [System.Serializable]
    public class CarConfig
    {
        public float acceleationMultiplier;
        public float brakeMultiplier;
        public float steeringMultiplier;
        public float maxForwardVelocity;
        public float maxSteerVelocity;
        public float minForwardVelocity;
    }

    [System.Serializable]
    public class CarConfigDictionary
    {
        public CarConfig Unikacz;
        public CarConfig Auto2;
    }
}