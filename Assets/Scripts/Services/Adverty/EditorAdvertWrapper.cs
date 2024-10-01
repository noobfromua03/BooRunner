using System;

public class EditorAdvertWrapper : IAdvertWrapper
{
    public Action OnOpenInterstitial { get; set; }
    public Action OnClosedInterstitial { get; set; }
    public Action OnOpenRewardedVideo { get; set; }
    public Action OnClosedRewardedVideo { get; set; }

    public string Name => "Editor";

    public void Init()
    {

    }

    public void OnApplicationPause(bool isPaused)
    {

    }

    public bool TryShowInterstitial(Action<bool> onShowed)
    {
        onShowed?.Invoke(true);
        return true;
    }

    public bool TryShowVideo(Action<bool> onShowed)
    {
        onShowed?.Invoke(true);
        return true;
    }

    public bool TryShowBanner()
    {
        return true;
    }
}
