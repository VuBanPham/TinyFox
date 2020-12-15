using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invisibleObject : MonoBehaviour
{
   
    public GameObject myGameObject;

    private void Start()
    {
        myGameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            myGameObject.SetActive(true);
        }
           
        
    }
}
