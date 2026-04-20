using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    static public float speed = 2f;
    Rigidbody2D rb;
    float deathZone = -5f;
    Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    void Update()
    {
        rb.linearVelocity =  Vector2.left * speed;

        float newY = startPos.y + Mathf.Sin(Time.time * 2f) * 0.5f;
        transform.position = new Vector3 (transform.position.x, newY, transform.position.z);

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
            Destroy(gameObject);
            //Debug.Log ("fuel destroyed!");
        }
    }
}
