using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class LevelEndingScence : MonoBehaviour
{
    [SerializeField] private string scenceName;
    public GameObject end;
    public static LevelEndingScence instance;
    private void Start()
    {
        end = GameObject.FindGameObjectWithTag("Score");
        if (instance == null)
        {
            instance = this;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {        
        if (collision.gameObject.tag == "Player" && end.GetComponent<ScoreManager>().gameEnd == true)
        {
            SceneManager.LoadScene(scenceName);
        }
    }

 
}
