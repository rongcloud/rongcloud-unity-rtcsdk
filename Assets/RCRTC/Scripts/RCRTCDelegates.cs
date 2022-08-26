using System;

namespace cn_rongcloud_rtc_unity
{
    /// <summary>
    /// 引擎发生错误
    /// </summary>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnErrorDelegate(int code, String errMsg);
    /// <summary>
    /// 本地用户被踢出房间回调
    /// </summary>
    /// <param name="roomId">房间id</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnKickedDelegate(String roomId, String errMsg);
    /// <summary>
    /// 本地用户加入房间回调
    /// </summary>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnRoomJoinedDelegate(int code, String errMsg);
    /// <summary>
    /// 本地用户离开房间回调
    /// </summary>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnRoomLeftDelegate(int code, String errMsg);
    /// <summary>
    /// 本地用户发布资源回调
    /// </summary>
    /// <param name="type">媒体类型</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnPublishedDelegate(RCRTCMediaType type, int code, String errMsg);
    /// <summary>
    /// 本地用户取消发布资源回调
    /// </summary>
    /// <param name="type">媒体类型</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnUnpublishedDelegate(RCRTCMediaType type, int code, String errMsg);
    /// <summary>
    /// 订阅远端用户发布的资源操作回调
    /// </summary>
    /// <param name="userId">远端用户 UserId</param>
    /// <param name="type">媒体类型</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnSubscribedDelegate(String userId, RCRTCMediaType type, int code, String errMsg);
    /// <summary>
    /// 取消订阅远端用户发布的资源操作回调
    /// </summary>
    /// <param name="userId">远端用户 UserId</param>
    /// <param name="type">媒体类型</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnUnsubscribedDelegate(String userId, RCRTCMediaType type, int code, String errMsg);
    /// <summary>
    /// 本地观众用户订阅合流资源操作回调
    /// </summary>
    /// <param name="type">媒体类型</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnLiveMixSubscribedDelegate(RCRTCMediaType type, int code, String errMsg);
    /// <summary>
    /// 本地观众用户取消订阅合流资源操作回调
    /// </summary>
    /// <param name="type">媒体类型</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnLiveMixUnsubscribedDelegate(RCRTCMediaType type, int code, String errMsg);
    /// <summary>
    /// 摄像头开启/关闭回调
    /// </summary>
    /// <param name="enable">true 开启 false 关闭</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnCameraEnabledDelegate(bool enable, int code, String errMsg);
#if UNITY_STANDALONE_WIN
    /// <summary>
    /// 摄像头切换回调
    /// </summary>
    /// <param name="camera">摄像头信息</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnCameraSwitchedDelegate(RCRTCDevice camera, int code, String errMsg);
#else
    /// <summary>
    /// 摄像头切换回调
    /// </summary>
    /// <param name="camera">摄像头</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnCameraSwitchedDelegate(RCRTCCamera camera, int code, String errMsg);
#endif
    /// <summary>
    /// 添加旁路推流URL操作回调
    /// </summary>
    /// <param name="url">CDN URL</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnLiveCdnAddedDelegate(String url, int code, String errMsg);
    /// <summary>
    /// 移除旁路推流URL操作回调参数
    /// </summary>
    /// <param name="url">CDN UR</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnLiveCdnRemovedDelegate(String url, int code, String errMsg);
    /// <summary>
    /// 设置合流布局类型操作回调
    /// </summary>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnLiveMixLayoutModeSetDelegate(int code, String errMsg);
    /// <summary>
    /// 设置合流布局填充类型操作回调
    /// </summary>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnLiveMixRenderModeSetDelegate(int code, String errMsg);
    /// <summary>
    /// 设置合流自定义布局操作回调
    /// </summary>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnLiveMixCustomLayoutsSetDelegate(int code, String errMsg);
    /// <summary>
    /// 设置需要合流音频操作回调
    /// </summary>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnLiveMixCustomAudiosSetDelegate(int code, String errMsg);
    /// <summary>
    /// 设置合流音频码率操作回调
    /// </summary>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnLiveMixAudioBitrateSetDelegate(int code, String errMsg);
    /// <summary>
    /// 设置默认视频合流码率操作回调
    /// </summary>
    /// <param name="tiny">是否小流</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnLiveMixVideoBitrateSetDelegate(bool tiny, int code, String errMsg);
    /// <summary>
    /// 设置默认视频分辨率操作回调参数
    /// </summary>
    /// <param name="tiny">是否小流</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnLiveMixVideoResolutionSetDelegate(bool tiny, int code, String errMsg);
    /// <summary>
    /// 设置默认视频帧率操作回调参数
    /// </summary>
    /// <param name="tiny">是否小流</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnLiveMixVideoFpsSetDelegate(bool tiny, int code, String errMsg);
    /// <summary>
    /// 创建音效操作回调
    /// </summary>
    /// <param name="effectId">音效文件ID</param>
    /// <param name="code">返回码</param>
    /// <param name="errMsg">返回消息</param>
    public delegate void OnAudioEffectCreatedDelegate(int effectId, int code, String errMsg);
    /// <summary>
    /// 播放音效结束回调
    /// </summary>
    /// <param name="effectId">音效文件ID</param>
    public delegate void OnAudioEffectFinishedDelegate(int effectId);
    /// <summary>
    /// 混音开始回调
    /// </summary>
    public delegate void OnAudioMixingStartedDelegate();
    /// <summary>
    /// 混音暂停回调
    /// </summary>
    public delegate void OnAudioMixingPausedDelegate();
    /// <summary>
    /// 混音停止回调
    /// </summary>
    public delegate void OnAudioMixingStoppedDelegate();
    /// <summary>
    /// 混音完成回调
    /// </summary>
    public delegate void OnAudioMixingFinishedDelegate();
    /// <summary>
    /// 远端用户加入房间操作回调
    /// </summary>
    /// <param name="userId">远端用户 userId</param>
    public delegate void OnUserJoinedDelegate(String userId);
    /// <summary>
    /// 远端用户因离线离开房间操作回调
    /// </summary>
    /// <param name="userId">远端用户 userId</param>
    public delegate void OnUserOfflineDelegate(String userId);
    /// <summary>
    /// 远端用户离开房间操作回调
    /// </summary>
    /// <param name="userId">远端用户 userId</param>
    public delegate void OnUserLeftDelegate(String userId);
    /// <summary>
    /// 远端用户发布资源操作回调参数
    /// </summary>
    /// <param name="userId">远端用户 userId</param>
    /// <param name="type">媒体类型</param>
    public delegate void OnRemotePublishedDelegate(String userId, RCRTCMediaType type);
    /// <summary>
    /// 远端用户取消发布资源操作回调参数
    /// </summary>
    /// <param name="userId">远端用户 userId</param>
    /// <param name="type">媒体类型</param>
    public delegate void OnRemoteUnpublishedDelegate(String userId, RCRTCMediaType type);
    /// <summary>
    /// 直播观众用户收到远端用户发布直播合流资源操作回调
    /// </summary>
    /// <param name="type">媒体类型</param>
    public delegate void OnRemoteLiveMixPublishedDelegate(RCRTCMediaType type);
    /// <summary>
    /// 直播观众用户收到远端用户取消发布直播合流资源操作回调
    /// </summary>
    /// <param name="type">媒体类型</param>
    public delegate void OnRemoteLiveMixUnpublishedDelegate(RCRTCMediaType type);
    /// <summary>
    /// 远端用户开关麦克风或摄像头操作回调
    /// </summary>
    /// <param name="userId">远端用户 userId</param>
    /// <param name="type">媒体类型</param>
    /// <param name="disabled">是否关闭, YES: 关闭, NO: 打开</param>
    public delegate void OnRemoteStateChangedDelegate(String userId, RCRTCMediaType type, bool disabled);
    /// <summary>
    /// 本地会议用户或直播主播用户收到远端用户第一帧回调
    /// </summary>
    /// <param name="userId">远端用户 userId</param>
    /// <param name="type">媒体类型</param>
    public delegate void OnRemoteFirstFrameDelegate(String userId, RCRTCMediaType type);
    /// <summary>
    /// 观众用户收到远端用户第一个音频或视频关键帧回调
    /// </summary>
    /// <param name="type">媒体类型</param>
    public delegate void OnRemoteLiveMixFirstFrameDelegate(RCRTCMediaType type);

#if UNITY_STANDALONE_WIN
    /// <summary>
    /// 摄像头改变回调
    /// </summary>
    public delegate void OnCameraListChangeDelegate();
    /// <summary>
    /// 扬声器改变回调
    /// </summary>
    public delegate void OnSpeakerListChangeDelegate();
    /// <summary>
    /// 麦克风改变回调
    /// </summary>
    public delegate void OnMicrophoneListChangeDelegate();
#endif
}