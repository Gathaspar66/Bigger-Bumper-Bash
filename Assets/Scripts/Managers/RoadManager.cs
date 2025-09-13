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
    Queue<GameObject> sectionsQueue = new();
    int sectionsCount = 3;
    private Transform playerCarTransform;
    private readonly WaitForSeconds waitFor100ms = new(0.1f);
    private const float sectionLength = 60;
    private bool isGameOver = false;

    public BoxCollider left, right;

    private void Awake()
    {
        instance = this;
        SetupInAwake();
    }

    private void SetupInAwake()
    {
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
        //road segments setup
        for (int i = 0; i < sectionsPool.Length; i++)
        {
            int randomPrefabIndex = Random.Range(0, allSectionsPrefabs.Length);
            sectionsPool[i] = Instantiate(allSectionsPrefabs[randomPrefabIndex]);
            sectionsPool[i].SetActive(false);
        }
        
        //first road segments spawn
        for (int i = 0; i < sectionsCount; i++)
        {
            GameObject randomSection = GetRandomSectionFromPool();
            randomSection.transform.position = new Vector3(0, 0, i * sectionLength);
            randomSection.SetActive(true);
            sectionsQueue.Enqueue(randomSection);
        }
    }

    private IEnumerator UpdateLessOftenCO()
    {
        while (!isGameOver)
        {
            UpdateSectionPositions();
            yield return waitFor100ms;
        }
    }

    private void UpdateSectionPositions()
    {
        GameObject currentLastSegment = sectionsQueue.Peek();
        if (currentLastSegment.transform.position.z - playerCarTransform.position.z < -sectionLength)
        {
            Vector3 lastSectionPosition = currentLastSegment.transform.position;
            currentLastSegment.SetActive(false);

            sectionsQueue.Dequeue();
            GameObject newSegment = GetRandomSectionFromPool();
            sectionsQueue.Enqueue(newSegment);

            newSegment.transform.position = new Vector3(lastSectionPosition.x, 0,
                lastSectionPosition.z + (sectionLength * sectionsCount));
            newSegment.SetActive(true);
        }
        UpdateBarrierColliders();
    }

    void UpdateBarrierColliders()
    {
        left.center = new Vector3(left.center.x, left.center.y, sectionsQueue.Peek().transform.position.z + left.size.z / 3);
        right.center = new Vector3(right.center.x, right.center.y, sectionsQueue.Peek().transform.position.z + right.size.z / 3);
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

    public void OnPlayerDeath()
    {
        isGameOver = true;
    }
}