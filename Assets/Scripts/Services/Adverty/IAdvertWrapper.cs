using System;

public interface IAdvertWrapper
{
    Action OnOpenInterstitial { get; set; }
    Action OnClosedInterstitial { get; set; }
    Action OnOpenRewardedVideo { get; set; }
    Action OnClosedRewardedVideo { get; set; }
    string Name { get;}

    void Init();

    bool TryShowInterstitial(Action<bool> onShowed);

    bool TryShowVideo(Action<bool> onShowed);

    bool TryShowBanner();

    void OnApplicationPause(bool isPaused);
}
