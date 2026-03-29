using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] objects; // 0: top, 1: bottom, 2: fuel, 3: Enemy
    [SerializeField] float minSpawnDelay = 1.5f;
    float yDifference = 1.6f;
    [SerializeField] float xDifference = 1f;
    [SerializeField] float fuelMaxXDifference = 2.0f;
    [SerializeField] float fuelMinXDifference = 1.5f;
    
    float currentGap;
    [SerializeField] float minGap = 4.4f;
    [SerializeField] float maxGap = 4.3f;
    [SerializeField] float gapDifference = 0.3f;
    [SerializeField] PlaneScript planeScript;

    float difficulty = 0;
    

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
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

    IEnumerator SpawnLoop()
    {
        while (planeScript.isAlive)
        {   
            float delay = Mathf.Lerp(2f, minSpawnDelay, difficulty);
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
        float topY = centerY + gapSize;
        float bottomY = centerY - gapSize;

        float x = Random.Range(-xDifference, xDifference);

        Instantiate(objects[0], new Vector3(transform.position.x + x, topY, 0), Quaternion.identity, gameObject.transform);
        Instantiate(objects[1], new Vector3(transform.position.x, bottomY, 0), Quaternion.identity, gameObject.transform);


        // Spawn Fuel or Enemy ----------------------------------------------------------------------------------------------------
        float spwanChance = Random.value;
        float fuelSpawnChange = Mathf.Lerp (0.4f, 0.2f, difficulty);
        float enemySpawnChange = Mathf.Lerp (0.05f, 0.2f, difficulty);

        if ( spwanChance < fuelSpawnChange)
        {
            SpawnFuel(centerY);
        }
        else if (spwanChance < (fuelSpawnChange + enemySpawnChange))
        {
            SpawnEnemy(centerY);
        }
    }

    void SpawnFuel(float y)
    {
        float x = Mathf.Lerp (fuelMaxXDifference, fuelMinXDifference, difficulty);

        Instantiate(objects[2], new Vector3(transform.position.x + x, y, 0), Quaternion.identity, gameObject.transform);
    }

    void SpawnEnemy(float y)
    {
        float x = Mathf.Lerp (fuelMinXDifference, fuelMaxXDifference, difficulty);

        Instantiate(objects[3], new Vector3(transform.position.x + x, y, 0), Quaternion.identity, gameObject.transform);
    }
}