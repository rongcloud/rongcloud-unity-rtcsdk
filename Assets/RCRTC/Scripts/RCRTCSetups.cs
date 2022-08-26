using System;

namespace cn_rongcloud_rtc_unity
{
    /// <summary>
    /// 引擎配置
    /// </summary>
    public class RCRTCEngineSetup
    {

        private RCRTCEngineSetup(bool reconnectable, int statusReportInterval, bool enableSRTP, RCRTCAudioSetup audioSetup, RCRTCVideoSetup videoSetup)
        {
            this.reconnectable = reconnectable;
            this.statusReportInterval = statusReportInterval;
            this.enableSRTP = enableSRTP;
            this.audioSetup = audioSetup;
            this.videoSetup = videoSetup;
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

            public RCRTCEngineSetup Build()
            {
                return new RCRTCEngineSetup(reconnectable, statusReportInterval, enableSRTP, audioSetup, videoSetup);
            }

            private bool reconnectable = true;
            private int statusReportInterval = 1000;
            private bool enableSRTP = false;
            private RCRTCAudioSetup audioSetup = null;
            private RCRTCVideoSetup videoSetup = null;
        }

        private bool reconnectable;
        private int statusReportInterval;
        private bool enableSRTP;

        private RCRTCAudioSetup audioSetup;
        private RCRTCVideoSetup videoSetup;
    }

    /// <summary>
    /// 引擎音频配置, Windows暂不支持
    /// </summary>
    public class RCRTCAudioSetup
    {
        private RCRTCAudioSetup(RCRTCAudioCodecType audioCodecType, int audioSource, int audioSampleRate, bool enableMicrophone, bool enableStereo)
        {
            this.audioCodecType = audioCodecType;
            this.audioSource = audioSource;
            this.audioSampleRate = audioSampleRate;
            this.enableMicrophone = enableMicrophone;
            this.enableStereo = enableStereo;
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
        /// 获取采样率
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

            public RCRTCAudioSetup Build()
            {
                return new RCRTCAudioSetup(audioCodecType, audioSource, audioSampleRate, enableMicrophone, enableStereo);
            }

            private RCRTCAudioCodecType audioCodecType = RCRTCAudioCodecType.OPUS;
            private int audioSource = 7;
            private int audioSampleRate = 16000;
            private bool enableMicrophone = true;
            private bool enableStereo = true;
        }

        private RCRTCAudioCodecType audioCodecType;
        private int audioSource;
        private int audioSampleRate;
        private bool enableMicrophone;
        private bool enableStereo;
    }

    /// <summary>
    /// 引擎视频配置
    /// </summary>
    public class RCRTCVideoSetup
    {
        private RCRTCVideoSetup(bool enableTinyStream)
        {
            this.enableTinyStream = enableTinyStream;
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

            public RCRTCVideoSetup Build()
            {
                return new RCRTCVideoSetup(enableTinyStream);
            }

            private bool enableTinyStream = true;
        }

        private bool enableTinyStream;

    }

    /// <summary>
    /// 房间配置
    /// </summary>
    public class RCRTCRoomSetup
    {
        private RCRTCRoomSetup(RCRTCRole role, RCRTCMediaType type)
        {
            this.role = role;
            this.type = type;
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
            return type;
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
                this.type = type;
                return this;
            }

            public RCRTCRoomSetup Build()
            {
                return new RCRTCRoomSetup(role, type);
            }

            private RCRTCRole role = RCRTCRole.MEETING_MEMBER;
            private RCRTCMediaType type = RCRTCMediaType.AUDIO_VIDEO;
        }

        private RCRTCRole role;
        private RCRTCMediaType type;
    }

}