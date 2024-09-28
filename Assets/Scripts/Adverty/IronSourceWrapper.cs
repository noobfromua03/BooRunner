using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class IronSourceWrapper : IAdvertWrapper
{
#if UNITY_ANDROID
    string appKey = "1ee1b7145";
#elif UNITY_IPHONE
    string appKey = "1ee1b7145";
#else
    string appKey = "unexpected_platform";
#endif

    public Action OnOpenInterstitial { get; set; }
    public Action OnClosedInterstitial { get; set; }
    public Action OnOpenRewardedVideo { get; set; }
    public Action OnClosedRewardedVideo { get; set; }

    private Action<bool> _onShowedInterstitial;
    private Action<bool> _onShowedRewardedVideo;

    public string Name => "IronSource";

    public void Init()
    {
        SubscribeToEvents();
        IronSource.Agent.validateIntegration();
        IronSource.Agent.init(appKey);
        IronSource.Agent.loadInterstitial();
    }

    void SubscribeToEvents()
    {
        //Add Init Event
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

        //Add AdInfo Interstitial Events
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;
    }

    public void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    public bool TryShowInterstitial(Action<bool> onShowed)
    {
        _onShowedInterstitial = onShowed;

        if (IronSource.Agent.isInterstitialReady())
        {
            OnOpenInterstitial?.Invoke();
            IronSource.Agent.showInterstitial();
            return true;
        }

        return false;
    }

    public bool TryShowVideo(Action<bool> onShowed)
    {
        _onShowedRewardedVideo = onShowed;

        IronSource.Agent.loadRewardedVideo();
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            OnOpenRewardedVideo?.Invoke();
            IronSource.Agent.showRewardedVideo();
            return true;
        }

        return false;
    }

    public bool TryShowBanner()
    {
        return false;
    }

    #region Init callback handlers

    void SdkInitializationCompletedEvent()
    {
        IronSource.Agent.loadInterstitial();
        IronSource.Agent.loadRewardedVideo();
        Debug.Log("[IronSourceWrapper] SdkInitializationCompletedEvent");
    }

    #endregion

    #region AdInfo Rewarded Video
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo) { }

    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        OnClosedRewardedVideo?.Invoke();
        IronSource.Agent.loadRewardedVideo();
    }

    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo) { }

    void RewardedVideoOnAdUnavailable() { }

    void RewardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        _onShowedRewardedVideo?.Invoke(false);
    }

    void RewardedVideoOnAdRewardedEvent(
        IronSourcePlacement ironSourcePlacement,
        IronSourceAdInfo adInfo
    )
    {
        _onShowedRewardedVideo?.Invoke(true);
    }

    void RewardedVideoOnAdClickedEvent(
        IronSourcePlacement ironSourcePlacement,
        IronSourceAdInfo adInfo
    ) { }

    #endregion

    #region AdInfo Interstitial

    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo) { }

    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError) { }

    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo) { }

    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        _onShowedInterstitial?.Invoke(false);
    }

    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
    {
        _onShowedInterstitial?.Invoke(true);
    }

    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        _onShowedInterstitial?.Invoke(false);
    }

    async void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        OnClosedInterstitial?.Invoke();
        IronSource.Agent.loadInterstitial();
        await UniTask.Delay(1000);
    }

    #endregion

    #region Banner

    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("BannerOnAdLoadedEvent");
    }

    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
    {
        Debug.Log("BannerOnAdLoadFailedEvent");
    }

    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("BannerOnAdClickedEvent");
    }

    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("BannerOnAdScreenPresentedEvent");
    }

    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("BannerOnAdScreenDismissedEvent");
    }

    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("BannerOnAdLeftApplicationEvent");
    }

    #endregion
}
