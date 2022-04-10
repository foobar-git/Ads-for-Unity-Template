using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class Script_BannerAd_GoogleAdMob : MonoBehaviour
{
    private string bannerAd_ID;
    private string test_bannerAd_ID;

    private Script_AdManager_GoogleAdMob script_AdManager;
    private BannerView bannerView;
    private AdRequest requestBanner;

    private string adID;
    private bool isTest;
    private bool request_newBannerAd;
    private bool adLoaded;
    private bool adNotLoaded;

    //====================================================================================================

    public Script_AdManager_GoogleAdMob set_ref_script_AdManager_GoogleAdMob {
        set { script_AdManager = value; }
	}

    public bool set_TEST_MODE {
        set {
            isTest = value;
            Debug.Log("TEST MODE (banner ad, setter): " + isTest);
            adID = SetAdID(isTest);
            Debug.Log("adID (banner ad, setter): " + adID);
            if (request_newBannerAd) this.RequestBannerAd();
        }
	}

    string SetAdID(bool b) {
        if (b) return script_AdManager.get_bannerAd_testID;
        else return script_AdManager.get_bannerAd_ID;
	}

    void Awake() {
        adLoaded = false;
        adNotLoaded = false;
        request_newBannerAd = true;

        Debug.Log("TEST MODE (banner ad, Awake()): " + isTest);
    }

    private void RequestBannerAd() {
        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adID, AdSize.Banner, AdPosition.Top);
        // OR create a 320x50 banner ad at coordinate (0,50) on screen.
        //BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, 0, 50);
        /*
        Size:       Description:                Avalability:            AdSize constant:
        320x50      Standard Banner             Phones and Tablets      BANNER
        320x100     Large Banner                Phones and Tablets      LARGE_BANNER
        300x250     IAB Medium Rectangle        Phones and Tablets      MEDIUM_RECTANGLE
        468x60      IAB Full - Size Banner      Tablets                 FULL_BANNER
        728x90      IAB Leaderboard             Tablets                 LEADERBOARD
        Width x Adaptive height Adaptive banner Phones and Tablets      N/ A
        */

        //====================================================================================================
        // SUBSCRIBE TO EVENTS:    ///////////////////////////////////////////////////////////////////////////
        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

        LoadBannerAd();
        //ShowBannerAd();
        request_newBannerAd = false;
    }

    void LoadBannerAd() {
        // Create an empty ad request.
        requestBanner = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(requestBanner);
        bannerView.Hide();

        // Set ad status text.
        script_AdManager.UpdateAdStatus("Loading new Banner ad...");
    }

    public void ShowBannerAd() {
        if (bannerView != null) {
            // Set ad status text.
            script_AdManager.UpdateAdStatus("Showing Banner ad...");
            bannerView.Show();
        } else {
            Debug.Log("Banner ad not enabled.");
            script_AdManager.UpdateAdStatus("Banner ad not enabled.");
        }
    }

    private void FixedUpdate() {
        Debug.Log("TEST MODE (banner ad, FixedUpdate): " + isTest);
        if (adLoaded) {
            script_AdManager.UpdateAdStatus("Banner ad Loaded.");
            adLoaded = false;
        }

        if (adNotLoaded) {
            script_AdManager.UpdateAdStatus("Banner ad failed to load.");
            adNotLoaded = false;
        }
    }

    public void RemoveBannerAd() { // before loading a new banner, make sure to destroy the previous one
        bannerView.Destroy();
    }

    //====================================================================================================
    // FOR EVENTS AND DELEGATES:    //////////////////////////////////////////////////////////////////////
    public void HandleOnAdLoaded(object sender, EventArgs args) {
        MonoBehaviour.print("HandleAdLoaded event received (banner ad)");
        adLoaded = true;
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        //MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
        adNotLoaded = true;
    }

    public void HandleOnAdOpened(object sender, EventArgs args) {
        MonoBehaviour.print("HandleAdOpened event received (banner ad)");
    }

    public void HandleOnAdClosed(object sender, EventArgs args) {
        MonoBehaviour.print("HandleAdClosed event received (banner ad)");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args) {
        MonoBehaviour.print("HandleAdLeavingApplication event received (banner ad)");
    }


} // end of class