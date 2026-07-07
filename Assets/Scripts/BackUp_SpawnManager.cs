using UnityEngine;
using System.Collections;

public class BackUp_SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] objects; // 0: top, 1: bottom, 2: fuel, 3: Bat, 4:Bee, 5:Fly
    [SerializeField] GameObject[] singleSpikes;
    [SerializeField] GameObject[] doubleSpikes;
    [SerializeField] GameObject[] tripleSpikes;



    [SerializeField] float minSpawnDelay; 
    [SerializeField] float maxSpawnDelay; 
    [SerializeField] float yDifference = 1.6f;
    [SerializeField] float xDifference = 1f;
    [SerializeField] float fuelMaxXDifference = 2.0f;
    [SerializeField] float fuelMinXDifference = 1.5f;
    
    float currentGap;
    [SerializeField] float minGap = 4.4f;
    [SerializeField] float maxGap = 4.3f;
    [SerializeField] float gapDifference = 0.3f;
    [SerializeField] PlaneScript planeScript;

    float difficulty = 0;
    [SerializeField] Transform Fragments;
    

    public void StartSpawn()
    {
        StartCoroutine(SpawnLoop());
        SpawnPattern();
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

    private IEnumerator SpawnLoop()
    {
        while (planeScript.isAlive)
        {   
            float delay = Mathf.Lerp(maxSpawnDelay, minSpawnDelay, difficulty);
            // print (delay);
            yield return new WaitForSeconds(delay);
            SpawnPattern();
        }
    }

    void SpawnPattern()
    {   
        float rand = Random.value;

        float easyLevel = Mathf.Lerp(0.6f, 0.2f , difficulty);
        float mediumLevel = Mathf.Lerp(0.3f, 0.2f , difficulty);

        currentGap = Mathf.Lerp (minGap, maxGap, difficulty);
        
        // Bottom ve top pipe arasindaki mesafeyi git gide daraltmak icin
        if (rand < easyLevel)
        {
            SpawnObstacle(currentGap + gapDifference); 
        }
        else if (rand < easyLevel + mediumLevel)
        {
            SpawnObstacle(currentGap); 
        }
        else
        {
            SpawnObstacle(currentGap - gapDifference); 
        }

    }

    void SpawnObstacle(float gapSize)
    {
        // Spawn Rocks -----------------------------------------------------------------------------------------------------------
        float centerY = Random.Range(-yDifference, yDifference);
        float topCenterY = centerY + gapSize;
        float bottomCenterY = centerY - gapSize;

        float xShift = Random.Range(-xDifference, xDifference);

        int value = Random.Range(0, 4);
        Instantiate(objects[value], new Vector3(transform.position.x, bottomCenterY, 0), Quaternion.identity, gameObject.transform);
        //value = Random.Range(0, 4);
        GameObject TopSpike = Instantiate(objects[value], new Vector3(transform.position.x + xShift, topCenterY, 0), Quaternion.Euler(0, 0, 180), gameObject.transform);
        TopSpike.GetComponent<RockScript>().isRotated = true;


        // Spawn Fuel or Enemy ----------------------------------------------------------------------------------------------------
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


    void SpawnSingleSpikes(float centerY, float gapSize)
    {   
        float topCenterY = centerY + gapSize;
        float bottomCenterY = centerY - gapSize;
        float xShift = Random.Range(-xDifference, xDifference);

        int value = Random.Range (0, singleSpikes.Length);
        Instantiate(singleSpikes[value], new Vector3(transform.position.x, bottomCenterY, 0), Quaternion.identity, gameObject.transform);
        GameObject TopSpike = Instantiate(objects[value], new Vector3(transform.position.x + xShift, topCenterY, 0), Quaternion.Euler(0, 0, 180), gameObject.transform);
        TopSpike.GetComponent<RockScript>().isRotated = true;

        minSpawnDelay = 2f;
    }

    void SpawnFuel(float y)
    {
        //float x = Mathf.Lerp (fuelMinXDifference, fuelMaxXDifference, difficulty);
        float shift =  Random.Range(-1, 1);
        Instantiate(objects[4], new Vector3(transform.position.x + minSpawnDelay, y + shift, 0), Quaternion.identity, gameObject.transform);
    }

    void SpawnEnemy(float y)
    {
        //float x = Mathf.Lerp (fuelMinXDifference, fuelMaxXDifference, difficulty);
        float shift =  Random.Range(-1, 1);
        Instantiate(objects[5], new Vector3(transform.position.x + minSpawnDelay, y + shift, 0), Quaternion.Euler(0, 0, 0), gameObject.transform);
    }

    void SpawnRocket(float y)
    {
        //float x = Mathf.Lerp (fuelMinXDifference, fuelMaxXDifference, difficulty);
        float shift =  Random.Range(-1, 1);
        Instantiate(objects[6], new Vector3(transform.position.x + minSpawnDelay, y + shift, 0), Quaternion.Euler(0, 0, 0), gameObject.transform);
    }

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
