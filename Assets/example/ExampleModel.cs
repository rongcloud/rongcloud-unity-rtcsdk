using System;
using System.Collections;
using System.Collections.Generic;
using cn_rongcloud_rtc_unity;
using UnityEngine;

namespace cn_rongcloud_rtc_unity_example
{
    public class LoginData
    {
        public String Name;

        [SerializeField]
        public String userId;
        public String Id { get { return userId; } private set { Id = value; } }

        [SerializeField]
        public String token;
        public String Token { get { return token; } private set { Token = value; } }
    }

    public class Message
    {
        public Message(String name, String content)
        {
            this.name = name;
            this.content = content;
        }

        public String name;
        public String content;
    }

    public class CDN
    {
        public CDN(String id, String name)
        {
            this.id = id;
            this.name = name;
        }
        public String id;
        public String name;
    }

    public class CDNInfo
    {
        public String push;
        public String rtmp;
        public String hls;
        public String flv;
    }

    public class StatsListenerProxy : RCRTCStatsListener
    {
        public StatsListenerProxy(OnNetworkStatsDelegate ons, OnLocalAudioStatsDelegate olas, OnLocalVideoStatsDelegate olvs, OnRemoteAudioStatsDelegate oras, OnRemoteVideoStatsDelegate orvs)
        {
            ONSDelegate = ons;
            OLASDelegate = olas;
            OLVSDelegate = olvs;
            ORASDelegate = oras;
            ORVSDelegate = orvs;
        }

        public void OnNetworkStats(RCRTCNetworkStats stats)
        {
            ONSDelegate.Invoke(stats);
        }

        public void OnLocalAudioStats(RCRTCLocalAudioStats stats)
        {
            OLASDelegate.Invoke(stats);
        }

        public void OnLocalVideoStats(RCRTCLocalVideoStats stats)
        {
            OLVSDelegate.Invoke(stats);
        }

        public void OnRemoteAudioStats(String userId, RCRTCRemoteAudioStats stats)
        {
            ORASDelegate.Invoke(userId, stats);
        }

        public void OnRemoteVideoStats(String userId, RCRTCRemoteVideoStats stats)
        {
            ORVSDelegate.Invoke(userId, stats);
        }

        public void OnLiveMixAudioStats(RCRTCRemoteAudioStats stats)
        {
        }

        public void OnLiveMixVideoStats(RCRTCRemoteVideoStats stats)
        {
        }

        public void OnLocalCustomAudioStats(String tag, RCRTCLocalAudioStats stats)
        {
        }

        public void OnLocalCustomVideoStats(String tag, RCRTCLocalVideoStats stats)
        {
        }

        public void OnRemoteCustomAudioStats(String userId, String tag, RCRTCRemoteAudioStats stats)
        {
        }

        public void OnRemoteCustomVideoStats(String userId, String tag, RCRTCRemoteVideoStats stats)
        {
        }

        public void OnLiveMixMemberAudioStats(string userId, int volume)
        {
            
        }

        public void OnLiveMixMemberCustomAudioStats(string userId, string tag, int volume)
        {
            
        }

        private readonly OnNetworkStatsDelegate ONSDelegate;
        private readonly OnLocalAudioStatsDelegate OLASDelegate;
        private readonly OnLocalVideoStatsDelegate OLVSDelegate;
        private readonly OnRemoteAudioStatsDelegate ORASDelegate;
        private readonly OnRemoteVideoStatsDelegate ORVSDelegate;
    }

    public class LiveStatsListenerProxy : RCRTCStatsListener
    {
        public LiveStatsListenerProxy(OnNetworkStatsDelegate ons, OnLiveMixAudioStatsDelegate oras, OnLiveMixVideoStatsDelegate orvs)
        {
            ONSDelegate = ons;
            OLMASDelegate = oras;
            OLMVSDelegate = orvs;
        }

        public void OnNetworkStats(RCRTCNetworkStats stats)
        {
            ONSDelegate.Invoke(stats);
        }

        public void OnLocalAudioStats(RCRTCLocalAudioStats stats)
        {
        }

        public void OnLocalVideoStats(RCRTCLocalVideoStats stats)
        {
        }

        public void OnRemoteAudioStats(String userId, RCRTCRemoteAudioStats stats)
        {
        }

        public void OnRemoteVideoStats(String userId, RCRTCRemoteVideoStats stats)
        {
        }

        public void OnLiveMixAudioStats(RCRTCRemoteAudioStats stats)
        {
            OLMASDelegate.Invoke(stats);
        }

        public void OnLiveMixVideoStats(RCRTCRemoteVideoStats stats)
        {
            OLMVSDelegate.Invoke(stats);
        }

        public void OnLocalCustomAudioStats(String tag, RCRTCLocalAudioStats stats)
        {
        }

        public void OnLocalCustomVideoStats(String tag, RCRTCLocalVideoStats stats)
        {
        }

        public void OnRemoteCustomAudioStats(String userId, String tag, RCRTCRemoteAudioStats stats)
        {
        }

        public void OnRemoteCustomVideoStats(String userId, String tag, RCRTCRemoteVideoStats stats)
        {
        }

        public void OnLiveMixMemberAudioStats(string userId, int volume)
        {
            
        }

        public void OnLiveMixMemberCustomAudioStats(string userId, string tag, int volume)
        {
            
        }

        private readonly OnNetworkStatsDelegate ONSDelegate;
        private readonly OnLiveMixAudioStatsDelegate OLMASDelegate;
        private readonly OnLiveMixVideoStatsDelegate OLMVSDelegate;
    }

    public class NetworkProbeProxy : RCRTCNetworkProbeListener
    {
        public NetworkProbeProxy(OnNetworkProbeDownLinkStatsDelegate down, OnNetworkProbeUpLinkStatsDelegate up)
        {
            downLinkStats = down;
            upLinkStats = up;
        }

        public void OnNetworkProbeDownLinkStats(RCRTCNetworkProbeStats stats)
        {
            downLinkStats.Invoke(stats);
        }

        public void OnNetworkProbeUpLinkStats(RCRTCNetworkProbeStats stats)
        {
            upLinkStats.Invoke(stats);
        }

        public void OnNetworkProbeFinished(int code, string message)
        {
            
        }

        private readonly OnNetworkProbeDownLinkStatsDelegate downLinkStats;
        private readonly OnNetworkProbeUpLinkStatsDelegate upLinkStats;
    }

    public delegate void OnNetworkStatsDelegate(RCRTCNetworkStats stats);
    public delegate void OnLocalAudioStatsDelegate(RCRTCLocalAudioStats stats);
    public delegate void OnLocalVideoStatsDelegate(RCRTCLocalVideoStats stats);
    public delegate void OnRemoteAudioStatsDelegate(String userId, RCRTCRemoteAudioStats stats);
    public delegate void OnRemoteVideoStatsDelegate(String userId, RCRTCRemoteVideoStats stats);
    public delegate void OnLiveMixAudioStatsDelegate(RCRTCRemoteAudioStats stats);
    public delegate void OnLiveMixVideoStatsDelegate(RCRTCRemoteVideoStats stats);
    public delegate void OnNetworkProbeDownLinkStatsDelegate(RCRTCNetworkProbeStats stats);
    public delegate void OnNetworkProbeUpLinkStatsDelegate(RCRTCNetworkProbeStats stats);
}
