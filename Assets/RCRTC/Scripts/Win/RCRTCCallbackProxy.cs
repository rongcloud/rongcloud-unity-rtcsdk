//
//  Copyright © 2021 RongCloud. All rights reserved.
//

#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

namespace cn_rongcloud_rtc_unity
{
    #region delegate

    internal delegate void OnNetworkStats(ref rtc_network_stats cstats);

    internal delegate void OnLocalAudioStats(ref rtc_local_audio_stats cstats);

    internal delegate void OnLocalVideoStats(ref rtc_local_video_stats cstats);

    internal delegate void OnRemoteAudioStats(string userId, ref rtc_remote_audio_stats cstats);

    internal delegate void OnRemoteVideoStats(string userId, ref rtc_remote_video_stats cstats);

    internal delegate void OnLiveMixAudioStats(ref rtc_remote_audio_stats cstats);

    internal delegate void OnLiveMixVideoStats(ref rtc_remote_video_stats cstats);

    internal delegate void OnVideoFrame(string uid, ref rtc_video_frame cframe);

    internal delegate void OnAudioFrame(string uid, ref rtc_audio_frame cframe);

    internal delegate void OnCameraSwitched(ref rtc_device camera, int code, String errMsg);

    #endregion

    #region callback proxy

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_stats_listener_proxy
    {
        [MarshalAs(UnmanagedType.U1)] public bool remove;
        public OnNetworkStats onNetworkStats;
        public OnLocalAudioStats onLocalAudioStats;
        public OnLocalVideoStats onLocalVideoStats;
        public OnRemoteAudioStats onRemoteAudioStats;
        public OnRemoteVideoStats onRemoteVideoStats;
        public OnLiveMixAudioStats onLiveMixAudioStats;
        public OnLiveMixVideoStats onLiveMixVideoStats;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_video_listener_proxy
    {
        [MarshalAs(UnmanagedType.U1)] public bool remove;
        public OnVideoFrame onVideoFrame;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_audio_listener_proxy
    {
        [MarshalAs(UnmanagedType.U1)] public bool remove;
        public OnAudioFrame onAudioFrame;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_writable_audio_frame_listener_proxy
    {
        [MarshalAs(UnmanagedType.U1)] public bool remove;
        public OnAudioFrame onAudioFrame;
    }

    #endregion
}
#endif