using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Pipe : MonoBehaviour
{
    public float speed;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (GameManager.instance.isGamePlay)
        {

            transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (transform.position.x < -11)
        {
            Destroy(gameObject);
        }
        }
      
    }
    

}

