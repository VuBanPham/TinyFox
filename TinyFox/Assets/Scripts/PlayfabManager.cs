using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }

    //Login into Playfab
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);

    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Sucessfull Login/ Account Created");
    }

    static void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/ create account!");
        Debug.Log(error.GenerateErrorReport());
    }

    //Leaderboard Function
    public static void SendLeaderboard(int cscore)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "CherryScore",
                    Value = cscore
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    static void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull sent score to Leaderboard");
    }

   public GameObject leaderboardPannel;
   public GameObject listingPrefab;
   public Transform listingContainer;

    //Get Results from Playfab
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "CherryScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
        Debug.Log("Sucessfull getting data");
    }
    void OnLeaderboardGet(GetLeaderboardResult result)
    {
            leaderboardPannel.SetActive(true);
            foreach (var item in result.Leaderboard)
            {
                GameObject tempListing = Instantiate(listingPrefab, listingContainer);
                LeaderboardListing LL = tempListing.GetComponent<LeaderboardListing>();
                LL.playerRankText.text = (item.Position+1).ToString();
                LL.playerNameText.text = item.DisplayName;
                LL.playerScoreText.text = item.StatValue.ToString();            
                Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
            }
    }
    

    public void CloseLeaderboardPanel()
    {
        leaderboardPannel.SetActive(false);
        for (int i = listingContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(listingContainer.GetChild(i).gameObject);
        }
    }
    
}
