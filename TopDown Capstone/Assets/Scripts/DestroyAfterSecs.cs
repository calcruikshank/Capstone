using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSecs : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

        //starts coroutine for .1 seconds on the instantiation of explosion
        StartCoroutine(waitForSec(.1f));
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator waitForSec(float sec)
    {
        yield return new WaitForSeconds(sec);
        //destroy game object 
        Destroy(gameObject, .1f);

    }
}
