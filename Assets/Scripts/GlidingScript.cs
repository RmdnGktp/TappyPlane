using UnityEngine;

public class GlidingScript : MonoBehaviour
{
    [SerializeField] float frequency = 3.0f;
    [SerializeField] float amplitude = 0.2f;
    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
