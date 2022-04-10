using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


[ExecuteAlways] // used to enable logic when in editor mode
public class Script_AdMaster : MonoBehaviour
{
    [SerializeField] private bool TEST_MODE = true; // for Unity Ads specificly, disable TEST MODE in 'Services > Ads > Configure' as well
    [SerializeField] private bool googleAdMobAds = true;
    [SerializeField] private bool unityAds;

    [SerializeField] GameObject AdManager_GoogleAdMob;
    [SerializeField] GameObject AdManager_UnityAds;
    [SerializeField] GameObject GoogleAdMob;
    [SerializeField] GameObject UnityAds;

    private Script_AdManager_GoogleAdMob script_AdManager_GoogleAdMob;
    private Script_BannerAd_GoogleAdMob script_BannerAd_GoogleAdMob;
    private Script_InterstitialAd_GoogleAdMob script_InterstitialAd_GoogleAdMob;
    private Script_RewardedAd_GoogleAdMob script_RewardedAd_GoogleAdMob;

    private Script_AdManager_UnityAds script_AdManager_UnityAds;
    private Script_BannerAd_UnityAds script_BannerAd_UnityAds;
    private Script_InterstitialAd_UnityAds script_InterstitialAd_UnityAds;
    private Script_RewardedAd_UnityAds script_RewardAd_UnityAds;

    private CanvasGroup canvasGroup;

    private bool enabled_EditorMode = true;
    private bool getAndSetReferences = true;
    private bool runOnce = true;

    //====================================================================================================

    public void ClearLog() { // Clear developer console
        #if UNITY_EDITOR
            var assembly = Assembly.GetAssembly(typeof(Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        #endif
    }

    //____________________________________________________________________________________________________
    // Update (when using [ExecuteAlways]) is only called when something in the Scene changed (editor mode)
    void Update() {
        if (Application.IsPlaying(this.gameObject)) {
            // Play logic (when play mode is on)
            enabled_EditorMode = false;
            if (runOnce) {
                GetReferencesToObjectsAndScripts();
                Initialize_AdPlatform();
                CheckFor_TEST_MODE();
                runOnce = false;
            }
        } else {
            // Editor logic (editor mode, when play mode is off)
            enabled_EditorMode = true;
            GetReferencesToObjectsAndScripts();
            Select_AD_PLATFORM();   // select via inspector to use Google AdMob or Unity Ads
            CheckFor_TEST_MODE();
            CheckFor_DummyGameObjects();
        }
    }//____________________________________________________________________________________________________

    void GetReferencesToObjectsAndScripts() {
        if (getAndSetReferences) {
            if (googleAdMobAds) {
                //AdManager_GoogleAdMob = GameObject.Find("AdManager_GoogleAdMob");
                script_AdManager_GoogleAdMob = AdManager_GoogleAdMob.GetComponent<Script_AdManager_GoogleAdMob>();
                script_BannerAd_GoogleAdMob = AdManager_GoogleAdMob.GetComponent<Script_BannerAd_GoogleAdMob>();
                script_InterstitialAd_GoogleAdMob = AdManager_GoogleAdMob.GetComponent<Script_InterstitialAd_GoogleAdMob>();
                script_RewardedAd_GoogleAdMob = AdManager_GoogleAdMob.GetComponent<Script_RewardedAd_GoogleAdMob>();
            }

            if (unityAds) {
                //AdManager_UnityAds = GameObject.Find("AdManager_UnityAds");
                script_AdManager_UnityAds = AdManager_UnityAds.GetComponent<Script_AdManager_UnityAds>();
                script_BannerAd_UnityAds = AdManager_UnityAds.GetComponent<Script_BannerAd_UnityAds>();
                script_InterstitialAd_UnityAds = AdManager_UnityAds.GetComponent<Script_InterstitialAd_UnityAds>();
                script_RewardAd_UnityAds = AdManager_UnityAds.GetComponent<Script_RewardedAd_UnityAds>();
            }

            SetReferences();

            getAndSetReferences = false; // ensure so it runs only once
        }
    }

    void SetReferences() {
        if (googleAdMobAds) {
            script_BannerAd_GoogleAdMob.set_ref_script_AdManager_GoogleAdMob = script_AdManager_GoogleAdMob;
            script_InterstitialAd_GoogleAdMob.set_ref_script_AdManager_GoogleAdMob = script_AdManager_GoogleAdMob;
            script_RewardedAd_GoogleAdMob.set_ref_script_AdManager_GoogleAdMob = script_AdManager_GoogleAdMob;
        }

        if (unityAds) {
            script_BannerAd_UnityAds.set_ref_script_AdManager_UnityAds = script_AdManager_UnityAds;
            script_InterstitialAd_UnityAds.set_ref_script_AdManager_UnityAds = script_AdManager_UnityAds;
            script_RewardAd_UnityAds.set_ref_script_AdManager_UnityAds = script_AdManager_UnityAds;
        }
    }

    void CheckFor_DummyGameObjects() { // Google AdMob may create a "dummy game object" called "New Game Object"
        DestroyImmediate(GameObject.Find("New Game Object"));
    }

    void CheckFor_TEST_MODE() {
        if (TEST_MODE) EnableTestMode(true);
        else EnableTestMode(false);
    }

    void Select_AD_PLATFORM() {
        // This is the normal logic:
        // googleAdMobAds = !unityAds;

        #if UNITY_EDITOR
            // However, we need it serialized because of Unity's Prefab/Instance logic.
            // Don't change it without checking the documentation, or ad platform switching may not work.
            // You can read more in the Unity documentation on SerializedObject and SerializedProperty
            var serializedObject = new SerializedObject(this);
            serializedObject.FindProperty("googleAdMobAds").boolValue = !unityAds;
            serializedObject.ApplyModifiedProperties();
        #endif

        //ClearLog();

        if (googleAdMobAds) Enable_GoogleAdMob(true);

        if (unityAds) Enable_UnityAds(true);
    }

    void Initialize_AdPlatform() {
        if (googleAdMobAds) {
            Debug.Log("Enable Google AdMob initialization.");
            script_AdManager_GoogleAdMob.Initialize_GoogleAdMobAds();
        }

        if (unityAds) {
            Debug.Log("Enable UnityAds initialization.");
            script_AdManager_UnityAds.Initialize_UnityAds();
        }

    }

    void EnableTestMode(bool b) {
        if (googleAdMobAds) {
            Debug.Log("TEST MODE (Google AdMob): " + TEST_MODE);
            if (script_AdManager_GoogleAdMob != null) {

                if (script_AdManager_GoogleAdMob.get_bannerAd_enabled) script_BannerAd_GoogleAdMob.set_TEST_MODE = b;
                if (script_AdManager_GoogleAdMob.get_interstititalAd_enabled) script_InterstitialAd_GoogleAdMob.set_TEST_MODE = b;
                if (script_AdManager_GoogleAdMob.get_rewardedAd_enabled) script_RewardedAd_GoogleAdMob.set_TEST_MODE = b;
            }
        }

        if (unityAds) {
            if (script_AdManager_UnityAds != null) {
                script_AdManager_UnityAds.set_TEST_MODE = b;

                if (!enabled_EditorMode) { // this bool is needed for specefic Unity Ads implementation, in contrast to Google AdMob
                    if (script_AdManager_UnityAds.get_bannerAd_enabled) script_BannerAd_UnityAds.LoadNewBanner();
                    if (script_AdManager_UnityAds.get_interstitialAd_enabled) script_InterstitialAd_UnityAds.LoadNewAd();
                    if (script_AdManager_UnityAds.get_rewardAd_enabled) script_RewardAd_UnityAds.LoadNewAd();
                }
            }
        }
    }

    void Enable_GoogleAdMob(bool b) {
        Select_CanvasGroup(GoogleAdMob, b);
        Select_CanvasGroup(UnityAds, !b);
    }

    void Enable_UnityAds(bool b) {
        Select_CanvasGroup(GoogleAdMob, !b);
        Select_CanvasGroup(UnityAds, b);
    }

    void Select_CanvasGroup(GameObject obj, bool b) { // to be visible or not
        canvasGroup = obj.GetComponent<CanvasGroup>();
        set_CanvasGroup = b;
    }

    private bool set_CanvasGroup {
        set {
            canvasGroup.alpha = value ? 1 : 0;
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }
    }


} // end of class
