using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    static public float speed = 2f;
    Rigidbody2D rb;
    float deathZone = -5f;
    Vector3 startPos;

    private bool dodged = false;
    private Transform plane;
    private QuestManager questManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        plane = GameObject.FindGameObjectWithTag("Player").transform;
        questManager = FindFirstObjectByType<QuestManager>();
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

        UpdateDodgeEnemyQuest();

    }



    void  UpdateDodgeEnemyQuest()
    {
        if (!dodged && transform.position.x < plane.position.x)
        {
            dodged = true;
            questManager.UpdateQuest(QuestType.AvoidEnemies,1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        int layerIndex = LayerMask.NameToLayer("Rock");
        if (other.gameObject.layer == layerIndex)
        {
            // Destroy(gameObject);
            // transform.position = new Vector3 (transform.position.x -1, transform.position.y, transform.position.z);
            Debug.Log ("Enemy destroyed!");
        }
    }
}
