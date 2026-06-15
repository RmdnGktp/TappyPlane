using UnityEngine;

public class FragmentScript : MonoBehaviour
{
    static public float speed = 2f;
    Rigidbody2D rb;
    float deathZone = -5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Invoke("DestroyFragment", Random.Range(1f, 3f));
    }

    void Update()
    {
        //rb.linearVelocity =  Vector2.left * speed;

        if (gameObject.transform.position.x < deathZone)
        {
           DestroyFragment();
        }
    }

    void DestroyFragment()
    {
        Destroy(gameObject);
    }
}
