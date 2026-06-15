
using UnityEngine;
using UnityEngine.Advertisements;

public class RockScript : MonoBehaviour
{
    static public float speed = 2f;
    Rigidbody2D rb;
    float deathZone = -5f;
    [SerializeField] GameObject sFragmentPrefab;
    [SerializeField] GameObject tFragmentPrefab;
    private float boundsMaxY = -9f;
    public bool isRotated = false;
    private GameObject Fragments;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Fragments = GameObject.Find("Fragments");
        
        if (transform.eulerAngles.z == 180f)
        {   
            Debug.Log ("Pipe Rotated!");
            //boundsMaxY = 9f;
        }

        if (isRotated)
        {
            boundsMaxY = 9f;
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
        
        SpawnSquareFragment();
        SpawnTriangleFragment();
        Destroy(gameObject);
    }

    void SpawnSquareFragment()
    {
        for (int i = 0; i < 25; i++)
        {   
            Vector2 spawnPos = new Vector2 (transform.position.x + Random.Range (-0.5f, 0.5f), transform.position.y + Random.Range(0f, boundsMaxY));
            GameObject fragment = Instantiate(sFragmentPrefab, spawnPos, Quaternion.identity, Fragments.transform);

            Rigidbody2D rbf = fragment.GetComponent<Rigidbody2D>();
            //Vector2 randomDirection = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f));
            //rbf.AddForce(randomDirection * Random.Range(2f,6f), ForceMode2D.Impulse);
            //rbf.AddTorgue(Random.Range(-200f,200f));

            Vector2 randomDirection = new Vector2(1f, Random.Range(-1f,1f));
            rbf.AddForce(randomDirection * Random.Range(0f,4f), ForceMode2D.Impulse);
        }
    }

    void SpawnTriangleFragment()
    {   
        float xValue = 0.5f;
        float yValue = 0.458f;


        for (int i = 0; i < 2; i++)
        {   
            Vector2 spawnPos = new Vector2 (transform.position.x + xValue, transform.position.y + yValue);

            if (!isRotated)
            {
                Instantiate(tFragmentPrefab, spawnPos, Quaternion.identity, Fragments.transform);
            }
            else
            {
                Instantiate(tFragmentPrefab, spawnPos, Quaternion.Euler(0, 0, 180), Fragments.transform);
            }
            
            xValue = -0.5f;
        }
    }


}
