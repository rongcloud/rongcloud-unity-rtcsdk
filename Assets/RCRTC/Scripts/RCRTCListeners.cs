using System;

namespace cn_rongcloud_rtc_unity
{
    /// <summary>
    /// 引擎状态监听
    /// </summary>
    public interface RCRTCStatsListener
    {
        /// <summary>
        /// 网络状态更新 
        /// </summary>
        /// <param name="stats">网络状态</param>
        void OnNetworkStats(RCRTCNetworkStats stats);

        /// <summary>
        /// 本地音频信息更新
        /// </summary>
        /// <param name="stats">音频信息</param>
        void OnLocalAudioStats(RCRTCLocalAudioStats stats);

        /// <summary>
        /// 本地视频信息更新
        /// </summary>
        /// <param name="stats">视频信息</param>
        void OnLocalVideoStats(RCRTCLocalVideoStats stats);

        /// <summary>
        /// 远程音频信息更新
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="stats">音频信息</param>
        void OnRemoteAudioStats(String userId, RCRTCRemoteAudioStats stats);

        /// <summary>
        /// 远程视频信息更新
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="stats">视频信息</param>
        void OnRemoteVideoStats(String userId, RCRTCRemoteVideoStats stats);

        /// <summary>
        /// 合流音频信息更新
        /// </summary>
        /// <param name="stats">音频信息</param>
        void OnLiveMixAudioStats(RCRTCRemoteAudioStats stats);

        /// <summary>
        /// 合流视频信息更新
        /// </summary>
        /// <param name="stats">视频信息</param>
        void OnLiveMixVideoStats(RCRTCRemoteVideoStats stats);

        /// <summary>
        /// 本地自定义音频信息更新
        /// </summary>
        /// <param name="tag">全局唯一自定义流id</param>
        /// <param name="stats">音频信息</param>
        void OnLocalCustomAudioStats(String tag, RCRTCLocalAudioStats stats);

        /// <summary>
        /// 本地自定义视频信息更新
        /// </summary>
        /// <param name="tag">全局唯一自定义流id</param>
        /// <param name="stats">视频信息</param>
        void OnLocalCustomVideoStats(String tag, RCRTCLocalVideoStats stats);

        /// <summary>
        /// 远程自定义音频信息更新
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="tag">全局唯一自定义流id</param>
        /// <param name="stats">音频信息</param>
        void OnRemoteCustomAudioStats(String userId, String tag, RCRTCRemoteAudioStats stats);

        /// <summary>
        /// 远程自定义视频信息更新
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="tag">全局唯一自定义流id</param>
        /// <param name="stats">视频信息</param>
        void OnRemoteCustomVideoStats(String userId, String tag, RCRTCRemoteVideoStats stats);
    }

    /// <summary>
    /// 音频修改监听
    /// </summary>
    public interface RCRTCOnWritableAudioFrameListener
    {
        /// <summary>
        /// 音频回调
        /// </summary>
        /// <param name="frame">音频帧数据</param>
        /// <returns>修改后的音频数据，返回frame.Data不修改</returns>
        byte[] OnAudioFrame(ref RCRTCAudioFrame frame);
    }

    /// <summary>
    /// 视频修改监听
    /// </summary>
    public interface RCRTCOnWritableVideoFrameListener
    {
        /// <summary>
        /// 视频回调
        /// </summary>
        /// <param name="frame">视频帧数据</param>
        /// <returns>修改后的视频数据</returns>
        byte[] OnVideoFrame(ref RCRTCVideoFrame frame);
    }

    /// <summary>
    /// 视频回调监听
    /// </summary>
    public interface RCRTCOnVideoFrameListener
    {
        /// <summary>
        /// 视频回调
        /// </summary>
        /// <param name="frame">视频帧数据</param>
        void OnVideoFrame(ref RCRTCVideoFrame frame);
    }

}