using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessLevelHandler : MonoBehaviour
{
    public GameObject[] sectionsPrefabs;
    GameObject[] sectionsPool = new GameObject[20];
    GameObject[] sections = new GameObject[20];
    Transform playerCarTransform;

    WaitForSeconds waitFor100ms = new WaitForSeconds(0.1f);
    const float sectionLength = 26;

    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;
        int prefabIndex = 0;

        // Tworzenie sekcji w puli
        for (int i = 0; i < sectionsPool.Length; i++)
        {
            sectionsPool[i] = Instantiate(sectionsPrefabs[prefabIndex]);
            sectionsPool[i].SetActive(false); // Sekcje w puli s¹ dezaktywowane
            prefabIndex++;

            if (prefabIndex > sectionsPrefabs.Length - 1)
            {
                prefabIndex = 0;
            }
        }

        // Aktywowanie pocz¹tkowych sekcji i ustawianie pozycji
        for (int i = 0; i < sections.Length; i++)
        {
            GameObject randomSection = GetRandomSectionFromPool();
            randomSection.transform.position = new Vector3(sectionsPool[i].transform.position.x, 0, i * sectionLength);
            randomSection.SetActive(true); // Uaktywnienie sekcji
            sections[i] = randomSection;
        }

        StartCoroutine(UpdateLessOftenCO());
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
                sections[i].transform.position = new Vector3(lastSectionPosition.x, 0, lastSectionPosition.z + sectionLength * sections.Length);
                sections[i].SetActive(true);
            }
        }
    }

    GameObject GetRandomSectionFromPool()
    {
        int randomIndex = Random.Range(0, sectionsPool.Length);
        bool isNewSectionFound = false;

        // Znalezienie nowej sekcji do aktywacji
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
