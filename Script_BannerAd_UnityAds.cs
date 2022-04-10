using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class Script_BannerAd_UnityAds : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////////////////////////////
    // NOTE //////////////////////////////////////////////////////////////////////////////////////
    // Unity Banner Ads are rarely used outside of the U.S. and revenue is low ///////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////

    // For the purpose of this example, these buttons are for functionality testing:
    //[SerializeField] Button _loadBannerButton;
    //[SerializeField] Button _showBannerButton;
    //[SerializeField] Button _hideBannerButton;
 
    [SerializeField] private BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER; // Edit in Inspector

    [SerializeField] private string _androidAdUnitId = "Banner_Android";
    [SerializeField] private string _iOSAdUnitId = "Banner_iOS";

    private string _adUnitId;
    private bool loaded_BannerAd;

    private Script_AdManager_UnityAds script_AdManager;

    //====================================================================================================

    public Script_AdManager_UnityAds set_ref_script_AdManager_UnityAds {
        set { script_AdManager = value; }
    }

    public void Initialize_BannerAd() {
        loaded_BannerAd = false;

        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSAdUnitId
            : _androidAdUnitId;
    }

    void Start() {
        // Disable the button until an ad is ready to show:
        //_showBannerButton.interactable = false; >>>>>>>>> COMMENTED OUT
        //_hideBannerButton.interactable = false; >>>>>>>>> COMMENTED OUT

        // Set the banner position:
        //Advertisement.Banner.SetPosition(_bannerPosition); >>>>>>>>> COMMENTED OUT

        // Configure the Load Banner button to call the LoadBanner() method when clicked:
        //_loadBannerButton.onClick.AddListener(LoadBanner); >>>>>>>>> COMMENTED OUT
        //_loadBannerButton.interactable = true; >>>>>>>>> COMMENTED OUT
    }

    // Implement a method to call when the Load Banner button is clicked:
    public void LoadNewBanner() {
        Advertisement.Banner.SetPosition(_bannerPosition);

        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
 
        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_adUnitId, options);

        //Load banner
        loaded_BannerAd = true;
    }

    // Implement a method to call when the Show Banner button is clicked:
    public void ShowBannerAd() {
        if (loaded_BannerAd) {
            // Set up options to notify the SDK of show events:
            BannerOptions options = new BannerOptions {
                clickCallback = OnBannerClicked,
                hideCallback = OnBannerHidden,
                showCallback = OnBannerShown
            };

            // Set ad status text.
            script_AdManager.UpdateAdStatus("Loading new banner ad...");

            // Show the loaded Banner Ad Unit:
            Advertisement.Banner.Show(_adUnitId, options);
        } else {
            Debug.Log("Banner ad not enabled.");
            script_AdManager.UpdateAdStatus("Banner ad not enabled.");
        }
    }

    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded() {
        Debug.Log("Banner ad loaded");
 
        // Configure the Show Banner button to call the ShowBannerAd() method when clicked:
        //_showBannerButton.onClick.AddListener(ShowBannerAd); >>>>>>>>> COMMENTED OUT
        // Configure the Hide Banner button to call the HideBannerAd() method when clicked:
        //_hideBannerButton.onClick.AddListener(HideBannerAd); >>>>>>>>> COMMENTED OUT
 
        // Enable both buttons:
        //_showBannerButton.interactable = true; >>>>>>>>> COMMENTED OUT
        //_hideBannerButton.interactable = true; >>>>>>>>> COMMENTED OUT
    }
 
    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message) {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }
 
    // Implement a method to call when the Hide Banner button is clicked:
    void HideBannerAd() {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }
 
    void OnBannerClicked() { }

    void OnBannerShown() { }

    void OnBannerHidden() { }
 
    void OnDestroy() {
        // Clean up the listeners:
        //_loadBannerButton.onClick.RemoveAllListeners(); >>>>>>>>> COMMENTED OUT
        //_showBannerButton.onClick.RemoveAllListeners(); >>>>>>>>> COMMENTED OUT
        //_hideBannerButton.onClick.RemoveAllListeners(); >>>>>>>>> COMMENTED OUT
    }


} // end of class