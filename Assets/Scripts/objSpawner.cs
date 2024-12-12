using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objSpawner : MonoBehaviour
{
    public GameObject obj;
    public Transform[] spawnPoints; // Array of spawn points (replaces individual Transform variables)
    public bool spawningbool = true;
    public float spawnTime = 5f;
    public int maxObjects = 5; // Maximum number of objects allowed at once

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private bool[] isOccupied; // Array to track occupied spawn points
    private int totalMeatCount = 0;

    public GameObject enemyPrefab; // Assign your enemy prefab in the inspector
    private bool enemySpawned = false;

    void Start()
    {
        // Initialize the occupied array to match the number of spawn points
        isOccupied = new bool[spawnPoints.Length];
        StartCoroutine(spawning());
    }
    void Update()
    {
        //int currentMeatCount = GameObject.FindGameObjectsWithTag("Meat").Length;

        if (totalMeatCount >= 5 && !enemySpawned)
        {
            SpawnEnemy();
        }
    }

    IEnumerator spawning()
    {
        //only spawns objects if there are less than the max amount I want on the field
        while (spawningbool)
        {
            if (spawnedObjects.Count < maxObjects)
            {
                yield return new WaitForSeconds(spawnTime);
                SpawnObject();
                if(totalMeatCount < 5){
                    totalMeatCount ++;
                }
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    void SpawnObject()
    {
        // Find available spawn points
        List<int> availablePoints = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!isOccupied[i]) availablePoints.Add(i);
        }

        // If there are available spawn points, spawn an object
        if (availablePoints.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePoints.Count);
            int spawnIndex = availablePoints[randomIndex];
            Transform spawnLocation = spawnPoints[spawnIndex];

            GameObject newObject = Instantiate(obj, spawnLocation.position, spawnLocation.rotation);
            spawnedObjects.Add(newObject);

            // Mark the spawn point as occupied
            isOccupied[spawnIndex] = true;

            // Get the MeatPickup component and subscribe to its OnPickedUp event
            MeatPickup pickupComponent = newObject.GetComponent<MeatPickup>();
            if (pickupComponent != null)
            {
                pickupComponent.OnPickedUp += () => OnObjectPickedUp(newObject, spawnIndex);
            }
        }
    }


    void OnObjectPickedUp(GameObject pickedObject, int spawnIndex)
    {
        // Free the spawn point and remove the object from the list
        isOccupied[spawnIndex] = false;
        spawnedObjects.Remove(pickedObject);
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(83, -0.35f, 45); // Adjust as needed
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemySpawned = true;
    }
}
