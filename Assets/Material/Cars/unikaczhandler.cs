using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unikaczhandler : MonoBehaviour
{
    public GameObject maska, maskabroken, spoiler, spoilerbroken, bumperF, bumperFbroken;
    public ParticleSystem fire, smoke;

    float t = 0f;
    bool a = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!a) return;

        t = Mathf.PingPong(Time.time * 1, 1);

        spoilerbroken.transform.rotation = Quaternion.Lerp(
            new Quaternion(-0.62327f, 0.00979f, -0.11237f, 0.77383f),
            new Quaternion(-0.63280f, -0.13526f, -0.09353f, 0.75665f),
            t);
        bumperFbroken.transform.rotation = Quaternion.Lerp(
            new Quaternion(-0.62327f, 0.00979f, -0.11237f, 0.77383f),
            new Quaternion(-0.63280f, -0.13526f, -0.09353f, 0.75665f),
            t);
    }

    public void SetBroken(int level)
    {
        print(PlayerManager.instance.GetPlayerHealth());
        switch (level)
        {
            case -4:
            case -3:
            case -2:
            case -1:
            case 0 :
                maska.SetActive(false);
                maskabroken.SetActive(true);
                maskabroken.transform.localPosition = new Vector3(0, -0.19f, 0.827f);
                maskabroken.transform.localRotation = Quaternion.Euler(new Vector3(-146, 0, 0));
                spoiler.SetActive(false);
                spoilerbroken.SetActive(true); 
                bumperF.SetActive(false); 
                bumperFbroken.SetActive(true);
                fire.Play();
                smoke.gameObject.transform.rotation = Quaternion.Euler(new Vector3(
                    0, 0, 0));
                smoke.Play();
                a = false;
                ParticleSystem.VelocityOverLifetimeModule chuj = smoke.velocityOverLifetime;

                chuj.y = 20;
                chuj.z = 0;

                break;

            case 1:
                maska.SetActive(false);
                maskabroken.SetActive(true);
                maskabroken.transform.localPosition = new Vector3(0, -0.19f, 0.234f);
                maskabroken.transform.localRotation = Quaternion.Euler(new Vector3(-105, 0, 0));
                spoiler.SetActive(false);
                spoilerbroken.SetActive(true);
                bumperF.SetActive(false);
                bumperFbroken.SetActive(true);
                fire.Stop();
                smoke.Play();
                break;

            case 2:
                maska.SetActive(true);
                maskabroken.SetActive(false);
                spoiler.SetActive(false);
                spoilerbroken.SetActive(true);
                bumperF.SetActive(false);
                bumperFbroken.SetActive(true);
                fire.Stop();
                smoke.Stop();
                break;

            default:
                maska.SetActive(true);
                maskabroken.SetActive(false);
                spoiler.SetActive(true);
                spoilerbroken.SetActive(false);
                bumperF.SetActive(true);
                bumperFbroken.SetActive(false);
                fire.Stop();
                smoke.Stop();
                break;

        }
    }
}
