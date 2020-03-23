using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlayer : MonoBehaviour
{
    public Enemy enemy;
    void OnTriggerExit2D(Collider2D hitInfo)
    {
        enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.LoseStock();
        }


    }
}
