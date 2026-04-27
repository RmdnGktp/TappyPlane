using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.Cinemachine;
using System;

public class PlaneScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float flapStrength = 3f;
    [SerializeField] float rotationStrength = 10f;
    float fuel = 100;
    [SerializeField] float fuelConsumeSpeed = 10f;
    [SerializeField] float fuelToAdd = 30f;
    [SerializeField] float fuelToRemove = 10f;
    float maxFuel = 100f;
    [SerializeField] TextMeshProUGUI fuelText;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] GameObject GameOverBoard;
    float distance = 0f;
    public bool isAlive = true;
    public bool isStarted = false;
    bool isPlayPressed = false;
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] GameObject startScripts;
    [SerializeField] GameObject board;
    [SerializeField] GameObject buttons;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] ParticleSystem flyingParticals;
    [SerializeField] Image flashImage;

    [SerializeField] CinemaschineShake cinemaschineShake;
    [SerializeField] GameManagerScript gameManagerScript;
    [SerializeField] bool isShieldOn;
    [SerializeField] GameObject Shield;
    CapsuleCollider2D collectCollider;

    void Start()
    {    
        rb = GetComponent<Rigidbody2D>();
        collectCollider = GetComponent<CapsuleCollider2D>();
    }

    public void setMaxFuel(float value)
    {
        maxFuel += value;
        fuel = maxFuel;
        fuelText.text = Mathf.RoundToInt(fuel) + "%";
    }
    
    void Update()
    {   
        if (!isStarted || !isAlive) return;
       
        // rotation
        float angle = rb.linearVelocity.y * rotationStrength;
        angle = Mathf.Clamp(angle, -30f, 30f);
        float t = Time.deltaTime * 10f;
        rb.rotation = Mathf.SmoothStep(rb.rotation, angle, t);

        // fuel managment
        fuel -= Time.deltaTime * fuelConsumeSpeed;
        fuel = Mathf.Clamp (fuel, 0f,maxFuel);
        fuelText.text = Mathf.RoundToInt(fuel) + "%";

        if (fuel == 0f)
        {
            GameOver();
        }
        else if (fuel <= 30)
        {
            fuelText.color = new Color (245f/255f, 105f/255f, 0f/255f);
            var main = flyingParticals.main;
            main.startColor = new Color (61f/255f, 61f/255f, 61f/255f);
            
        }
        else if (fuel <= 60)
        {
            fuelText.color = new Color (253f/255f, 241f/255f, 0f/255f);
            var main = flyingParticals.main;
            main.startColor = new Color (138f/255f, 139f/255f, 152f/255f);
        }
        else
        {
            fuelText.color = new Color (4f/255f, 226f/255f, 67f/255f);
            var main = flyingParticals.main;
            main.startColor = new Color (208f/255f, 211f/255f, 229f/255f);
        }

        // distance managment
        distance += Time.deltaTime * 20f; 
        distanceText.text = Mathf.RoundToInt(distance) + "m";
    }

    void OnTap (InputValue value)
    {
        if (!isPlayPressed) return;

        if (value.isPressed && isAlive && isStarted)
        {
          rb.linearVelocity =  Vector2.up * flapStrength;
        }
        else if (value.isPressed && isAlive && !isStarted)
        {
            StartGame();
            rb.linearVelocity =  Vector2.up * flapStrength;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag ("Fuel") && other.IsTouching(collectCollider))
        {
            addFuel(fuelToAdd);
            Destroy(other.gameObject);
            //gameManagerScript.UpdateEnemyQuest(1);
        }

        else if (other.CompareTag ("Bat"))
        {
            CrashEnemy(-5);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag ("Bee"))
        {
            CrashEnemy(0);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag ("Fly"))
        {   
            CrashEnemy(5);
            Destroy(other.gameObject);
        }
        
    }

    void CrashEnemy(float value)
    {
        if (!isShieldOn)
        {
            addFuel(-fuelToRemove + value);
            cinemaschineShake.ShakeCamera(2f, 0.1f);
        }
    
        gameManagerScript.UpdateEnemyQuest(1);
    }

    public void addFuel (float value)
    {
        fuel += value;
    }

    void StartGame()
    {
        isStarted = true;
        rb.simulated = true;
        spawnManager.StartSpawn();
        //Destroy(startScripts);
        //flyingParticals.Play();
        startScripts.SetActive(false);
    }

    // GAME OVER --------------------------------------------------------
    void OnCollisionEnter2D(Collision2D collision)
    {   
        Impact();
        
        if (collision.gameObject.CompareTag ("Ground"))
        {
            explosion.Play();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            cinemaschineShake.ShakeCamera(5f, 0.2f);
        } 

        if (!isAlive) return;
        GameOver();
    }

    void GameOver ()
    {   
        isAlive = false;
        flyingParticals.Stop();
        gameObject.GetComponent<Animator>().enabled = false;
        GameOverBoard.SetActive(true);
        GameOverBoard.GetComponent<GameOverScript>().GameOver(distance); 
        gameManagerScript.UpdateGameQuest(1);
        Shield.SetActive(false);
    }

    public void Impact()
    {
        StartCoroutine(ImpactEffect());
    }

    IEnumerator ImpactEffect ()
    {
        flashImage.color = new Color (1f, 1f, 1f, 1f);
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * 5f;
            flashImage.color = new Color (1f, 1f, 1f, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(5f);
    }

    public void Play()
    {
        isPlayPressed = true;
    }
    
    public void ActivateShield()
    {
        isShieldOn = true;
        Shield.SetActive(true);
    }

    public void Revive()
    {
        StartCoroutine(RevivePlayer());
    }

    IEnumerator RevivePlayer()
    {
        yield return new WaitForSeconds(0f);
        isAlive = true;
        transform.position = new Vector3(-1.5f, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);

        flyingParticals.Play();
        gameObject.GetComponent<Animator>().enabled = true;
        isStarted = false;
        rb.simulated = false;
        startScripts.SetActive(true);

        //fuel = maxFuel;
        //fuelText.text = Mathf.RoundToInt(fuel) + "%";

        if (isShieldOn)
        {
            Shield.SetActive(true);
        }
        
        spawnManager.ReviveDeleteAllChilds();
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        GameOverBoard.GetComponent<GameOverScript>().ReviveUIReset(); 
        GameOverBoard.SetActive(false);

        yield return null;
    }
    
}
