
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Advertisements;

public class RockScript : MonoBehaviour
{
    static public float speed = 2f;
    Rigidbody2D rb;
    float deathZone = -5f;
    [SerializeField] GameObject sFragmentPrefab;
    [SerializeField] GameObject tFragmentPrefab;
    public bool isRotated = false;
    private GameObject Fragments;
    [SerializeField] int spikeSize;
    [SerializeField] float firstColumnDisplacement;
    [SerializeField] float secondColumnDisplacement;
    [SerializeField] float thirdColumnDisplacement;
    private float fragmentSpawnStartX;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Fragments = GameObject.Find("Fragments");
        
        if (transform.eulerAngles.z == 180f)
        {   
            //Debug.Log ("Pipe Rotated!");
            //boundsMaxY = 9f;
        }
    }

    void Update()
    {
        rb.linearVelocity =  Vector2.left * speed;

        if (gameObject.transform.position.x < deathZone)
        {
            Destroy(gameObject);
        }
    }
    
    [ContextMenu ("Destroy Pipe")]
    public void DestroyPipe()
    {
        SpawnFragments();
        Destroy(gameObject);
    }

    void SpawnFragments ()
    {
        switch (spikeSize)
        {
            case 1:
            fragmentSpawnStartX = 0;
            SpawnSquareFragment(firstColumnDisplacement);
            SpawnTriangleFragment(firstColumnDisplacement);
            break;

            case 2:
            fragmentSpawnStartX = -0.5f;
            SpawnSquareFragment(firstColumnDisplacement);
            SpawnTriangleFragment(firstColumnDisplacement);

            fragmentSpawnStartX = 0.5f;
            SpawnSquareFragment(secondColumnDisplacement);
            SpawnTriangleFragment(secondColumnDisplacement);
            break;

            case 3:
            fragmentSpawnStartX = -1f;
            SpawnSquareFragment(firstColumnDisplacement);
            SpawnTriangleFragment(firstColumnDisplacement);

            fragmentSpawnStartX = 0f;
            SpawnSquareFragment(secondColumnDisplacement);
            SpawnTriangleFragment(secondColumnDisplacement);

            fragmentSpawnStartX = 1f;
            SpawnSquareFragment(thirdColumnDisplacement);
            SpawnTriangleFragment(thirdColumnDisplacement);
            break;
        }


    }

    void SpawnSquareFragment(float displacement)
    {   
        if (!isRotated)
        {
            float spawnStartY = -0.9f - displacement;
            float spawnStartX = -0.25f;

            while (spawnStartY > -9)
            {   
                // first column of fragments
                Vector2 spawnPos = new Vector2 (transform.position.x + fragmentSpawnStartX + spawnStartX, transform.position.y + spawnStartY);
                GameObject fragment = Instantiate(sFragmentPrefab, spawnPos, Quaternion.identity, Fragments.transform);

                Rigidbody2D rbf = fragment.GetComponent<Rigidbody2D>();
                Vector2 randomDirection = new Vector2(1f, Random.Range(-1f,1f));
                rbf.AddForce(randomDirection * Random.Range(0f,4f), ForceMode2D.Impulse);

                // second column of fragments
                spawnPos = new Vector2 (transform.position.x + fragmentSpawnStartX - spawnStartX, transform.position.y + spawnStartY);
                fragment = Instantiate(sFragmentPrefab, spawnPos, Quaternion.identity, Fragments.transform);

                rbf = fragment.GetComponent<Rigidbody2D>();
                randomDirection = new Vector2(1f, Random.Range(-1f,1f));
                rbf.AddForce(randomDirection * Random.Range(0f,4f), ForceMode2D.Impulse);

                spawnStartY -= 0.5f;
            }
        }
        else
        {
            float spawnStartY = 0.9f + displacement;
            float spawnStartX = -0.25f;

            while (spawnStartY < 9)
            {   
                // first column of fragments
                Vector2 spawnPos = new Vector2 (transform.position.x + fragmentSpawnStartX + spawnStartX, transform.position.y + spawnStartY);
                GameObject fragment = Instantiate(sFragmentPrefab, spawnPos, Quaternion.identity, Fragments.transform);

                Rigidbody2D rbf = fragment.GetComponent<Rigidbody2D>();
                Vector2 randomDirection = new Vector2(1f, Random.Range(-1f,1f));
                rbf.AddForce(randomDirection * Random.Range(0f,4f), ForceMode2D.Impulse);

                // second column of fragments
                spawnPos = new Vector2 (transform.position.x + fragmentSpawnStartX - spawnStartX, transform.position.y + spawnStartY);
                fragment = Instantiate(sFragmentPrefab, spawnPos, Quaternion.identity, Fragments.transform);

                rbf = fragment.GetComponent<Rigidbody2D>();
                randomDirection = new Vector2(1f, Random.Range(-1f,1f));
                rbf.AddForce(randomDirection * Random.Range(0f,4f), ForceMode2D.Impulse);

                spawnStartY += 0.5f;
            }
        }

    }

    void SpawnTriangleFragment(float displacement)
    {   
        if (!isRotated)
        {   
            float spawnStartY = -0.463f - displacement;
            Vector2 spawnPos = new Vector2 (transform.position.x + fragmentSpawnStartX, transform.position.y + spawnStartY);
            Instantiate(tFragmentPrefab, spawnPos, Quaternion.identity, Fragments.transform);
        }
        else
        {   
            float spawnStartY = 0.463f + displacement;
            Vector2 spawnPos = new Vector2 (transform.position.x + fragmentSpawnStartX, transform.position.y + spawnStartY);
            Instantiate(tFragmentPrefab, spawnPos, Quaternion.Euler(0, 0, 180), Fragments.transform);
        }
        
    }


}
