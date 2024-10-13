using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject effect;
    GameObject currentEffect;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnAnEffect(Vector3 location, bool isPositive = true)
    {
        if(currentEffect != null)
        {
            Destroy(currentEffect);
        }
        currentEffect = Instantiate(effect, location, Quaternion.identity);
        currentEffect.transform.parent = Camera.main.transform;
        currentEffect.GetComponent<Effect>().Activate(isPositive);
    }
}
