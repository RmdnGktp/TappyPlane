using UnityEngine;

public class FuelScript : MonoBehaviour
{
    
    static public float speed = 2f;
    Rigidbody2D rb;
    float deathZone = -5f;

    Transform player;
    float magnetSpeed = 1.5f;
    bool isInMagnetRange;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
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

        int layerIndex = LayerMask.NameToLayer("Rock");
        if (other.gameObject.layer == layerIndex)
        {
            // Destroy(gameObject);
            transform.position = new Vector3 (transform.position.x - 1, transform.position.y, transform.position.z);
            Debug.Log ("Fuel destroyed!");
        }
    }

    

}
