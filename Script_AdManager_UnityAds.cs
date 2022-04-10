using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class Script_AdManager_UnityAds : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private bool _testMode = true; // SET TO 'FALSE' ON FINAL BUILD
    [SerializeField] private string _android_ID;
    [SerializeField] private string _iPhone_ID;
    
    [SerializeField] private bool enabled_BannerAd;
    [SerializeField] private bool enabled_InterstitialAd;
    [SerializeField] private bool enabled_RewardedAd;

    [SerializeField] Text adStatus;
    private string cumulativeText;

	private string _gameId; bool init = false;

    //====================================================================================================

    public bool set_TEST_MODE {
        set {
            _testMode = value;
            Debug.Log("TEST MODE (Unity Ads): " + _testMode);
        }
    }
 
    public void Initialize_UnityAds() {
        UpdateAdStatus("Loading Unity Ads...");

        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iPhone_ID
            : _android_ID;

        Advertisement.Initialize(_gameId, _testMode, this);
        if (enabled_BannerAd) this.gameObject.GetComponentInChildren<Script_BannerAd_UnityAds>().Initialize_BannerAd();
        if (enabled_InterstitialAd) this.gameObject.GetComponentInChildren<Script_InterstitialAd_UnityAds>().Initialize_InterstitialAd();
        if (enabled_RewardedAd) this.gameObject.GetComponentInChildren<Script_RewardedAd_UnityAds>().Initialize_RewardedAd();
    }

    void FixedUpdate() {
        if (init) {
            UpdateAdStatus("Unity Ads initialization complete.");
            init = false;
        }
    }

    public void OnInitializationComplete() {
        init = true;
        Debug.Log("Unity Ads initialization complete.");
    }
 
    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        UpdateAdStatus($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public bool get_bannerAd_enabled {
        get { return enabled_BannerAd; }
    }

    public bool get_interstitialAd_enabled {
        get { return enabled_InterstitialAd; }
    }

    public bool get_rewardAd_enabled {
        get { return enabled_RewardedAd; }
    }

    public Text AdStatus {
        get { return adStatus; }
        set { adStatus = value; }
    }

    public void UpdateAdStatus(string statusText) {
        AdStatus.text = statusText + "\n" + AdStatus.text;
    }


} // end of class