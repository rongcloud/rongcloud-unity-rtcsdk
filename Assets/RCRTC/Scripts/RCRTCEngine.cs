using System;
using AOT;
using System.Collections;
using System.Collections.Generic;

namespace cn_rongcloud_rtc_unity
{
    
    /// <summary>
    /// 音视频引擎类
    /// </summary>
    public abstract class RCRTCEngine
    {
        private static readonly object SynRoot = new object();
    
        internal static RCRTCEngine instance;
    
        /// <summary>
        /// 创建音视频引擎
        /// </summary>
        /// <param name="im_client">im引擎句柄</param>
        /// <returns>音视频引擎</returns>
        public static RCRTCEngine Create()
        {
            return Create(null);
        }
    
        /// <summary>
        /// 创建音视频引擎
        /// </summary>
        /// <param name="setup">初始化配置参数</param>
        /// <param name="im_client">im引擎句柄</param>
        /// <returns>音视频引擎</returns>
        public static RCRTCEngine Create(RCRTCEngineSetup setup)
        {
            if (null == instance)
            {
                lock (SynRoot)
                {
                    if (null == instance)
                    {
#if UNITY_IOS
                        instance = new RCRTCEngineIOS(setup);
#elif UNITY_ANDROID
                        instance = new RCRTCEngineAndroid(setup);
#elif UNITY_STANDALONE_WIN
                        instance = new RCRTCEngineWin(setup);
#else
                        throw new Exception();
#endif
                    }
                }
            }
            return instance;
        }
    
        /// <summary>
        /// 销毁引擎
        /// </summary>
        public virtual void Destroy()
        {
            lock (SynRoot)
            {
                instance = null;
            }
        }
    
        /// <summary>
        /// 加入房间
        /// </summary>
        /// <param name="roomId">房间 id</param>
        /// <param name="setup">房间配置信息</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int JoinRoom(String roomId, RCRTCRoomSetup setup);
    
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int LeaveRoom();
    
        /// <summary>
        /// 加入房间后, 发布本地资源
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int Publish(RCRTCMediaType type);
    
        /// <summary>
        /// 加入房间后, 取消发布已经发布的本地资源
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int Unpublish(RCRTCMediaType type);
    
        /// <summary>
        /// 加入房间后, 订阅远端用户发布的资源
        /// </summary>
        /// <param name="userId">远端用户 UserId</param>
        /// <param name="type">资源类型</param>
        /// <param name="tiny">视频小流, true:订阅视频小流 false:订阅视频大流</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int Subscribe(String userId, RCRTCMediaType type, bool tiny = false);
    
        /// <summary>
        /// 加入房间后, 订阅远端用户发布的资源
        /// </summary>
        /// <param name="userIds">远端用户 UserId 列表</param>
        /// <param name="type">资源类型</param>
        /// <param name="tiny">视频小流, true:订阅视频小流 false:订阅视频大流</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int Subscribe(IList<String> userIds, RCRTCMediaType type, bool tiny = false);
    
        /// <summary>
        /// 加入房间后, 订阅发布的混合资源
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <param name="tiny">视频小流, true:订阅视频小流 false:订阅视频大流</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SubscribeLiveMix(RCRTCMediaType type, bool tiny = false);
    
        /// <summary>
        /// 加入房间后, 取消订阅远端用户发布的资源
        /// </summary>
        /// <param name="userId">远端用户 UserId</param>
        /// <param name="type">资源类型</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int Unsubscribe(String userId, RCRTCMediaType type);
    
        /// <summary>
        /// 加入房间后, 取消订阅远端用户发布的资源
        /// </summary>
        /// <param name="userIds">远端用户 UserId 列表</param>
        /// <param name="type">资源类型</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int Unsubscribe(IList<String> userIds, RCRTCMediaType type);
    
        /// <summary>
        /// 加入房间后, 取消订阅发布的混合资源
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int UnsubscribeLiveMix(RCRTCMediaType type);
    
        /// <summary>
        /// 音频参数配置
        /// </summary>
        /// <param name="config">音频配置信息</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetAudioConfig(RCRTCAudioConfig config);
    
        /// <summary>
        /// 视频参数配置
        /// </summary>
        /// <param name="config">视频配置信息</param>
        /// <param name="tiny">视频大小流, true:视频小流 false:视频大流</param>
        /// <returns> 0: 成功, 非0: 失败</returns>
        public abstract int SetVideoConfig(RCRTCVideoConfig config, bool tiny = false);
    
        /// <summary>
        /// 调整麦克风的音量
        /// </summary>
        /// <param name="volume">0 ~ 100, 默认值: 100</param>
        /// <returns> 0: 成功, 非0: 失败</returns>
        public abstract int AdjustLocalVolume(int volume);
    
#if UNITY_STANDALONE_WIN
        /// <summary>
        /// 获取摄像头设备列表
        /// </summary>
        /// <returns>设备列表 RCRTCDevice[]</returns>
        public abstract RCRTCDevice[] GetCameraList();
    
        /// <summary>
        /// 开启关闭指定摄像头设备
        /// </summary>
        /// <param name="camera">摄像头设备详细信息</param>
        /// <param name="enable">true: 开启 false: 关闭</param>
        /// <param name="asDefault">是否设置为默认视频流采集设备</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int EnableCamera(RCRTCDevice camera, bool enable, bool asDefault);
    
        /// <summary>
        /// 切换摄像头
        /// </summary>
        /// <param name="camera">摄像头设备详细信息</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SwitchCamera(RCRTCDevice camera);
    
        /// <summary>
        /// 获取当前使用摄像头信息
        /// </summary>
        /// <returns>摄像头信息</returns>
        public abstract RCRTCDevice WhichCamera();
    
        /// <summary>
        /// 获取麦克风设备列表
        /// </summary>
        /// <returns>设备列表 RCRTCDevice[]</returns>
        public abstract RCRTCDevice[] GetMicrophoneList();
    
        /// <summary>
        /// 开启关闭指定麦克风设备
        /// </summary>
        /// <param name="microphone">麦克风设备详细信息</param>
        /// <param name="enable">true: 开启 false: 关闭</param>
        /// <param name="asDefault">是否设置为默认音频流采集设备</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int EnableMicrophone(RCRTCDevice microphone, bool enable, bool asDefault);
    
        /// <summary>
        /// 获取扬声器设备列表
        /// </summary>
        /// <returns>设备列表 RCRTCDevice[]</returns>
        public abstract RCRTCDevice[] GetSpeakerList();
    
        /// <summary>
        /// 开启关闭指定扬声器设备
        /// </summary>
        /// <param name="speaker">扬声器设备详细信息</param>
        ///  <param name="enable">true: 开启 false: 关闭</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int EnableSpeaker(RCRTCDevice speaker, bool enable);
#else
        /// <summary>
        /// 开启关闭麦克风
        /// </summary>
        /// <param name="enable">true: 开启 false: 关闭</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int EnableMicrophone(bool enable);
    
        /// <summary>
        /// 听筒/扬声器切换
        /// </summary>
        /// <param name="enable">true: 扬声器 false: 听筒</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int EnableSpeaker(bool enable);
    
        /// <summary>
        /// 开启关闭摄像头
        /// </summary>
        /// <param name="enable">true: 开启 false: 关闭</param>
        /// <param name="camera">FRONT 前置摄像头 BACK 后置摄像头</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int EnableCamera(bool enable, RCRTCCamera camera = RCRTCCamera.FRONT);
    
        /// <summary>
        /// 切换摄像头
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SwitchCamera();
    
        /// <summary>
        /// 获取当前使用摄像头
        /// </summary>
        /// <returns>摄像头信息</returns>
        public abstract RCRTCCamera WhichCamera();
    
        /// <summary>
        /// 获取摄像头是否支持区域对焦
        /// </summary>
        /// <returns>true: 支持 false: 不支持</returns>
        public abstract bool IsCameraFocusSupported();
    
        /// <summary>
        /// 获取摄像头是否支持区域测光
        /// </summary>
        /// <returns>true: 支持 false: 不支持</returns>
        public abstract bool IsCameraExposurePositionSupported();
    
        /// <summary>
        /// 设置在指定点区域对焦
        /// </summary>
        /// <param name="x">对焦点，视图上的 x 轴坐标</param>
        /// <param name="y">对焦点，视图上的 y 轴坐标</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetCameraFocusPositionInPreview(double x, double y);
    
        /// <summary>
        /// 设置在指定点区域测光
        /// </summary>
        /// <param name="x">对焦点，视图上的 x 轴坐标</param>
        /// <param name="y">对焦点，视图上的 y 轴坐标</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetCameraExposurePositionInPreview(double x, double y);
    
        /// <summary>
        /// 设置摄像头采集方向
        /// </summary>
        /// <param name="orientation">默认以 Portrait 角度进行采集</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetCameraCaptureOrientation(RCRTCCameraCaptureOrientation orientation);
#endif
    
        /// <summary>
        /// 设置本地视频渲染窗口
        /// </summary>
        /// <param name="view">渲染窗口</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLocalView(RCRTCView view);
    
        /// <summary>
        /// 移除本地视频渲染窗口
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int RemoveLocalView();
    
        /// <summary>
        /// 设置远程视频渲染窗口
        /// </summary>
        /// <param name="userId">远端用户 userId</param>
        /// <param name="view">渲染窗口</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetRemoteView(String userId, RCRTCView view);
    
        /// <summary>
        /// 移除远程视频渲染窗口
        /// </summary>
        /// <param name="userId">远端用户 userId</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int RemoveRemoteView(String userId);
    
        /// <summary>
        /// 设置合流视频窗口
        /// </summary>
        /// <param name="view">渲染窗口</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLiveMixView(RCRTCView view);
    
        /// <summary>
        /// 移除合流视频窗口
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int RemoveLiveMixView();
    
        /// <summary>
        /// 设置直播内置 cdn 流预览窗口
        /// </summary>
        /// <param name="view">渲染窗口</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLiveMixInnerCdnStreamView(RCRTCView view);
    
        /// <summary>
        /// 设置本地自定义视频预览窗口
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLocalCustomStreamView(string tag, RCRTCView view);
    
        /// <summary>
        /// 设置远端自定义视频预览窗口
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetRemoteCustomStreamView(string userId, string tag, RCRTCView view);
    
        /// <summary>
        /// 停止本地音视频数据发送
        /// </summary>
        /// <param name="type">媒体类型</param>
        /// <param name="mute">true: 不发送 false: 发送</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int MuteLocalStream(RCRTCMediaType type, bool mute);
    
        /// <summary>
        /// 停止远端用户音视频数据的接收
        /// </summary>
        /// <param name="userId">远端用户 userId</param>
        /// <param name="type">媒体类型</param>
        /// <param name="mute">true: 不接收 false: 接收</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int MuteRemoteStream(String userId, RCRTCMediaType type, bool mute);
    
        /// <summary>
        /// 停止所有远端音频数据渲染
        /// </summary>
        /// <param name="mute">true: 不接收 false: 接收</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int MuteAllRemoteAudioStreams(bool mute);
    
        /// <summary>
        /// 设置 CDN 直播推流地址, 仅供直播主播用户使用
        /// </summary>
        /// <param name="url">推流地址</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int AddLiveCdn(String url);
    
        /// <summary>
        /// 移除 CDN 直播推流地址, 仅供直播主播用户使用
        /// </summary>
        /// <param name="url">推流地址</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int RemoveLiveCdn(String url);
    
        /// <summary>
        /// 设置直播合流布局类型, 仅供直播主播用户使用
        /// </summary>
        /// <param name="mode">布局类型</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLiveMixLayoutMode(RCRTCLiveMixLayoutMode mode);
    
        /// <summary>
        /// 设置直播合流布局填充类型, 仅供直播主播用户使用
        /// </summary>
        /// <param name="mode">填充类型</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLiveMixRenderMode(RCRTCLiveMixRenderMode mode);
    
        /// <summary>
        /// 设置直播混流布局配置, 仅供直播主播用户使用
        /// </summary>
        /// <param name="layouts">混流布局列表</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLiveMixCustomLayouts(IList<RCRTCCustomLayout> layouts);
    
        /// <summary>
        /// 设置直播自定义音频流列表, 仅供直播主播用户使用
        /// </summary>
        /// <param name="userIds">音频流列表</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLiveMixCustomAudios(IList<String> userIds);
    
        /// <summary>
        /// 设置直播合流音频码率, 仅供直播主播用户使用
        /// </summary>
        /// <param name="bitrate">音频码率 单位：kbps</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLiveMixAudioBitrate(int bitrate);
    
        /// <summary>
        /// 设置直播合流视频码率, 仅供直播主播用户使用
        /// </summary>
        /// <param name="bitrate">视频码率 单位：kbps</param>
        /// <param name="tiny">视频大小流, true:视频小流 false:视频大流</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLiveMixVideoBitrate(int bitrate, bool tiny);
    
        /// <summary>
        /// 设置直播合流视频分辨率, 仅供直播主播用户使用
        /// </summary>
        /// <param name="width">视频宽度 单位像素</param>
        /// <param name="height">视频高 单位像素</param>
        /// <param name="tiny">视频大小流, true:视频小流 false:视频大流</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLiveMixVideoResolution(int width, int height, bool tiny);
    
        /// <summary>
        /// 设置直播合流视频帧率, 仅供直播主播用户使用
        /// </summary>
        /// <param name="fps">帧率</param>
        /// <param name="tiny">视频大小流, true:视频小流 false:视频大流</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLiveMixVideoFps(RCRTCVideoFps fps, bool tiny);
    
        /// <summary>
        /// 引擎状态回调通知
        /// </summary>
        /// <param name="listener">监听回调对象</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetStatsListener(RCRTCStatsListener listener);
    
        /// <summary>
        /// 创建音效文件缓存, 仅供会议用户或直播主播用户使用
        /// </summary>
        /// <param name="path">本地文件地址</param>
        /// <param name="effectId">自定义全局唯一音效Id</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int CreateAudioEffect(String path, int effectId);
    
        /// <summary>
        /// 释放音效文件缓存, 仅供会议用户或直播主播用户使用
        /// </summary>
        /// <param name="effectId">自定义全局唯一音效Id</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int ReleaseAudioEffect(int effectId);
    
        /// <summary>
        /// 播放音效文件, 仅供会议用户或直播主播用户使用
        /// </summary>
        /// <param name="effectId">自定义全局唯一音效Id</param>
        /// <param name="volume">音效文件播放音量</param>
        /// <param name="loop">循环播放次数</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int PlayAudioEffect(int effectId, int volume, int loop);
    
        /// <summary>
        /// 暂停音效文件播放, 仅供会议用户或直播主播用户使用
        /// </summary>
        /// <param name="effectId">自定义全局唯一音效Id</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int PauseAudioEffect(int effectId);
    
        /// <summary>
        /// 暂停全部音效文件播放, 仅供会议用户或直播主播用户使用
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int PauseAllAudioEffects();
    
        /// <summary>
        /// 恢复音效文件播放, 仅供会议用户或直播主播用户使用
        /// </summary>
        /// <param name="effectId">自定义全局唯一音效Id</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int ResumeAudioEffect(int effectId);
    
        /// <summary>
        /// 恢复全部音效文件播放, 仅供会议用户或直播主播用户使用
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int ResumeAllAudioEffects();
    
        /// <summary>
        /// 停止全部音效文件播放, 仅供会议用户或直播主播用户使用
        /// </summary>
        /// <param name="effectId">自定义全局唯一音效Id</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int StopAudioEffect(int effectId);
    
        /// <summary>
        /// 停止音效文件播放, 仅供会议用户或直播主播用户使用
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int StopAllAudioEffects();
    
        /// <summary>
        /// 设置音效文件播放音量, 仅供会议用户或直播主播用户使用
        /// </summary>
        /// <param name="effectId">自定义全局唯一音效Id</param>
        /// <param name="volume">音量 0~100, 默认 100</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int AdjustAudioEffectVolume(int effectId, int volume);
    
        /// <summary>
        /// 获取音效文件播放音量，仅供会议用户或主播用户使用
        /// </summary>
        /// <param name="effectId">自定义全局唯一音效Id</param>
        /// <returns>音量0-100</returns>
        public abstract int GetAudioEffectVolume(int effectId);
    
        /// <summary>
        /// 设置全局音效文件播放音量，仅供会议用户或主播用户使用
        /// </summary>
        /// <param name="volume">音量0-100，默认100</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int AdjustAllAudioEffectsVolume(int volume);
    
        /// <summary>
        /// 开启混音，仅支持混合本地音频文件，仅供会议用户或主播用户使用
        /// </summary>
        /// <param name="path">本地音频文件地址</param>
        /// <param name="mode">混音模式</param>
        /// <param name="playback">是否本地播放</param>
        /// <param name="loop">循环混音或播放次数</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int StartAudioMixing(String path, RCRTCAudioMixingMode mode, bool playback, int loop);
    
        /// <summary>
        /// 停止混音，仅供会议用户或主播用户使用
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int StopAudioMixing();
    
        /// <summary>
        /// 暂停混音，仅供会议用户或主播用户使用
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int PauseAudioMixing();
    
        /// <summary>
        /// 恢复混音，仅供会议用户或主播用户使用
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int ResumeAudioMixing();
    
        /// <summary>
        /// 设置混音输入音量，包含本地播放和发送音量，仅供会议用户或主播用户使用
        /// </summary>
        /// <param name="volume">音量0-100，默认100</par
        public abstract int AdjustAudioMixingVolume(int volume);
    
        /// <summary>
        /// 设置混音本地播放音量，仅供会议用户或主播用户使用
        /// </summary>
        /// <param name="volume">音量0-100，默认100</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int AdjustAudioMixingPlaybackVolume(int volume);
    
        /// <summary>
        /// 设置混音发送音量，仅供会议用户或主播用户使用
        /// </summary>
        /// <param name="volume">音量0-100，默认100</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int AdjustAudioMixingPublishVolume(int volume);
    
        /// <summary>
        /// 获取混音本地播放音量，仅供会议用户或主播用户使用
        /// </summary>
        /// <returns>音量</returns>
        public abstract int GetAudioMixingPlaybackVolume();
    
        /// <summary>
        /// 获取混音发送音量，仅供会议用户或主播用户使用
        /// </summary>
        /// <returns>音量</returns>
        public abstract int GetAudioMixingPublishVolume();
    
        /// <summary>
        /// 设置混音文件合流进度，仅供会议用户或主播用户使用
        /// </summary>
        /// <param name="position">进度0-1</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetAudioMixingPosition(double position);
    
        /// <summary>
        /// 获取混音文件合流进度，仅供会议用户或主播用户使用
        /// </summary>
        /// <returns>进度0-1</returns>
        public abstract double GetAudioMixingPosition();
    
        /// <summary>
        /// 获取音频文件时长，单位：秒，仅供会议用户或主播用户使用
        /// </summary>
        /// <param name="engine">rtc 引擎句柄</param>
        /// <returns>时长</returns>
        public abstract int GetAudioMixingDuration();
    
        /// <summary>
        /// 设置本地音频流监听
        /// </summary>
        /// <param name="listener">回调监听</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLocalAudioCapturedListener(RCRTCOnWritableAudioFrameListener listener);
    
        /// <summary>
        /// 设置远程音频流监听
        /// </summary>
        /// <param name="userId">用户 UserId</param>
        /// <param name="listener">回调监听</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetRemoteAudioReceivedListener(String userId, RCRTCOnWritableAudioFrameListener listener);
    
        /// <summary>
        /// 获取房间会话Id
        /// </summary>
        /// <returns>会话Id</returns>
        public abstract String GetSessionId();
    
        /// <summary>
        /// 开始网络检测
        /// </summary>
        /// <param name="listener">检测回调</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int StartNetworkProbe(RCRTCNetworkProbeListener listener);
    
        /// <summary>
        /// 添加水印
        /// </summary>
        /// <returns>接口调用状态码 0: 成功, 非0: 失败</returns>
        public abstract int SetWatermark(String path, double x, double y, double zoom);
    
        /// <summary>
        /// 创建基于文件的自定义流
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tag"></param>
        /// <param name="replace"></param>
        /// <param name="playback"></param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int CreateCustomStreamFromFile(string path, string tag, bool replace, bool playback);
    
        /// <summary>
        /// 设置自定义流的视频配置
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="config"></param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetCustomStreamVideoConfig(string tag, RCRTCVideoConfig config);
    
        /// <summary>
        /// 停止合流数据渲染
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mute"></param>
        public abstract int MuteLiveMixStream(RCRTCMediaType type, bool mute);
    
        /// <summary>
        /// 设置直播合流布局背景颜色, 仅供直播主播用户使用
        /// </summary>
        /// <param name="color">
        /// 用于旁路直播的输出视频的背景色，格式为 RGB 定义下的 Hex 值，不要带 # 号，如 0xFFB6C1
        /// 表示浅粉色。默认0x000000，黑色。取值范围 [0x000000, 0xFFFFFF]
        /// </param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SetLiveMixBackgroundColor(int color);
    
        /// <summary>
        /// 停止本地数据渲染
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="mute"></param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int MuteLocalCustomStream(string tag, bool mute);
    
        /// <summary>
        /// 移除本地自定义视频预览窗口
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int RemoveLocalCustomStreamView(string tag);
    
        /// <summary>
        /// 发布自定义视频
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int PublishCustomStream(string tag);
    
        /// <summary>
        /// 取消发布自定义视频
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int UnpublishCustomStream(string tag);
    
        /// <summary>
        /// 停止远端数据渲染
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tag"></param>
        /// <param name="type"></param>
        /// <param name="mute"></param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int MuteRemoteCustomStream(string userId, string tag, RCRTCMediaType type, bool mute);
    
        /// <summary>
        /// 移除远端自定义视频预览窗口
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tag"></param>
        public abstract int RemoveRemoteCustomStreamView(string userId, string tag);
    
        /// <summary>
        /// 订阅自定义流
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="tag"></param>
        /// <param name="tiny"></param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SubscribeCustomStream(string userId, string tag, RCRTCMediaType type, bool tiny);
    
        /// <summary>
        /// 取消订阅自定义流
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="tag"></param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int UnsubscribeCustomStream(string userId, string tag, RCRTCMediaType type);
    
        /// <summary>
        /// 请求加入子房间（跨房间连麦）
        /// </summary>
        /// <param name="roomId">目标房间id</param>
        /// <param name="userId">目标主播id</param>
        /// <param name="autoLayout">
        /// 是否自动布局。<p>。开启自动布局。如果被邀请方在加入邀请方房间之前发布了资源，当被邀请方加入邀请者房间成功后，服务器会把被邀请方流资源合并到邀请方视图（默认仅悬浮布局合流）上。如果被邀请方在加入邀请方房间之前没有发布过资源，将会在被邀请方发布资源成功后，服务器会把被邀请方流资源合并到邀请方视图（默认仅悬浮布局合流）上。双方可以主动设置合流布局。一旦主动设置过合流布局，后续音视频直播过程中设置的自动合流参数将失效。</>
        /// </param>
        /// <param name="extra">附加信息，默认为空</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int RequestJoinSubRoom(string roomId, string userId, bool autoLayout, string extra);
    
        /// <summary>
        /// 取消正在进行中的加入子房间（跨房间连麦）请求
        /// </summary>
        /// <param name="roomId">正在请求的目标房间id</param>
        /// <param name="userId">正在请求的目标主播id</param>
        /// <param name="extra">附加信息，默认为空</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int CancelJoinSubRoomRequest(string roomId, string userId, string extra);
    
        /// <summary>
        /// 响应加入子房间（跨房间连麦）请求
        /// </summary>
        /// <param name="roomId">发起请求的目标房间id</param>
        /// <param name="userId">发起请求的主播id</param>
        /// <param name="agree">是否同意（跨房间连麦）</param>
        /// <param name="autoLayout">
        /// 是否自动布局。<p>。开启自动布局。如果被邀请方在加入邀请方房间之前发布了资源，当被邀请方加入邀请者房间成功后，服务器会把被邀请方流资源合并到邀请方视图（默认仅悬浮布局合流）上。如果被邀请方在加入邀请方房间之前没有发布过资源，将会在被邀请方发布资源成功后，服务器会把被邀请方流资源合并到邀请方视图（默认仅悬浮布局合流）上。双方可以主动设置合流布局。一旦主动设置过合流布局，后续音视频直播过程中设置的自动合流参数将失效。</>
        /// </param>
        /// <param name="extra">附加信息，默认为空</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int ResponseJoinSubRoomRequest(string roomId, string userId, bool agree, bool autoLayout,
                                                       string extra);
    
        /// <summary>
        /// 加入子房间。前提必须已经 通过 {@link RCRTCIWEngine#joinRoom(String, RCRTCIWRoomSetup)}
        /// 加入了主房间。未建立连麦时，需先调用 {@link RCRTCIWEngine#requestJoinSubRoom(String, String, boolean, String)}
        /// 发起加入请求
        /// </summary>
        /// <param name="roomId">目标房间id</param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int JoinSubRoom(string roomId);
    
        /// <summary>
        /// 离开子房间
        /// </summary>
        /// <param name="roomId">子房间id</param>
        /// <param name="disband">
        /// 是否解散，解散后再次加入需先调用 {@link RCRTCIWEngine#requestJoinSubRoom(String, String, boolean, String)}
        /// 发起加入请求
        /// </param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int LeaveSubRoom(string roomId, bool disband);
    
        /// <summary>
        /// 切换直播角色
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SwitchLiveRole(RCRTCRole role);
    
        /// <summary>
        /// 开启直播内置 cdn 功能
        /// </summary>
        /// <param name="enable"></param>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int EnableLiveMixInnerCdnStream(bool enable);
    
        /// <summary>
        /// 订阅直播内置 cdn 流
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int SubscribeLiveMixInnerCdnStream();
    
        /// <summary>
        /// 取消订阅直播内置 cdn 流
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int UnsubscribeLiveMixInnerCdnStream();
    
        /// <summary>
        /// 移除直播内置 cdn 流预览窗口
        /// </summary>
        /// <returns>0: 成功, 非0: 失败</returns>
        public abstract int RemoveLiveMixInnerCdnStreamView();
    
        /// <summary>
        /// 观众端 设置订阅 cdn 流的分辨率
        /// </summary>
        /// <param name="width">分辨率宽</param>
        /// <param name="height">高</param>
        /// <returns>接口调用状态码 0: 成功, 非0: 失败</returns>
        public abstract int SetLocalLiveMixInnerCdnVideoResolution(int width, int height);
    
        /// <summary>
        /// 观众端设置订阅 cdn 流的帧率
        /// </summary>
        /// <param name="fps">帧率</param>
        /// <returns>接口调用状态码 0: 成功, 非0: 失败</returns>
        public abstract int SetLocalLiveMixInnerCdnVideoFps(RCRTCVideoFps fps);
    
        /// <summary>
        /// 观众端禁用或启用融云内置 CDN 流
        /// </summary>
        /// <param name="mute">true: 停止资源渲染，false: 恢复资源渲染</param>
        /// <returns>接口调用状态码 0: 成功, 非0: 失败</returns>
        public abstract int MuteLiveMixInnerCdnStream(bool mute);
    
        /// <summary>
        /// 移除水印
        /// </summary>
        /// <returns>接口调用状态码 0: 成功, 非0: 失败</returns>
        public abstract int RemoveWatermark();
    
        /// <summary>
        /// 停止网络探测
        /// </summary>
        /// <returns>接口调用状态码 0: 成功, 非0: 失败</returns>
        public abstract int StopNetworkProbe();
    
        /// <summary>
        /// 开始麦克风&扬声器检测
        /// </summary>
        /// <param name="timeInterval">麦克风录制时间</param>
        /// <returns>接口调用状态码 0: 成功, 非0: 失败</returns>
        public abstract int StartEchoTest(int timeInterval);
    
        /// <summary>
        /// 停止麦克风&扬声器检测，结束检测后必须手动调用停止方法
        /// </summary>
        /// <returns>接口调用状态码 0: 成功, 非0: 失败</returns>
        public abstract int StopEchoTest();
    
        /// <summary>
        /// 开启 SEI 功能，观众身份调用无效
        /// </summary>
        /// <param name="enable">是否开启</param>
        /// <returns>接口调用状态码 0: 成功, 非0: 失败</returns>
        public abstract int EnableSei(bool enable);
    
        /// <summary>
        /// 发送 SEI 信息，需开启 SEI 功能之后调用
        /// </summary>
        /// <param name="sei">SEI 信息</param>
        /// <returns>接口调用状态码 0: 成功, 非0: 失败</returns>
        public abstract int SendSei(string sei);
    
        /// <summary>
        /// 预链接到媒体服务器
        /// </summary>
        /// <returns>接口调用状态码 0: 成功, 非0: 失败</returns>
        public abstract int PreconnectToMediaServer();
    
        /// <summary>
        /// 引擎错误回调
        /// </summary>
        public OnErrorDelegate OnError { set; internal get; }
    
        /// <summary>
        /// 用户被踢下线回调
        /// </summary>
        public OnKickedDelegate OnKicked { set; internal get; }
    
        /// <summary>
        /// 加入房间回调
        /// </summary>
        public OnRoomJoinedDelegate OnRoomJoined { set; internal get; }
    
        /// <summary>
        /// 离开房间回调
        /// </summary>
        public OnRoomLeftDelegate OnRoomLeft { set; internal get; }
    
        /// <summary>
        /// 发布成功回调
        /// </summary>
        public OnPublishedDelegate OnPublished { set; internal get; }
    
        /// <summary>
        /// 取消发布成功回调
        /// </summary>
        public OnUnpublishedDelegate OnUnpublished { set; internal get; }
    
        /// <summary>
        /// 订阅成功回调
        /// </summary>
        public OnSubscribedDelegate OnSubscribed { set; internal get; }
    
        /// <summary>
        /// 取消订阅成功回调
        /// </summary>
        public OnUnsubscribedDelegate OnUnsubscribed { set; internal get; }
    
        /// <summary>
        /// 合流订阅成功回调
        /// </summary>
        public OnLiveMixSubscribedDelegate OnLiveMixSubscribed { set; internal get; }
    
        /// <summary>
        /// 合流取消订阅成功回调
        /// </summary>
        public OnLiveMixUnsubscribedDelegate OnLiveMixUnsubscribed { set; internal get; }
    
        /// <summary>
        /// 摄像头开启成功回调
        /// </summary>
        public OnCameraEnabledDelegate OnCameraEnabled { set; internal get; }
    
        /// <summary>
        /// 摄像头切换回调
        /// </summary>
        public OnCameraSwitchedDelegate OnCameraSwitched { set; internal get; }
    
        /// <summary>
        /// 设置 CDN 直播推流地址成功回调
        /// </summary>
        public OnLiveCdnAddedDelegate OnLiveCdnAdded { set; internal get; }
    
        /// <summary>
        /// 移除 CDN 直播推流地址成功回调
        /// </summary>
        public OnLiveCdnRemovedDelegate OnLiveCdnRemoved { set; internal get; }
    
        /// <summary>
        /// 合流布局模式设置成功回调
        /// </summary>
        public OnLiveMixLayoutModeSetDelegate OnLiveMixLayoutModeSet { set; internal get; }
    
        /// <summary>
        /// 合流渲染模式设置成功回调
        /// </summary>
        public OnLiveMixRenderModeSetDelegate OnLiveMixRenderModeSet { set; internal get; }
    
        /// <summary>
        /// 设置直播合流自定义音频流成功回调
        /// </summary>
        public OnLiveMixCustomAudiosSetDelegate OnLiveMixCustomAudiosSet { set; internal get; }
    
        /// <summary>
        /// 设置直播合流自定义布局成功回调
        /// </summary>
        public OnLiveMixCustomLayoutsSetDelegate OnLiveMixCustomLayoutsSet { set; internal get; }
    
        /// <summary>
        /// 设置直播合流自定义音频流采样率成功回调
        /// </summary>
        public OnLiveMixAudioBitrateSetDelegate OnLiveMixAudioBitrateSet { set; internal get; }
    
        /// <summary>
        /// 设置直播合流自定义视频流采样率成功回调
        /// </summary>
        public OnLiveMixVideoBitrateSetDelegate OnLiveMixVideoBitrateSet { set; internal get; }
    
        /// <summary>
        /// 设置直播合流分辨率成功回调
        /// </summary>
        public OnLiveMixVideoResolutionSetDelegate OnLiveMixVideoResolutionSet { set; internal get; }
    
        /// <summary>
        /// 设置直播合流帧率成功回调
        /// </summary>
        public OnLiveMixVideoFpsSetDelegate OnLiveMixVideoFpsSet { set; internal get; }
    
        /// <summary>
        /// 音效创建成功回调
        /// </summary>
        public OnAudioEffectCreatedDelegate OnAudioEffectCreated { set; internal get; }
    
        /// <summary>
        /// 音效完成回调
        /// </summary>
        public OnAudioEffectFinishedDelegate OnAudioEffectFinished { set; internal get; }
    
        /// <summary>
        /// 开始混音回调
        /// </summary>
        public OnAudioMixingStartedDelegate OnAudioMixingStarted { set; internal get; }
    
        /// <summary>
        /// 混音暂停回调
        /// </summary>
        public OnAudioMixingPausedDelegate OnAudioMixingPaused { set; internal get; }
    
        /// <summary>
        /// 混音停止回调
        /// </summary>
        public OnAudioMixingStoppedDelegate OnAudioMixingStopped { set; internal get; }
    
        /// <summary>
        /// 混音完成回调
        /// </summary>
        public OnAudioMixingFinishedDelegate OnAudioMixingFinished { set; internal get; }
    
        /// <summary>
        /// 用户加入房间回调
        /// </summary>
        public OnUserJoinedDelegate OnUserJoined { set; internal get; }
    
        /// <summary>
        /// 用户离线回调
        /// </summary>
        public OnUserOfflineDelegate OnUserOffline { set; internal get; }
    
        /// <summary>
        /// 用户离开房间回调
        /// </summary>
        public OnUserLeftDelegate OnUserLeft { set; internal get; }
    
        /// <summary>
        /// 远程用户发布回调
        /// </summary>
        public OnRemotePublishedDelegate OnRemotePublished { set; internal get; }
    
        /// <summary>
        /// 远程用户取消发布回调
        /// </summary>
        public OnRemoteUnpublishedDelegate OnRemoteUnpublished { set; internal get; }
    
        /// <summary>
        /// 合流发布回调
        /// </summary>
        public OnRemoteLiveMixPublishedDelegate OnRemoteLiveMixPublished { set; internal get; }
    
        /// <summary>
        /// 合流取消发布回调
        /// </summary>
        public OnRemoteLiveMixUnpublishedDelegate OnRemoteLiveMixUnpublished { set; internal get; }
    
        /// <summary>
        /// 状态改变回调
        /// </summary>
        public OnRemoteStateChangedDelegate OnRemoteStateChanged { set; internal get; }
    
        /// <summary>
        /// 远程首帧渲染回调
        /// </summary>
        public OnRemoteFirstFrameDelegate OnRemoteFirstFrame { set; internal get; }
    
        /// <summary>
        /// 远程合流首帧渲染回调
        /// </summary>
        public OnRemoteLiveMixFirstFrameDelegate OnRemoteLiveMixFirstFrame { set; internal get; }
    
#if UNITY_STANDALONE_WIN
        /// <summary>
        /// 摄像头设备改变回调
        /// </summary>
        public OnCameraListChangeDelegate OnCameraListChanged { set; internal get; }
    
        /// <summary>
        /// 麦克风设备改变回调
        /// </summary>
        public OnMicrophoneListChangeDelegate OnMicrophoneListChanged { set; internal get; }
    
        /// <summary>
        /// 扬声器设备改变回调
        /// </summary>
        public OnSpeakerListChangeDelegate OnSpeakerListChanged { set; internal get; }
#endif
    
        /// <summary>
        /// 设置合流布局背景颜色操作回调
        /// </summary>
        public OnLiveMixBackgroundColorSetDelegate OnLiveMixBackgroundColorSet { set; internal get; }
    
        /// <summary>
        /// 混音进度
        /// </summary>
        [Obsolete("请使用方法对应的监听")]
        public OnAudioMixingProgressReportedDelegate OnAudioMixingProgressReported { set; internal get; }
    
        /// <summary>
        /// 本地用户发布自定义资源完毕
        /// </summary>
        public OnCustomStreamPublishedDelegate OnCustomStreamPublished { set; internal get; }
    
        /// <summary>
        /// 本地用户发布自定义资源完毕
        /// </summary>
        public OnCustomStreamPublishFinishedDelegate OnCustomStreamPublishFinished { set; internal get; }
    
        /// <summary>
        /// 本地用户取消发布自定义资源回调
        /// </summary>
        public OnCustomStreamUnpublishedDelegate OnCustomStreamUnpublished { set; internal get; }
    
        /// <summary>
        /// 远端用户发布自定义资源操作回调
        /// </summary>
        public OnRemoteCustomStreamPublishedDelegate OnRemoteCustomStreamPublished { set; internal get; }
    
        /// <summary>
        /// 远端用户取消发布自定义资源操作回调
        /// </summary>
        public OnRemoteCustomStreamUnpublishedDelegate OnRemoteCustomStreamUnpublished { set; internal get; }
    
        /// <summary>
        /// 远端用户自定义资源状态操作回调
        /// </summary>
        public OnRemoteCustomStreamStateChangedDelegate OnRemoteCustomStreamStateChanged { set; internal get; }
    
        /// <summary>
        /// 收到远端用户自定义资源第一帧数据
        /// </summary>
        public OnRemoteCustomStreamFirstFrameDelegate OnRemoteCustomStreamFirstFrame { set; internal get; }
    
        /// <summary>
        /// 订阅远端用户发布的自定义资源操作回调
        /// </summary>
        public OnCustomStreamSubscribedDelegate OnCustomStreamSubscribed { set; internal get; }
    
        /// <summary>
        /// 取消订阅远端用户发布的自定义资源操作回调
        /// </summary>
        public OnCustomStreamUnsubscribedDelegate OnCustomStreamUnsubscribed { set; internal get; }
    
        /// <summary>
        /// 请求加入子房间回调
        /// </summary>
        public OnJoinSubRoomRequestedDelegate OnJoinSubRoomRequested { set; internal get; }
    
        /// <summary>
        /// 取消请求加入子房间回调
        /// </summary>
        public OnJoinSubRoomRequestCanceledDelegate OnJoinSubRoomRequestCanceled { set; internal get; }
    
        /// <summary>
        /// 响应请求加入子房间回调
        /// </summary>
        public OnJoinSubRoomRequestRespondedDelegate OnJoinSubRoomRequestResponded { set; internal get; }
    
        /// <summary>
        /// 收到加入请求回调
        /// </summary>
        public OnJoinSubRoomRequestReceivedDelegate OnJoinSubRoomRequestReceived { set; internal get; }
    
        /// <summary>
        /// 收到取消加入请求回调
        /// </summary>
        public OnCancelJoinSubRoomRequestReceivedDelegate OnCancelJoinSubRoomRequestReceived { set; internal get; }
    
        /// <summary>
        /// 收到加入请求响应回调
        /// </summary>
        public OnJoinSubRoomRequestResponseReceivedDelegate OnJoinSubRoomRequestResponseReceived { set; internal get; }
    
        /// <summary>
        /// 加入子房间回调
        /// </summary>
        public OnSubRoomJoinedDelegate OnSubRoomJoined { set; internal get; }
    
        /// <summary>
        /// 离开子房间回调
        /// </summary>
        public OnSubRoomLeftDelegate OnSubRoomLeft { set; internal get; }
    
        /// <summary>
        /// 连麦中的子房间回调
        /// </summary>
        public OnSubRoomBandedDelegate OnSubRoomBanded { set; internal get; }
    
        /// <summary>
        /// 子房间结束连麦回调
        /// </summary>
        public OnSubRoomDisbandDelegate OnSubRoomDisband { set; internal get; }
    
        /// <summary>
        /// 切换直播角色回调
        /// </summary>
        public OnLiveRoleSwitchedDelegate OnLiveRoleSwitched { set; internal get; }
    
        /// <summary>
        /// 远端用户身份切换回调
        /// </summary>
        public OnRemoteLiveRoleSwitchedDelegate OnRemoteLiveRoleSwitched { set; internal get; }
    
        /// <summary>
        /// 开启直播内置 cdn 结果回调
        /// </summary>
        public OnLiveMixInnerCdnStreamEnabledDelegate OnLiveMixInnerCdnStreamEnabled { set; internal get; }
    
        /// <summary>
        /// 直播内置 cdn 资源发布回调
        /// </summary>
        public OnRemoteLiveMixInnerCdnStreamPublishedDelegate OnRemoteLiveMixInnerCdnStreamPublished { set; internal get; }
    
        /// <summary>
        /// 直播内置 cdn 资源取消发布回调
        /// </summary>
        public OnRemoteLiveMixInnerCdnStreamUnpublishedDelegate OnRemoteLiveMixInnerCdnStreamUnpublished { set;
                                                                                                           internal get; }
    
        /// <summary>
        /// 订阅直播内置 cdn 资源回调
        /// </summary>
        public OnLiveMixInnerCdnStreamSubscribedDelegate OnLiveMixInnerCdnStreamSubscribed { set; internal get; }
    
        /// <summary>
        /// 取消订阅直播内置 cdn 资源回调
        /// </summary>
        public OnLiveMixInnerCdnStreamUnsubscribedDelegate OnLiveMixInnerCdnStreamUnsubscribed { set; internal get; }
    
        /// <summary>
        /// 观众端设置订阅 cdn 流的分辨率的回调
        /// </summary>
        public OnLocalLiveMixInnerCdnVideoResolutionSetDelegate OnLocalLiveMixInnerCdnVideoResolutionSet { set;
                                                                                                           internal get; }
    
        /// <summary>
        /// 观众端 设置订阅 cdn 流的帧率的回调
        /// </summary>
        public OnLocalLiveMixInnerCdnVideoFpsSetDelegate OnLocalLiveMixInnerCdnVideoFpsSet { set; internal get; }
    
        /// <summary>
        /// 设置水印的回调
        /// </summary>
        public OnWatermarkSetDelegate OnWatermarkSet { set; internal get; }
    
        /// <summary>
        /// 移除水印的回调
        /// </summary>
        public OnWatermarkRemovedDelegate OnWatermarkRemoved { set; internal get; }
    
        /// <summary>
        /// 开启网络探测结果回调
        /// </summary>
        public OnNetworkProbeStartedDelegate OnNetworkProbeStarted { set; internal get; }
    
        /// <summary>
        /// 关闭网络探测结果回调
        /// </summary>
        public OnNetworkProbeStoppedDelegate OnNetworkProbeStopped { set; internal get; }
    
        /// <summary>
        /// 开启 SEI 功能结果回调
        /// </summary>
        public OnSeiEnabledDelegate OnSeiEnabled { set; internal get; }
    
        /// <summary>
        /// 收到 SEI 信息回调
        /// </summary>
        public OnSeiReceivedDelegate OnSeiReceived { set; internal get; }
    
        /// <summary>
        /// 观众收到合流 SEI 信息回调
        /// </summary>
        public OnLiveMixSeiReceivedDelegate OnLiveMixSeiReceived { set; internal get; }
    }
    
}