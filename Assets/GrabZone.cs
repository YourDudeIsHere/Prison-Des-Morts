using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabZone : MonoBehaviour
{
    //To reference the AI script
    public AI AI;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //To check if the player is in the grab zone
        if (collision.tag == "Player")
        {
            AI.Grab();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        //To check if the player is out of the grab zone
        if (collision.tag == "Player")
        {
            AI.Release();
        }
    }
}
