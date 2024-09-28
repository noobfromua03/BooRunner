using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdvertWrapper : MonoBehaviour
{
    #region Instance
    private static AdvertWrapper _instance;

    public static AdvertWrapper Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("_AdvertWrapper");
                _instance = go.AddComponent<AdvertWrapper>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    #endregion
    public static bool DisableAds = false;

    private List<IAdvertWrapper> advertWrappers = new List<IAdvertWrapper>();
    public CrossPromoWrapper crossPromoWrapper;


    private Action onAnyFail;

    public async UniTask Init()
    {
#if UNITY_EDITOR
        InitEditor();
#endif
        InitCrossPromo();
        await UniTask.Yield();
        InitIronSource();

        //TODO check intrnet connection or no video ads
        //onAnyFail = () => Windows.Open(WindowType.NoInternet);
    }

    private void InitIronSource()
    {
        Debug.Log("AdvertWrapper: InitIronSource");
        var adWrapper = new IronSourceWrapper();
        adWrapper.Init();
        advertWrappers.Add(adWrapper);
    }

    private void InitCrossPromo()
    {
        crossPromoWrapper = new CrossPromoWrapper();
        crossPromoWrapper.Init();
        advertWrappers.Add(crossPromoWrapper);
    }

    private void InitEditor()
    {
        var adWrapper = new EditorAdvertWrapper();
        adWrapper.Init();
        advertWrappers.Add(adWrapper);
    }

    public void ShowInterstitial(string location, Action<bool> onShow, Action onAnyError = null)
    {

        if (onAnyError == null)
        {
            onAnyError = onAnyFail;
        }

        foreach (var advertWrapper in advertWrappers)
        {
            SetSubsForAd(advertWrapper, location, "Interstitial");
            if (advertWrapper.TryShowInterstitial(onShow))
            {
                return;
            }
        }

        SetSubsForAd(crossPromoWrapper, location, "Interstitial");
        if (crossPromoWrapper.TryForceShowInterstitial(onShow))
        {
            return;
        }

        onAnyError?.Invoke();
    }

    public void ShowRewardedVideo(string location, Action<bool> onShow, Action onAnyError = null)
    {
        if (onAnyError == null)
        {
            onAnyError = onAnyFail;
        }

        foreach (var advertWrapper in advertWrappers)
        {
            SetSubsForAd(advertWrapper, location, "Reward");
            if (advertWrapper.TryShowVideo(onShow))
            {
                return;
            }
        }

        SetSubsForAd(crossPromoWrapper, location, "Reward");
        if (crossPromoWrapper.TryForceShowVideo(onShow))
        {
            return;
        }

        onAnyError?.Invoke();
    }

    public void ShowBanner()
    {
        foreach (var advertWrapper in advertWrappers)
        {
            advertWrapper.TryShowBanner();
        }
    }

    public void SetSubsForAd(IAdvertWrapper wrapper, string location, string type)
    {
        Action onOpenAd = () =>
        {
            //Analytics.LogAdEvent(type, location, wrapper.Name);
            //GUILoadingAnimation.Instance.ShowOnlyTint();
            //AudioController.Instance.PauseAll();
            Debug.Log("AdvertWrapper: Open" + wrapper.Name + " " + type + " " + location);
        };

        Action onClosedAd = () =>
        {
            //GUILoadingAnimation.Instance.Hide();
            //AudioController.Instance.UnpauseAll();
            Debug.Log("AdvertWrapper: Close" + wrapper.Name + " " + type + " " + location);
        };

        if (type == "Interstitial")
        {
            wrapper.OnOpenInterstitial = onOpenAd;
            wrapper.OnClosedInterstitial = onClosedAd;
        }
        else if (type == "Reward")
        {
            wrapper.OnOpenRewardedVideo = onOpenAd;
            wrapper.OnClosedRewardedVideo = onClosedAd;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        foreach (var advertWrapper in advertWrappers)
        {
            advertWrapper.OnApplicationPause(pause);
        }
    }
}
