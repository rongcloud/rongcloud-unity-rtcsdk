using System;

namespace cn_rongcloud_rtc_unity
{
    /// <summary>
    /// 引擎配置
    /// </summary>
    public class RCRTCEngineSetup
    {

        private RCRTCEngineSetup(bool reconnectable, int statusReportInterval, bool enableSRTP, RCRTCAudioSetup audioSetup, RCRTCVideoSetup videoSetup, string mediaUrl, string logPath)
        {
            this.reconnectable = reconnectable;
            this.statusReportInterval = statusReportInterval;
            this.enableSRTP = enableSRTP;
            this.audioSetup = audioSetup;
            this.videoSetup = videoSetup;
            this.mediaUrl = mediaUrl;
            this.logPath = logPath;
        }

        /// <summary>
        /// 断网后是否自动重连（超过一分钟不在重连），默认为false
        /// </summary>
        /// <returns></returns>
        public bool IsReconnectable()
        {
            return reconnectable;
        }

        /// <summary>
        /// 状态上报间隔，默认为2s
        /// </summary>
        /// <returns></returns>
        public int GetStatsReportInterval()
        {
            return statusReportInterval;
        }

        /// <summary>
        /// 设置是否使用SRTP，默认为false
        /// </summary>
        /// <returns></returns>
        public bool IsEnableSRTP()
        {
            return enableSRTP;
        }

        /// <summary>
        /// 获取音频设置
        /// </summary>
        /// <returns></returns>
        public RCRTCAudioSetup GetAudioSetup()
        {
            return audioSetup;
        }

        /// <summary>
        /// 获取视频设置
        /// </summary>
        /// <returns></returns>
        public RCRTCVideoSetup GetVideoSetup()
        {
            return videoSetup;
        }
        
        /// <summary>
        /// 媒体文件远端 url
        /// </summary>
        public string GetMediaUrl()
        {
            return mediaUrl;
        }

        /// <summary>
        /// windows 端日志文件输出路径，路径必须真实存在，设置为null不输出日志
        /// </summary>
        public string GetLogPath()
        {
            return logPath;
        }

        /// <summary>
        /// 引擎设置生成
        /// </summary>
        public class Builder
        {
            private Builder()
            {
            }

            public static Builder Create()
            {
                return new Builder();
            }

            public Builder WithReconnectable(bool reconnectable)
            {
                this.reconnectable = reconnectable;
                return this;
            }

            public Builder WithStatsReportInterval(int statusReportInterval)
            {
                this.statusReportInterval = statusReportInterval;
                return this;
            }

            public Builder WithEnableSRTP(bool enableSRTP)
            {
                this.enableSRTP = enableSRTP;
                return this;
            }

            public Builder WithAudioSetup(RCRTCAudioSetup setup)
            {
                this.audioSetup = setup;
                return this;
            }

            public Builder WithVideoSetup(RCRTCVideoSetup setup)
            {
                this.videoSetup = setup;
                return this;
            }

            public Builder WithMediaUrl(string mediaUrl)
            {
                this.mediaUrl = mediaUrl;
                return this;
            }

            public Builder WithLogPath(string logPath)
            {
                this.logPath = logPath;
                return this;
            }

            public RCRTCEngineSetup Build()
            {
                return new RCRTCEngineSetup(reconnectable, statusReportInterval, enableSRTP, audioSetup, videoSetup, mediaUrl, logPath);
            }

            private bool reconnectable = true;
            private int statusReportInterval = 1000;
            private bool enableSRTP = false;
            private RCRTCAudioSetup audioSetup = null;
            private RCRTCVideoSetup videoSetup = null;
            private string mediaUrl = null;
            private string logPath = null;
        }

        private bool reconnectable;
        private int statusReportInterval;
        private bool enableSRTP;
        private string mediaUrl;
        private string logPath;

        private RCRTCAudioSetup audioSetup;
        private RCRTCVideoSetup videoSetup;
    }

    /// <summary>
    /// 引擎音频配置, Windows暂不支持
    /// </summary>
    public class RCRTCAudioSetup
    {
        private RCRTCAudioSetup(RCRTCAudioCodecType audioCodecType, int audioSource, int audioSampleRate, bool enableMicrophone, bool enableStereo, bool mixOtherAppsAudio)
        {
            this.audioCodecType = audioCodecType;
            this.audioSource = audioSource;
            this.audioSampleRate = audioSampleRate;
            this.enableMicrophone = enableMicrophone;
            this.enableStereo = enableStereo;
            this.mixOtherAppsAudio = mixOtherAppsAudio;
        }

        /// <summary>
        /// 获取音频编码类型
        /// </summary>
        /// <returns></returns>
        public RCRTCAudioCodecType GetAudioCodecType()
        {
            return audioCodecType;
        }

        /// <summary>
        /// 获取音源类型 0 默认音频源 1 麦克风 2 语音呼叫上行音频源 3 语音呼叫下行音频源 4 语音呼叫音频源 5 同方向的相机麦克风，若相机无内置相机或无法识别，则使用预设的麦克风 6 进过语音识别后的麦克风音频源 7 针对VoIP调整后的麦克风音频源
        /// </summary>
        /// <returns></returns>
        public int GetAudioSource()
        {
            return audioSource;
        }

        /// <summary>
        /// 仅在 android 平台生效
        /// 音频采样率，支持的音频采样率有：8000，16000， 32000， 44100， 48000。 默认为 16000
        /// </summary>
        /// <returns></returns>
        public int GetAudioSampleRate()
        {
            return audioSampleRate;
        }

        /// <summary>
        /// 获取麦克风是否开启
        /// </summary>
        /// <returns></returns>
        public bool IsEnableMicrophone()
        {
            return enableMicrophone;
        }

        /// <summary>
        /// 仅在 iOS 平台生效
        /// 默认 YES：是否可以和其它后台 App 进行混音
        /// 特别注意：如果该属性设置为 NO，切换到其它 App 操作麦克风或者扬声器时，会导致自己 App 麦克风采集和播放被打断。
        /// </summary>
        public bool IsMixOtherAppsAudio()
        {
            return mixOtherAppsAudio;
        }

        /// <summary>
        /// 是否开启立体声
        /// </summary>
        /// <returns></returns>
        public bool IsEnableStereo()
        {
            return enableStereo;
        }

        /// <summary>
        /// 引擎音频设置生成
        /// </summary>
        public class Builder
        {
            private Builder()
            {
            }

            public static Builder Create()
            {
                return new Builder();
            }

            public Builder WithAudioCodecType(RCRTCAudioCodecType type)
            {
                this.audioCodecType = type;
                return this;
            }

            public Builder WithAudioSource(int audioSource)
            {
                this.audioSource = audioSource;
                return this;
            }

            public Builder WithAudioSampleRate(int audioSampleRate)
            {
                this.audioSampleRate = audioSampleRate;
                return this;
            }

            public Builder WithEnableMicrophone(bool enableMicrophone)
            {
                this.enableMicrophone = enableMicrophone;
                return this;
            }

            public Builder WithEnableStereo(bool enableStereo)
            {
                this.enableStereo = enableStereo;
                return this;
            }

            public Builder withMixOtherAppsAudio(bool mixOtherAppsAudio)
            {
                this.mixOtherAppsAudio = mixOtherAppsAudio;
                return this;
            }

            public RCRTCAudioSetup Build()
            {
                return new RCRTCAudioSetup(audioCodecType, audioSource, audioSampleRate, enableMicrophone, enableStereo, mixOtherAppsAudio);
            }

            private RCRTCAudioCodecType audioCodecType = RCRTCAudioCodecType.OPUS;
            private int audioSource = 7;
            private int audioSampleRate = 16000;
            private bool enableMicrophone = true;
            private bool enableStereo = true;
            private bool mixOtherAppsAudio = true;
        }

        private RCRTCAudioCodecType audioCodecType;
        /// 仅在 android 平台生效
        private int audioSource;
        private int audioSampleRate;
        private bool enableMicrophone;
        private bool enableStereo;
        /// 仅在 iOS 平台生效
        private bool mixOtherAppsAudio;
    }

    /// <summary>
    /// 引擎视频配置
    /// </summary>
    public class RCRTCVideoSetup
    {
        private RCRTCVideoSetup(bool enableTinyStream, bool enableHardwareDecoder, bool enableHardwareEncoder, bool enableHardwareEncoderHighProfile, int hardwareEncoderFrameRate, bool enableTexture)
        {
            this.enableTinyStream = enableTinyStream;
            this.enableHardwareDecoder = enableHardwareDecoder;
            this.enableHardwareEncoder = enableHardwareEncoder;
            this.enableHardwareEncoderHighProfile = enableHardwareEncoderHighProfile;
            this.hardwareEncoderFrameRate = hardwareEncoderFrameRate;
            this.enableTexture = enableTexture;
        }

        /// <summary>
        /// 是否发布小流
        /// </summary>
        /// <returns></returns>
        public bool IsEnableTinyStream()
        {
            return enableTinyStream;
        }

        /// <summary>
        /// 是否使用硬解码，SDK 会根据硬件支持情况创建硬解码器，如果创建失败会使用软解 默认 true
        /// </summary>
        public bool IsEnableHardwareDecoder()
        {
            return enableHardwareDecoder;
        }

        /// <summary>
        /// 是否使用硬编码,SDK 会根据硬件支持情况创建硬编码器，如果创建失败则使用软编 默认 true
        /// </summary>
        public bool IsEnableHardwareEncoder()
        {
            return enableHardwareEncoder;
        }

        /// <summary>
        /// 设置硬编码压缩等级是否为 MediaCodecInfo.CodecProfileLevel.AVCProfileHigh ，ProfileHigh 比 AVCProfileBaseline 压缩率更高，但是 AVCProfileBaseline 兼容性更好， AVCProfileHigh 压缩等级为 MediaCodecInfo.CodecProfileLevel.AVCLevel3
        /// 默认 false
        /// </summary>
        public bool IsEnableHardwareEncoderHighProfile()
        {
            return enableHardwareEncoderHighProfile;
        }

        /// <summary>
        /// 设置系统硬编码器的编码帧率 默认 30
        /// </summary>
        public int GetHardwareEncoderFrameRate()
        {
            return hardwareEncoderFrameRate;
        }

        /// <summary>
        /// 视频流采集方式，设置视频流是否采用 texture 采集 默认 true
        /// </summary>
        public bool IsEnableTexture()
        {
            return enableTexture;
        }

        /// <summary>
        /// 引擎视频设置生成
        /// </summary>
        public class Builder
        {
            private Builder()
            {
            }

            public static Builder Create()
            {
                return new Builder();
            }

            public Builder WithEnableTinyStream(bool enable)
            {
                this.enableTinyStream = enable;
                return this;
            }

            public Builder WithEnableHardwareDecoder(bool enable)
            {
                this.enableHardwareDecoder = enable;
                return this;
            }

            public Builder WithEnableHardwareEncoder(bool enable)
            {
                this.enableHardwareEncoder = enable;
                return this;
            }

            public Builder WithEnableHardwareEncoderHighProfile(bool enable)
            {
                this.enableHardwareEncoderHighProfile = enable;
                return this;
            }

            public Builder WithHardwareEncoderFrameRate(int rate)
            {
                this.hardwareEncoderFrameRate = rate;
                return this;
            }

            public Builder WithEnableTexture(bool enable)
            {
                this.enableTexture = enable;
                return this;
            }

            public RCRTCVideoSetup Build()
            {
                return new RCRTCVideoSetup(enableTinyStream, enableHardwareDecoder, enableHardwareEncoder, enableHardwareEncoderHighProfile, hardwareEncoderFrameRate, enableTexture);
            }

            private bool enableTinyStream = true;
            private bool enableHardwareDecoder = true;
            private bool enableHardwareEncoder = true;
            private bool enableHardwareEncoderHighProfile = false;
            private int hardwareEncoderFrameRate = 30;
            private bool enableTexture = true;
        }

        private bool enableTinyStream;

        /// 以下参数仅在android平台生效
        private bool enableHardwareDecoder;
        private bool enableHardwareEncoder;
        private bool enableHardwareEncoderHighProfile;
        private int hardwareEncoderFrameRate;
        private bool enableTexture;
    }

    /// <summary>
    /// 房间配置
    /// </summary>
    public class RCRTCRoomSetup
    {
        private RCRTCRoomSetup(RCRTCRole role, RCRTCMediaType mediaType, RCRTCJoinType joinType)
        {
            this.role = role;
            this.mediaType = mediaType;
            this.joinType = joinType;
        }

        /// <summary>
        /// 获取用户角色 默认 MeetingMember
        /// </summary>
        /// <returns></returns>
        public RCRTCRole GetRole()
        {
            return role;
        }

        /// <summary>
        /// 获取通话类型 默认 AudioVideo
        /// </summary>
        /// <returns></returns>
        public RCRTCMediaType GetMediaType()
        {
            return mediaType;
        }

        /// <summary>
        /// 多端加入房间处理类型 默认 KICK
        /// </summary>
        /// <returns></returns>
        public RCRTCJoinType GetJoinType()
        {
            return joinType;
        }

        /// <summary>
        /// 房间设置生成
        /// </summary>
        public class Builder
        {

            public static Builder Create()
            {
                return new Builder();
            }

            private Builder()
            {
            }

            public Builder WithRole(RCRTCRole role)
            {
                this.role = role;
                return this;
            }

            public Builder WithMediaType(RCRTCMediaType type)
            {
                this.mediaType = type;
                return this;
            }

            public Builder WithJoinType(RCRTCJoinType type)
            {
                this.joinType = type;
                return this;
            }

            public RCRTCRoomSetup Build()
            {
                return new RCRTCRoomSetup(role, mediaType, joinType);
            }

            private RCRTCRole role = RCRTCRole.MEETING_MEMBER;
            private RCRTCMediaType mediaType = RCRTCMediaType.AUDIO_VIDEO;
            private RCRTCJoinType joinType = RCRTCJoinType.KICK;
        }

        private RCRTCRole role;
        private RCRTCMediaType mediaType;
        private RCRTCJoinType joinType;
    }

}