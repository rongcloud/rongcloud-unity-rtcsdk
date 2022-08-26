using System;

namespace cn_rongcloud_rtc_unity
{
    /// <summary>
    /// 音频设置
    /// </summary>
    public struct RCRTCAudioConfig
    {
        private RCRTCAudioConfig(RCRTCAudioQuality quality, RCRTCAudioScenario scenario)
        {
            this.quality = quality;
            this.scenario = scenario;
        }

        /// <summary>
        /// 创建音频设置
        /// </summary>
        /// <returns>音频设置</returns>
        public static RCRTCAudioConfig Create()
        {
            return new RCRTCAudioConfig(RCRTCAudioQuality.SPEECH, RCRTCAudioScenario.DEFAULT);
        }

        /// <summary>
        /// 获取音频质量
        /// </summary>
        /// <returns>音频质量</returns>
        public RCRTCAudioQuality GetQuality()
        {
            return quality;
        }

        /// <summary>
        /// 获取音频场景
        /// </summary>
        /// <returns>音频场景</returns>
        public RCRTCAudioScenario GetScenario()
        {
            return scenario;
        }

        /// <summary>
        /// 设置音频质量
        /// </summary>
        /// <param name="quality">音频质量</param>
        /// <returns></returns>
        public RCRTCAudioConfig SetQuality(RCRTCAudioQuality quality)
        {
            this.quality = quality;
            return this;
        }

        /// <summary>
        /// 获取音频场景
        /// </summary>
        /// <param name="scenario">音频场景</param>
        /// <returns></returns>
        public RCRTCAudioConfig SetScenario(RCRTCAudioScenario scenario)
        {
            this.scenario = scenario;
            return this;
        }

        private RCRTCAudioQuality quality;
        private RCRTCAudioScenario scenario;
    }

    /// <summary>
    /// 视频设置
    /// </summary>
    public struct RCRTCVideoConfig
    {
        private RCRTCVideoConfig(int minBitrate, int maxBitrate, RCRTCVideoFps fps, RCRTCVideoResolution resolution)
        {
            this.minBitrate = minBitrate;
            this.maxBitrate = maxBitrate;
            this.fps = fps;
            this.resolution = resolution;
        }

        /// <summary>
        /// 创建视频设置
        /// </summary>
        /// <returns>音频设置</returns>
        public static RCRTCVideoConfig Create()
        {
            return new RCRTCVideoConfig(250, 2200, RCRTCVideoFps.FPS_24, RCRTCVideoResolution.RESOLUTION_720_1280);
        }
        /// <summary>
        /// 获取最小码率
        /// </summary>
        /// <returns></returns>
        public int GetMinBitrate()
        {
            return minBitrate;
        }

        /// <summary>
        /// 设置最小码率
        /// </summary>
        /// <param name="minBitrate">单位kbs</param>
        /// <returns></returns>
        public RCRTCVideoConfig SetMinBitrate(int minBitrate)
        {
            this.minBitrate = minBitrate;
            return this;
        }

        /// <summary>
        /// 获取最大码率
        /// </summary>
        /// <returns></returns>
        public int GetMaxBitrate()
        {
            return maxBitrate;
        }

        /// <summary>
        /// 设置最大码率
        /// </summary>
        /// <param name="minBitrate">单位kbs</param>
        /// <returns></returns>
        public RCRTCVideoConfig SetMaxBitrate(int maxBitrate)
        {
            this.maxBitrate = maxBitrate;
            return this;
        }

        /// <summary>
        /// 获取帧率
        /// </summary>
        /// <returns></returns>
        public RCRTCVideoFps GetFps()
        {
            return fps;
        }

        /// <summary>
        /// 设置帧率
        /// </summary>
        /// <param name="fps"></param>
        /// <returns></returns>
        public RCRTCVideoConfig SetFps(RCRTCVideoFps fps)
        {
            this.fps = fps;
            return this;
        }

        /// <summary>
        /// 获取分辨率
        /// </summary>
        /// <returns></returns>
        public RCRTCVideoResolution GetResolution()
        {
            return resolution;
        }

        /// <summary>
        /// 设置分辨率
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public RCRTCVideoConfig SetResolution(RCRTCVideoResolution resolution)
        {
            this.resolution = resolution;
            return this;
        }

        private int minBitrate;
        private int maxBitrate;
        private RCRTCVideoFps fps;
        private RCRTCVideoResolution resolution;

        public override string ToString()
        {
            return $"Bitrate [{minBitrate} ~ {maxBitrate}] FPS {fps} Resolution {resolution}";
        }
    }
}