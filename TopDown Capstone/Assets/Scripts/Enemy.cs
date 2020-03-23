using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    public int currentPercentage = 0;
    public Rigidbody2D rb;
    public GameObject deathEffect;
    public DamagePercentBlue damagePercentBlue;
    //public Rigidbody2D explosion;
    public float knockbackValue;
    private Vector3 knockDir;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float startTimeBetweenShots;
    private float timeBetweenShots;
    public GameObject knockbackAnim;
    public CameraShake cameraShake;
    public DeathZone deathZone;
    public int stocksLeft = 3;
    public Vector3 originalPos;
    public GameObject stock0;
    public GameObject stock1;
    public GameObject stock2;
    public bool Respawning = false;
    public float respawn_timer = 0f;
    public float TimeIWantInSeconds = 3f;
    public GameObject respawnEffect;

    private State state;
    private enum State
    {
        Normal,
        Knockback, 
        Respawning
    }

    private void Awake()
    {
        state = State.Normal;
        Respawning = false;
        
    }

    void Start()
    {
        originalPos = (gameObject.transform.position);
        timeBetweenShots = startTimeBetweenShots;
        damagePercentBlue.SetStartingPercent(currentPercentage);
        
        //this will spawn one bullet on start making sure it works
        //put spawner in handle ai later
        //Shoot();
    }


    void Update()
    {
        
        switch (state)
        {
            case State.Normal:
                HandleAI();
                break;
            case State.Respawning:
                HandleRespawn();
                break;

        }
        if (deathZone.BluePlayerLostStock)
        {
            LoseStock();
        }

        if (stocksLeft == 0)
        {
            stock0.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        if (stocksLeft == 2)
        {
            stock2.SetActive(false);
        }
        if (stocksLeft == 1)
        {
            stock1.SetActive(false);
        }

        if (Respawning == true)
        {
            gameObject.transform.position = originalPos;
        }
        
        


    }

    void FixedUpdate()
    {
        switch (state)
        {
            
            case State.Knockback:
                BeingKnocked();
                break;

        }
    }
    public void TakeDamage(int damage)
    {
        currentPercentage += damage;
        //DamagePercentBlue.SetHealth(damagePercentage);
        damagePercentBlue.SetPercent(currentPercentage);
        //Knockback(damage, currentPercentage);


    }

    // Update is called once per frame
    public void Knockback(int damage, float locationOfExplosionX, float locationOfExplosionY)
    {
        state = State.Knockback;
        //Vector3 bulletPosition = Input.mousePosition;
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        Vector2 exploisionPosition = new Vector2(locationOfExplosionX, locationOfExplosionY);
        //mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        //transform.right = direction;

        //distance between explosion position and rigidbody(bluePlayer)
        Vector3 direction = new Vector2(locationOfExplosionX - rb.position.x, locationOfExplosionY - rb.position.y);
        //Debug.Log("WOrked");
        //Debug.Log(currentPercentage + "%");
        direction = direction.normalized;
        //Debug.Log(direction);
        //Debug.Log(locationOfExplosionY);
        //Debug.Log(rb.position.y);
        //rb.AddForce(10, 10, ForceMode2D.Impulse);
        //knockbackValue = damage * 2;

        //knockback that scales
        knockbackValue = (14 * ((currentPercentage + damage) * (damage / 3)) / 180) + 7;
        
        //Debug.Log(knockbackValue);
        //ill figure out the specifics later but this adds force in the opposite direction of the explosion
        knockDir = direction;
    }

    public void BeingKnocked()
    {
        GameObject effect = Instantiate(knockbackAnim, transform.position, firePoint.rotation);
        transform.position += -knockDir * knockbackValue * Time.deltaTime;
        //rb.AddForce(-direction * 10, ForceMode2D.Impulse);
        knockbackValue -= knockbackValue * 3f * Time.deltaTime;
        //Debug.Log(Time.deltaTime);
        if (knockbackValue < 4f)
        {
            state = State.Normal;
        }
    }

    public void HandleAI()
    {
        respawnEffect.SetActive(false);
        respawn_timer = 0f;
        //Debug.Log(state);
        if(timeBetweenShots<= 0)
        {
           
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
            
        }
        
    }

    void Shoot()
    {

        
        //Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        //rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }

    public void LoseStock()
    {
        StartCoroutine(cameraShake.Shake(.15f, .05f));
        Debug.Log("Lost a Stock");
        //Destroy(gameObject);
        deathZone.ResetTimer();
        GameObject effect = Instantiate(deathEffect, transform.position, firePoint.rotation);
        Destroy(effect, .6f);
        deathZone.BluePlayerLostStock = false;

        Respawning = true;
        
        state = State.Respawning;
        //change ai target to spawn
        gameObject.transform.position = originalPos;
        stocksLeft--;
        damagePercentBlue.SetStartingPercent(0);
        currentPercentage = 0;
    }

    public void HandleRespawn()
    {
        respawnEffect.SetActive(true);
        //set invulenerability enemy to explosion
        //i do this in shooting
        //Physics2D.IgnoreLayerCollision(13, 14, false);

        //make a timer to count to x seconds
        respawn_timer += Time.deltaTime;

        Debug.Log(respawn_timer);
        //once timer is greater than 3 then set Respawning to false
        if (respawn_timer > TimeIWantInSeconds)
        {
            Respawning = false;
            state = State.Normal;
        }
    }

}
