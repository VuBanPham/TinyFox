using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Cherry : MonoBehaviour
{
    int cherryValue = 1;   

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {        
            ScoreManager.instance.ChangeScore(cherryValue);            
        }
    }
}
