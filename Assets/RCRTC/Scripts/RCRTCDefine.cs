using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

namespace cn_rongcloud_rtc_unity
{
    public enum RCRTCNetworkQualityLevel
    {
        /// <summary>
        ///
        /// </summary>
        QUALITY_EXCELLENT,
    
        /// <summary>
        ///
        /// </summary>
        QUALITY_GOOD,
    
        /// <summary>
        ///
        /// </summary>
        QUALITY_POOR,
    
        /// <summary>
        ///
        /// </summary>
        QUALITY_BAD,
    
        /// <summary>
        ///
        /// </summary>
        QUALITY_VERY_BAD,
    
        /// <summary>
        ///
        /// </summary>
        QUALITY_DOWN
    }
    
    public enum RCRTCRole
    {
        /// <summary>
        ///
        /// </summary>
        MEETING_MEMBER,
    
        /// <summary>
        ///
        /// </summary>
        LIVE_BROADCASTER,
    
        /// <summary>
        ///
        /// </summary>
        LIVE_AUDIENCE
    }
    
    public enum RCRTCVideoFps
    {
        /// <summary>
        ///
        /// </summary>
        FPS_10,
    
        /// <summary>
        ///
        /// </summary>
        FPS_15,
    
        /// <summary>
        ///
        /// </summary>
        FPS_24,
    
        /// <summary>
        ///
        /// </summary>
        FPS_30
    }
    
    public enum RCRTCMediaType
    {
        /// <summary>
        ///
        /// </summary>
        AUDIO,
    
        /// <summary>
        ///
        /// </summary>
        VIDEO,
    
        /// <summary>
        ///
        /// </summary>
        AUDIO_VIDEO
    }
}
