using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class Script_RewardedAd_UnityAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] private string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] private string _iOSAdUnitId = "Rewarded_iOS";

    private string _adUnitId;
    private bool loaded_RewardedAd;
    private bool reward = false;

    private Script_AdManager_UnityAds script_AdManager;

    //====================================================================================================

    public Script_AdManager_UnityAds set_ref_script_AdManager_UnityAds {
        set { script_AdManager = value; }
    }

    public void Initialize_RewardedAd() {
        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSAdUnitId
            : _androidAdUnitId;

        //Disable button until ad is ready to show
        //_showAdButton.interactable = false; >>>>>>>>> COMMENTED OUT
    }

    void FixedUpdate() {
        if (reward) {
            script_AdManager.UpdateAdStatus("Unity Ads Rewarded Ad Completed.");
            LoadNewAd();
            reward = false;
        }
    }

    // Load content to the Ad Unit:
    public void LoadNewAd() {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);

        loaded_RewardedAd = true;
    }

    // Implement a method to execute when the user clicks the button.
    public void ShowAd() {
        if (loaded_RewardedAd) {
            // Disable the button: 
            //_showAdButton.interactable = false; >>>>>>>>> COMMENTED OUT

            // Set ad status text.
            script_AdManager.UpdateAdStatus("Loading new rewarded ad...");

            // Then show the ad:
            Advertisement.Show(_adUnitId, this);
        } else {
            Debug.Log("Reward ad not enabled.");
            script_AdManager.UpdateAdStatus("Rewarded ad not enabled.");
        }
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId) {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId)) {
            // Configure the button to call the ShowAd() method when clicked:
            //_showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            //_showAdButton.interactable = true; >>>>>>>>> COMMENTED OUT
        }
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) {
            reward = true; // Grant a reward.
            Debug.Log("Reward received.");
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
        script_AdManager.UpdateAdStatus($"Rewarded ad failed to load. {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
        script_AdManager.UpdateAdStatus($"Rewarded ad failed to load. {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy() {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }


} // end of class