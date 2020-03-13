using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    

    private State state;
    private enum State
    {
        Normal,
        Dashing,
        Knockback
    }

    // Update is called once per frame
    private void Awake()
    {
        state = State.Normal;

        damagePercentRed.SetStartingPercent(currentPercentage);
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

            

        }

    }

    //use fixed update when dealing with physics
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        switch (state)
        {
            case State.Knockback:
                BeingKnocked();
                break;
        }
    }

    private void HandleMovement()
    {
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



}
