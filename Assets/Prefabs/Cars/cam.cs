using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{

    public List<GameObject> points;

    int currentPoint = 0;

    Vector3 start, end;
    Quaternion startR, endR;
    float lerpCounter = 0;
    float speed = 0.5f;

    bool snap = false;

    // Start is called before the first frame update
    void Start()
    {
        start = points[currentPoint].transform.position;
        end = points[currentPoint + 1].transform.position;
        startR = points[currentPoint].transform.rotation;
        endR = points[currentPoint + 1].transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (snap)
        {
            lerpCounter = 1;
        }
        lerpCounter += Time.deltaTime * speed;
        gameObject.transform.position = Vector3.Lerp(start, end, lerpCounter);
        gameObject.transform.rotation = Quaternion.Lerp(startR, endR, lerpCounter);
        if (lerpCounter >= 1)
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        lerpCounter = 0;
        if (currentPoint >= points.Count - 1)
        {
            currentPoint = 0;
        }
        else
        {
            currentPoint++;
        }
        start = gameObject.transform.position;
        end = points[currentPoint].transform.position;
        startR = gameObject.transform.rotation;
        endR = points[currentPoint].transform.rotation;
        snap = !snap;
    }
}
