using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Secret : MonoBehaviour
{
    // Start is called before the first frame update
    

    void Start()
    {
        GetComponent<TilemapRenderer>().enabled = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GetComponent<TilemapRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }

        else
        {
            GetComponent<TilemapRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
