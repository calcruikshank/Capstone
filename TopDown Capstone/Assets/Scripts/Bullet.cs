using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int explosionDamage = 10;
    public GameObject hitEffect;
    // Update is called once per frame
    public Transform bulletRotation;
    

    private void Awake()
    {

        Physics2D.IgnoreLayerCollision(10, 11, true);
        Physics2D.IgnoreLayerCollision(11, 14, true);
        Physics2D.IgnoreLayerCollision(11, 15, true);
    }
    void Update()
    {
        
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        direction = direction.normalized;
        //holy shit this is fun
        rb.AddForce(direction / 5, ForceMode2D.Impulse);
        
        if (Input.GetButtonUp("Fire1"))
            {
                
                Detonate();
            }
    }
     
    public void Detonate()
    {
        var locationOfBullet = transform.position;
        //Debug.Log(locationOfBullet);
        Destroy(gameObject);
        GameObject effect = Instantiate(hitEffect, transform.position, bulletRotation.rotation);
        //destorys the explosion after 5 seconds
        

        //send damage and location of explosion to Enemy.TakeDamage() function
        //Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
        //if (enemy != null)
        //{
        //enemy.TakeDamage(explosionDamage);
        //enemy.Knockback(explosionDamage, locationOfBullet);
        //}
    }

    
}
