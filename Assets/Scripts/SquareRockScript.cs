using UnityEngine;

public class SquareRockScript : MonoBehaviour
{
   static public float speed = 2f;
    Rigidbody2D rb;
    float deathZone = -5f;
    [SerializeField] GameObject squareExplosionParticles;
    [SerializeField] GameObject sFragmentPrefab;
    private GameObject Fragments;
    Vector3 startPos;
    private float glidingRange = 1.5f;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Fragments = GameObject.Find("Fragments");
        startPos = transform.position;

        if (Random.Range(0, 2) == 0)
        {   
            glidingRange = -glidingRange;
        }       
    }

    void Update()
    {
        rb.linearVelocity =  Vector2.left * speed;

        float newY = startPos.y + Mathf.Sin(Time.time * 2f) * glidingRange;
        transform.position = new Vector3 (transform.position.x, newY, transform.position.z);

        if (gameObject.transform.position.x < deathZone)
        {   
            Destroy(gameObject);
        }
    }
    
    [ContextMenu ("Destroy Pipe")]
    public void DestroyPipe()
    {
        //Instantiate(squareExplosionParticles, transform.position, quaternion.identity);
        SpawnSquareFragment();
        Destroy(gameObject);
    }

    void SpawnSquareFragment ()
    {
        float x = 0.25f;
        float y = 0.25f;

        int spawnNumber = 0;
        while (spawnNumber < 2)
        {
            Vector2 spawnPos = new Vector2 (transform.position.x + x, transform.position.y + y);
            GameObject fragment = Instantiate(sFragmentPrefab, spawnPos, Quaternion.identity, Fragments.transform);
            fragment.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

            Rigidbody2D rbf = fragment.GetComponent<Rigidbody2D>();
            Vector2 randomDirection = new Vector2(1f, Random.Range(-1f,1f));
            rbf.AddForce(randomDirection * Random.Range(0f,4f), ForceMode2D.Impulse);
            
            spawnNumber++;
            y = -y;
        }
        
        spawnNumber = 0;
        while (spawnNumber < 2)
        {
            Vector2 spawnPos = new Vector2 (transform.position.x - x, transform.position.y + y);
            GameObject fragment = Instantiate(sFragmentPrefab, spawnPos, Quaternion.identity, Fragments.transform);
            fragment.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

            Rigidbody2D rbf = fragment.GetComponent<Rigidbody2D>();
            Vector2 randomDirection = new Vector2(1f, Random.Range(-1f,1f));
            rbf.AddForce(randomDirection * Random.Range(0f,4f), ForceMode2D.Impulse);
            
            spawnNumber++;
            y = -y;
        }

    }

}
