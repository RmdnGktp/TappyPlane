using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlaneScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float flapStrength = 3f;
    [SerializeField] float rotationStrength = 10f;
    float fuel = 100;
    [SerializeField] float fuelConsumeSpeed = 10f;
    [SerializeField] float fuelToAdd = 30f;
    [SerializeField] float fuelToRemove = 10f;
    [SerializeField] TextMeshProUGUI fuelText;
    [SerializeField] TextMeshProUGUI distanceText;
    float distance = 0f;

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

        // fuel managment
        fuel -= Time.deltaTime * fuelConsumeSpeed;
        fuel = Mathf.Clamp (fuel, 0f,100f);
        fuelText.text = Mathf.RoundToInt(fuel) + "%";

        if (fuel <= 30)
        {
            fuelText.color = new Color (245f/255f, 105f/255f, 0f/255f);
        }
        else if (fuel <= 60)
        {
            fuelText.color = new Color (253f/255f, 241f/255f, 0f/255f);
        }
        else
        {
            fuelText.color = new Color (4f/255f, 226f/255f, 67f/255f);
        }

        // distance managment
        distance += Time.deltaTime * 20f;
        distanceText.text = Mathf.RoundToInt(distance) + "m";

    }

    void OnTap (InputValue value)
    {
        if (value.isPressed)
        {
          rb.linearVelocity =  Vector2.up * flapStrength;
        }
    }

    public void addFuel (float value)
    {
        fuel += value;
    }

    void OnTriggerEnter2D(Collider2D other)
    {   

        if (other.CompareTag ("Fuel"))
        {
            addFuel(fuelToAdd);
            Destroy(other.gameObject);
        }

        else if (other.CompareTag ("Enemy"))
        {
            addFuel(-fuelToRemove);
            Destroy(other.gameObject);
        }
        
    }



}
