using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] objects; // 0: top, 1: bottom, 2: fuel, 3: Enemy
    [SerializeField] float spawnDelay = 1.5f;
    [SerializeField] float minY = -1.6f;
    [SerializeField] float maxY = 1.6f;
    [SerializeField] float minX = -1f;
    [SerializeField] float maxX = 1f;
    [SerializeField] float fuelMaxX = 2.0f;
    [SerializeField] float fuelMinX = 1.5f;
    [SerializeField] float fuelSpawnChange = 0.3f;
    [SerializeField] float enemySpawnChange = 0.1f;
    [SerializeField] float gapSizeEasy = 4.5f;
    [SerializeField] float gapSizeMedium = 4.25f;
    [SerializeField] float gapSizeHard = 4.0f;
    float minRand = 0f;
    float maxRand = 0f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            SpawnPattern();
        }
    }

    void SpawnPattern()
    {   
        if (Time.time < 5f)
        {
            // Easy Mode
            maxRand = 0.3f;
        } 
        else if (Time.time < 10f)
        {
            // Medium Mode
            maxRand = 0.7f;
        }
        else if (Time.time < 20f)
        {
            // Hard Mode
            maxRand = 1f;
        }
        else if (Time.time < 30f)
        {
            // Harder Mode
            minRand = 0.4f;
        }
        else
        {   
            // Insane Mode
            minRand = 0.75f;
        }


        float rand = Random.Range (minRand, maxRand);

        if (rand < 0.4f)
        {
            SpawnObstacle(gapSizeEasy);
        }
        else if (rand < 0.75f)
        {
            SpawnObstacle(gapSizeMedium);
        }
        else
        {
            SpawnObstacle(gapSizeHard); 
        }
    }

    void SpawnObstacle(float gapSize)
    {
        float centerY = Random.Range(minY, maxY);

        float topY = centerY + gapSize;
        float bottomY = centerY - gapSize;
        float x = Random.Range(minX, maxX);

        Instantiate(objects[0], new Vector3(transform.position.x + x, topY, 0), Quaternion.identity, gameObject.transform);
        Instantiate(objects[1], new Vector3(transform.position.x, bottomY, 0), Quaternion.identity, gameObject.transform);

        float spwanChance = Random.value;
        if ( spwanChance < fuelSpawnChange)
        {
            SpawnFuel();
        }
        else if (spwanChance < (fuelSpawnChange + enemySpawnChange))
        {
            SpawnEnemy();
        }
    }

    void SpawnFuel()
    {
        float y = Random.Range(minY, maxY);
        float x = Random.Range (fuelMinX, fuelMaxX);

        Instantiate(objects[2], new Vector3(transform.position.x + x, y, 0), Quaternion.identity, gameObject.transform);
    }

    void SpawnEnemy()
    {
        float y = Random.Range(minY, maxY);
        float x = Random.Range (fuelMinX, fuelMaxX);

        Instantiate(objects[3], new Vector3(transform.position.x + x, y, 0), Quaternion.identity, gameObject.transform);
    }
}