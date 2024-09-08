using System;

public static class AdvertService
{
    public static void ShowInterstitial(Action onWatch, Action onError)
    {
        onWatch?.Invoke();
    }

    public static void ShowRewarded(Action onWatch, Action onError)
    {
        onWatch?.Invoke();
    }
}

