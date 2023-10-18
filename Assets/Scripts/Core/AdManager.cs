using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Advertisements;
#if UNITY_ANDROID || UNITY_IOS
using GoogleMobileAds.Api;
#endif

public class AdManager : MonoBehaviour {

	public static AdManager Instance = null;

	#if UNITY_ANDROID || UNITY_IOS
	public string _admobInterstitialId = "";
	private string videoAdMobId;
	private InterstitialAd _admobInterstitial = null;

	public string _unityAdsId = "";
	public bool _unityAdsUseTestMode = false;
	#endif

	// Display ads after 3 game over
	public int _adsDisplayFrequency = 2;
	public int _admobDisplayCount = 2;

	private RewardBasedVideoAd rewardBasedAdMobVideo; 
	AdRequest AdMobVideoRequest;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
		LoadInterstitial();
		this.rewardBasedAdMobVideo = RewardBasedVideoAd.Instance;
		this.RequestRewardedVideo();

		if (!UnityEngine.Advertisements.Advertisement.isInitialized)
			UnityEngine.Advertisements.Advertisement.Initialize (_unityAdsId, _unityAdsUseTestMode);
	}

	private void LoadInterstitial()
	{
		#if UNITY_ANDROID || UNITY_IPHONE
		if (_admobInterstitial != null)
			_admobInterstitial.Destroy();
		_admobInterstitial = new InterstitialAd(_admobInterstitialId);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		_admobInterstitial.LoadAd(request);
		#endif
	}

	// Update is called once per frame
	void Update () {

	}

	public void ShowInterstitial(bool admob)
	{
		print("show ad: " + admob);
		if (admob) // Admob
		{
			#if UNITY_ANDROID || UNITY_IPHONE
			if (_admobInterstitial.IsLoaded())
				_admobInterstitial.Show();
			else
				LoadInterstitial();
			#endif
		}
		else // Unity ads
		{
			if (UnityEngine.Advertisements.Advertisement.IsReady())
				UnityEngine.Advertisements.Advertisement.Show();

		}
	}

	private void RequestRewardedVideo()
	{
		// Called when an ad request has successfully loaded.
		rewardBasedAdMobVideo.OnAdLoaded += HandleRewardBasedVideoLoadedAdMob;
		// Called when an ad request failed to load.
		rewardBasedAdMobVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoadAdMob;
		// Called when an ad is shown.
		rewardBasedAdMobVideo.OnAdOpening += HandleRewardBasedVideoOpenedAdMob;
		// Called when the ad starts to play.
		rewardBasedAdMobVideo.OnAdStarted += HandleRewardBasedVideoStartedAdMob;
		// Called when the user should be rewarded for watching a video.
		rewardBasedAdMobVideo.OnAdRewarded += HandleRewardBasedVideoRewardedAdMob;
		// Called when the ad is closed.
		rewardBasedAdMobVideo.OnAdClosed += HandleRewardBasedVideoClosedAdMob;
		// Called when the ad click caused the user to leave the application.
		rewardBasedAdMobVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplicationAdMob;
		// Create an empty ad request.
		AdMobVideoRequest = new AdRequest.Builder().Build();
		// Load the rewarded video ad with the request.
		this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
	}

	public void HandleRewardBasedVideoLoadedAdMob(object sender, EventArgs args)
	{


	}

	public void HandleRewardBasedVideoFailedToLoadAdMob(object sender, AdFailedToLoadEventArgs args)
	{
			
	}

	public void HandleRewardBasedVideoOpenedAdMob(object sender, EventArgs args)
	{

	}

	public void HandleRewardBasedVideoStartedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
	}

	public void HandleRewardBasedVideoClosedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
		this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);

	}

	public void HandleRewardBasedVideoRewardedAdMob(object sender, Reward args)
	{
		CharacterSelect.Instance.UnlockChar ();
		this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);

	}
	public void HandleRewardBasedVideoLeftApplicationAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
	}

	public void ShowVideoReward()
	{
		

		if (rewardBasedAdMobVideo.IsLoaded ()) {
			rewardBasedAdMobVideo.Show ();	
		} else if (UnityEngine.Advertisements.Advertisement.IsReady ())
			UnityAdsShowVideo ();


	}

	void UnityAdsShowVideo()
	{
		ShowOptions options = new ShowOptions();
		options.resultCallback = HandleShowResultUnity;

		Advertisement.Show(options);
	}

	void HandleShowResultUnity(ShowResult result)
	{
		if (result == ShowResult.Finished)
		{            
			CharacterSelect.Instance.UnlockChar ();
			UnityEngine.Advertisements.Advertisement.Initialize (_unityAdsId, _unityAdsUseTestMode);
		}
		else if (result == ShowResult.Skipped)
		{
			Debug.LogWarning("Video was skipped - Do NOT reward the player");

		}
		else if (result == ShowResult.Failed)
		{
			
		}

	}

}
