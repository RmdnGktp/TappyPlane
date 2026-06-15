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
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;
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
            Debug.Log ("Magnet is moving towards Player!!!!");
        }
        else
        {
            float newY = startPos.y + Mathf.Sin(Time.time * 2f) * 0.5f;
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

    

}
