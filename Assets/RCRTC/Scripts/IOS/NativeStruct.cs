#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace cn_rongcloud_rtc_unity
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct ios_c_map_item
    {
        public string key;
        public string value;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct ios_c_list
    {
        public IntPtr list;
        public long count;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct ios_class_warpper
    {
        public IntPtr obj;
        public string type;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_audio_setup
    {
        public int audioCodecType;
        public bool mixOtherAppsAudio;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_video_setup
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool enableTinyStream;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_engine_setup
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool reconnectable;
    
        public int statsReportInterval;
    
        [MarshalAs(UnmanagedType.U1)]
        public bool enableSRTP;
        public IntPtr audioSetup;
        public IntPtr videoSetup;
        public string mediaUrl;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_room_setup
    {
        public int role;
        public int mediaType;
        public int joinType;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_audio_config
    {
        public int quality;
        public int scenario;
        public int recordingVolume;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_video_config
    {
        public int minBitrate;
        public int maxBitrate;
        public int fps;
        public int resolution;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_custom_layout
    {
        public int type;
        public string id;
        public string tag;
        public int x;
        public int y;
        public int width;
        public int height;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_network_stats
    {
        public int type;
        public string ip;
        public int sendBitrate;
        public int receiveBitrate;
        public int rtt;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_local_audio_stats
    {
        public int codec;
        public int bitrate;
        public int volume;
        public double packageLostRate;
        public int rtt;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_local_video_stats
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool tiny;
        public int codec;
        public int bitrate;
        public int fps;
        public int width;
        public int height;
        public double packageLostRate;
        public int rtt;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_remote_audio_stats
    {
        public int codec;
        public int bitrate;
        public int volume;
        public double packageLostRate;
        public int rtt;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_remote_video_stats
    {
        public int codec;
        public int bitrate;
        public int fps;
        public int width;
        public int height;
        public double packageLostRate;
        public int rtt;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_audio_frame
    {
        public IntPtr data;
        public int length;
        public int channels;
        public int sampleRate;
        public int bytesPerSample;
        public int samples;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct rtc_network_probe_stats
    {
        public int qualityLevel;
        public long rtt;
        public float packetLostRate;
    }
    
}
    
#endif