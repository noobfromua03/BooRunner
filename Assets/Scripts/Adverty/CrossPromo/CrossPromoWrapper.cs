using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrossPromo;
using Cysharp.Threading.Tasks;

public class CrossPromoWrapper : IAdvertWrapper
{
    public Action OnOpenInterstitial { get; set; }
    public Action OnClosedInterstitial { get; set; }
    public Action OnOpenRewardedVideo { get; set; }
    public Action OnClosedRewardedVideo { get; set; }

    public string Name => "CrossPromo";

    private ICrossPromoWrapper crossPromo;

    private bool isRunInterstitial = false;
    private bool isRewardGranted = false;
    private Action<bool> onShowed;

    public void Init()
    {
#if UNITY_ANDROID
        crossPromo = AndroidWrapper.Instance;
#elif UNITY_IOS
        crossPromo = IOSWrapper.Instance;
#endif

        crossPromo.OnRewardGranted += OnRewardGranted;
        crossPromo.OnAdClosed += OnAdClosed;
        //NetworkManager.Instance.SubOrInvokeToNetworkAvailable(crossPromo.Init);
    }

    public void OnApplicationPause(bool isPaused)
    {

    }

    public bool TryShowInterstitial(Action<bool> onShowed)
    {
        if (crossPromo.AdShowRateReady)
        {
            TryForceShowInterstitial(onShowed);
            return true;
        }

        crossPromo.IncrementShowRate();
        return false;
    }

    public bool TryShowVideo(Action<bool> onShowed)
    {
        if (crossPromo.AdShowRateReady)
        {
            TryForceShowVideo(onShowed);
            return true;
        }

        crossPromo.IncrementShowRate();
        return false;
    }

    public bool TryShowBanner()
    {
        return false;
    }

    public bool TryForceShowInterstitial(Action<bool> onShowed)
    {
        Debug.Log("TryForceShowInterstitial");
        isRunInterstitial = true;
        this.onShowed = onShowed;

        if (crossPromo.AdIsReady)
        {
            OnOpenInterstitial?.Invoke();
            crossPromo.ShowInterstitial();
            return true;
        }

        return false;
    }

    public bool TryForceShowVideo(Action<bool> onShowed)
    {
        Debug.Log("TryForceShowVideo");
        isRunInterstitial = false;
        this.onShowed = onShowed;
        isRewardGranted = false;

        if (crossPromo.AdIsReady)
        {
            OnOpenRewardedVideo?.Invoke();
            crossPromo.ShowRewardedVideo();
            return true;
        }

        return false;
    }

    private void OnRewardGranted()
    {
        isRewardGranted = true;
    }

    private async void OnAdClosed()
    {
        if (isRunInterstitial)
        {
            OnClosedInterstitial?.Invoke();
            onShowed?.Invoke(true);
        }
        else
        {
            OnClosedRewardedVideo?.Invoke();
            onShowed?.Invoke(isRewardGranted);
        }

        onShowed = null;
        crossPromo.ResetShowRate();
        await UniTask.Delay(1000);
        crossPromo.RequestVideo();
    }

}
