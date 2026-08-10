using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] singleSpikes;
    [SerializeField] GameObject[] doubleSpikes;
    [SerializeField] GameObject[] tripleSpikes;
    [SerializeField] GameObject[] objects;
    [SerializeField] GameObject[] objectGroups;

    //[SerializeField] GameObject[] singlePatterns;
    [SerializeField] GameObject singleSquare;
    
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
    private bool isObjectGroupsSpawned = false;
    private bool isObjectGroupdReadytoSpawn = false;

    
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
            StartCoroutine(SpawnObstacle());
            yield return new WaitForSeconds(spawnTime);
        }
    }

    void SpawnPattern()
    {   
        //currentGap = Mathf.Lerp (minGap, maxGap, difficulty);
        //SpawnObstacle(); 
    }

    IEnumerator SpawnObstacle()
    {   
        isObjectGroupsSpawned = false;
        // gittikze pipe y ekseninceki kaymasi 1.5f düser
        // yShifting = Mathf.Lerp (yShifting, 1.5f , difficulty);
        // gittikze pipe arasi mesafe  1f düser
        gapBetweenPipes = Mathf.Lerp (gapBetweenPipes, 3f , difficulty);


        // Without diffuculty, fixed yShifting value, fixed gapbetweenPipes value 
        float centerY = Random.Range(-yShifting, yShifting);
        currentGap = Mathf.Lerp (minGap, maxGap, difficulty);

        int maxValue = Mathf.RoundToInt(Mathf.Lerp(1, 6, difficulty));
        if (isObjectGroupdReadytoSpawn)
        {
            //maxValue = 5;
        }

        int value = Random.Range(0, maxValue);
        switch (value)
        {
            case 0:
            SpawnSingleSpikes(centerY, currentGap);
            break;

            case 1:
            SpawnDoubleSpikes(centerY, currentGap);
            break;

            case 2:
            SpawnSinglePattern();
            break;

            case 3:
            SpawnTripleSpikes(centerY, currentGap);
            break;

            case 4:
            SpawnDoublePattern();
            break;

            case 5:
            SpawnTriplePattern();
            break;

            //Spawn breathing moments
            case 40:
            StartCoroutine(SpawnObjectGroups());
            isObjectGroupsSpawned = true;
            break;
        }

        if (isObjectGroupsSpawned) yield break;
        SpawnSingleObject(centerY);
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
    void SpawnSingleObject (float centerY)
    {
        isObjectGroupdReadytoSpawn = false;
        float spwanChance = Random.value;
        float fuelSpawnChange = Mathf.Lerp (0.4f, 0.3f, difficulty);
        float enemySpawnChange = Mathf.Lerp (0.3f, 0.4f, difficulty);
        float rocketSpawnChange = Mathf.Lerp (0.1f, 0.1f, difficulty);

        float yShiftingObjects =  Random.Range(-1, 1);
        float y = centerY + yShiftingObjects;
        xShiftingObjects = (gapBetweenPipes / 2 + spikeSize);
        

        if (spwanChance < fuelSpawnChange)
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
        else
        {
            isObjectGroupdReadytoSpawn = true;
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

    IEnumerator SpawnObjectGroups ()
    {   
        spikeSize = 1f;

        int x = Random.Range (2,4);

        for (int i = 0; i < x; i++)
        {
            float y =  Random.Range(-1.5f, 1.5f);
            int value = Random.Range(0, objectGroups.Length);
            Instantiate(objectGroups[value], new Vector3(transform.position.x + (spikeSize/2), y, 0), Quaternion.Euler(0, 0, 0), gameObject.transform);

            yield return new WaitForSeconds(1f); 
        }

        spawnTime = (gapBetweenPipes + spikeSize) / RockScript.speed;

    }

    // SPAWN PATTERN ----------------------------------------------------------------------------------------------------
    void SpawnSinglePattern()
    {
        spikeSize = 1f;
        float y = Random.Range(-2f, 2f);

        int value = Random.Range (0, singleSpikes.Length);
        GameObject BottomSpike = Instantiate(singleSpikes[value], new Vector3(transform.position.x + (spikeSize/2), y - 2, 0), Quaternion.identity, gameObject.transform);
        GameObject TopSpike = Instantiate(singleSpikes[value], new Vector3(transform.position.x + (spikeSize/2), y + 2, 0), Quaternion.identity, gameObject.transform);
        TopSpike.transform.localScale = new Vector3 (1,-1,1);
        TopSpike.GetComponent<RockScript>().isRotated = true;
        SpawnSingleSquare(1,y);

        spawnTime = (gapBetweenPipes + spikeSize) / RockScript.speed;
    }

    void SpawnDoublePattern()
    {
        spikeSize = 2f;
        float y = Random.Range(-2f, 2f);

        int value = Random.Range (0, doubleSpikes.Length);
        GameObject BottomSpike = Instantiate(doubleSpikes[value], new Vector3(transform.position.x + (spikeSize/2), y - 2, 0), Quaternion.identity, gameObject.transform);
        value = Random.Range (0, doubleSpikes.Length);
        GameObject TopSpike = Instantiate(doubleSpikes[value], new Vector3(transform.position.x + (spikeSize/2), y + 2, 0), Quaternion.identity, gameObject.transform);
        TopSpike.transform.localScale = new Vector3 (1,-1,1);
        TopSpike.GetComponent<RockScript>().isRotated = true;
        
        SpawnSingleSquare(2,y);

        spawnTime = (gapBetweenPipes + spikeSize) / RockScript.speed;

    }

    void SpawnTriplePattern()
    {
        spikeSize = 3f;
        float y = Random.Range(-2f, 2f);

        int value = Random.Range (0, tripleSpikes.Length);
        GameObject BottomSpike = Instantiate(tripleSpikes[value], new Vector3(transform.position.x + (spikeSize/2), y - 2, 0), Quaternion.identity, gameObject.transform);
        value = Random.Range (0, doubleSpikes.Length);
        GameObject TopSpike = Instantiate(tripleSpikes[value], new Vector3(transform.position.x + (spikeSize/2), y + 2, 0), Quaternion.identity, gameObject.transform);
        TopSpike.transform.localScale = new Vector3 (1,-1,1);
        TopSpike.GetComponent<RockScript>().isRotated = true;

        SpawnSingleSquare(3,y);

        spawnTime = (gapBetweenPipes + spikeSize) / RockScript.speed;

    }

    void SpawnSingleSquare (int value, float y)
    {
        
        switch (value)
        {
            case 1:
            Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2), y, 0), Quaternion.identity, gameObject.transform);
            break;

            case 2:
            int i = Random.Range(0,3);
            switch (i)
            {
                // Spawn all
                case 0:
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2) -0.5f , y, 0), Quaternion.identity, gameObject.transform);
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2) +0.5f , y, 0), Quaternion.identity, gameObject.transform);
                break;
                // Spawn only left one
                case 1:
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2) -0.5f , y, 0), Quaternion.identity, gameObject.transform);
                break;
                // Spawn only right one
                case 2:
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2) +0.5f , y, 0), Quaternion.identity, gameObject.transform);
                break;
            }
            break;

            case 3:
            i = Random.Range(0,7);
            switch (i)
            {
                // Spawn all
                case 0:
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2), y, 0), Quaternion.identity, gameObject.transform);
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2) -1f , y, 0), Quaternion.identity, gameObject.transform);
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2) +1f , y, 0), Quaternion.identity, gameObject.transform);
                break;
                // Spawn only middle
                case 1:
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2), y, 0), Quaternion.identity, gameObject.transform);
                break;
                // Spawn only left one
                case 2:
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2) -1f , y, 0), Quaternion.identity, gameObject.transform);
                break;
                // Spawn only right one
                case 3:
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2) +1f , y, 0), Quaternion.identity, gameObject.transform);
                break;
                // Spawn only left and right 
                case 4:
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2) -1f , y, 0), Quaternion.identity, gameObject.transform);
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2) +1f , y, 0), Quaternion.identity, gameObject.transform);
                break;
                // Spawn only left and middle 
                case 5:
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2) -1f , y, 0), Quaternion.identity, gameObject.transform);
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2), y, 0), Quaternion.identity, gameObject.transform);
                break;
                // Spawn only middle and right 
                case 6:
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2), y, 0), Quaternion.identity, gameObject.transform);
                    Instantiate(singleSquare, new Vector3(transform.position.x + (spikeSize/2) +1f , y, 0), Quaternion.identity, gameObject.transform);
                break;

            }
            break;

        }
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