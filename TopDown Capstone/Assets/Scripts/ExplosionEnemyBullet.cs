using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemyBullet : MonoBehaviour
{

    public Rigidbody2D rb;
    // Start is called before the first frame update
    public GameObject explosionEffect;

    void Awake()
    {
        //need to ignore layer enemy and enemy bullet on awake start
        Physics2D.IgnoreLayerCollision(14, 15, true);
        Physics2D.IgnoreLayerCollision(15, 16, true);
    }
    void Start()
    {
        rb = GetComponent <Rigidbody2D>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Detonate();
    }

    public void Detonate()
    {
        Destroy(gameObject);
    }
}
