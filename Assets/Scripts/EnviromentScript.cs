using UnityEngine;

public class EnviromentScript : MonoBehaviour
{
    
    [SerializeField] PlaneScript planeScript;
    [SerializeField] float speed;
    [SerializeField] float width;

    void Update()
    {
        if (!planeScript.isAlive || !planeScript.isStarted) return;

        transform.Translate (Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < -width)
        {   
            transform.position += new Vector3(width, 0f, 0f);
        }

    }
}
