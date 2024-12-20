using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadManager : MonoBehaviour
{
    public static RoadManager instance;

    public GameObject[] sectionsPrefabs;
    GameObject[] sectionsPool = new GameObject[20];
    GameObject[] sections = new GameObject[20];
    Transform playerCarTransform;

    WaitForSeconds waitFor100ms = new WaitForSeconds(0.1f);
    const float sectionLength = 30;

    private void Awake()
    {
        instance = this;
    }

    public void Activate()
    {
        playerCarTransform = PlayerManager.instance.GetPlayerInstance().transform;
        SetStartPosition();

        StartCoroutine(UpdateLessOftenCO());
    }

    public void SetStartPosition()
    {
        int prefabIndex = 0;


        for (int i = 0; i < sectionsPool.Length; i++)
        {
            sectionsPool[i] = Instantiate(sectionsPrefabs[prefabIndex]);
            sectionsPool[i].SetActive(false);
            prefabIndex++;

            if (prefabIndex > sectionsPrefabs.Length - 1)
            {
                prefabIndex = 0;
            }
        }


        for (int i = 0; i < sections.Length; i++)
        {
            GameObject randomSection = GetRandomSectionFromPool();
            randomSection.transform.position = new Vector3(sectionsPool[i].transform.position.x, 0, i * sectionLength);
            randomSection.SetActive(true);
            sections[i] = randomSection;
        }
    }

    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            UpdateSectionPositions();
            yield return waitFor100ms;
        }
    }

    void UpdateSectionPositions()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            if (sections[i].transform.position.z - playerCarTransform.position.z < -sectionLength)
            {
                Vector3 lastSectionPosition = sections[i].transform.position;
                sections[i].SetActive(false);

                sections[i] = GetRandomSectionFromPool();
                sections[i].transform.position = new Vector3(lastSectionPosition.x, 0,
                    lastSectionPosition.z + sectionLength * sections.Length);
                sections[i].SetActive(true);
            }
        }
    }

    GameObject GetRandomSectionFromPool()
    {
        int randomIndex = Random.Range(0, sectionsPool.Length);
        bool isNewSectionFound = false;


        while (!isNewSectionFound)
        {
            if (!sectionsPool[randomIndex].activeInHierarchy)
            {
                isNewSectionFound = true;
            }
            else
            {
                randomIndex++;

                if (randomIndex > sectionsPool.Length - 1)
                {
                    randomIndex = 0;
                }
            }
        }

        return sectionsPool[randomIndex];
    }
}