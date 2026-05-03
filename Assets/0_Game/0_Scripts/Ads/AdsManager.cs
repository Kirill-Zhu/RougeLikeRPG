using TMPro;
using Unity.Services.LevelPlay;
using UnityEngine;
public class AdsManager : MonoBehaviour {
    [Header("App key")]
    [SerializeField] string androidAppKey;
    [Header("Banner ad Unit Id")]
    [SerializeField] string andoridBannerAdUnitId;
    [Header("Interstitial Ad Unit Id")]
    [SerializeField] string androidIntestitialAdUnitId;
    [Header("Rewarded Ad Unit Id")]
    [SerializeField] string androidRewardedAdUnitId;

    [Header("Conin UI")]
    [SerializeField] TextMeshProUGUI coninsText;
    public int Coints {
        get => PlayerPrefs.GetInt("PLAYER_COINS", 0);

            set {

            PlayerPrefs.SetInt("PLAYER_COINS", value);
            PlayerPrefs.Save();
            UpdateCoin();
        }
    }
    void UpdateCoin() {
        if (coninsText != null) {
            coninsText.text = Coints.ToString();
        }
    }
    LevelPlayBannerAd bannerAd;
    LevelPlayInterstitialAd interstitialAd;
    LevelPlayRewardedAd rewardedAd;
    private void Start() {

        LevelPlay.ValidateIntegration();
        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
        LevelPlay.Init(androidAppKey);


    }

    private void SdkInitializationFailedEvent(LevelPlayInitError error) {
      
        Debug.Log("Initialization failed");
    }

    private void SdkInitializationCompletedEvent(LevelPlayConfiguration configuration) {
        CreateBannerAd();
        CreateInterstitialAd();
        CreateRewardedAD();
        Debug.Log("Initialaized succesfully");
    }

    #region BANNER
    public void ShowBanner() {

        bannerAd.LoadAd();
    }
    public void DestroyBanner() {
        bannerAd.DestroyAd();
    }
    void CreateBannerAd() {
        var adConfig = new LevelPlayBannerAd.Config.Builder()
            .SetPosition(LevelPlayBannerPosition.BottomCenter)
            .Build();

        bannerAd = new LevelPlayBannerAd(andoridBannerAdUnitId, adConfig);

        // Register to the events 
        bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
        bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
        bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        bannerAd.OnAdClicked += BannerOnAdClickedEvent;
        bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
        bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;
    }
    void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError) { }
    void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo) {
        Debug.Log("Banner clicked");
    }
    void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
    void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo) { }
    #endregion

    #region INTERSTITIAL
    void CreateInterstitialAd() {
        interstitialAd = new LevelPlayInterstitialAd(androidIntestitialAdUnitId);

        //Subscribe InterstitialAd events
        interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;
    }
    public void LoadInterstitialAd() {
        //Load or reload InterstitialAd 	
        interstitialAd.LoadAd();
    }
    public void ShowInterstitialAd() {
        //Show InterstitialAd, check if the ad is ready before showing
        if (interstitialAd.IsAdReady()) {
            interstitialAd.ShowAd();
        }
    }
    //Implement InterstitialAd events
    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError ironSourceError) { LoadInterstitialAd(); }
    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo) { LoadInterstitialAd(); }
    void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }

    #endregion
    #region REWARDED

    void CreateRewardedAD() {
        rewardedAd = new LevelPlayRewardedAd(androidRewardedAdUnitId);


        rewardedAd.OnAdLoaded += RewardedOnAdLoadedEvent;
        rewardedAd.OnAdLoadFailed += RewardedOnAdLoadFailedEvent;
        rewardedAd.OnAdDisplayed += RewardedOnAdDisplayedEvent;
        rewardedAd.OnAdDisplayFailed += RewardedOnAdDisplayFailedEvent;
        rewardedAd.OnAdRewarded += RewardedOnAdRewardedEvent;
        rewardedAd.OnAdClosed += RewardedOnAdClosedEvent;
        // Optional
        rewardedAd.OnAdClicked += RewardedOnAdClickedEvent;
        rewardedAd.OnAdInfoChanged += RewardedOnAdInfoChangedEvent;
    }
    public void LoadRewardedAd() {
        rewardedAd.LoadAd();
        Debug.Log("#MYGAME Rewarded Ad loaded");
    }
    public void ShowRewardedAd() {
        if (rewardedAd.IsAdReady()) {
            rewardedAd.ShowAd();
            Debug.Log("#MYGAME rewarded loaded");
        }
    }
    void RewardedOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdLoadFailedEvent(LevelPlayAdError error) {
        // LoadRewardedAd();
    }
    void RewardedOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
    void RewardedOnAdRewardedEvent(LevelPlayAdInfo adInfo, LevelPlayReward adReward) {
        string rewardedName = adReward.Name;
        int rewardAount = adReward.Amount;
        Coints += rewardAount;
        Debug.Log($"#MYGAME Get reward : Reward Name : {rewardedName}, Amount {rewardAount}");
    }
    void RewardedOnAdClosedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }
    #endregion
}
