using System;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;


public class Script_AdManager_GoogleAdMob : MonoBehaviour
{
    [SerializeField] private bool enabled_BannerAd;
    [SerializeField] private bool enabled_InterstitialAd;
    [SerializeField] private bool enabled_RewardedAd;

    [SerializeField] Text adStatus;

    [SerializeField] private string ANDROID_BANNER_AD_ID, ANDROID_INTERSTITIAL_AD_ID, ANDROID_REWARDED_AD_ID;
    /*[SerializeField]*/ private string IPHONE_BANNER_AD_ID, IPHONE_INTERSTITIAL_AD_ID, IPHONE_REWARDED_AD_ID;

    private string bannerAd_ID, interstitialAd_ID, rewardedAd_ID;
    private string bannerAd_testID, interstitialAd_testID, rewardedAd_testID; // Google test IDs

    //====================================================================================================

    public void Initialize_GoogleAdMobAds() {
        // Initialize the Google Mobile Ads SDK.
        SetPlatformIDs();
        MobileAds.Initialize(initStatus => {
            UpdateAdStatus("Initializing Google AdMob...");
        });
    }

    void SetPlatformIDs() {
        #if UNITY_ANDROID
            bannerAd_ID = ANDROID_BANNER_AD_ID;
            interstitialAd_ID = ANDROID_INTERSTITIAL_AD_ID;
            rewardedAd_ID = ANDROID_REWARDED_AD_ID;
            bannerAd_testID = "ca-app-pub-3940256099942544/6300978111";
            interstitialAd_testID = "ca-app-pub-3940256099942544/1033173712";
            rewardedAd_testID = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
            bannerAd_ID = IPHONE_BANNER_AD_ID;
            interstitialAd_ID = IPHONE_INTERSTITIAL_AD_ID;
            rewardedAd_ID = IPHONE_REWARDED_AD_ID;
            bannerAd_testID = "ca-app-pub-3940256099942544/2934735716"
            interstitialAd_testID = "ca-app-pub-3940256099942544/4411468910";
            rewardedAd_testID = "ca-app-pub-3940256099942544/1712485313";
        #else
            bannerAd_ID = interstitialAd_ID = rewardedAd_ID = "unexpected_platform";
            bannerAd_testID = interstitialAd_testID = rewardedAd_testID = "unexpected_platform";
        #endif
    }

    public Text AdStatus {
        get { return adStatus; }
        set { adStatus = value; }
    }

    public void UpdateAdStatus(string statusText) {
        AdStatus.text = statusText + "\n" + AdStatus.text;
    }

    public bool get_bannerAd_enabled {
        get { return enabled_BannerAd; }
    }

    public bool get_interstititalAd_enabled {
        get { return enabled_InterstitialAd; }
	}

    public bool get_rewardedAd_enabled {
        get { return enabled_RewardedAd; }
    }

    public string get_bannerAd_ID {
        get { return bannerAd_ID; }
    }

    public string get_bannerAd_testID {
        get { return bannerAd_testID; }
    }

    public string get_interstitialAd_ID {
        get { return interstitialAd_ID; }
    }

    public string get_interstitialAd_testID {
        get { return interstitialAd_testID; }
    }

    public string get_rewardedAd_ID {
        get { return rewardedAd_ID; }
    }

    public string get_rewardedAd_testID {
        get { return rewardedAd_testID; }
    }


} // end of class