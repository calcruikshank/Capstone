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
    

    private State state;
    private enum State
    {
        Normal,
        Dashing
    }

    // Update is called once per frame
    private void Awake()
    {
        state = State.Normal;
    }
    void Update()
    {
        
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

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        Physics2D.IgnoreLayerCollision(8, 10, false);
    }
    private void HandleDash()
    {
        if (Input.GetKeyDown("space"))
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
    
    
    
}
