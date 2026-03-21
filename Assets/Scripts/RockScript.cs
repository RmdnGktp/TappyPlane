using UnityEngine;

public class RockScript : MonoBehaviour
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

        if (gameObject.transform.position.x < deathZone)
        {
            Destroy(gameObject);
        }
    }
}
