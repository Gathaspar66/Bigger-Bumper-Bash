using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectDEPRECATED: MonoBehaviour
{
    List<string> positiveWords = new List<string>
        { "Bash!", "Smash!", "Wham!", "Boom!", "Whack!", "Bam!", "Slam!", "Bang!", "Pound!" };

    List<string> negativeWords = new List<string> { "Crash!", "Thrash!" };

    public List<GameObject> backgrounds;
    public TMP_Text text;


    bool active = false;
    bool isPositive = true;
    float scaleSpeed = 5;
    float waitForDestroy = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (transform.localScale.x >= 2.0f)
            {
                waitForDestroy -= Time.deltaTime;
                if (waitForDestroy <= 0)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                transform.localScale += transform.localScale * (1.1f * Time.deltaTime * scaleSpeed);
                transform.localPosition += Time.deltaTime * new Vector3(0, 10, 0);
            }
        }
    }

    public void Activate(bool isPositive)
    {
        this.isPositive = isPositive;
        RandomizeBackground();
        RandomizeText();
        active = true;
    }

    void RandomizeBackground()
    {
        int choice = Random.Range(0, backgrounds.Count);
        backgrounds[choice].SetActive(true);
    }

    void RandomizeText()
    {
        string textToInsert;
        if (isPositive)
        {
            int choice = Random.Range(0, positiveWords.Count);
            textToInsert = positiveWords[choice];
            text.color = Color.yellow;
        }
        else
        {
            int choice = Random.Range(0, negativeWords.Count);
            textToInsert = negativeWords[choice];
            text.color = Color.red;
        }

        text.text = textToInsert;
    }
}