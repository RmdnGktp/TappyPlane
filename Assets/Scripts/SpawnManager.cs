using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] singleSpikes;
    [SerializeField] GameObject[] doubleSpikes;
    [SerializeField] GameObject[] tripleSpikes;
    [SerializeField] GameObject[] objects;
    
    [SerializeField] float yShifting;
    [SerializeField] float xShifting;
    [SerializeField] float minGap;
    [SerializeField] float maxGap;
    [SerializeField] float gapBetweenPipes = 2.0f;

    private float spikeSize = 0;
    private float spawnTime; 
    private float currentGap;
    private float xShiftingObjects;
    private float gameTime;
    
    [SerializeField] PlaneScript planeScript;

    private float difficulty = 0;
    [SerializeField] Transform Fragments;

    private void Start()
    {
        difficulty = 0f;
        gameTime = 0f;
    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnLoop());
        //SpawnPattern();
        //SpawnObstacle(0);
    }

    void Update()
    {
        if (!planeScript.isStarted) { return;}

        gameTime += Time.deltaTime;
        difficulty = Mathf.Clamp01 (gameTime/ 30f); 
        // print ("Time" + gameTime + "difficultiy:" + difficulty);

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
            SpawnObstacle();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    void SpawnPattern()
    {   
        //currentGap = Mathf.Lerp (minGap, maxGap, difficulty);
        //SpawnObstacle(); 
    }

    void SpawnObstacle()
    {   
        // gittikze pipe y ekseninceki kaymasi 1.5f düser
        // yShifting = Mathf.Lerp (yShifting, 1.5f , difficulty);
        // gittikze pipe arasi mesafe  1f düser
        gapBetweenPipes = Mathf.Lerp (gapBetweenPipes, 3f , difficulty);


        // Without diffuculty, fixed yShifting value, fixed gapbetweenPipes value 
        float centerY = Random.Range(-yShifting, yShifting);
        currentGap = Mathf.Lerp (minGap, maxGap, difficulty);

        int value = Random.Range(0, 3);
        switch (value)
        {
            case 0:
            SpawnSingleSpikes(centerY, currentGap);
            break;

            case 1:
            SpawnDoubleSpikes(centerY, currentGap);
            break;

            case 2:
            SpawnTripleSpikes(centerY, currentGap);
            break;
        }

        SpawnObjects(centerY);
    }

    void SpawnSingleSpikes(float centerY, float gapSize)
    {   
        float topCenterY = centerY + gapSize;
        float bottomCenterY = centerY - gapSize;
        float xShift = Random.Range(-xShifting, xShifting);
        spikeSize = 1f;

        int value = Random.Range (0, singleSpikes.Length);
        Instantiate(singleSpikes[value], new Vector3(transform.position.x + (spikeSize/2), bottomCenterY, 0), Quaternion.identity, gameObject.transform);
        GameObject TopSpike = Instantiate(singleSpikes[value], new Vector3(transform.position.x + (spikeSize/2) + xShift, topCenterY, 0), Quaternion.identity, gameObject.transform);
        TopSpike.transform.localScale = new Vector3 (1,-1,1);
        TopSpike.GetComponent<RockScript>().isRotated = true;

        spawnTime = (gapBetweenPipes + spikeSize) / RockScript.speed;
    }

    void SpawnDoubleSpikes(float centerY, float gapSize)
    {   
        float topCenterY = centerY + gapSize;
        float bottomCenterY = centerY - gapSize;
        float xShift = Random.Range(-xShifting, xShifting);
        spikeSize = 2f;

        int value = Random.Range (0, doubleSpikes.Length);
        Instantiate(doubleSpikes[value], new Vector3(transform.position.x + (spikeSize/2), bottomCenterY, 0), Quaternion.identity, gameObject.transform);
        value = Random.Range (0, doubleSpikes.Length);
        GameObject TopSpike = Instantiate(doubleSpikes[value], new Vector3(transform.position.x + (spikeSize/2) + xShift, topCenterY, 0), Quaternion.identity, gameObject.transform);
        TopSpike.transform.localScale = new Vector3 (1,-1,1);
        TopSpike.GetComponent<RockScript>().isRotated = true;

        spawnTime = (gapBetweenPipes + spikeSize) / RockScript.speed;
    }

     void SpawnTripleSpikes(float centerY, float gapSize)
    {   
        float topCenterY = centerY + gapSize;
        float bottomCenterY = centerY - gapSize;
        float xShift = Random.Range(-xShifting, xShifting);
        spikeSize = 3f;

        int value = Random.Range (0, tripleSpikes.Length);
        Instantiate(tripleSpikes[value], new Vector3(transform.position.x + (spikeSize/2), bottomCenterY, 0), Quaternion.identity, gameObject.transform);
        value = Random.Range (0, tripleSpikes.Length);
        GameObject TopSpike = Instantiate(tripleSpikes[value], new Vector3(transform.position.x + (spikeSize/2) + xShift, topCenterY, 0), Quaternion.identity, gameObject.transform);
        TopSpike.transform.localScale = new Vector3 (1,-1,1);
        TopSpike.GetComponent<RockScript>().isRotated = true;

        spawnTime = (gapBetweenPipes + spikeSize) / RockScript.speed;
    }


    // SPAWN OBJECTS ----------------------------------------------------------------------------------------------------
    void SpawnObjects (float centerY)
    {
        
        float spwanChance = Random.value;
        float fuelSpawnChange = Mathf.Lerp (0.4f, 0.2f, difficulty);
        float enemySpawnChange = Mathf.Lerp (0.2f, 0.3f, difficulty);
        float rocketSpawnChange = Mathf.Lerp (0.1f, 0.2f, difficulty);

        float yShiftingObjects =  Random.Range(-1, 1);
        float y = centerY + yShiftingObjects;
        xShiftingObjects = (gapBetweenPipes / 2 + spikeSize);
        

        if ( spwanChance < fuelSpawnChange)
        {
            SpawnFuel(y, xShiftingObjects);
        }
        else if (spwanChance < (fuelSpawnChange + enemySpawnChange))
        {
            SpawnEnemy(y, xShiftingObjects);
        }
        else if ( spwanChance < (fuelSpawnChange + enemySpawnChange + rocketSpawnChange))
        {
            SpawnRocket(y, xShiftingObjects);
        }
    }

    void SpawnFuel(float y, float xShiftingObjects)
    {
        
        Instantiate(objects[0], new Vector3(transform.position.x + xShiftingObjects, y, 0), Quaternion.Euler(0, 0, 0), gameObject.transform);
    }

    void SpawnEnemy(float y, float xShiftingObjects)
    {

        Instantiate(objects[1], new Vector3(transform.position.x + xShiftingObjects, y, 0), Quaternion.Euler(0, 0, 0), gameObject.transform);
    }

    void SpawnRocket(float y, float xShiftingObjects)
    {

        Instantiate(objects[2], new Vector3(transform.position.x + xShiftingObjects, y, 0), Quaternion.Euler(0, 0, 0), gameObject.transform);
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