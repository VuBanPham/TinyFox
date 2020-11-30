using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuddenDeath : MonoBehaviour
{
    //to see what class the script should use but mainly to prevent errors
    private enum collideOrTrigger2D
    {
        trigger,
        collider
    }
    private collideOrTrigger2D cOrT;
    private Collider2D col;
    // Use this for initialization
    void Start()
    {
        col = GetComponent<Collider2D>();
        if (col.isTrigger)
        {
            cOrT = collideOrTrigger2D.trigger;
        }
        else
        {
            cOrT = collideOrTrigger2D.collider;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (cOrT == collideOrTrigger2D.trigger)
        {          
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (cOrT == collideOrTrigger2D.collider)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
