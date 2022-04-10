using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Script_InterstitialAd_UnityAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] private string _iOSAdUnitId = "Interstitial_iOS";

    string _adUnitId;
    private bool loaded_InterstitialAd;

    private Script_AdManager_UnityAds script_AdManager;

    //====================================================================================================

    public Script_AdManager_UnityAds set_ref_script_AdManager_UnityAds {
        set { script_AdManager = value; }
    }

    public void Initialize_InterstitialAd() {
        loaded_InterstitialAd = false;

        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSAdUnitId
            : _androidAdUnitId;
    }

    // Load content to the Ad Unit:
    public void LoadNewAd() {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);

        loaded_InterstitialAd = true;
    }

    // Show the loaded content in the Ad Unit: 
        public void ShowAd() {
        if (loaded_InterstitialAd) {
            // Note that if the ad content wasn't previously loaded, this method will fail

            Debug.Log("Showing Ad: " + _adUnitId);
            // Set ad status text.
            script_AdManager.UpdateAdStatus("Loading new interstitial ad...");

            // Show the loaded ad
            Advertisement.Show(_adUnitId, this);

            // Load new ad
            LoadNewAd();
        } else {
            Debug.Log("Interstitial ad not enabled.");
            script_AdManager.UpdateAdStatus("Interstitial ad not enabled.");
        }
    }

    // Implement Load Listener and Show Listener interface methods:  
    public void OnUnityAdsAdLoaded(string adUnitId) {
        // Optionally execute code if the Ad Unit successfully loads content.
        Debug.Log("Ad Loaded: " + adUnitId);
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execite code if the Ad Unit fails to load, such as attempting to try again.
        script_AdManager.UpdateAdStatus($"Interstitial ad failed to load. {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execite code if the Ad Unit fails to show, such as loading another ad.
        script_AdManager.UpdateAdStatus($"Interstitial ad failed to load. {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId) { }

    public void OnUnityAdsShowClick(string adUnitId) { }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { }

    void OnDestroy() { }


} // end of class