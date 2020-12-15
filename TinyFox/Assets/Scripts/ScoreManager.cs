using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{   
    public static ScoreManager instance;
    public TextMeshProUGUI text;
    public TextMeshProUGUI text1;

    public int dScore = 0;
    public int cScore = 0;

    public bool gameEnd = false;
  
    // Start is called before the first frame update
    void Start()
    {       
        if (instance == null)
        {
            instance = this;
        }      
    }
 
    public void ChangeScore(int cherryValue)
    {       
        cScore += cherryValue;
        text.text = "X" + cScore.ToString();        
        PlayfabManager.SendLeaderboard(cScore);
    }

    
    public void ChangeDiamondScore(int diamondValue)
    {
        dScore += diamondValue;
        text1.text = "X" + dScore.ToString() + "/3";
        
        
        if (dScore == 3)
        {
            gameEnd = true;
            text1.color = Color.green;            
        }       
    }
    
    
}
