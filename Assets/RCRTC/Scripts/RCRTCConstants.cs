using System;

namespace cn_rongcloud_rtc_unity
{
    /// <summary>
    /// 网络类型
    /// </summary>
    public enum RCRTCNetworkType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOWN = 0,
        /// <summary>
        /// Wifi
        /// </summary>
        WIFI,
        /// <summary>
        /// 手机
        /// </summary>
        MOBILE,
    }

    /// <summary>
    /// 获取音频编码类型
    /// </summary>
    public enum RCRTCAudioCodecType
    {
        PCMU = 0,
        OPUS = 111
    }

    /// <summary>
    /// 视频编码
    /// </summary>
    public enum RCRTCVideoCodecType
    {
        H264 = 0
    }

    /// <summary>
    /// 音质
    /// </summary>
    public enum RCRTCAudioQuality
    {
        /// <summary>
        /// 人声音质，编码码率最大值为 32Kbps
        /// </summary>
        SPEECH = 32,
        /// <summary>
        /// 标清音乐音质，编码码率最大值为 64Kbps
        /// </summary>
        MUSIC = 64,
        /// <summary>
        /// 高清音乐音质，编码码率最大值为 128Kbps
        /// </summary>
        MUSIC_HIGH = 128
    }

    /// <summary>
    /// 音频通话模式
    /// </summary>
    public enum RCRTCAudioScenario
    {
        /// <summary>
        /// 普通通话模式(普通音质模式), 满足正常音视频场景
        /// </summary>
        DEFAULT = 0,
        /// <summary>
        /// 音乐聊天室模式, 提升声音质量，适用对音乐演唱要求较高的场景
        /// </summary>
        MUSIC_CHATROOM,
        /// <summary>
        /// 音乐教室模式，提升声音质量，适用对乐器演奏音质要求较高的场景
        /// </summary>
        MUSIC_CLASSROOM
    }

    /// <summary>
    /// 摄像头类型
    /// </summary>
    public enum RCRTCCamera
    {
        /// <summary>
        /// 无
        /// </summary>
        NONE = -1,
        /// <summary>
        /// 前置摄像头
        /// </summary>
        FRONT = 0,
        /// <summary>
        /// 后置摄像头
        /// </summary>
        BACK
    }

#if UNITY_STANDALONE_WIN
    /// <summary>
    /// 硬件设备
    /// </summary>
    public class RCRTCDevice
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string name;
        /// <summary>
        /// 设备id
        /// </summary>
        public string id;
        /// <summary>
        /// 设备序号
        /// </summary>
        public int index;

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return id == ((RCRTCDevice)obj).id;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
#endif

    /// <summary>
    /// 音视频类型
    /// </summary>
    public enum RCRTCMediaType
    {
        /// <summary>
        /// 仅音频
        /// </summary>
        AUDIO = 0,
        /// <summary>
        /// 仅视频
        /// </summary>
        VIDEO,
        /// <summary>
        /// 音频 + 视频
        /// </summary>
        AUDIO_VIDEO,
    }

    /// <summary>
    /// 角色类型
    /// </summary>
    public enum RCRTCRole
    {
        /// <summary>
        /// 会议类型房间中用户
        /// </summary>
        MEETING_MEMBER = 0,
        /// <summary>
        /// 直播类型房间中主播
        /// </summary>
        LIVE_BROADCASTER,
        /// <summary>
        /// 直播类型房间中观众
        /// </summary>
        LIVE_AUDIENCE
    }

    /// <summary>
    /// 视频帧率
    /// </summary>
    public enum RCRTCVideoFps
    {
        /// <summary>
        /// 每秒 10 帧
        /// </summary>
        FPS_10 = 0,
        /// <summary>
        /// 每秒 15 帧
        /// </summary>
        FPS_15,
        /// <summary>
        /// 每秒 24 帧
        /// </summary>
        FPS_24,
        /// <summary>
        /// 每秒 30 帧
        /// </summary>
        FPS_30
    }

    /// <summary>
    /// 视频分辨率
    /// </summary>
    public enum RCRTCVideoResolution
    {
        RESOLUTION_144_176,
        RESOLUTION_144_256,
        RESOLUTION_180_180,
        RESOLUTION_180_240,
        RESOLUTION_180_320,
        RESOLUTION_240_240,
        RESOLUTION_240_320,
        RESOLUTION_360_360,
        RESOLUTION_360_480,
        RESOLUTION_360_640,
        RESOLUTION_480_480,
        RESOLUTION_480_640,
        RESOLUTION_480_848,
        RESOLUTION_480_720,
        RESOLUTION_720_960,
        RESOLUTION_720_1280,
        RESOLUTION_1080_1920
    }

    /// <summary>
    /// 摄像头采集方向
    /// </summary>
    public enum RCRTCCameraCaptureOrientation
    {
        /// <summary>
        /// 竖直, home 键在下部
        /// </summary>
        PORTRAIT = 1,
        /// <summary>
        /// 顶部向下, home 键在上部
        /// </summary>
        PORTRAIT_UPSIDE_DOWN,
        /// <summary>
        /// 顶部向右, home 键在左侧
        /// </summary>
        LANDSCAPE_RIGHT,
        /// <summary>
        /// 顶部向左, home 键在右侧
        /// </summary>
        LANDSCAPE_LEFT
    }

    /// <summary>
    /// 渲染模式
    /// </summary>
    public enum RCRTCViewSurfaceType
    {
        Renderer = 0,
        RawImage = 1,
    }

    /// <summary>
    /// 填充模式
    /// </summary>
    public enum RCRTCViewFitType
    {
        /// <summary>
        /// 填充
        /// </summary>
        FILL = 0,
        /// <summary>
        /// 裁剪
        /// </summary>
        COVER,
        /// <summary>
        /// 自适应
        /// </summary>
        CENTER
    }
#if UNITY_IOS
    /// <summary>
    /// 渲染类型
    /// </summary>    
    public enum RCRTCViewRenderType
    {
        /// <summary>
        /// Metal渲染
        /// </summary>
        MetalRender,
        /// <summary>
        /// Unity渲染
        /// </summary>
        UnityRender
    }
#endif
    /// <summary>
    /// 合流布局模式
    /// </summary>
    public enum RCRTCLiveMixLayoutMode
    {
        /// <summary>
        /// 自定义布局
        /// </summary>
        CUSTOM = 1,
        /// <summary>
        /// 悬浮布局
        /// </summary>
        SUSPENSION,
        /// <summary>
        /// 自适应布局
        /// </summary>
        ADAPTIVE
    }

    /// <summary>
    /// 输出视频流的裁剪模式
    /// </summary>
    public enum RCRTCLiveMixRenderMode
    {
        /// <summary>
        /// 自适应裁剪
        /// </summary>
        CROP = 1,
        /// <summary>
        /// 填充
        /// </summary>
        WHOLE
    }

    /// <summary>
    /// 混音行为模式
    /// </summary>
    public enum RCRTCAudioMixingMode
    {
        /// <summary>
        /// 对端只能听见麦克风采集的声音
        /// </summary>
        NONE = 0,
        /// <summary>
        /// 对端能够听到麦克风采集的声音和音频文件的声音
        /// </summary>
        MIX,
        /// <summary>
        /// 对端只能听到音频文件的声音
        /// </summary>
        REPLACE
    }

    /// <summary>
    /// 纹理类型
    /// </summary>
    public enum RCRTCTextureType
    {
        NONE = -1,
        OES = 0,
        RGB
    }

    /// <summary>
    /// 视频帧类型
    /// </summary>
    public enum RCRTCVideoFrameDataType
    {
        TEXTURE,
        NV21,
    }

}