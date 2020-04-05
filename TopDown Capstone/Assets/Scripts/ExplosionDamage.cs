using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{

    public int damage = 10;
    public CircleCollider2D m_Collider;
   

    void Start()
    {
        
        //starts coroutine for .1 seconds on the instantiation of explosion
        StartCoroutine(waitForSec(.1f));
    }

    // Update is called once per frame
    void Update()
    {
       
        //destroys the game object after 3 seconds gameobject in this case is the explosion
        Destroy(gameObject, .3f);
        
    }

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        var locationOfExplosion = transform.position;

        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null && !enemy.Respawning)
        {
            Shooting.ammo = 0; //resets ammo to zero when theres a collision with the enemy
            enemy.TakeDamage(damage);
            //i seriously dont think i can pass a vector
            enemy.Knockback(damage, locationOfExplosion.x, locationOfExplosion.y);
        }
    }
    private IEnumerator waitForSec(float sec)
    {
        yield return new WaitForSeconds(sec);
        //disable the collider after certain amount of time
        m_Collider.enabled = false;

    }
}
