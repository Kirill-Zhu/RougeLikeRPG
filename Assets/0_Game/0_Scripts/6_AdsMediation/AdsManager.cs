using Unity.Services.LevelPlay;
using UnityEngine;
public class AdsManager : MonoBehaviour {
    [Header("App Key")]
    [SerializeField] string AppKey;
    [Header("BannerID")]
    [SerializeField] string AndroidID;
    private LevelPlayBannerAd bannerAd;
    //[Header("Interstitial")]
    //private LevelPlayInterstitialAd interstitialAd;
    //private LevelPlayRewardedAd rewardedAd;
    public void Start() {

        // Register OnInitFailed and OnInitSuccess listeners
        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
        // SDK init
        LevelPlay.Init(AppKey);
    }
    void SdkInitializationCompletedEvent(LevelPlayConfiguration config) {
        CreateBannerAd();   
        Debug.Log($" Received SdkInitializationCompletedEvent with Config: {config}");
    }

    void SdkInitializationFailedEvent(LevelPlayInitError error) {
        Debug.Log($" Received SdkInitializationFailedEvent with Error: {error}");
    }

    #region Banner

    void CreateBannerAd() {
        // Create ad configuration - optional
        var adConfig = new LevelPlayBannerAd.Config.Builder()
          .SetSize(LevelPlayAdSize.BANNER)
          .SetPosition(LevelPlayBannerPosition.BottomCenter)
          .SetDisplayOnLoad(true)
          .SetRespectSafeArea(true)
          .Build();

        // Create banner instance
        bannerAd = new LevelPlayBannerAd(AndroidID, adConfig);

        // Subscribe BannerAd events
        bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
        bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
        bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        bannerAd.OnAdClicked += BannerOnAdClickedEvent;
        bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
        bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;
    }

    [ContextMenu("LoadBanner")]
    public void LoadBannerAd() {
        //Load the banner ad 
        bannerAd.LoadAd();
    }

    public void ShowBannerAd() {
        //Show the banner ad, call this method only if you turned off the auto show when you created this banner instance.
        bannerAd.ShowAd();
    }

    public void HideBannerAd() {
        //Hide banner
        bannerAd.HideAd();
    }

    public void DestroyBannerAd() {
        //Destroy banner
        bannerAd.DestroyAd();
    }

    //Implement BannerAd Events
    public void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    public void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError) { }
    public void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    public void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    public void BannerOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
    public void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo) { }
    public void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo) { }
    public void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo) { }

    #endregion
    //#region InterstitialAd
    //void CreateInterstitialAd() {
    //    // Create InterstitialAd instance
    //    interstitialAd = new LevelPlayInterstitialAd("YOUR_INTERSTITIAL_AD_UNIT_ID");

    //    // Subscribe InterstitialAd events
    //    interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
    //    interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
    //    interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
    //    interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
    //    interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
    //    interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
    //    interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;
    //}

    //void LoadInterstitialAd() {
    //    // Load or reload InterstitialAd
    //    interstitialAd.LoadAd();
    //}

    //void ShowInterstitialAd() {
    //    // Show InterstitialAd, check if the ad is ready before showing
    //    if (interstitialAd.IsAdReady()) {
    //        interstitialAd.ShowAd();
    //    }
    //}

    //void DestroyInterstitialAd() {
    //    // Destroy InterstitialAd
    //    interstitialAd.DestroyAd();
    //}

    //// Implement InterstitialAd events
    //void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    //void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error) { }
    //void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    //void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    //void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
    //void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo) { }
    //void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }
    //#endregion
    //#region RewardAd
    //void CreateRewardedAd() {
    //    rewardedAd = new LevelPlayRewardedAd("YOUR_REWARDED_AD_UNIT_ID");
    //    rewardedAd.OnAdLoaded += OnAdLoaded;
    //    rewardedAd.OnAdLoadFailed += OnAdLoadFailed;
    //    rewardedAd.OnAdDisplayed += OnAdDisplayed;
    //    rewardedAd.OnAdDisplayFailed += RewardedAd_OnAdDisplayFailed;
    //    rewardedAd.OnAdRewarded += OnAdRewarded;
    //    rewardedAd.OnAdClicked += OnAdClicked;
    //    rewardedAd.OnAdClosed += OnAdClosed;
    //    rewardedAd.OnAdInfoChanged += OnAdInfoChanged;
    //}

    //private void RewardedAd_OnAdDisplayFailed(LevelPlayAdInfo arg1, LevelPlayAdError arg2) {
    //    Debug.Log($"Rewarded ad displayed with error {arg2}");
    //}

    //void LoadRewardedAd() {
    //    rewardedAd.LoadAd();
    //}

    //void ShowRewardedAd(string placementName = null) {
    //    if (rewardedAd.IsAdReady() && !LevelPlayRewardedAd.IsPlacementCapped(placementName)) {
    //        rewardedAd.ShowAd(placementName);
    //    }
    //}

    //bool CheckIfRewardedAdIsReady() {
    //    return rewardedAd.IsAdReady();
    //}

    //bool CheckIfPlacementIsCapped(string placementName) {
    //    return LevelPlayRewardedAd.IsPlacementCapped(placementName);
    //}

    //void OnAdLoaded(LevelPlayAdInfo adInfo) {
    //    Debug.Log($"Rewarded ad loaded with ad info {adInfo}");
    //}

    //void OnAdLoadFailed(LevelPlayAdError adError) {
    //    Debug.Log($"Rewarded ad failed to load with ad error {adError}");
    //}

    //void OnAdDisplayed(LevelPlayAdInfo adInfo) {
    //    Debug.Log($"Rewarded ad displayed with ad info {adInfo}");
    //}

    //void OnAdRewarded(LevelPlayAdInfo adInfo, LevelPlayReward adReward) {
    //    Debug.Log($"Rewarded ad gained reward with adInfo {adInfo} and reward {adReward}");
    //}

    //void OnAdClicked(LevelPlayAdInfo adInfo) {
    //    Debug.Log($"Rewarded ad clicked with ad info {adInfo}");
    //}

    //void OnAdClosed(LevelPlayAdInfo adInfo) {
    //    Debug.Log($"Rewarded ad closed with ad info {adInfo}");
    //}

    //void OnAdInfoChanged(LevelPlayAdInfo adInfo) {
    //    Debug.Log($"Rewarded ad info changed with ad info {adInfo}");
    //}
    //#endregion
}


