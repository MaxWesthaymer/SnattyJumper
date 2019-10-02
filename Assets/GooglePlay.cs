using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlay : MonoBehaviour
{
	
	#region Private Fields
	public static GooglePlay _instance;
	private string _leaderboardID ;
	#endregion
		
	
	#region Unity Methods
	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}
	
	private void Start()
	{
		_leaderboardID = "CgkI9evHpuUEEAIQBg";
			PlayGamesPlatform.Activate();
	}
				
	#endregion
		//Share Status
	public void LogIn()
	{
		Social.localUser.Authenticate((bool success) =>
		{
			if (success)
			{
					Debug.Log("You've successfully logged in");
			PostToLeaderboard(PlayerPrefs.GetInt ("Score"));
			ShowSpecificLeaderboard();
			}
			else
			{
					Debug.Log("Login failed for some reason");
			}
		});
	}
										

				//Leaderboard
	public void PostToLeaderboard(int addScore)	
	{
			if (Social.localUser.authenticated)
			{
			Social.ReportScore(addScore, _leaderboardID, (bool success) =>
							{
									if (success)
									{
									Debug.Log("success!!!!!");
									}
									else
									{
											Debug.Log("Login failed!!!!! for some reason");
									}
							});
			}
	}
	
	
		// Show Leaderboard
	
		public void ShowLeaderboard()
		{
				Social.ShowLeaderboardUI();
		}
//
//
//
		//Show Specific Leaderboard
		public void ShowSpecificLeaderboard()
		{
				((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(_leaderboardID);
		}
				
//
		//Sign Out
		public void SignOut()
		{
				((PlayGamesPlatform)Social.Active).SignOut();
		}
//

}