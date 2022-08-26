using System;

namespace cn_rongcloud_rtc_unity
{
    /// <summary>
    /// 网络信息
    /// </summary>
    public readonly struct RCRTCNetworkStats
    {

        public RCRTCNetworkStats(RCRTCNetworkType type, String ip, int sendBitrate, int receiveBitrate, int rtt)
        {
            this.Type = type;
            this.Ip = ip;
            this.SendBitrate = sendBitrate;
            this.ReceiveBitrate = receiveBitrate;
            this.Rtt = rtt;
        }

        /// <summary>
        /// 网络类型, WLAN 4G
        /// </summary>
        public RCRTCNetworkType Type { get; }

        /// <summary>
        /// 网络地址
        /// </summary>
        public String Ip { get; }

        /// <summary>
        /// 发送码率
        /// </summary>
        public int SendBitrate { get; }

        /// <summary>
        /// 接收码率
        /// </summary>
        public int ReceiveBitrate { get; }

        /// <summary>
        /// 发送到服务端往返时间
        /// </summary>
        public int Rtt { get; }

    }

    /// <summary>
    /// 本地音频信息
    /// </summary>
    public readonly struct RCRTCLocalAudioStats
    {

        public RCRTCLocalAudioStats(RCRTCAudioCodecType codec, int bitrate, int volume, double packageLostRate, int rtt)
        {
            this.Codec = codec;
            this.Bitrate = bitrate;
            this.Volume = volume;
            this.PackageLostRate = packageLostRate;
            this.Rtt = rtt;
        }

        /// <summary>
        /// 音频编码
        /// </summary>
        public RCRTCAudioCodecType Codec { get; }
        /// <summary>
        /// 码率
        /// </summary>
        public int Bitrate { get; }
        /// <summary>
        /// 音量, 0 ~ 9 表示音量高低
        /// </summary>
        public int Volume { get; }
        /// <summary>
        /// 丢包率
        /// </summary>
        public double PackageLostRate { get; }
        /// <summary>
        /// 发送到服务端往返时间
        /// </summary>
        public int Rtt { get; }

    }

    /// <summary>
    /// 本地视频信息
    /// </summary>
    public readonly struct RCRTCLocalVideoStats
    {

        public RCRTCLocalVideoStats(bool tiny, RCRTCVideoCodecType codec, int bitrate, int fps, int width, int height, double packageLostRate, int rtt)
        {
            this.Tiny = tiny;
            this.Codec = codec;
            this.Bitrate = bitrate;
            this.Fps = fps;
            this.Width = width;
            this.Height = height;
            this.PackageLostRate = packageLostRate;
            this.Rtt = rtt;
        }
        /// <summary>
        /// 是否小流
        /// </summary>
        public bool Tiny { get; }
        /// <summary>
        /// 视频编码
        /// </summary>
        public RCRTCVideoCodecType Codec { get; }
        /// <summary>
        /// 码率
        /// </summary>
        public int Bitrate { get; }
        /// <summary>
        /// 帧率
        /// </summary>
        public int Fps { get; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// 丢包率
        /// </summary>
        public double PackageLostRate { get; }
        /// <summary>
        /// 发送到服务端往返时间
        /// </summary>
        public int Rtt { get; }

    }

    /// <summary>
    /// 远程音频信息
    /// </summary>
    public readonly struct RCRTCRemoteAudioStats
    {

        public RCRTCRemoteAudioStats(RCRTCAudioCodecType codec, int bitrate, int volume, double packageLostRate, int rtt)
        {
            this.Codec = codec;
            this.Bitrate = bitrate;
            this.Volume = volume;
            this.PackageLostRate = packageLostRate;
            this.Rtt = rtt;
        }
        /// <summary>
        /// 音频编码
        /// </summary>
        public RCRTCAudioCodecType Codec { get; }
        /// <summary>
        /// 码率
        /// </summary>
        public int Bitrate { get; }
        /// <summary>
        /// 音量, 0 ~ 9 表示音量高低
        /// </summary>
        public int Volume { get; }
        /// <summary>
        /// 丢包率
        /// </summary>
        public double PackageLostRate { get; }
        /// <summary>
        /// 发送到服务端往返时间
        /// </summary>
        public int Rtt { get; }

    }

    /// <summary>
    /// 远程视频信息
    /// </summary>
    public readonly struct RCRTCRemoteVideoStats
    {

        public RCRTCRemoteVideoStats(RCRTCVideoCodecType codec, int bitrate, int fps, int width, int height, double packageLostRate, int rtt)
        {
            this.Codec = codec;
            this.Bitrate = bitrate;
            this.Fps = fps;
            this.Width = width;
            this.Height = height;
            this.PackageLostRate = packageLostRate;
            this.Rtt = rtt;
        }
        /// <summary>
        /// 视频编码
        /// </summary>
        public RCRTCVideoCodecType Codec { get; }
        /// <summary>
        /// 码率
        /// </summary>
        public int Bitrate { get; }
        /// <summary>
        /// 帧率
        /// </summary>
        public int Fps { get; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// 丢包率
        /// </summary>
        public double PackageLostRate { get; }
        /// <summary>
        /// 发送到服务端往返时间
        /// </summary>
        public int Rtt { get; }

    }

}