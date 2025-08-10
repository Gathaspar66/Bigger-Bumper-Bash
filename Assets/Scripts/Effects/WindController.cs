using UnityEngine;

public class WindController : MonoBehaviour
{
    public ParticleSystem particles;
    public float maxSpeed;

    private void Start()
    {
        if (particles == null)
        {
            particles = GetComponent<ParticleSystem>();
        }
        maxSpeed = PlayerSteering.instance.maxForwardVelocity;
    }

    private void Update()
    {
        if (PlayerSteering.instance == null)
        {
            return;
        }

        float currentSpeed = PlayerSteering.instance.rb.velocity.z;

        if (currentSpeed >= maxSpeed)
        {
            if (!particles.isPlaying)
            {
                particles.Play();
            }
        }
        else
        {
            if (particles.isPlaying)
            {
                particles.Stop();
            }
        }
    }
}