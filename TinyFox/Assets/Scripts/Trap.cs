using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

     void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Equals ("Player"))
        {
            rb.isKinematic = false;
        }
    }
}
