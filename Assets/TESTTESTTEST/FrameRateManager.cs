using System.Collections;

using System.Threading;

using UnityEngine;

public class FrameRateManager : MonoBehaviour

{

    [Header("Frame Settings")]
    private readonly int MaxRate = 9999;

    public float TargetFrameRate = 120.0f;
    private float currentFrameTime;

    private void Awake()

    {

        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = MaxRate;

        currentFrameTime = Time.realtimeSinceStartup;

        _ = StartCoroutine("WaitForNextFrame");

    }

    private IEnumerator WaitForNextFrame()

    {

        while (true)

        {

            yield return new WaitForEndOfFrame();

            currentFrameTime += 1.0f / TargetFrameRate;

            float t = Time.realtimeSinceStartup;

            float sleepTime = currentFrameTime - t - 0.01f;

            if (sleepTime > 0)
            {
                Thread.Sleep((int)(sleepTime * 1000));
            }

            while (t < currentFrameTime)
            {
                t = Time.realtimeSinceStartup;
            }
        }

    }

}