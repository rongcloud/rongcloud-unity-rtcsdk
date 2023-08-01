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
    public delegate void OnUserJoinedDelegate(String roomId, String userId);
    /// <summary>
    /// 远端用户因离线离开房间操作回调
    /// </summary>
    /// <param name="userId">远端用户 userId</param>
    public delegate void OnUserOfflineDelegate(String roomId, String userId);
    /// <summary>
    /// 远端用户离开房间操作回调
    /// </summary>
    /// <param name="userId">远端用户 userId</param>
    public delegate void OnUserLeftDelegate(String roomId, String userId);
    /// <summary>
    /// 远端用户发布资源操作回调参数
    /// </summary>
    /// <param name="userId">远端用户 userId</param>
    /// <param name="type">媒体类型</param>
    public delegate void OnRemotePublishedDelegate(String roomId, String userId, RCRTCMediaType type);
    /// <summary>
    /// 远端用户取消发布资源操作回调参数
    /// </summary>
    /// <param name="userId">远端用户 userId</param>
    /// <param name="type">媒体类型</param>
    public delegate void OnRemoteUnpublishedDelegate(String roomId, String userId, RCRTCMediaType type);
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
    public delegate void OnRemoteStateChangedDelegate(String roomId, String userId, RCRTCMediaType type, bool disabled);
    /// <summary>
    /// 本地会议用户或直播主播用户收到远端用户第一帧回调
    /// </summary>
    /// <param name="userId">远端用户 userId</param>
    /// <param name="type">媒体类型</param>
    public delegate void OnRemoteFirstFrameDelegate(String roomId, String userId, RCRTCMediaType type);
    /// <summary>
    /// 观众用户收到远端用户第一个音频或视频关键帧回调
    /// </summary>
    /// <param name="type">媒体类型</param>
    public delegate void OnRemoteLiveMixFirstFrameDelegate(RCRTCMediaType type);
    
    /// <summary>
    /// 设置合流布局背景颜色操作回调
    /// </summary>
    /// <param name="code">0: 调用成功, 非0: 失败</param>
    /// <param name="errMsg">失败原因</param>
    public delegate void OnLiveMixBackgroundColorSetDelegate(int code, string errMsg);
    
    /// <summary>
    /// 混音进度
    /// </summary>
    /// <param name="progress">进度</param>
    public delegate void OnAudioMixingProgressReportedDelegate(float progress);
    
    /// <summary>
    /// 本地用户发布自定义资源完毕
    /// </summary>
    /// <param name="tag">自定义流标签</param>
    public delegate void OnCustomStreamPublishedDelegate(string tag, int code, string errMsg);
    
    /// <summary>
    /// 本地用户发布自定义资源完毕
    /// </summary>
    /// <param name="tag">自定义流标签</param>
    public delegate void OnCustomStreamPublishFinishedDelegate(string tag);
    
    /// <summary>
    /// 本地用户取消发布自定义资源回调
    /// </summary>
    /// <param name="tag">自定义流标签</param>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误信息</param>
    public delegate void OnCustomStreamUnpublishedDelegate(string tag, int code, string errMsg);
    
    /// <summary>
    /// 远端用户发布自定义资源操作回调
    /// </summary>
    /// <param name="roomId">房间id</param>
    /// <param name="userId">用户id</param>
    /// <param name="tag">自定义流标签</param>
    public delegate void OnRemoteCustomStreamPublishedDelegate(string roomId, string userId, string tag,
                                                               RCRTCMediaType type);
    
    /// <summary>
    /// 远端用户取消发布自定义资源操作回调
    /// </summary>
    /// <param name="roomId">房间id</param>
    /// <param name="userId">用户id</param>
    /// <param name="tag">自定义流标签</param>
    public delegate void OnRemoteCustomStreamUnpublishedDelegate(string roomId, string userId, string tag,
                                                                 RCRTCMediaType type);
    
    /// <summary>
    /// 远端用户自定义资源状态操作回调
    /// </summary>
    /// <param name="roomId">房间id</param>
    /// <param name="userId">用户id</param>
    /// <param name="tag">自定义流标签</param>
    /// <param name="disabled">是否禁用</param>
    public delegate void OnRemoteCustomStreamStateChangedDelegate(string roomId, string userId, string tag,
                                                                  RCRTCMediaType type, bool disabled);
    
    /// <summary>
    /// 收到远端用户自定义资源第一帧数据
    /// </summary>
    /// <param name="roomId">房间id</param>
    /// <param name="userId">用户id</param>
    /// <param name="tag">自定义流标签</param>
    public delegate void OnRemoteCustomStreamFirstFrameDelegate(string roomId, string userId, string tag,
                                                                RCRTCMediaType type);
    
    /// <summary>
    /// 订阅远端用户发布的自定义资源操作回调
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="tag">自定义流标签</param>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误信息</param>
    public delegate void OnCustomStreamSubscribedDelegate(string userId, string tag, RCRTCMediaType type, int code,
                                                          string errMsg);
    
    /// <summary>
    /// 取消订阅远端用户发布的自定义资源操作回调
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="tag">自定义流标签</param>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误信息</param>
    public delegate void OnCustomStreamUnsubscribedDelegate(string userId, string tag, RCRTCMediaType type, int code,
                                                            string errMsg);
    
    /// <summary>
    /// 请求加入子房间回调
    /// </summary>
    /// <param name="roomId">目标房间id</param>
    /// <param name="userId">目标主播id</param>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误信息</param>
    public delegate void OnJoinSubRoomRequestedDelegate(string roomId, string userId, int code, string errMsg);
    
    /// <summary>
    /// 取消请求加入子房间回调
    /// </summary>
    /// <param name="roomId">目标房间id</param>
    /// <param name="userId">目标主播id</param>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误信息</param>
    public delegate void OnJoinSubRoomRequestCanceledDelegate(string roomId, string userId, int code, string errMsg);
    
    /// <summary>
    /// 响应请求加入子房间回调
    /// </summary>
    /// <param name="roomId">目标房间id</param>
    /// <param name="userId">目标主播id</param>
    /// <param name="agree">是否同意</param>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误信息</param>
    public delegate void OnJoinSubRoomRequestRespondedDelegate(string roomId, string userId, bool agree, int code,
                                                               string errMsg);
    
    /// <summary>
    /// 收到加入请求回调
    /// </summary>
    /// <param name="roomId">请求来源房间id</param>
    /// <param name="userId">请求来源主播id</param>
    /// <param name="extra">扩展信息</param>
    public delegate void OnJoinSubRoomRequestReceivedDelegate(string roomId, string userId, string extra);
    
    /// <summary>
    /// 收到取消加入请求回调
    /// </summary>
    /// <param name="roomId">请求来源房间id</param>
    /// <param name="userId">请求来源主播id</param>
    /// <param name="extra">扩展信息</param>
    public delegate void OnCancelJoinSubRoomRequestReceivedDelegate(string roomId, string userId, string extra);
    
    /// <summary>
    /// 收到加入请求响应回调
    /// </summary>
    /// <param name="roomId">响应来源房间id</param>
    /// <param name="userId">响应来源主播id</param>
    /// <param name="agree">是否同意</param>
    /// <param name="extra">扩展信息</param>
    public delegate void OnJoinSubRoomRequestResponseReceivedDelegate(string roomId, string userId, bool agree,
                                                                      string extra);
    
    /// <summary>
    /// 加入子房间回调
    /// </summary>
    /// <param name="roomId">子房间id</param>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误信息</param>
    public delegate void OnSubRoomJoinedDelegate(string roomId, int code, string errMsg);
    
    /// <summary>
    /// 离开子房间回调
    /// </summary>
    /// <param name="roomId">子房间id</param>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误信息</param>
    public delegate void OnSubRoomLeftDelegate(string roomId, int code, string errMsg);
    
    /// <summary>
    /// 连麦中的子房间回调
    /// </summary>
    /// <param name="roomId">子房间id</param>
    public delegate void OnSubRoomBandedDelegate(string roomId);
    
    /// <summary>
    /// 子房间结束连麦回调
    /// </summary>
    /// <param name="roomId">子房间id</param>
    /// <param name="userId">结束连麦的用户id</param>
    public delegate void OnSubRoomDisbandDelegate(string roomId, string userId);
    
    /// <summary>
    /// 切换直播角色回调
    /// </summary>
    /// <param name="current">当前角色</param>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误消息</param>
    public delegate void OnLiveRoleSwitchedDelegate(RCRTCRole current, int code, string errMsg);
    
    /// <summary>
    /// 远端用户身份切换回调
    /// </summary>
    /// <param name="roomId">房间号</param>
    /// <param name="userId">用户id</param>
    /// <param name="role">用户角色</param>
    public delegate void OnRemoteLiveRoleSwitchedDelegate(string roomId, string userId, RCRTCRole role);
    
    /// <summary>
    /// 开启直播内置 cdn 结果回调
    /// </summary>
    /// <param name="enable">是否开启</param>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误消息</param>
    public delegate void OnLiveMixInnerCdnStreamEnabledDelegate(bool enable, int code, string errMsg);
    
    /// <summary>
    /// 直播内置 cdn 资源发布回调
    /// </summary>
    public delegate void OnRemoteLiveMixInnerCdnStreamPublishedDelegate();
    
    /// <summary>
    /// 直播内置 cdn 资源取消发布回调
    /// </summary>
    public delegate void OnRemoteLiveMixInnerCdnStreamUnpublishedDelegate();
    
    /// <summary>
    /// 订阅直播内置 cdn 资源回调
    /// </summary>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误消息</param>
    public delegate void OnLiveMixInnerCdnStreamSubscribedDelegate(int code, string errMsg);
    
    /// <summary>
    /// 取消订阅直播内置 cdn 资源回调
    /// </summary>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误消息</param>
    public delegate void OnLiveMixInnerCdnStreamUnsubscribedDelegate(int code, string errMsg);
    
    /// <summary>
    /// 观众端设置订阅 cdn 流的分辨率的回调
    /// </summary>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误消息</param>
    public delegate void OnLocalLiveMixInnerCdnVideoResolutionSetDelegate(int code, string errMsg);
    
    /// <summary>
    /// 观众端 设置订阅 cdn 流的帧率的回调
    /// </summary>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误消息</param>
    public delegate void OnLocalLiveMixInnerCdnVideoFpsSetDelegate(int code, string errMsg);
    
    /// <summary>
    /// 设置水印的回调
    /// </summary>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误消息</param>
    public delegate void OnWatermarkSetDelegate(int code, string errMsg);
    
    /// <summary>
    /// 移除水印的回调
    /// </summary>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误消息</param>
    public delegate void OnWatermarkRemovedDelegate(int code, string errMsg);
    
    /// <summary>
    /// 开启网络探测结果回调
    /// </summary>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误消息</param>
    public delegate void OnNetworkProbeStartedDelegate(int code, string errMsg);
    
    /// <summary>
    /// 关闭网络探测结果回调
    /// </summary>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误消息</param>
    public delegate void OnNetworkProbeStoppedDelegate(int code, string errMsg);
    
    /// <summary>
    /// 开启 SEI 功能结果回调
    /// </summary>
    /// <param name="enable">是否开启</param>
    /// <param name="code">错误码</param>
    /// <param name="errMsg">错误消息</param>
    public delegate void OnSeiEnabledDelegate(bool enable, int code, string errMsg);
    
    /// <summary>
    /// 收到 SEI 信息回调
    /// </summary>
    /// <param name="roomId">房间 id</param>
    /// <param name="userId">远端用户 id</param>
    /// <param name="sei">SEI 信息</param>
    public delegate void OnSeiReceivedDelegate(string roomId, string userId, string sei);
    
    /// <summary>
    /// 观众收到合流 SEI 信息回调
    /// </summary>
    /// <param name="sei">SEI 信息</param>
    public delegate void OnLiveMixSeiReceivedDelegate(string sei);
    
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