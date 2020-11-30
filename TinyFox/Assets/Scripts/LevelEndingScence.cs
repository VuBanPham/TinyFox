using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndingScence : MonoBehaviour
{
    [SerializeField] private string scenceName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(scenceName);
        }
    }
}
