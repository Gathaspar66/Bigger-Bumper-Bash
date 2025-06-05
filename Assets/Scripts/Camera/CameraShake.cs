using UnityEngine;
public class CameraShake : MonoBehaviour
{
    private Vector3 shakeOffset = Vector3.zero;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;
    public static CameraShake Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            shakeOffset = new Vector3(x, y, 0);

            shakeDuration -= Time.unscaledDeltaTime;
        }
        else
        {
            shakeOffset = Vector3.zero;
        }
    }

    public Vector3 GetShakeOffset()
    {
        return shakeOffset;
    }

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}