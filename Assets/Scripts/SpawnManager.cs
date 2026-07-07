using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] singleSpikes;
    [SerializeField] GameObject[] doubleSpikes;
    [SerializeField] GameObject[] tripleSpikes;
    [SerializeField] GameObject[] objects;


    [SerializeField] float spawnDelay; 
    [SerializeField] float yShifting;
    [SerializeField] float xShifting;
    [SerializeField] float minGap;
    [SerializeField] float maxGap;

    private float currentGap;
    private float xShiftingObjects;
    
    [SerializeField] PlaneScript planeScript;

    private float difficulty = 0;
    [SerializeField] Transform Fragments;
    

    public void StartSpawn()
    {
        StartCoroutine(SpawnLoop());
        //SpawnPattern();
        SpawnObstacle(0);
    }

    void Update()
    {
        if (!planeScript.isStarted) return;

        difficulty = Mathf.Clamp01 (Time.time / 30f); 

        if (!planeScript.isAlive)
        {
            Rigidbody2D[] rigidbodies = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D rb in rigidbodies)
            {
                rb.simulated = false;
            }
        }
    }

    // SPAWN PIPES ----------------------------------------------------------------------------------------------------
    IEnumerator SpawnLoop()
    {
        while (planeScript.isAlive)
        {   

            int value = Random.Range(0, 3);

            switch (value)
            {
                case 0:
                spawnDelay = 2f;
                break;

                case 1:
                spawnDelay = 2.5f;
                break;

                case 2:
                spawnDelay = 3f;
                break;
            }

            yield return new WaitForSeconds(spawnDelay);
            SpawnObstacle(value);
        }
    }

    void SpawnPattern()
    {   
        //currentGap = Mathf.Lerp (minGap, maxGap, difficulty);
        //SpawnObstacle(); 
    }

    void SpawnObstacle(int value)
    {
        float centerY = Random.Range(-yShifting, yShifting);
        currentGap = Mathf.Lerp (minGap, maxGap, difficulty);

        switch (value)
        {
            case 0:
            SpawnSingleSpikes(centerY, currentGap);
            xShiftingObjects = 1.5f;
            break;

            case 1:
            SpawnDoubleSpikes(centerY, currentGap);
            xShiftingObjects = 2.0f;
            break;

            case 2:
            SpawnTripleSpikes(centerY, currentGap);
            xShiftingObjects = 2.5f;
            break;
        }

        SpawnObjects(centerY);
    }

    void SpawnSingleSpikes(float centerY, float gapSize)
    {   
        float topCenterY = centerY + gapSize;
        float bottomCenterY = centerY - gapSize;
        float xShift = Random.Range(-xShifting, xShifting);

        int value = Random.Range (0, singleSpikes.Length);
        Instantiate(singleSpikes[value], new Vector3(transform.position.x, bottomCenterY, 0), Quaternion.identity, gameObject.transform);
        GameObject TopSpike = Instantiate(singleSpikes[value], new Vector3(transform.position.x + xShift, topCenterY, 0), Quaternion.Euler(0, 0, 180), gameObject.transform);
        TopSpike.GetComponent<RockScript>().isRotated = true;

    }

    void SpawnDoubleSpikes(float centerY, float gapSize)
    {   
        float topCenterY = centerY + gapSize;
        float bottomCenterY = centerY - gapSize;
        float xShift = Random.Range(-xShifting, xShifting);

        int value = Random.Range (0, doubleSpikes.Length);
        Instantiate(doubleSpikes[value], new Vector3(transform.position.x, bottomCenterY, 0), Quaternion.identity, gameObject.transform);
        value = Random.Range (0, doubleSpikes.Length);
        GameObject TopSpike = Instantiate(doubleSpikes[value], new Vector3(transform.position.x + xShift, topCenterY, 0), Quaternion.Euler(0, 0, 180), gameObject.transform);
        TopSpike.GetComponent<RockScript>().isRotated = true;
    }

     void SpawnTripleSpikes(float centerY, float gapSize)
    {   
        float topCenterY = centerY + gapSize;
        float bottomCenterY = centerY - gapSize;
        float xShift = Random.Range(-xShifting, xShifting);

        int value = Random.Range (0, tripleSpikes.Length);
        Instantiate(tripleSpikes[value], new Vector3(transform.position.x, bottomCenterY, 0), Quaternion.identity, gameObject.transform);
        value = Random.Range (0, tripleSpikes.Length);
        GameObject TopSpike = Instantiate(tripleSpikes[value], new Vector3(transform.position.x + xShift, topCenterY, 0), Quaternion.Euler(0, 0, 180), gameObject.transform);
        TopSpike.GetComponent<RockScript>().isRotated = true;
    }


    // SPAWN OBJECTS ----------------------------------------------------------------------------------------------------
    void SpawnObjects (float centerY)
    {
        
        float spwanChance = Random.value;
        float fuelSpawnChange = Mathf.Lerp (0.4f, 0.3f, difficulty);
        float enemySpawnChange = Mathf.Lerp (0.3f, 0.3f, difficulty);
        float rocketSpawnChange = Mathf.Lerp (0.1f, 0.2f, difficulty);

        if ( spwanChance < fuelSpawnChange)
        {
            SpawnFuel(centerY);
        }
        else if (spwanChance < (fuelSpawnChange + enemySpawnChange))
        {
            SpawnEnemy(centerY);
        }
        else if ( spwanChance < (fuelSpawnChange + enemySpawnChange + rocketSpawnChange))
        {
            SpawnRocket(centerY);
        }
    }

    void SpawnFuel(float y)
    {
        float shift =  Random.Range(-1, 1);
        Instantiate(objects[0], new Vector3(transform.position.x + xShiftingObjects, y + shift, 0), Quaternion.identity, gameObject.transform);
    }

    void SpawnEnemy(float y)
    {

        float shift =  Random.Range(-1, 1);
        Instantiate(objects[1], new Vector3(transform.position.x + xShiftingObjects, y + shift, 0), Quaternion.Euler(0, 0, 0), gameObject.transform);
    }

    void SpawnRocket(float y)
    {

        float shift =  Random.Range(-1, 1);
        Instantiate(objects[2], new Vector3(transform.position.x + xShiftingObjects, y + shift, 0), Quaternion.Euler(0, 0, 0), gameObject.transform);
    }

    // REVIVE ----------------------------------------------------------------------------------------------------
    public void ReviveDeleteAllChilds()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in Fragments)
        {
            Destroy(child.gameObject);
        }

    }
}