using UnityEngine;

public class FuelScript : MonoBehaviour
{
    
    static public float speed = 2f;
    Rigidbody2D rb;
    float deathZone = -5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity =  Vector2.left * speed;

        float angle = Mathf.Sin (Time.time * 2f) * 15f;
        rb.rotation = angle;

        if (gameObject.transform.position.x < deathZone)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEntD(Collider2D other)
    {   
        int layerIndex = LayerMask.NameToLayer("Rock");
        if (other.gameObject.layer == layerIndex)
        {
            // Destroy(gameObject);
            transform.position = new Vector3 (transform.position.x - 1, transform.position.y, transform.position.z);
            Debug.Log ("Fuel destroyed!");
        }
    }
}
