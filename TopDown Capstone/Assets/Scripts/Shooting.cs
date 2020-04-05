using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    
    public Transform SnipePoint;
    public GameObject bulletPrefab;
    //public float bulletForce = 2f;
    private float dashSpeed;
    public int snipeDamage = 3;
    public GameObject impactEffect;
    public GameObject snipeAnimation;
    public LineRenderer lineRenderer;
    public CameraShake cameraShake;
    //private float chargeTimer;
    public float fireRate = 15f;
    private float nextFire = 0f;
    public static float ammo = 0f; //accessed in explosion damage line 39

    private State state;
    private enum State
    {
        Normal,
        Dashing,
        Shooting,
        Sniping, 
        
    }

    private void Awake()
    {
        state = State.Normal;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Normal:
                //i need handle dash in this class because i dont want to be able to shoot and dash at the same time theres gotta be an easier way
                HandleDash();
                HandleShoot();
                break;

            case State.Dashing:
                HandleDashing();
                break;

            case State.Shooting:
                HandleShooting(); 
                break;

            case State.Sniping:
                HandleSniping();
                
                break;
            
        }
        
    }

    private void HandleShoot()
    {
        if (Input.GetButtonDown("Fire2") && !PauseMenu.GameIsPaused)
        {
            state = State.Shooting;
            Shoot();
        }

        
        if (Input.GetButtonDown("Fire1") && !PauseMenu.GameIsPaused)
        {

            state = State.Sniping;
            
            
            
        } 
        
        

    }


    void HandleShooting()
    {
        //Debug.Log(state);
        
        if (Input.GetButtonUp("Fire2"))
        {
            StartCoroutine(cameraShake.Shake(.15f, .05f));
            state = State.Normal;
            //Debug.Log(state);
            //Bullet.Detonate();
        }
        if (Input.GetButtonDown("Fire1"))
        {

            state = State.Sniping;
            StartCoroutine (Snipe());
            //Debug.Log(state);
            //Bullet.Detonate();
        }
    }

    void Shoot()
    {

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        
        //rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown("space"))
        {
            state = State.Dashing;
            dashSpeed = 40;
        }
    }


    private void HandleDashing()
    {

        dashSpeed -= dashSpeed * 10f * Time.deltaTime;

        if (dashSpeed < 4f)
        {
            state = State.Normal;
        }
    }

    void HandleSniping()
    {
        //Debug.Log(state);
        
        if (Input.GetButtonDown("Fire2"))
        {
            state = State.Shooting;
            Shoot();
            //Debug.Log(state);
            //Bullet.Detonate();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            state = State.Normal;
        }
        if (Time.time >= nextFire && ammo < 5)
        {
            StartCoroutine(Snipe());
            
        }
        
    }

    IEnumerator Snipe()
    {
        ammo++;
        nextFire = Time.time + 1f / fireRate;
        //Debug.Log("Sniped");
        GameObject effect = Instantiate(snipeAnimation, SnipePoint.position, SnipePoint.rotation);
        //have the raycast ignore layer 13 and 11 and 16 which is the ally bullet and deathline
        int layerMask = 1 << 11 | 1<<13 | 1 << 16;
        
        layerMask = ~layerMask;
        
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, Mathf.Infinity, layerMask);
        
        Destroy(effect, .3f);
        


        if (hitInfo)
        {
            //StartCoroutine(cameraShake.Shake(.15f, .05f));
            //Debug.Log(hitInfo.transform.name);
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            
            if(enemy!= null && !enemy.Respawning)
            {
                enemy.TakeDamage(snipeDamage);
                var locationOfImpact = transform.position;
                enemy.Knockback(snipeDamage, locationOfImpact.x, locationOfImpact.y);
            }

            ExplosionEnemyBullet enemyBullet = hitInfo.transform.GetComponent<ExplosionEnemyBullet>();
            
            if (enemyBullet != null)
            {
                
                enemyBullet.Detonate();
            }

            //impact animation instantiated
            GameObject impact = Instantiate(impactEffect, hitInfo.point, firePoint.rotation);
            //impact deleted after x seconds
            Destroy(impact, .6f);
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
            //Debug.Log(firePoint.rotation);


        }
        else {
            //this makes the line go on for a while
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);

        }

        
        
        lineRenderer.enabled = true;
        //wait a frame
        //yield return 0;
        yield return new WaitForSeconds(.01f);
        lineRenderer.enabled = false;
        

        

       


    }


}
