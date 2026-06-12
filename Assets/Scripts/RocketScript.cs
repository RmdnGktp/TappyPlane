using UnityEngine;

public class RocketScript : MonoBehaviour
{
    [SerializeField] float speed = 4f;
    Rigidbody2D rb;
    float deathZone = -10f;
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
}
