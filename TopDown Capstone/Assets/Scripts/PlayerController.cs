using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    
    private Vector3 dashDir;
    private float dashSpeed;
    public Transform firePoint;
    public GameObject dashEffect;
    public Transform dashPoint;
    public int currentPercentage = 0;
    public float knockbackValue;
    private Vector3 knockDir;
    public GameObject knockbackAnim;
    public DamagePercentRed damagePercentRed;
    public CameraShake cameraShake;
    public DeathZoneRed deathZoneRed;
    public int stocksLeft = 3;
    public Vector3 originalPos;
    public GameObject stock0;
    public GameObject stock1;
    public GameObject stock2;
    public bool Respawning = false;
    public GameObject respawnEffect;
    public float respawn_timer = 0f;
    public float TimeIWantInSeconds = 3f;

    private State state;
    private enum State
    {
        Normal,
        Dashing,
        Knockback,
        Respawning
    }

    // Update is called once per frame
    private void Awake()
    {
        originalPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        state = State.Normal;

        damagePercentRed.SetStartingPercent(currentPercentage);
        Respawning = false;
    }


    void Update()
    {
        //GameObject effect = Instantiate(knockbackAnim, transform.position, firePoint.rotation)
        switch (state)
        {
            case State.Normal:
                HandleMovement();
                HandleDash();
                
                break;
            case State.Dashing:
                HandleDashing();
                break;
            case State.Respawning:
                HandleRespawn();
                break;



        }
        if (deathZoneRed.RedPlayerLostStock)
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
        

    }

    //use fixed update when dealing with physics
    void FixedUpdate()
    {
        if (Respawning == false) {rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); }
            

        switch (state)
        {
            case State.Knockback:
                BeingKnocked();
                break;
        }
    }

    private void HandleMovement()
    {
        respawn_timer = 0f;
        respawnEffect.SetActive(false);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        Physics2D.IgnoreLayerCollision(8, 10, false);
    }
    private void HandleDash()
    {
        if (Input.GetKeyDown("space") && !PauseMenu.GameIsPaused)
        {
            
            state = State.Dashing;

            //mouse location normalized
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            direction = direction.normalized;
            //adding dash animation
            GameObject effect = Instantiate(dashEffect, dashPoint.position, dashPoint.rotation);
            //destorying after x seconds
            Destroy(effect, 1f);
            dashDir = direction;
            dashSpeed = 40;
        }
    }

    private void HandleDashing()
    {
        transform.position += dashDir * dashSpeed * Time.deltaTime;
        dashSpeed -= dashSpeed * 10f * Time.deltaTime;
        
        //dash through walls
        Physics2D.IgnoreLayerCollision(8, 10, true);
        if (dashSpeed < 4f)
        {
            state = State.Normal;
        }
    }

    public void TakeDamage(int damage)
    {
        currentPercentage += damage;
        damagePercentRed.SetPercent(currentPercentage);


    }
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
        
        //knockback that scales based on current percentage as well as damage of hit
        knockbackValue = (14 * ((currentPercentage + damage) * (damage / 3)) / 180) + 7;
        
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

    public void LoseStock()
    {
        StartCoroutine(cameraShake.Shake(.15f, .05f));
        Debug.Log("Lost a Stock");
        deathZoneRed.ResetTimer();
        deathZoneRed.RedPlayerLostStock = false;
        //Destroy(gameObject);
        Respawning = true;
        state = State.Respawning;
        gameObject.transform.position = originalPos;
        stocksLeft--;
        damagePercentRed.SetStartingPercent(0);
        currentPercentage = 0;
    }

    public void HandleRespawn()
    {
        gameObject.transform.position = originalPos;
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
