using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class Script_InterstitialAd_GoogleAdMob : MonoBehaviour
{
    private string interstitialAd_ID;
    private string test_interstitialAd_ID;

    private Script_AdManager_GoogleAdMob script_AdManager;
    private InterstitialAd interstitialAd;

    private string adID;
    private bool isTest;
    private bool request_newInterstitialAd;
    private bool adLoaded;
    private bool adNotLoaded;

    //====================================================================================================

    public Script_AdManager_GoogleAdMob set_ref_script_AdManager_GoogleAdMob {
        set { script_AdManager = value; }
    }

    public bool set_TEST_MODE {
        set {
            isTest = value;
            Debug.Log("TEST MODE (interstitial ad, setter): " + isTest);
            adID = SetAdID(isTest);
            Debug.Log("adID (interstitial ad, setter): " + adID);
            if (request_newInterstitialAd) this.RequestInterstitialAd();
        }
    }

    string SetAdID(bool b) {
        if (b) return script_AdManager.get_interstitialAd_testID;
        else return script_AdManager.get_interstitialAd_ID;
    }

    void Awake() {
        adLoaded = false;
        adNotLoaded = false;
        request_newInterstitialAd = true;
    }

    public void RequestInterstitialAd() {
        // Initialize an InterstitialAd.
        this.interstitialAd = new InterstitialAd(adID);

        //====================================================================================================
        // SUBSCRIBE TO EVENTS:    ///////////////////////////////////////////////////////////////////////////
        // Called when an ad request has successfully loaded.
        this.interstitialAd.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitialAd.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitialAd.OnAdOpening += this.HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitialAd.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //this.interstitialAd.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;


        LoadInterstitialAd();
        //ShowInterstitialAd();
        request_newInterstitialAd = false;
    }

    public void LoadInterstitialAd() {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the interstitial with the request.
        this.interstitialAd.LoadAd(request);

        // Set ad status text.
        script_AdManager.UpdateAdStatus("Loading new Interstitial ad...");
    }

    public void ShowInterstitialAd() {
        if (interstitialAd != null) {
            if (this.interstitialAd.IsLoaded()) {
                this.interstitialAd.Show();
            }
        } else {
            Debug.Log("Interstitial ad not enabled.");
            script_AdManager.UpdateAdStatus("Interstitial ad not enabled.");
        }
    }

    private void FixedUpdate() {
        Debug.Log("TEST MODE (interstitial ad, FixedUpdate): " + isTest);
        if (adLoaded) {
            script_AdManager.UpdateAdStatus("Interstitial ad Loaded."); ;
            adLoaded = false;
        }

        if (adNotLoaded) {
            script_AdManager.UpdateAdStatus("Interstitial ad failed to load.");
            adNotLoaded = false;
        }
    }

    public void RemoveInterstitialAd() {
        interstitialAd.Destroy();
    }

    //====================================================================================================
    // FOR EVENTS AND DELEGATES:    //////////////////////////////////////////////////////////////////////
    public void HandleOnAdLoaded(object sender, EventArgs args) {
        MonoBehaviour.print("HandleAdLoaded event received (interstitial ad)");
        adLoaded = true;
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        //MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
        adNotLoaded = true;
    }

    public void HandleOnAdOpened(object sender, EventArgs args) {
        MonoBehaviour.print("HandleAdOpened event received (interstitial ad)");
    }

    public void HandleOnAdClosed(object sender, EventArgs args) {
        MonoBehaviour.print("HandleAdClosed event received (interstitial ad)");
        RemoveInterstitialAd();
        LoadInterstitialAd();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args) {
        MonoBehaviour.print("HandleAdLeavingApplication event received (interstitial ad)");
    }


} // end of class