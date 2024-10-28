using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brumbrum : MonoBehaviour
{
    public List<GameObject> points;
    public GameObject car;

    int lastPoint = 3;
    int nextPoint = 0;

    float lerpCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lerpCounter >= 1)
        //if (Vector3.Distance(points[lastPoint].transform.position, points[nextPoint].transform.position) < 0.1f)
        {
            lerpCounter = 0;
            if (nextPoint == points.Count - 1)
            {
                nextPoint = 0;
                lastPoint = 3;
            }
            else
            {
                nextPoint++;

                lastPoint = nextPoint - 1;
            }
        }
        car.transform.LookAt(points[nextPoint].transform.position);
        lerpCounter += Time.deltaTime;
        car.transform.position = Vector3.Lerp(points[lastPoint].transform.position, points[nextPoint].transform.position, lerpCounter);
    }
}
