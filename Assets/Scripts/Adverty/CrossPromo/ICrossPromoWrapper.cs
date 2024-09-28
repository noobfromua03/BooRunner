using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossPromo
{
    public interface ICrossPromoWrapper
    {
        Action OnAdClosed { get; set; }
        Action OnRewardGranted { get; set; }

        bool IsInited { get; }
        bool AdIsReady { get; }
        bool AdShowRateReady { get; }

        void Init();

        void ShowRewardedVideo();

        void ShowInterstitial();

        void RequestVideo();

        void ResetShowRate();

        void IncrementShowRate();

    }
}
