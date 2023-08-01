#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace cn_rongcloud_rtc_unity
{
#region Delegate
    
    internal delegate void OnNetworkStats(ref rtc_network_stats cstats);
    
    internal delegate void OnLocalAudioStats(ref rtc_local_audio_stats cstats);
    
    internal delegate void OnLocalVideoStats(ref rtc_local_video_stats cstats);
    
    internal delegate void OnRemoteAudioStats(string userId, ref rtc_remote_audio_stats cstats);
    
    internal delegate void OnRemoteVideoStats(string userId, ref rtc_remote_video_stats cstats);
    
    internal delegate void OnLiveMixAudioStats(ref rtc_remote_audio_stats cstats);
    
    internal delegate void OnLiveMixVideoStats(ref rtc_remote_video_stats cstats);
    
    internal delegate void OnLiveMixMemberAudioStats(String userId, int volume);
    
    internal delegate void OnLiveMixMemberCustomAudioStats(String userId, String tag, int volume);
    
    internal delegate void OnAudioFrame(String userId, ref rtc_audio_frame cframe);
    
#region IRCRTCIWListener
    internal delegate void OnLiveMixBackgroundColorSet(int code, string errMsg);
    
    internal delegate void OnAudioMixingProgressReported(float progress);
    
    internal delegate void OnCustomStreamPublished(string tag, int code, string errMsg);
    
    internal delegate void OnCustomStreamPublishFinished(string tag);
    
    internal delegate void OnCustomStreamUnpublished(string tag, int code, string errMsg);
    
    internal delegate void OnRemoteCustomStreamPublished(string roomId, string userId, string tag, RCRTCMediaType type);
    
    internal delegate void OnRemoteCustomStreamUnpublished(string roomId, string userId, string tag, RCRTCMediaType type);
    
    internal delegate void OnRemoteCustomStreamStateChanged(string roomId, string userId, string tag, RCRTCMediaType type,
                                                            bool disabled);
    
    internal delegate void OnRemoteCustomStreamFirstFrame(string roomId, string userId, string tag, RCRTCMediaType type);
    
    internal delegate void OnCustomStreamSubscribed(string userId, string tag, RCRTCMediaType type, int code,
                                                    string errMsg);
    
    internal delegate void OnCustomStreamUnsubscribed(string userId, string tag, RCRTCMediaType type, int code,
                                                      string errMsg);
    
    internal delegate void OnJoinSubRoomRequested(string roomId, string userId, int code, string errMsg);
    
    internal delegate void OnJoinSubRoomRequestCanceled(string roomId, string userId, int code, string errMsg);
    
    internal delegate void OnJoinSubRoomRequestResponded(string roomId, string userId, bool agree, int code, string errMsg);
    
    internal delegate void OnJoinSubRoomRequestReceived(string roomId, string userId, string extra);
    
    internal delegate void OnCancelJoinSubRoomRequestReceived(string roomId, string userId, string extra);
    
    internal delegate void OnJoinSubRoomRequestResponseReceived(string roomId, string userId, bool agree, string extra);
    
    internal delegate void OnSubRoomJoined(string roomId, int code, string errMsg);
    
    internal delegate void OnSubRoomLeft(string roomId, int code, string errMsg);
    
    internal delegate void OnSubRoomBanded(string roomId);
    
    internal delegate void OnSubRoomDisband(string roomId, string userId);
    
    internal delegate void OnLiveRoleSwitched(RCRTCRole current, int code, string errMsg);
    
    internal delegate void OnRemoteLiveRoleSwitched(string roomId, string userId, RCRTCRole role);
    
    internal delegate void OnLiveMixInnerCdnStreamEnabled(bool enable, int code, string errMsg);
    
    internal delegate void OnRemoteLiveMixInnerCdnStreamPublished();
    
    internal delegate void OnRemoteLiveMixInnerCdnStreamUnpublished();
    
    internal delegate void OnLiveMixInnerCdnStreamSubscribed(int code, string errMsg);
    
    internal delegate void OnLiveMixInnerCdnStreamUnsubscribed(int code, string errMsg);
    
    internal delegate void OnLocalLiveMixInnerCdnVideoResolutionSet(int code, string errMsg);
    
    internal delegate void OnLocalLiveMixInnerCdnVideoFpsSet(int code, string errMsg);
    
    internal delegate void OnWatermarkSet(int code, string errMsg);
    
    internal delegate void OnWatermarkRemoved(int code, string errMsg);
    
    internal delegate void OnNetworkProbeStarted(int code, string errMsg);
    
    internal delegate void OnNetworkProbeStopped(int code, string errMsg);
    
    internal delegate void OnSeiEnabled(bool enable, int code, string errMsg);
    
    internal delegate void OnSeiReceived(string roomId, string userId, string sei);
    
    internal delegate void OnLiveMixSeiReceived(string sei);
    
#endregion
    
#endregion
    
#region Callback Proxy
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_stats_listener_proxy
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool remove;
        public OnNetworkStats onNetworkStats;
        public OnLocalAudioStats onLocalAudioStats;
        public OnLocalVideoStats onLocalVideoStats;
        public OnRemoteAudioStats onRemoteAudioStats;
        public OnRemoteVideoStats onRemoteVideoStats;
        public OnLiveMixAudioStats onLiveMixAudioStats;
        public OnLiveMixVideoStats onLiveMixVideoStats;
        public OnLiveMixMemberAudioStats onLiveMixMemberAudioStats;
        public OnLiveMixMemberCustomAudioStats OnLiveMixMemberCustomAudioStats;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_writable_audio_frame_listener_proxy
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool remove;
        public OnAudioFrame onAudioFrame;
    }
    
#region RCRTCIWNetworkProbeListener
    internal delegate void IOSNetworkProbeProxyOnNetworkProbeUpLinkStats(IntPtr stats, Int64 handle);
    
    internal delegate void IOSNetworkProbeProxyOnNetworkProbeDownLinkStats(IntPtr stats, Int64 handle);
    
    internal delegate void IOSNetworkProbeProxyOnNetworkProbeFinished(int code, string errMsg, Int64 handle);
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct ios_network_probe_proxy
    {
        public Int64 handle;
        public IOSNetworkProbeProxyOnNetworkProbeUpLinkStats onNetworkProbeUpLinkStats;
        public IOSNetworkProbeProxyOnNetworkProbeDownLinkStats onNetworkProbeDownLinkStats;
        public IOSNetworkProbeProxyOnNetworkProbeFinished onNetworkProbeFinished;
    }
    
#endregion
    
#endregion
}
    
#endif