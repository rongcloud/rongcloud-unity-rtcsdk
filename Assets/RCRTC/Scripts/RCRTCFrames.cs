using System;

namespace cn_rongcloud_rtc_unity
{
    /// <summary>
    /// 音频帧
    /// </summary>
    public readonly struct RCRTCAudioFrame
    {
        /// <summary>
        /// 创建音频帧
        /// </summary>
        public RCRTCAudioFrame(byte[] data, int length, int channels, int sampleRate, int bytesPerSample, int samples)
        {
            this.Data = data;
            this.Length = length;
            this.Channels = channels;
            this.SampleRate = sampleRate;
            this.BytesPerSample = bytesPerSample;
            this.Samples = samples;
        }

        /// <summary>
        /// pcm数据
        /// </summary>
        public byte[] Data { get; }
        /// <summary>
        /// 数据长度 单位 字节
        /// </summary>
        public int Length { get; }
        /// <summary>
        /// 声道
        /// </summary>
        public int Channels { get; }
        /// <summary>
        /// 采样率
        /// </summary>
        public int SampleRate { get; }
        /// <summary>
        /// 位深
        /// </summary>
        public int BytesPerSample { get; }
        /// <summary>
        /// 帧数
        /// </summary>
        public int Samples { get; }
    }

    /// <summary>
    /// 视频帧
    /// </summary>
    public readonly struct RCRTCVideoFrame
    {
        /// <summary>
        /// 创建视频帧
        /// </summary>
        public RCRTCVideoFrame(IntPtr data_y, IntPtr data_u, IntPtr data_v, int length, int stride_y, int stride_u, int stride_v, int width, int height)
        {
            this.DataY = data_y;
            this.DataU = data_u;
            this.DataV = data_v;
            this.StrideY = stride_y;
            this.StrideU = stride_u;
            this.StrideV = stride_v;
            this.Length = length;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// 视频帧Y数据指针
        /// </summary>
        public IntPtr DataY { get; }
        /// <summary>
        /// 视频帧U数据指针
        /// </summary>
        public IntPtr DataU { get; }
        /// <summary>
        /// 视频帧V数据指针
        /// </summary>
        public IntPtr DataV { get; }
        /// <summary>
        /// 数据长度 单位 字节
        /// </summary>
        public int Length { get; }
        /// <summary>
        /// Y跨距
        /// </summary>
        public int StrideY { get; }
        /// <summary>
        /// U跨距
        /// </summary>
        public int StrideU { get; }
        /// <summary>
        /// V跨距
        /// </summary>
        public int StrideV { get; }
        /// <summary>
        /// 视频宽度
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// 视频高度
        /// </summary>
        public int Height { get; }
    }

    // public struct RCRTCVideoFrame
    // {

    //     public RCRTCVideoFrame(byte[] data, int length, int width, int height, int rotation, float[] matrix, int timestamp)
    //     {
    //         this.type = RCRTCVideoFrameDataType.NV21;
    //         this.Data = data;
    //         this.Length = length;
    //         this.Width = width;
    //         this.Height = height;
    //         this.Rotation = rotation;
    //         this.Matrix = matrix;
    //         this.Timestamp = timestamp;

    //         this.TextureId = -1;
    //         this.TextureType = RCRTCTextureType.NONE;
    //     }

    //     public RCRTCVideoFrame(int textureId, RCRTCTextureType textureType, int width, int height, int rotation, float[] matrix, int timestamp)
    //     {
    //         this.type = RCRTCVideoFrameDataType.TEXTURE;
    //         this.TextureId = textureId;
    //         this.TextureType = textureType;
    //         this.Width = width;
    //         this.Height = height;
    //         this.Rotation = rotation;
    //         this.Matrix = matrix;
    //         this.Timestamp = timestamp;

    //         this.Data = null;
    //         this.Length = 0;
    //     }

    //     public RCRTCVideoFrameDataType type { get; }
    //     public int TextureId { get; }
    //     public RCRTCTextureType TextureType { get; }
    //     public byte[] Data { get; }
    //     public int Length { get; }
    //     public int Width { get; }
    //     public int Height { get; }
    //     public int Rotation { get; }
    //     public float[] Matrix { get; }
    //     public int Timestamp { get; }

    // }

}