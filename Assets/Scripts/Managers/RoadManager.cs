using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public static RoadManager instance;

    public GameObject[] normalSectionsPrefabs;
    public GameObject[] specificSectionsPrefabs;

    public GameObject[] allSectionsPrefabs;
    public int normalSectionsMultiplier = 1;
    private readonly GameObject[] sectionsPool = new GameObject[20];
    private readonly GameObject[] sections = new GameObject[3];
    private Transform playerCarTransform;
    private readonly WaitForSeconds waitFor100ms = new(0.1f);
    private const float sectionLength = 60;

    private void Awake()
    {
        instance = this;

        List<GameObject> tempList = new();

        for (int i = 0; i < normalSectionsMultiplier; i++)
        {
            tempList.AddRange(normalSectionsPrefabs);
        }

        tempList.AddRange(specificSectionsPrefabs);
        allSectionsPrefabs = tempList.ToArray();
    }

    public void Activate()
    {
        playerCarTransform = PlayerManager.instance.GetPlayerInstance().transform;
        SetStartPosition();

        _ = StartCoroutine(UpdateLessOftenCO());
    }

    public void SetStartPosition()
    {
        for (int i = 0; i < sectionsPool.Length; i++)
        {
            int randomPrefabIndex = Random.Range(0, allSectionsPrefabs.Length);
            sectionsPool[i] = Instantiate(allSectionsPrefabs[randomPrefabIndex]);
            sectionsPool[i].SetActive(false);
        }

        for (int i = 0; i < sections.Length; i++)
        {
            GameObject randomSection = GetRandomSectionFromPool();
            randomSection.transform.position = new Vector3(0, 0, i * sectionLength);
            randomSection.SetActive(true);
            sections[i] = randomSection;
        }
    }

    private IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            UpdateSectionPositions();
            yield return waitFor100ms;
        }
    }

    private void UpdateSectionPositions()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            if (sections[i].transform.position.z - playerCarTransform.position.z < -sectionLength)
            {
                Vector3 lastSectionPosition = sections[i].transform.position;
                sections[i].SetActive(false);

                sections[i] = GetRandomSectionFromPool();
                sections[i].transform.position = new Vector3(lastSectionPosition.x, 0,
                    lastSectionPosition.z + (sectionLength * sections.Length));
                sections[i].SetActive(true);
            }
        }
    }

    private GameObject GetRandomSectionFromPool()
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
                if (randomIndex >= sectionsPool.Length)
                {
                    randomIndex = 0;
                }
            }
        }

        return sectionsPool[randomIndex];
    }
}