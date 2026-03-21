using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float flapStrength = 3f;
    [SerializeField] float rotationStrength = 10f;
    float fuel = 100;
    [SerializeField] float fuelConsumeSpeed = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        float angle = rb.linearVelocity.y * rotationStrength;
        angle = Mathf.Clamp(angle, -30f, 30f);
        float t = Time.deltaTime * 10f;
        rb.rotation = Mathf.SmoothStep(rb.rotation, angle, t);
        fuel -= Time.deltaTime * fuelConsumeSpeed;
        fuel = Mathf.Clamp (fuel, 0f,100f);
        Debug.Log(fuel);
    }

    void OnTap (InputValue value)
    {
        if (value.isPressed)
        {
          rb.linearVelocity =  Vector2.up * flapStrength;
        }
    }
    


}
