using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int explosionDamage = 10;
    public GameObject hitEffect;
    // Update is called once per frame
    public Transform bulletRotation;
    public Vector2 directionOnCreation;

    private void Awake()
    {
        //bullet ignore player
        Physics2D.IgnoreLayerCollision(10, 11, true);
        //bullet ignore enemy
        Physics2D.IgnoreLayerCollision(11, 14, true);
        //bullet ignore enemy bullet
        Physics2D.IgnoreLayerCollision(11, 15, true);
        //bullet ignore deathline
        Physics2D.IgnoreLayerCollision(11, 16, true);
    }

    void Start()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        
        Vector2 directionOnCreation = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        //directionOnCreation = directionOnCreation.normalized;

        


        //Debug.Log(directionOnCreation);

    }
    void Update()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        direction = direction.normalized;
        //Vector2 direction = new Vector2(directionUpdated.x - directionOnCreation.x, directionUpdated.y - directionOnCreation.y);
        //direction = direction.normalized;
        

        rb.AddForce(direction / 5, ForceMode2D.Impulse);

        if (Input.GetButtonUp("Fire2"))
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
        

    }

    
}
