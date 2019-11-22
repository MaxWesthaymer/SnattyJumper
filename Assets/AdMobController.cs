using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobController : MonoBehaviour {

	#region Private Fields
	public static AdMobController _instance;
	private BannerView bannerView;
	private InterstitialAd interstitial;
	#endregion
	
	// Use this for initialization
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
		DontDestroyOnLoad(gameObject);
	}


	
	void Start () 
	{
#if UNITY_ANDROID
		string appId = "ca-app-pub-9584057112688151~6134008838";

        #else
            string appId = "unexpected_platform";
        #endif
		MobileAds.Initialize(initStatus => { });
		//RequestBanner();
		RequestInterstitial();
	}
	
	#endregion
	
	private void RequestBanner()
	{
#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        #else
            string adUnitId = "unexpected_platform";
        #endif

		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();

		// Load the banner with the request.
		bannerView.LoadAd(request);
	}
	
	private void RequestInterstitial()
	{
//#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-9584057112688151/3678734878";
		//string adUnitId = "ca-app-pub-3940256099942544/1033173712";  //test
  //  #else
       // string adUnitId = "unexpected_platform";
  //  #endif

		// Initialize an InterstitialAd.
		this.interstitial = new InterstitialAd(adUnitId);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().AddTestDevice("8DE8AB319A0AF834").Build();
		// Load the interstitial with the request.
		this.interstitial.LoadAd(request);
	}
	
	public void GameOver()
	{
		if (this.interstitial.IsLoaded()) 
		{
			this.interstitial.Show();
		}
	}
}
