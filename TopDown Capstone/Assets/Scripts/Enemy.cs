using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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


    private State state;
    private enum State
    {
        Normal,
        Knockback
    }

    private void Awake()
    {
        state = State.Normal;
    }

    void Start()
    {
        timeBetweenShots = startTimeBetweenShots;
        damagePercentBlue.SetStartingPercent(currentPercentage);
        
        //this will spawn one bullet on start making sure it works
        //put spawner in handle ai later
        Shoot();
    }


    void Update()
    {
        switch (state)
        {
            case State.Normal:
                HandleAI();
                break;
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
        knockbackValue = damage * 2;
        //ill figure out the specifics later but this adds force in the opposite direction of the explosion
        knockDir = direction;
    }

    public void BeingKnocked()
    {
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

}
