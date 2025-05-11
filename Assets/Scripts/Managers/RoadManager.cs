using System.Collections;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public static RoadManager instance;

    [Header("Prefabs")]
    public GameObject[] neutralSectionsPrefabs;
    public GameObject[] specialSectionsPrefabs;

    [Header("Spawn Chance")]
    [Range(0f, 100f)]
    public float neutralSpawnChance = 95f;

    private readonly GameObject[] sectionsPool = new GameObject[20];
    private readonly GameObject[] sections = new GameObject[4];
    private Transform playerCarTransform;
    private readonly WaitForSeconds waitFor100ms = new(0.1f);
    private const float sectionLength = 60;

    private void Awake()
    {
        instance = this;
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
            GameObject prefabToUse = GetRandomPrefabByChance();
            sectionsPool[i] = Instantiate(prefabToUse);
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

                if (randomIndex > sectionsPool.Length - 1)
                {
                    randomIndex = 0;
                }
            }
        }

        return sectionsPool[randomIndex];
    }

    private GameObject GetRandomPrefabByChance()
    {
        float randomValue = Random.Range(0f, 100f);
        GameObject[] sourceArray = (randomValue <= neutralSpawnChance)
            ? neutralSectionsPrefabs
            : specialSectionsPrefabs;

        int randomIndex = Random.Range(0, sourceArray.Length);
        return sourceArray[randomIndex];
    }
}
