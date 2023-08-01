using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace cn_rongcloud_rtc_unity
{
    public interface RCRTCNetworkProbeListener
    {
        /// <summary>
        ///
        /// </summary>
        void OnNetworkProbeUpLinkStats(RCRTCNetworkProbeStats stats);
    
        /// <summary>
        ///
        /// </summary>
        void OnNetworkProbeDownLinkStats(RCRTCNetworkProbeStats stats);
    
        /// <summary>
        ///
        /// </summary>
        void OnNetworkProbeFinished(int code, string errMsg);
    }
}
