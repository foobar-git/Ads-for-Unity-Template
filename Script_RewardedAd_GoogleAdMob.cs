using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class Script_RewardedAd_GoogleAdMob : MonoBehaviour
{
    private string rewardedAd_ID;
    private string test_rewardedAd_ID;

    private Script_AdManager_GoogleAdMob script_AdManager;
    private RewardedAd rewardedAd;

    private string type;
    private double amount;

    private string adID;
    private bool isTest;
    private bool request_newRewardedAd;
    private bool adLoaded;
    private bool adNotLoaded;
    private bool adFailedToShow;
    private bool receivedReward;

    //====================================================================================================

    public Script_AdManager_GoogleAdMob set_ref_script_AdManager_GoogleAdMob {
        set { script_AdManager = value; }
    }

    public bool set_TEST_MODE {
        set {
            isTest = value;
            Debug.Log("TEST MODE (rewarded ad, setter): " + isTest);
            adID = SetAdID(isTest);
            Debug.Log("adID (rewarded ad, setter): " + adID);
            if (request_newRewardedAd) this.RequestRewardedAd();
        }
    }

    string SetAdID(bool b) {
        if (b) return script_AdManager.get_rewardedAd_testID;
        else return script_AdManager.get_rewardedAd_ID;
    }

    void Awake() {
        adLoaded = false;
        adNotLoaded = false;
        adFailedToShow = false;
        receivedReward = false;
        request_newRewardedAd = true;
    }

    void RequestRewardedAd() {
        this.rewardedAd = new RewardedAd(adID);

        //====================================================================================================
        // SUBSCRIBE TO EVENTS:    ///////////////////////////////////////////////////////////////////////////
        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += this.HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += this.HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += this.HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += this.HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += this.HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += this.HandleRewardedAdClosed;

        
        LoadRewardedAd();
        request_newRewardedAd = false;
    }

    public void LoadRewardedAd() {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

        // Set ad status text.
        script_AdManager.UpdateAdStatus("Loading new Rewarded ad...");
    }

    public void ShowRewardedAd() {
        if (rewardedAd != null) {
            if (this.rewardedAd.IsLoaded()) {
                this.rewardedAd.Show();
            }
            CreateAndLoadNewRewardedAd();
        } else {
            Debug.Log("Rewarded ad not enabled.");
            script_AdManager.UpdateAdStatus("Rewarded ad not enabled.");
        }
    }

    public void CreateAndLoadNewRewardedAd() { // when loading a new reward video ad after the previous finishes
        this.rewardedAd = new RewardedAd(adID);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        LoadRewardedAd();
    }

    private void FixedUpdate() {

        if (adLoaded) {
            script_AdManager.UpdateAdStatus("Rewarded ad loaded.");
            adLoaded = false;
        }

        if (adNotLoaded) {
            script_AdManager.UpdateAdStatus("Rewarded ad failed to load.");
            adNotLoaded = false;
        }

        if (adFailedToShow) {
            script_AdManager.UpdateAdStatus("Rewarded ad failed to show.");
            adFailedToShow = false;
        }

        if (receivedReward) {
            Debug.Log("You get a reward!");
            script_AdManager.UpdateAdStatus("Reward received.");
            receivedReward = false;
        }
    }

    //====================================================================================================
    // FOR EVENTS AND DELEGATES:    //////////////////////////////////////////////////////////////////////
    public void HandleRewardedAdLoaded(object sender, EventArgs args) {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
        adLoaded = true;
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        //MonoBehaviour.print("HandleRewardedAdFailedToLoad event received with message: " + args);
        adNotLoaded = true;
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args) {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args) {
        //MonoBehaviour.print("HandleRewardedAdFailedToShow event received with message: " + args.Message);
        adFailedToShow = true;
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args) {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args) {
        type = args.Type;
        amount = args.Amount;
        MonoBehaviour.print("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);
        receivedReward = true;
    }


} // end of class