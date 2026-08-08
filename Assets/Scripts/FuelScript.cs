using UnityEngine;

public class FuelScript : MonoBehaviour
{
    
    static public float speed = 2f;
    Rigidbody2D rb;
    float deathZone = -5f;

    Transform player;
    float magnetSpeed = 1.5f;
    bool isInMagnetRange;
    Vector3 startPos;

    [SerializeField] GameObject _deathVFX;
    [SerializeField] float glidingAmount = 0.5f;
    private float glidingRange;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;

        glidingRange = Random.Range(0.5f, glidingAmount);
        if (Random.Range(0, 2) == 0)
        {   
            glidingRange = -glidingRange;
        }       
    }

    void Update()
    {
        rb.linearVelocity =  Vector2.left * speed;

        float angle = Mathf.Sin (Time.time * 2f) * 15f;
        rb.rotation = angle;

        
        // Fuel Magnet
        if (isInMagnetRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, magnetSpeed * Time.deltaTime);
        }
        else
        {   
            float newY = startPos.y + Mathf.Sin(Time.time * 2f) * glidingRange;
            transform.position = new Vector3 (transform.position.x, newY, transform.position.z);
        }

        //Destroy
        if (gameObject.transform.position.x < deathZone)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        
        int layer = LayerMask.NameToLayer("Magnet");
        if (other.gameObject.layer == layer)
        {
            isInMagnetRange = true;
            
        }
    }

    public void DestroyGameObject()
    {
        SpawnDeathVFX();
        Destroy(gameObject);
    }

    void SpawnDeathVFX()
    {
        Instantiate(_deathVFX, transform.position, transform.rotation);
    }

    

}
