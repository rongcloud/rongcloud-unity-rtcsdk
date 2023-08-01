using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;

# if PLATFORM_ANDROID
using UnityEngine.Android;
# endif

using Newtonsoft.Json;

using cn_rongcloud_im_unity;
using cn_rongcloud_rtc_unity;

namespace cn_rongcloud_rtc_unity_example
{
    public class Example : MonoBehaviour
    {
        private void Start()
        {
            ConnectCanvas = GameObject.Find("Connect").GetComponent<Canvas>().gameObject;
            ConnectCanvasAppKeyInput = GameObject.Find("/Connect/AppKeyInput").GetComponent<InputField>();
            ConnectCanvasNaviServerInput = GameObject.Find("/Connect/NaviServerInput").GetComponent<InputField>();
            ConnectCanvasFileServerInput = GameObject.Find("/Connect/FileServerInput").GetComponent<InputField>();
            ConnectCanvasMediaServerInput = GameObject.Find("/Connect/MediaServerInput").GetComponent<InputField>();
            ConnectCanvasTokenInput = GameObject.Find("/Connect/TokenInput").GetComponent<InputField>();

            ConnectedCanvas = GameObject.Find("/Connected").GetComponent<Canvas>().gameObject;
            ConnectedCanvasModeGroup = GameObject.Find("/Connected/ModeGroup").GetComponent<ToggleGroup>().gameObject;
            ConnectedCanvasJoinText = GameObject.Find("/Connected/Join/Text").GetComponent<Text>();
            ConnectedRoomStreamConfig = GameObject.Find("/Connected/RoomStreamConfigPanel")
                .GetComponent<RoomStreamConfigPrefab>();

            MeetingCanvas = GameObject.Find("Meeting").GetComponent<Canvas>().gameObject;
            MeetingCanvasTitle = GameObject.Find("/Meeting/Title").GetComponent<Text>();
            MeetingVideoPanel = GameObject.Find("Meeting").GetComponent<VideoPanelPrefab>();
            MeetingCanvasEnableMicrophoneSwitchInfo =
                GameObject.Find("/Meeting/EnableMicrophoneSwitch/Text").GetComponent<Text>();
            MeetingCanvasEnableCameraSwitchInfo =
                GameObject.Find("/Meeting/EnableCameraSwitch/Text").GetComponent<Text>();
            MeetingCanvasPublishAudioSwitchInfo =
                GameObject.Find("/Meeting/PublishAudioSwitch/Text").GetComponent<Text>();
            MeetingCanvasSwitchAudioOutputInfo =
                GameObject.Find("/Meeting/SwitchAudioOutput/Text").GetComponent<Text>();
            MeetingCanvasStatsTable = GameObject.Find("/Meeting/StatsTable/Viewport/Content");
            MeetingCanvasUserList = GameObject.Find("/Meeting/UserList/Viewport/Content");
            MeetingPublishVideoText = GameObject.Find("/Meeting/PublishVideoSwitch/Text").GetComponent<Text>();
            MeetingVideoConfigPanel = GameObject.Find("/Meeting/VideoConfigPanel").GetComponent<VideoConfigPrefab>();
            MeetingVideoConfigPanel.OnValueChanged = OnVideoConfigChanged;

            TinyVideoConfigPrefab MeetingTinyVideo = GameObject.Find("/Meeting/TinyVideoConfigPanel").GetComponent<TinyVideoConfigPrefab>();
            MeetingTinyVideo.OnValueChanged = OnTinyVideoConfigChanged;

            HostCanvas = GameObject.Find("Host").GetComponent<Canvas>().gameObject;
            HostCanvasTitle = GameObject.Find("/Host/Title").GetComponent<Text>();
            HostVideoPanel = GameObject.Find("/Host").GetComponent<VideoPanelPrefab>();
            HostCanvasEnableMicrophoneSwitchInfo =
                GameObject.Find("/Host/EnableMicrophoneSwitch/Text").GetComponent<Text>();
            HostCanvasEnableCameraSwitchInfo = GameObject.Find("/Host/EnableCameraSwitch/Text").GetComponent<Text>();
            HostCanvasPublishAudioSwitchInfo = GameObject.Find("/Host/PublishAudioSwitch/Text").GetComponent<Text>();
            HostCanvasSwitchAudioOutputInfo = GameObject.Find("/Host/SwitchAudioOutput/Text").GetComponent<Text>();
            HostPublishVideoText = GameObject.Find("/Host/PublishVideoSwitch/Text").GetComponent<Text>();
            HostCanvasStatsTable = GameObject.Find("/Host/StatsTable/Viewport/Content");
            HostCanvasUserList = GameObject.Find("/Host/UserList/Viewport/Content");
            HostVideoConfigPanel = GameObject.Find("/Host/VideoConfigPanel").GetComponent<VideoConfigPrefab>();
            HostVideoConfigPanel.OnValueChanged = OnVideoConfigChanged;
            VideoConfigPrefab HostCustomStream = GameObject.Find("/Host/CustomStream/VideoConfigPanel").GetComponent<VideoConfigPrefab>();
            HostCustomStream.OnValueChanged = OnCustomStreamVideoConfigChanged;

            TinyVideoConfigPrefab HostTinyVideo = GameObject.Find("/Host/TinyVideoConfigPanel").GetComponent<TinyVideoConfigPrefab>();
            HostTinyVideo.OnValueChanged = OnTinyVideoConfigChanged;

            AudienceCanvas = GameObject.Find("Audience").GetComponent<Canvas>().gameObject;
            AudienceCanvasTitle = GameObject.Find("/Audience/Title").GetComponent<Text>();
            AudienceVideoPanel = GameObject.Find("/Audience").GetComponent<VideoPanelPrefab>();
            AudienceSubscribeButton = GameObject.Find("/Audience/Subscribe").GetComponent<Button>();
            AudienceCanvasSwitchAudioOutputInfo =
                GameObject.Find("/Audience/SwitchAudioOutput/Text").GetComponent<Text>();
            AudienceCanvasStatsTable = GameObject.Find("/Audience/StatsTable/Viewport/Content");
            AudienceAudioToggle = GameObject.Find("/Audience/Audio").GetComponent<Toggle>();
            AudienceVideoToggle = GameObject.Find("/Audience/Video").GetComponent<Toggle>();
            AudienceAudioVideoToggle = GameObject.Find("/Audience/AudioVideo").GetComponent<Toggle>();
            AudienceTinyStream = GameObject.Find("/Audience/TinyStream").GetComponent<Toggle>();
            resetAudienceUI();

            MessageCanvas = GameObject.Find("Message").GetComponent<Canvas>().gameObject;
            MessageCanvasActionInfo = GameObject.Find("/Message/Panel/Action/Text").GetComponent<Text>();
            MessageCanvasMessageList = GameObject.Find("/Message/Panel/MessageList/Viewport/Content");
            MessageCanvasMessageInput = GameObject.Find("/Message/Panel/MessageInput").GetComponent<InputField>();

            ConfigCDNCanvas = GameObject.Find("ConfigCDN").GetComponent<Canvas>().gameObject;
            ConfigCDNCanvasCDNList = GameObject.Find("/ConfigCDN/Panel/CDNList/Viewport/Content");
            ConfigCDNCanvasDialog = GameObject.Find("/ConfigCDN/Dialog");
            ConfigCDNCanvasDialogCDNList = GameObject.Find("/ConfigCDN/Dialog/Panel/CDNList/Viewport/Content");

            AudioEffectCanvas = GameObject.Find("/AudioEffect").GetComponent<Canvas>().gameObject;
            AudioEffectCanvasEffectList = GameObject.Find("/AudioEffect/Background/EffectList/Viewport/Content");

            AudioMixCanvas = GameObject.Find("/AudioMix").GetComponent<Canvas>().gameObject;
            AudioMixCanvasMixVolume = GameObject.Find("/AudioMix/Background/MixVolume").GetComponent<Slider>();
            AudioMixCanvasPublishVolume = GameObject.Find("/AudioMix/Background/PublishVolume").GetComponent<Slider>();
            AudioMixCanvasPlayback = GameObject.Find("/AudioMix/Background/Playback").GetComponent<Toggle>();
            AudioMixCanvasPlaybackVolume =
                GameObject.Find("/AudioMix/Background/PlaybackVolume").GetComponent<Slider>();

            HostMixConfig = GameObject.Find("HostMixConfig").GetComponent<Canvas>().gameObject;
            HostMixBgConfig = GameObject.Find("HostMixVideoBgConfig");
            HostMixVideoStreamConfig = GameObject.Find("HostMixVideoStreamConfig").GetComponent<Canvas>().gameObject;
            HostMixTinyVideoStreamConfig =
                GameObject.Find("HostMixTinyVideoStreamConfig").GetComponent<Canvas>().gameObject;
            HostMixVideoCustomLayoutConfig =
                GameObject.Find("HostMixVideoCustomLayoutConfig").GetComponent<Canvas>().gameObject;
            HostMixAddVideoCustomLayout = GameObject
                .Find("HostMixAddVideoCustomLayout").GetComponent<Canvas>().gameObject;
            HostMixAudioConfig = GameObject.Find("HostMixAudioConfig").GetComponent<Canvas>().gameObject;
            HostMixAudioCustomConfig = GameObject.Find("HostMixAudioCustomConfig").GetComponent<Canvas>().gameObject;

            DeviceListCanvas = GameObject.Find("DeviceList");
            DeviceList = GameObject.Find("DeviceList/Scroll View/Viewport/Content");

            SubRoomCanvas = GameObject.Find("SubRoom");

            SeiCanvas = GameObject.Find("SEI");

            HostMixConfig.SetActive(false);
            HostMixBgConfig.SetActive(false);
            HostMixVideoStreamConfig.SetActive(false);
            HostMixTinyVideoStreamConfig.SetActive(false);
            HostMixVideoCustomLayoutConfig.SetActive(false);
            HostMixAddVideoCustomLayout.SetActive(false);
            HostMixAudioConfig.SetActive(false);
            HostMixAudioCustomConfig.SetActive(false);

            ConnectedCanvas.SetActive(false);
            MeetingCanvas.SetActive(false);
            HostCanvas.SetActive(false);
            AudienceCanvas.SetActive(false);
            MessageCanvas.SetActive(false);
            ConfigCDNCanvasDialog.SetActive(false);
            ConfigCDNCanvas.SetActive(false);
            AudioEffectCanvas.SetActive(false);
            AudioMixCanvas.SetActive(false);
            DeviceListCanvas.SetActive(false);
            SubRoomCanvas.SetActive(false);
            SeiCanvas.SetActive(false);

            Connected = false;

            Users = new Dictionary<String, GameObject>();
            UserSubscribeAudioStates = new Dictionary<string, bool>();
            UserSubscribeVideoStates = new Dictionary<string, bool>();
            Statses = new Dictionary<string, GameObject>();
            Messages = new List<GameObject>();
            CDNs = new Dictionary<CDN, GameObject>();
            SelectedCDNs = new List<GameObject>();
            AudioEffects = new Dictionary<int, GameObject>();
            SavedJsonPath = Application.persistentDataPath + "/savedInfo.json";
            _videoCustomLayouts.Clear();
            PushEnabled = GameObject.Find("/Connect/PushEnabled").GetComponent<Toggle>();
            IpcDisEnabled = GameObject.Find("/Connect/IpcDisEnabled").GetComponent<Toggle>();
#if !UNITY_ANDROID
            PushEnabled.gameObject.SetActive(false);
            IpcDisEnabled.gameObject.SetActive(false);
#else
            PushEnabled.isOn = mPushEnabled;
            IpcDisEnabled.isOn = mDisableIPC;
#endif
#if UNITY_STANDALONE_WIN
            CurrentMicrophones = new List<RCRTCDevice>();
            CurrentSpeakers = new List<RCRTCDevice>();
#endif

            LoadTokenInfo();
        }

        private void Update()
        {
            if (!RunOnMainThread.IsEmpty)
            {
                while (RunOnMainThread.TryDequeue(out var action))
                {
                    action?.Invoke();
                }
            }
            if (EchoTest)
            {
                var time = Time.realtimeSinceStartup;
                int count = 0;
                if (EchoTestDeltaTime == 0)
                {
                    EchoTestDeltaTime = time;
                }
                else
                {
                    count = (int)(time - EchoTestDeltaTime);
                }
                Text countDown = GameObject.Find("/EchoTest/CountDown").GetComponent<Text>();
                Text tip = GameObject.Find("/EchoTest/Tip").GetComponent<Text>();
                if (count <= EchoTestTime)
                {
                    tip.text = "请说一些话";
                    countDown.text = count.ToString();
                }
                else if (count <= EchoTestTime * 2)
                {
                    tip.text = "现在应该能听到刚才的声音";
                    countDown.text = (EchoTestTime * 2 - count).ToString();
                }
                else
                {
                    tip.text = "";
                    countDown.text = "";
                }
            }
            if (Sei)
            {
                var time = Time.realtimeSinceStartup;
                int delta = (int)(time - SeiDeltaTime);
                if (delta >= 2)
                {
                    SeiDeltaTime = time;
                    SeiCountDown += 1;
                    if (SeiCountDown <= SeiCount)
                    {
                        int ret = Engine.SendSei(SeiText + SeiCountDown.ToString());
                        Debug.Log($"sendSEI code:{ret}");
                    }
                    else
                    {
                        SeiDeltaTime = 0;
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            Engine?.LeaveRoom();
            Engine?.Destroy();
            imEngine?.Destroy();
        }
#endif

        private void OnDestroy()
        {
            Engine?.LeaveRoom();
            Engine?.Destroy();
            imEngine?.Destroy();
        }

        #region UI Event

        #region 连接页面
        public void OnClickConnect()
        {
            if (Connected)
            {
                imEngine?.Disconnect(true);
                ChangeToUnconnect();
            }
            else
            {
                if (String.IsNullOrEmpty(ConnectCanvasTokenInput.text))
                {
                    ExampleUtils.ShowToast("请输入Token!");
                    return;
                }
                ConnectIM(ConnectCanvasTokenInput.text);
            }
        }

        public void OnClickGenerate()
        {
            if (string.IsNullOrEmpty(ConnectCanvasAppKeyInput.text))
            {
                ExampleUtils.ShowToast("请输入Appkey");
                return;
            }

            ExampleConfig.AppKey = ConnectCanvasAppKeyInput.text;
            ExampleConfig.NavServer = ConnectCanvasNaviServerInput.text;
            ExampleConfig.FileServer = ConnectCanvasFileServerInput.text;
            ExampleConfig.MediaServer = ConnectCanvasMediaServerInput.text;

            GenerateToken();
        }

        public void OnClickJoin()
        {
            if (String.IsNullOrEmpty(RoomId))
            {
                ExampleUtils.ShowToast("房间号不能为空！");
                return;
            }

            ExampleUtils.ShowLoading();
            RCRTCRoomSetup.Builder roomSetupBuilder = RCRTCRoomSetup.Builder.Create();
            RCRTCEngineSetup.Builder engineSetupBuilder = RCRTCEngineSetup.Builder.Create();
            RCRTCVideoSetup.Builder videoSetupBuilder = RCRTCVideoSetup.Builder.Create();
            engineSetupBuilder.WithMediaUrl(ExampleConfig.MediaServer);
            switch (Role)
            {
                case RCRTCRole.MEETING_MEMBER:
                    engineSetupBuilder.WithEnableSRTP(ConnectedRoomStreamConfig.SrtpEnabled);
                    videoSetupBuilder.WithEnableTinyStream(ConnectedRoomStreamConfig.TinyEnabled);
                    break;
                case RCRTCRole.LIVE_BROADCASTER:
                    RCRTCMediaType mediaType = RCRTCMediaType.AUDIO_VIDEO;
                    if (ConnectedRoomStreamConfig.AudioOnlyMode)
                    {
                        mediaType = RCRTCMediaType.AUDIO;
                    }

                    roomSetupBuilder.WithMediaType(mediaType);
                    videoSetupBuilder.WithEnableTinyStream(ConnectedRoomStreamConfig.TinyEnabled);
                    engineSetupBuilder.WithEnableSRTP(ConnectedRoomStreamConfig.SrtpEnabled);
                    break;
                case RCRTCRole.LIVE_AUDIENCE:
                    engineSetupBuilder.WithEnableSRTP(ConnectedRoomStreamConfig.SrtpEnabled);
                    break;
            }

            engineSetupBuilder.WithVideoSetup(videoSetupBuilder.Build());
            Engine = RCRTCEngine.Create(engineSetupBuilder.Build());
#if UNITY_STANDALONE_WIN
            Engine.OnCameraListChanged = OnCameraLiseChange;
            Engine.OnMicrophoneListChanged = OnMicrophoneLiseChange;
            Engine.OnSpeakerListChanged = OnSpeakerLiseChange;
            Engine.OnCameraSwitched = OnCameraChange;
#else
            Engine.OnCameraSwitched = OnCameraChange;
#endif

            switch (Role)
            {
                case RCRTCRole.MEETING_MEMBER:
                    Engine.OnUserJoined = MeetingOnUserJoined;
                    Engine.OnUserOffline = MeetingOnUserOffline;
                    Engine.OnUserLeft = MeetingOnUserLeft;

                    Engine.OnRemotePublished = MeetingOnRemotePublished;
                    Engine.OnRemoteUnpublished = MeetingOnRemoteUnpublished;
                    break;
                case RCRTCRole.LIVE_BROADCASTER:
                    Engine.OnUserJoined = OnUserJoined;
                    Engine.OnUserOffline = OnUserOffline;
                    Engine.OnUserLeft = OnUserLeft;

                    Engine.OnRemotePublished = OnRemotePublished;
                    Engine.OnRemoteUnpublished = OnRemoteUnpublished;
                    Engine.OnSubRoomBanded = OnSubRoomBanded;
                    Engine.OnSubRoomDisband = OnSubRoomDisband;
                    Engine.OnJoinSubRoomRequestReceived = OnJoinSubRoomRequestReceived;
                    Engine.OnJoinSubRoomRequestResponseReceived = OnJoinSubRoomRequestResponseReceived;
                    Engine.OnRemoteCustomStreamPublished = OnRemoteCustomStreamPublished;
                    Engine.OnRemoteCustomStreamUnpublished = OnRemoteCustomStreamUnpublished;
                    Engine.OnSeiReceived = OnSeiReceived;
                    Engine.OnRemoteLiveRoleSwitched = OnRemoteLiveRoleSwitched;
                    break;
                case RCRTCRole.LIVE_AUDIENCE:
                    Engine.OnUserJoined = OnUserJoined;
                    Engine.OnUserOffline = OnUserOffline;
                    Engine.OnUserLeft = OnUserLeft;
                    Engine.OnRemotePublished = OnRemotePublished;
                    Engine.OnRemoteUnpublished = OnRemoteUnpublished;
                    Engine.OnRemoteLiveMixPublished = AudienceOnRemoteLiveMixPublished;
                    Engine.OnRemoteLiveMixUnpublished = AudienceOnRemoteLiveMixUnpublished;
                    Engine.OnLiveMixSeiReceived = OnLiveMixSeiReceived;
                    break;
            }
            Engine.OnRoomJoined = delegate (int code, String message)
            {
                Engine.OnRoomJoined = null;

                RunOnMainThread.Enqueue(() =>
                {
                    ExampleUtils.HideLoading();
                    if (code == 0)
                    {
                        ConnectedCanvas.SetActive(false);
                        switch (Role)
                        {
                            case RCRTCRole.MEETING_MEMBER:
                                ChangeToMeeting(RoomId);
                                break;
                            case RCRTCRole.LIVE_BROADCASTER:
                                ChangeToHost(RoomId);
                                break;
                            case RCRTCRole.LIVE_AUDIENCE:
                                ChangeToAudience(RoomId);
                                break;
                        }
                    }
                    else
                    {
                        ExampleUtils.ShowToast(message);
                    }
                });
            };

            RCRTCRoomSetup setup = roomSetupBuilder.WithRole(Role).Build();
            int ret = Engine.JoinRoom(RoomId, setup);
            if (ret != 0)
            {
                ExampleUtils.HideLoading();
                ExampleUtils.ShowToast("Error " + ret);
            }
        }

        public void OnClickEnablePush()
        {
#if UNITY_ANDROID
            mPushEnabled = PushEnabled.isOn;
#endif
        }

        public void OnClickEnableIpc()
        {
#if UNITY_ANDROID
            mDisableIPC = IpcDisEnabled.isOn;
#endif
        }

        private bool EchoTest;
        private int EchoTestTime;
        private float EchoTestDeltaTime;

        public void OnClickEchoTest()
        {
            if (EchoTest)
            {
                StopEchoTest();
            }
            else
            {
                InputField input = GameObject.Find("/EchoTest/InputField").GetComponent<InputField>();
                string text = input.text;
                if (text == null || text.Length == 0)
                {
                    ExampleUtils.ShowToast("请输入时间");
                    return;
                }
                int time = int.Parse(text);
                if (time < 2 || time > 10)
                {
                    time = 10;
                }
                if (Engine == null)
                {
                    Engine = RCRTCEngine.Create();
                }
                int ret = Engine.StartEchoTest(time);
                if (ret == 0)
                {
                    Text button = GameObject.Find("/EchoTest/Start/Text").GetComponent<Text>();
                    button.text = "停止检测";
                    EchoTestTime = time;
                    EchoTest = true;
                    ExampleUtils.ShowToast("已开始");
                }
                else
                {
                    ExampleUtils.ShowToast($"开始失败 code:{ret}");
                }
            }
        }

        public void OnClickCloseEchoTest()
        {
            StopEchoTest();
            GameObject.Find("/EchoTest").SetActive(false);
        }

        #region Token保存
        private string SavedJsonPath;

        public void SaveTokenInfo()
        {
            if (User != null)
            {
                Config config = new Config
                {
                    AppKey = ExampleConfig.AppKey,
                    NavServer = ExampleConfig.NavServer,
                    FileServer = ExampleConfig.FileServer,
                    MediaServer = ExampleConfig.MediaServer,
                    Token = User.Token,
                    Id = User.Id,
                };
                if (!SavedInfos.Contains(config))
                {
                    SavedInfos.Add(config);
                    ReloadTokenInfo(false);
                }
                var json = JsonConvert.SerializeObject(SavedInfos);
                File.WriteAllText(SavedJsonPath, json);
            }
        }

        public void LoadTokenInfo()
        {
            if (SavedInfos == null)
            {
                SavedInfos = new List<Config>();
                if (File.Exists(SavedJsonPath))
                {
                    var json = File.ReadAllText(SavedJsonPath);
                    if (json.Length > 0)
                    {
                        SavedInfos = JsonConvert.DeserializeObject<List<Config>>(json);
                    }
                }
            }
            if (SavedInfos.Count == 0)
            {
                SavedInfos.Add(Config.DefaultConfig());
            }
            ReloadTokenInfo(true);
        }

        public void ClearTokenInfo()
        {
            SavedInfos = new List<Config>() { Config.DefaultConfig() };
            var json = JsonConvert.SerializeObject(SavedInfos);
            File.WriteAllText(SavedJsonPath, json);
            ReloadTokenInfo(true);
        }

        public void OnTokenOptionsValueChanged()
        {
            Dropdown tokenOptions = GameObject.Find("/Connect/History").GetComponent<Dropdown>();
            LoadTokenConfig(SavedInfos[tokenOptions.value]);
        }

        private void LoadTokenConfig(Config conifg)
        {
            ExampleConfig.AppKey = conifg.AppKey;
            ConnectCanvasAppKeyInput.text = conifg.AppKey;
            ExampleConfig.NavServer = conifg.NavServer;
            ConnectCanvasNaviServerInput.text = conifg.NavServer;
            ExampleConfig.FileServer = conifg.FileServer;
            ConnectCanvasFileServerInput.text = conifg.FileServer;
            ExampleConfig.MediaServer = conifg.MediaServer;
            ConnectCanvasMediaServerInput.text = conifg.MediaServer;
            ConnectCanvasTokenInput.text = conifg.Token;
            User = new LoginData();
            User.userId = conifg.Id;
            User.token = conifg.Token;
        }

        private void ReloadTokenInfo(bool reset)
        {
            Dropdown tokenOptions = GameObject.Find("/Connect/History").GetComponent<Dropdown>();
            tokenOptions.options.Clear();
            var optionDatas = new List<Dropdown.OptionData>();
            foreach (var item in SavedInfos)
            {
                optionDatas.Add(new Dropdown.OptionData(item.Title()));
            }
            tokenOptions.AddOptions(optionDatas);
            if (reset)
            {
                tokenOptions.SetValueWithoutNotify(0);
                LoadTokenConfig(SavedInfos[0]);
            }
            else
            {
                var value = optionDatas.Count - 1;
                tokenOptions.SetValueWithoutNotify(value);
                LoadTokenConfig(SavedInfos[value]);
            }
        }
        #endregion

        #endregion

        #region 模式选择页面
        public void OnMeetingModeSelectStateChanged(bool selected)
        {
            if (selected)
            {
                Role = RCRTCRole.MEETING_MEMBER;
                ConnectedCanvasJoinText.text = "加入会议";
                ConnectedRoomStreamConfig.SetRoomMode(RoomStreamConfigPrefab.RoomMode.MeetingRoom);
            }
        }

        public void OnHostModeSelectStateChanged(bool selected)
        {
            if (selected)
            {
                Role = RCRTCRole.LIVE_BROADCASTER;
                ConnectedCanvasJoinText.text = "开始直播";
                ConnectedRoomStreamConfig.SetRoomMode(RoomStreamConfigPrefab.RoomMode.HostRoom);
            }
        }

        public void OnAudienceModeSelectStateChanged(bool selected)
        {
            if (selected)
            {
                Role = RCRTCRole.LIVE_AUDIENCE;
                ConnectedCanvasJoinText.text = "观看直播";
                ConnectedRoomStreamConfig.SetRoomMode(RoomStreamConfigPrefab.RoomMode.AudienceRoom);

            }
        }

        public void OnRoomIdChanged(String id)
        {
            RoomId = id;
        }

        private bool NetDetect;

        public void OnClickNetDetect()
        {
            if (NetDetect)
            {
                int ret = Engine.StopNetworkProbe();
                if (ret == 0)
                {
                    Text button = GameObject.Find("/NetDetect/Start/Text").GetComponent<Text>();
                    button.text = "开启探测";
                    NetDetect = false;
                    Engine.Destroy();
                    Engine = null;
                    ExampleUtils.ShowToast("停止成功");
                }
                else
                {
                    ExampleUtils.ShowToast("停止失败");
                }
            }
            else
            {
                if (Engine == null)
                {
                    Engine = RCRTCEngine.Create();
                }
                Text up = GameObject.Find("/NetDetect/Up").GetComponent<Text>();
                Text down = GameObject.Find("/NetDetect/Down").GetComponent<Text>();
                int ret = Engine.StartNetworkProbe(new NetworkProbeProxy((RCRTCNetworkProbeStats stats) =>
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        down.text = $"下行 ｜ 质量：{stats.qualityLevel} | 往返：{stats.rtt}ms | 丢包率：{stats.packetLostRate}";
                    });
                }, (RCRTCNetworkProbeStats stats) => {
                    RunOnMainThread.Enqueue(() =>
                    {
                        up.text = $"上行 ｜ 质量：{stats.qualityLevel} | 往返：{stats.rtt}ms | 丢包率：{stats.packetLostRate}";
                    });
                }));
                if (ret == 0)
                {
                    Text button = GameObject.Find("/NetDetect/Start/Text").GetComponent<Text>();
                    button.text = "主动结束";
                    NetDetect = true;
                    ExampleUtils.ShowToast("开始成功");
                }
                else
                {
                    ExampleUtils.ShowToast($"开始失败 code:{ret}");
                }
            }
        }

        public void OnClickCloseNetDetect()
        {
            if (NetDetect)
            {
                int ret = Engine.StopNetworkProbe();
                if (ret == 0)
                {
                    Text button = GameObject.Find("/NetDetect/Start/Text").GetComponent<Text>();
                    button.text = "开启探测";
                    NetDetect = false;
                    Engine.Destroy();
                    Engine = null;
                    ExampleUtils.ShowToast("停止成功");
                }
                else
                {
                    ExampleUtils.ShowToast("停止失败");
                }
            }
            GameObject.Find("/NetDetect").SetActive(false);
        }
        #endregion

        #region Metting
        public void OnClickLeaveMeeting()
        {
            ExampleUtils.ShowLoading();
            Engine.OnRoomLeft = delegate (int code, String message)
            {
                Engine.OnRoomLeft = null;

                RunOnMainThread.Enqueue(() =>
                {
                    ExampleUtils.HideLoading();
                    ChangeToConnected();
                    if (code != 0)
                    {
                        ExampleUtils.ShowToast(message);
                    }
                    Engine.Destroy();
                    Engine = null;
                });
            };
            Engine.SetStatsListener(null);
            Engine.LeaveRoom();

            resetMeetingUI();
        }

        public void OnClickMeetingEnableMicrophoneSwitch()
        {
            if (!CheckMicrophonePermission())
            {
                ExampleUtils.ShowToast("没有麦克风权限");
                return;
            }
#if UNITY_STANDALONE_WIN
            RCRTCDevice[] list = Engine.GetMicrophoneList();
            Debug.Log("获取到麦克风列表" + list.Length);
            if (list.Length > 0)
            {
                ReloadDeviceList(list, CurrentMicrophones, true, true, (device, value) => {
                    if (CurrentMicrophones.Contains(device))
                    {
                        if (!value)
                        {
                            Engine.EnableMicrophone(device, false, false);
                        }
                    }
                    else
                    {
                        foreach (var item in CurrentMicrophones)
                        {
                            Engine.EnableMicrophone(item, false, false);
                        }
                        Engine.EnableMicrophone(device, value, true);
                    }
                    CurrentMicrophones.Clear();
                    if (value)
                    {
                        CurrentMicrophones.Add(device);
                    }
                    /*
                    if (value)
                    {
                        CurrentMicrophones.Add(device);
                    }
                    else
                    {
                        CurrentMicrophones.Remove(device);
                    }*/
                });
            }
            else
            {
                ExampleUtils.ShowToast("没有可用的麦克风");
            }
#else
            int ret = Engine.EnableMicrophone(!Microphone);
            if (ret == 0)
            {
                Microphone = !Microphone;
                MeetingCanvasEnableMicrophoneSwitchInfo.text = Microphone ? "关闭麦克风" : "开启麦克风";
            }
#endif
        }

        public void OnClickMeetingEnableCameraSwitch()
        {
            bool? isEnabled = _enableCamera(MeetingVideoPanel.videoView);
            if (isEnabled != null)
            {
                MeetingCanvasEnableCameraSwitchInfo.text = isEnabled.Value ? "关闭摄像头" : "开启摄像头";
            }
        }

        public void OnClickMeetingPublishAudioSwitch()
        {
            ExampleUtils.ShowLoading();
            if (!PublishAudio)
            {
                Engine.OnPublished = delegate (RCRTCMediaType type, int code, String message)
                {
                    Engine.OnPublished = null;

                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        if (code != 0)
                        {
                            ExampleUtils.ShowToast(message);
                        }
                        else
                        {
                            PublishAudio = true;
                            MeetingCanvasPublishAudioSwitchInfo.text = PublishAudio ? "取消发布音频" : "发布音频";
                        }
                    });
                };

                int ret = Engine.Publish(RCRTCMediaType.AUDIO);
                if (ret != 0)
                {
#if UNITY_STANDALONE_WIN
                    if (CurrentMicrophones.Count == 0)
                    {
                        RCRTCDevice[] list = Engine.GetMicrophoneList();
                        if (list.Length == 0)
                        {
                            CurrentMicrophones = new List<RCRTCDevice> { list[0] };
                        }
                    }
#endif
                    ExampleUtils.HideLoading();
                    ExampleUtils.ShowToast("Error " + ret);
                }
            }
            else
            {
                Engine.OnUnpublished = delegate (RCRTCMediaType type, int code, String message)
                {
                    Engine.OnUnpublished = null;

                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        if (code != 0)
                        {
                            ExampleUtils.ShowToast(message);
                        }
                        else
                        {
                            PublishAudio = false;
                            MeetingCanvasPublishAudioSwitchInfo.text = PublishAudio ? "取消发布音频" : "发布音频";
                            RemoveAudioStats(MeetingCanvasStatsTable);
                        }
                    });
                };
                int ret = Engine.Unpublish(RCRTCMediaType.AUDIO);
                if (ret != 0)
                {
                    ExampleUtils.HideLoading();
                    ExampleUtils.ShowToast("Error" + ret);
                }
            }
        }

        public void OnClickMeetingPublishVideo()
        {
            ExampleUtils.ShowLoading();
            if (!PublishVideo)
            {
                Engine.OnPublished = delegate (RCRTCMediaType type, int code, String errMsg)
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        if (code != 0)
                        {
                            string toast = $"Publish video error: {errMsg}";
                            ExampleUtils.ShowToast(toast);
                        }
                        else
                        {
                            PublishVideo = true;
                            MeetingPublishVideoText.text = "取消发布视频";
                        }
                    });
                };
                int ret = Engine.Publish(RCRTCMediaType.VIDEO);
                if (ret != 0)
                {
                    ExampleUtils.HideLoading();
                    ExampleUtils.ShowToast("Error" + ret);
                }
                RCRTCVideoConfig config = MeetingVideoConfigPanel.VideoConfig;
                Engine.SetVideoConfig(config);
                Debug.Log($"SetVideoConfig {config}");
            }
            else
            {
                Engine.OnUnpublished = delegate (RCRTCMediaType type, int code, string msg)
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        if (code != 0)
                        {
                            string toast = $"UnPublish video error: {msg}";
                            ExampleUtils.ShowToast(toast);
                        }
                        else
                        {
                            PublishVideo = false;
                            MeetingPublishVideoText.text = "发布视频";
                            RemoveVideoStats(MeetingCanvasStatsTable);
                        }
                    });
                };
                int ret = Engine.Unpublish(RCRTCMediaType.VIDEO);
                if (ret != 0)
                {
                    ExampleUtils.HideLoading();
                    ExampleUtils.ShowToast("Error" + ret);
                }
            }
        }

        public void OnClickMeetingVideoMirror(bool value)
        {
            MeetingVideoPanel.Mirror = value;
        }

        public void OnClickMeetingSwitchAudioOutput()
        {
#if UNITY_STANDALONE_WIN
            RCRTCDevice[] list = Engine.GetSpeakerList();
            Debug.Log("获取到扬声器列表" + list.Length);
            if (list.Length > 0)
            {
                ReloadDeviceList(list, CurrentSpeakers, false, true, (device, value) => {
                    if (CurrentSpeakers.Contains(device))
                    {
                        if (!value)
                        {
                            Engine.EnableSpeaker(device, false);
                        }
                    }
                    else
                    {
                        foreach (var item in CurrentSpeakers)
                        {
                            Engine.EnableSpeaker(item, false);
                        }
                        Engine.EnableSpeaker(device, value);
                    }
                    CurrentSpeakers.Clear();
                    if (value)
                    {
                        CurrentSpeakers.Add(device);
                    }
                    /*if (value)
                    {
                        CurrentSpeakers.Add(device);
                    }
                    else
                    {
                        CurrentSpeakers.Remove(device);
                    }*/
                });
            }
            else
            {
                ExampleUtils.ShowToast("没有可用的扬声器");
            }
#else
            int ret = Engine.EnableSpeaker(!Speaker);
            if (ret == 0)
            {
                Speaker = !Speaker;
                MeetingCanvasSwitchAudioOutputInfo.text = Speaker ? "扬声器" : "听筒";
            }
#endif
        }

        public void OnClickMeetingAudioEffect()
        {
#if UNITY_STANDALONE_WIN
            ExampleUtils.ShowToast("Windows 暂不支持此功能");
            return;
#endif
            Engine.OnAudioEffectCreated = delegate (int id, int code, String message)
            {
                RunOnMainThread.Enqueue(() =>
                {
                    if (code != 0)
                    {
                        ExampleUtils.ShowToast("Create Audio Effect " + id + " Error: " + message);
                    }
                    else
                    {
                        AddAudioEffect(id);
                    }
                });
            };
            for (int i = 0; i < AudioEffectNames.Length; i++)
            {
                String file = String.Format(AudioEffectFilePathFormat, i);
#if UNITY_ANDROID
                String path = "file:///android_asset/" + file;
#else
                String path = Path.Combine(Application.streamingAssetsPath, file);
#endif
                int ret = Engine.CreateAudioEffect(path, i);
                if (ret != 0)
                {
                    ExampleUtils.ShowToast("Create Audio Effect " + file + " Error " + ret);
                }
            }
            AudioEffectCanvas.SetActive(true);
        }

        #endregion

        #region Host 
        public void OnClickLeaveHost()
        {
            ExampleUtils.ShowLoading();
            Engine.OnRoomLeft = delegate (int code, String message)
            {
                Engine.OnRoomLeft = null;

                RunOnMainThread.Enqueue(() =>
                {
                    ExampleUtils.HideLoading();
                    ChangeToConnected();
                    if (code != 0)
                    {
                        ExampleUtils.ShowToast(message);
                    }
                    Engine.Destroy();
                    Engine = null;
                });
            };
            Engine.SetStatsListener(null);
            Engine.LeaveRoom();

            resetHostUI();
        }

        public void OnClickHostEnableMicrophoneSwitch()
        {
            if (!CheckMicrophonePermission())
            {
                ExampleUtils.ShowToast("没有麦克风权限");
                return;
            }
#if UNITY_STANDALONE_WIN
            RCRTCDevice[] list = Engine.GetMicrophoneList();
            Debug.Log("获取到麦克风列表" + list.Length);
            if (list.Length > 0)
            {
                ReloadDeviceList(list, CurrentMicrophones, false, true, (device, value) => {
                    if (CurrentMicrophones.Contains(device))
                    {
                        if (!value)
                        {
                            Engine.EnableMicrophone(device, false, false);
                        }
                    }
                    else
                    {
                        foreach (var item in CurrentMicrophones)
                        {
                            Engine.EnableMicrophone(item, false, false);
                        }
                        Engine.EnableMicrophone(device, value, true);
                    }
                    CurrentMicrophones.Clear();
                    if (value)
                    {
                        CurrentMicrophones.Add(device);
                    }
                    /*if (value)
                    {
                        CurrentMicrophones.Add(device);
                    }
                    else
                    {
                        CurrentMicrophones.Remove(device);
                    }*/
                });
            }
            else
            {
                ExampleUtils.ShowToast("没有可用的麦克风");
            }
#else
            int ret = Engine.EnableMicrophone(!Microphone);
            if (ret == 0)
            {
                Microphone = !Microphone;
                HostCanvasEnableMicrophoneSwitchInfo.text = Microphone ? "关闭麦克风" : "开启麦克风";
            }
#endif
        }

        public void OnClickHostPublishAudioSwitch()
        {
            ExampleUtils.ShowLoading();
            if (!PublishAudio)
            {
                Engine.OnPublished = delegate (RCRTCMediaType type, int code, String message)
                {
                    Engine.OnPublished = null;

                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        if (code != 0)
                        {
                            ExampleUtils.ShowToast(message);
                        }
                        else
                        {
                            PublishAudio = true;
                            HostCanvasPublishAudioSwitchInfo.text = PublishAudio ? "取消发布音频" : "发布音频";
                        }
                    });
                };
                int ret = Engine.Publish(RCRTCMediaType.AUDIO);
                if (ret != 0)
                {
                    ExampleUtils.HideLoading();
                    ExampleUtils.ShowToast("Error " + ret);
                }
            }
            else
            {
                Engine.OnUnpublished = delegate (RCRTCMediaType type, int code, String message)
                {
                    Engine.OnUnpublished = null;

                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        if (code != 0)
                        {
                            ExampleUtils.ShowToast(message);
                        }
                        else
                        {
                            PublishAudio = false;
                            HostCanvasPublishAudioSwitchInfo.text = PublishAudio ? "取消发布音频" : "发布音频";
                            RemoveAudioStats(HostCanvasStatsTable);
                        }
                    });
                };
                int ret = Engine.Unpublish(RCRTCMediaType.AUDIO);
                if (ret != 0)
                {
                    ExampleUtils.HideLoading();
                    ExampleUtils.ShowToast("Error" + ret);
                }
            }
        }

        public void OnClickHostSwitchAudioOutput()
        {
#if UNITY_STANDALONE_WIN
            RCRTCDevice[] list = Engine.GetSpeakerList();
            Debug.Log("获取到扬声器列表" + list.Length);
            if (list.Length > 0)
            {
                ReloadDeviceList(list, CurrentSpeakers, false, true, (device, value) => {
                    if (CurrentSpeakers.Contains(device))
                    {
                        if (!value)
                        {
                            Engine.EnableSpeaker(device, false);
                        }
                    }
                    else
                    {
                        foreach (var item in CurrentSpeakers)
                        {
                            Engine.EnableSpeaker(item, false);
                        }
                        Engine.EnableSpeaker(device, value);
                    }
                    CurrentSpeakers.Clear();
                    if (value)
                    {
                        CurrentSpeakers.Add(device);
                    }
                    /*if (value)
                    {
                        CurrentSpeakers.Add(device);
                    }
                    else
                    {
                        CurrentSpeakers.Remove(device);
                    }*/
                });
            }
            else
            {
                ExampleUtils.ShowToast("没有可用的扬声器");
            }
#else
            int ret = Engine.EnableSpeaker(!Speaker);
            if (ret == 0)
            {
                Speaker = !Speaker;
                HostCanvasSwitchAudioOutputInfo.text = Speaker ? "扬声器" : "听筒";
            }
#endif
        }

        public void OnClickHostEnableCameraSwitch()
        {
            bool? isEnabled = _enableCamera(HostVideoPanel.videoView);
            if (isEnabled != null)
            {
                HostCanvasEnableCameraSwitchInfo.text = isEnabled.Value ? "关闭摄像头" : "开启摄像头";
            }
        }

        public void OnClickHostPublishVideo()
        {
            ExampleUtils.ShowLoading();
            if (!PublishVideo)
            {
                Engine.OnPublished = delegate (RCRTCMediaType type, int code, String errMsg)
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        if (code != 0)
                        {
                            string toast = $"Publish video error: {errMsg}";
                            ExampleUtils.ShowToast(toast);
                        }
                        else
                        {
                            PublishVideo = true;
                            HostPublishVideoText.text = "取消发布视频";
                        }
                    });
                };
                int ret = Engine.Publish(RCRTCMediaType.VIDEO);
                if (ret != 0)
                {
                    ExampleUtils.HideLoading();
                    ExampleUtils.ShowToast("Error" + ret);
                }
                RCRTCVideoConfig config = HostVideoConfigPanel.VideoConfig;
                Engine.SetVideoConfig(config);
                Debug.Log($"SetVideoConfig {config}");
            }
            else
            {
                Engine.OnUnpublished = delegate (RCRTCMediaType type, int code, string msg)
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        if (code != 0)
                        {
                            string toast = $"UnPublish video error: {msg}";
                            ExampleUtils.ShowToast(toast);
                        }
                        else
                        {
                            PublishVideo = false;
                            HostPublishVideoText.text = "发布视频";
                            RemoveVideoStats(HostCanvasStatsTable);
                        }
                    });
                };
                int ret = Engine.Unpublish(RCRTCMediaType.VIDEO);
                if (ret != 0)
                {
                    ExampleUtils.HideLoading();
                    ExampleUtils.ShowToast("Error" + ret);
                }
            }
        }

        public void OnClickHostVideoMirror(bool value)
        {
            HostVideoPanel.Mirror = value;
        }

        private string filePath;

        public void OnClickHostCustomStreamFile()
        {
            ExampleUtils.PickVideo((path, dur) =>
            {
                if (path != null)
                {
                    filePath = path;
                    GameObject.Find("/Host/CustomStream/Text").GetComponent<Text>().text = $"已选文件：{path}";
                }
                else
                {
                    GameObject.Find("/Host/CustomStream/Text").GetComponent<Text>().text = "已选文件：";
                }
            });
        }

        public void OnClickHostCustomStreamPublish()
        {
            if (PublishCustomStream)
            {
                Engine.OnCustomStreamUnpublished = delegate (string tag, int code, string msg) {
                    Engine.OnCustomStreamUnpublished = null;
                    RunOnMainThread.Enqueue(() =>
                    {
                        if (code == 0)
                        {
                            Engine.RemoveLocalCustomStreamView(StreamTag());
                            ExampleUtils.ShowToast("取消发布成功");
                        }
                        else
                        {
                            ExampleUtils.ShowToast($"取消发布失败 code:{code}-message:{msg}");
                        }
                    });
                };
                int ret = Engine.UnpublishCustomStream(StreamTag());
                if (ret != 0)
                {
                    ExampleUtils.ShowToast($"取消发布自定义流错误 code:{ret}");
                }
            }
            else
            {
                if (filePath == null)
                {
                    ExampleUtils.ShowToast("先选择文件");
                    return;
                }
                int ret = Engine.CreateCustomStreamFromFile(filePath, StreamTag(), false, false);
                if (ret != 0)
                {
                    ExampleUtils.ShowToast($"创建自定义流失败 code:{ret}");
                    return;
                }
                Engine.OnCustomStreamPublished = delegate (string tag, int code, string msg) {
                    Engine.OnCustomStreamPublished = null;
                    RunOnMainThread.Enqueue(() => {
                        ExampleUtils.HideLoading();
                        if (code == 0)
                        {
                            RCRTCView view = HostCanvas.transform.Find("CustomStream/Panel").GetComponent<RCRTCView>();
                            Engine.SetLocalCustomStreamView(StreamTag(), view);
                            ExampleUtils.ShowToast("发布成功");
                        }
                        else
                        {
                            ExampleUtils.ShowToast($"发布失败 code:{code}-message:{msg}");
                        }
                    });
                };
                ExampleUtils.ShowLoading();
                var publishCode = Engine.PublishCustomStream(StreamTag());
                if (publishCode != 0)
                {
                    ExampleUtils.HideLoading();
                    ExampleUtils.ShowToast($"发布自定义流错误 code:{publishCode}");
                }
            }
        }
        #endregion

        #region Host More
        public void AddThirdCDN()
        {

            if (CDNs.Values.Count > 0)
            {
                ConfigCDNCanvasDialog.SetActive(true);
            }
            else
            {
                LoadCDNs();
            }
        }

        private bool InnerCDN;

        public void OnClickInnerCDN()
        {
            if (!PublishVideo)
            {
                ExampleUtils.ShowToast("请先发布视频资源");
                return;
            }
            int ret = Engine.EnableLiveMixInnerCdnStream(!InnerCDN);
            if (ret == 0)
            {
                InnerCDN = !InnerCDN;
                string text = InnerCDN ? "关闭融云内置CDN" : "开启融云内置CDN";
                GameObject.Find("/Host/MorePanel/List/InnerCDN/Text").GetComponent<Text>().text = text;
                ExampleUtils.ShowToast("设置成功");
            }
            else
            {
                ExampleUtils.ShowToast($"设置失败 code:{ret}");
            }
        }

        public void OnWaterMarkXChange(float value)
        {
            GameObject.Find("/WaterMark/x/Value").GetComponent<Text>().text = value.ToString();
        }

        public void OnWaterMarkYChange(float value)
        {
            GameObject.Find("/WaterMark/y/Value").GetComponent<Text>().text = value.ToString();
        }

        public void OnWaterMarkZoomChange(float value)
        {
            GameObject.Find("/WaterMark/zoom/Value").GetComponent<Text>().text = value.ToString();
        }

        public void AddVideoMask()
        {
            float x = GameObject.Find("/WaterMark/x").GetComponent<Slider>().value;
            float y = GameObject.Find("/WaterMark/y").GetComponent<Slider>().value;
            float zoom = GameObject.Find("/WaterMark/zoom").GetComponent<Slider>().value;
            ExampleUtils.PickImage((string path) => {
                if (path != null && path.Length > 0)
                {
                    int ret = Engine.SetWatermark(path, x, y, zoom);
                    if (ret == 0)
                    {
                        ExampleUtils.ShowToast("添加水印成功");
                    }
                    else
                    {
                        ExampleUtils.ShowToast("添加水印失败");
                    }
                }
            });
        }

        public void RemoveVideoMask()
        {
            int ret = Engine.RemoveWatermark();
            if (ret == 0)
            {
                ExampleUtils.ShowToast("移除水印成功");
            }
            else
            {
                ExampleUtils.ShowToast("移除水印失败");
            }
        }

        public void OnClickJoinSubRoom()
        {
            string roomId = SubRoomCanvas.transform.Find("Room").GetComponent<InputField>().text;
            string userId = SubRoomCanvas.transform.Find("User").GetComponent<InputField>().text;
            if (roomId.Length == 0 || userId.Length == 0)
            {
                ExampleUtils.ShowToast("请输入房间ID和用户ID");
                return;
            }
            int ret = Engine.RequestJoinSubRoom(roomId, userId, true, null);
            if (ret == 0)
            {
                ExampleUtils.ShowToast("申请已发出，等待对方处理");
            }
            else
            {
                ExampleUtils.ShowToast($"申请发生错误 code:{ret}");
            }
        }

        private bool Sei;
        private int SeiCount;
        private int SeiCountDown;
        private string SeiText;
        private float SeiDeltaTime;

        public void OnClickSEI(bool value)
        {
            Engine.OnSeiEnabled = delegate (bool enable, int code, string errMsg)
            {
                Engine.OnSeiEnabled = null;
                RunOnMainThread.Enqueue(() =>
                {
                    if (code != 0)
                    {
                        ExampleUtils.ShowToast($"enableSei error code:{code} msg:{errMsg}");
                    }
                });
            };
            int ret = Engine.EnableSei(value);
            if (ret != 0)
            {
                Engine.OnSeiEnabled = null;
                ExampleUtils.ShowToast($"enableSei error code:{ret}");
            }
        }

        public void OnSendSEI()
        {
            if (!GameObject.Find("SEI/Switch").GetComponent<Toggle>().isOn)
            {
                ExampleUtils.ShowToast("请先开启SEI");
                return;
            }
            if (!Sei)
            {
                string content = SeiCanvas.transform.Find("Content").GetComponent<InputField>().text;
                if (content == null || content.Length == 0)
                {
                    ExampleUtils.ShowToast("请输入内容");
                    return;
                }
                string count = SeiCanvas.transform.Find("Count").GetComponent<InputField>().text;
                if (count == null || count.Length == 0)
                {
                    ExampleUtils.ShowToast("请输入次数");
                    return;
                }
                SeiText = content;
                SeiCount = int.Parse(count);
                SeiDeltaTime = 0;
                SeiCountDown = 0;
            }
            string text = Sei ? "发送" : "取消发送";
            SeiCanvas.transform.Find("Start/Text").GetComponent<Text>().text = text;
            Sei = !Sei;
        }

        public void OnClickSwitchRole()
        {
            Engine.OnLiveRoleSwitched = delegate (RCRTCRole current, int code, string errMsg)
            {
                Engine.OnLiveRoleSwitched = null;
                RunOnMainThread.Enqueue(() =>
                {
                    ExampleUtils.HideLoading();
                    if (code != 0)
                    {
                        ExampleUtils.ShowToast($"切换失败 code:{code} msg:{errMsg}");
                    }
                    else
                    {
                        GameObject.Find("Host/MorePanel").SetActive(false);
                        if (Camera)
                        {
                            _enableCamera(HostVideoPanel.videoView);
                        }
                        SwitchRole(current);
                    }
                });
            };
            ExampleUtils.ShowLoading();
            int ret = Engine.SwitchLiveRole(RCRTCRole.LIVE_AUDIENCE);
            if (ret != 0)
            {
                ExampleUtils.HideLoading();
                Engine.OnLiveRoleSwitched = null;
                ExampleUtils.ShowToast($"切换失败 code:{ret}");
            }
        }
        #endregion

        #region 合流布局面板
        public void OnHostMixVideoBgChanged(int value)
        {
            HostMixBgConfig.SetActive(false);
            int color = 0;
            switch (value)
            {
                case 0://red
                    color = 0xDC143C;
                    break;
                case 1://green
                    color = 0x008000;
                    break;
                case 2://blue
                    color = 0x0000FF;
                    break;
                case 3://org
                    color = 0xFF4500;
                    break;
                case 4://yellow
                    color = 0xFFFF00;
                    break;
                default:
                    break;
            }
            int ret = Engine.SetLiveMixBackgroundColor(color);
            if (ret == 0)
            {
                ExampleUtils.ShowToast("设置成功");
            }
            else
            {
                ExampleUtils.ShowToast("设置失败");
            }
        }

        public void OnHostMixVideoCustomModeChanged(bool value)
        {
            if (value)
            {
                HostMixVideoCustomLayoutConfig.SetActive(true);
            }
        }

        public void OnHostMixVideoModeChanged(int value)
        {
            RCRTCLiveMixLayoutMode mode = (RCRTCLiveMixLayoutMode)value;
            Engine?.SetLiveMixLayoutMode(mode);
        }

        public void OnHostMixVideoRenderModeChanged(int value)
        {
            Engine?.SetLiveMixRenderMode((RCRTCLiveMixRenderMode)value);
        }

        public void OnClickHostVideoStreamConfigConfirm()
        {
            var config = GameObject.Find("/HostMixVideoStreamConfig/Background/VideoStreamConfig")
                .GetComponent<VideoStreamConfig>();

            Engine?.SetLiveMixVideoBitrate(config.BitRateValue, false);
            Engine?.SetLiveMixVideoFps(config.FPS, false);
            Engine?.SetLiveMixVideoResolution(GetResolutionWidth(config.Resolution),
                GetResolutionHeight(config.Resolution), false);

            HostMixVideoStreamConfig.SetActive(false);
        }

        public void OnClickHostTinyVideoStreamConfigConfirm()
        {
            var config = GameObject.Find("/HostMixTinyVideoStreamConfig/Background/TinyVideoStreamConfig")
                .GetComponent<VideoStreamConfig>();

            Engine?.SetLiveMixVideoBitrate(config.BitRateValue, true);
            Engine?.SetLiveMixVideoFps(config.FPS, true);
            Engine?.SetLiveMixVideoResolution(GetResolutionWidth(config.Resolution),
                GetResolutionHeight(config.Resolution), true);

            HostMixTinyVideoStreamConfig.SetActive(false);
        }

        public void OnClickHostVideoCustomLayoutConfig()
        {
            HostMixVideoCustomLayoutConfig.SetActive(true);
            RefreshCustomVideoLayoutList();
        }

        public void OnClickHostVideoCustomLayoutConfigConfirm()
        {
            Engine?.SetLiveMixLayoutMode(RCRTCLiveMixLayoutMode.CUSTOM);
            Engine?.SetLiveMixCustomLayouts(_videoCustomLayouts);
            HostMixVideoCustomLayoutConfig.SetActive(false);
        }

        public void OnClickHostVideoCustomLayoutAdd()
        {
            HostMixAddVideoCustomLayout.SetActive(true);

            var userOptions = GameObject
                .Find("/HostMixAddVideoCustomLayout/Background/SelectUserDropdown").GetComponent<Dropdown>();
            OnSelectUserDropdownValueChanged(0);
            userOptions.options.Clear();
            var optionDatas = new List<Dropdown.OptionData> { new Dropdown.OptionData(User.Id) };
            optionDatas.AddRange(Users.Select(item => new Dropdown.OptionData(item.Key)));

            userOptions.AddOptions(optionDatas);
            userOptions.SetValueWithoutNotify(0);
        }

        public void OnSelectUserDropdownValueChanged(int value)
        {
            InputField inputXPos = GameObject.Find("/HostMixAddVideoCustomLayout/Background/InputFieldVideoXPos")
            .GetComponent<InputField>();
            InputField inputYPos = GameObject.Find("/HostMixAddVideoCustomLayout/Background/InputFieldVideoYPos")
                .GetComponent<InputField>();
            InputField inputWidth = GameObject.Find("/HostMixAddVideoCustomLayout/Background/InputFieldVideoWidth")
                .GetComponent<InputField>();
            InputField inputHeight = GameObject.Find("/HostMixAddVideoCustomLayout/Background/InputFieldVideoHeight")
                .GetComponent<InputField>();

            inputXPos.text = "0";
            inputXPos.text = "0";
            inputWidth.text = "180";
            inputHeight.text = "320";
        }

        public void OnClickHostVideoCustomLayoutAddConfirm()
        {
            var userOptions = GameObject
                .Find("/HostMixAddVideoCustomLayout/Background/SelectUserDropdown").GetComponent<Dropdown>();
            var selectedValue = userOptions.value;
            if (userOptions.options.Count == 0)
                return;
            var selectedUserId = userOptions.options[selectedValue].text;

            var inputXPos = GameObject.Find("/HostMixAddVideoCustomLayout/Background/InputFieldVideoXPos")
                .GetComponent<InputField>().text;
            var inputYPos = GameObject.Find("/HostMixAddVideoCustomLayout/Background/InputFieldVideoYPos")
                .GetComponent<InputField>().text;
            var inputWidth = GameObject.Find("/HostMixAddVideoCustomLayout/Background/InputFieldVideoWidth")
                .GetComponent<InputField>().text;
            var inputHeight = GameObject.Find("/HostMixAddVideoCustomLayout/Background/InputFieldVideoHeight")
                .GetComponent<InputField>().text;
            if (false == Int32.TryParse(inputXPos, out var videoPosX))
            {
                return;
            }

            if (false == Int32.TryParse(inputYPos, out var videoPosY))
            {
                return;
            }

            if (false == Int32.TryParse(inputWidth, out var videoWidth))
            {
                return;
            }

            if (false == Int32.TryParse(inputHeight, out var videoHeight))
            {
                return;
            }

            _videoCustomLayouts.Add(new RCRTCCustomLayout(selectedUserId, videoPosX, videoPosY, videoWidth, videoHeight));

            HostMixAddVideoCustomLayout.SetActive(false);
            HostMixVideoCustomLayoutConfig.SetActive(true);
            RefreshCustomVideoLayoutList();
        }

        public void OnClickHostAudioConfigConfirm()
        {
            var liveMixAudioConfig = GameObject.Find("/HostMixAudioConfig/Background/LiveMixAudioBitrate")
                .GetComponent<LiveMixAudioBitrate>();
            if (liveMixAudioConfig == null)
                return;
            Engine?.SetLiveMixAudioBitrate(liveMixAudioConfig.BitRateValue);
            HostMixAudioConfig.SetActive(false);
        }

        public void OnClickHostAudioCustomConfig()
        {
            HostMixAudioCustomConfig.SetActive(true);

            var userOptions = GameObject
                .Find("/HostMixAudioCustomConfig/Background/SelectUserDropdown").GetComponent<Dropdown>();

            userOptions.options.Clear();
            var optionDatas = new List<Dropdown.OptionData>()
            {
                new Dropdown.OptionData(User.Id)
            };
            optionDatas.AddRange(Users.Select(item => new Dropdown.OptionData(item.Key)));

            userOptions.AddOptions(optionDatas);
            userOptions.SetValueWithoutNotify(0);
            RefreshCustomAudioMixUserList();
        }

        public void OnClickHostAudioCustomConfigConfirm()
        {
            var mixUserList = _audioMixUserList.Distinct().ToList();

            Engine?.SetLiveMixCustomAudios(mixUserList);

            HostMixAudioCustomConfig.SetActive(false);
        }

        public void OnClickHostLiveMixAddAudioMixUser()
        {
            var userOptions = GameObject
                .Find("/HostMixAudioCustomConfig/Background/SelectUserDropdown").GetComponent<Dropdown>();
            var selectedValue = userOptions.value;
            if (userOptions.options.Count == 0)
                return;
            var selectedUserId = userOptions.options[selectedValue].text;

            _audioMixUserList.Add(selectedUserId);

            RefreshCustomAudioMixUserList();
        }
        #endregion

        #region Audience
        public void OnClickLeaveAudience()
        {
            ExampleUtils.ShowLoading();
            Engine.OnRoomLeft = delegate (int code, String message)
            {
                Engine.OnRoomLeft = null;

                RunOnMainThread.Enqueue(() =>
                {
                    ExampleUtils.HideLoading();
                    ChangeToConnected();
                    if (code != 0)
                    {
                        ExampleUtils.ShowToast(message);
                    }
                    Engine.Destroy();
                    Engine = null;
                });
            };
            Engine.SetStatsListener(null);
            Engine.LeaveRoom();

            resetAudienceUI();
        }

        public void OnClickAudienceSubscribe()
        {
            if (AudienceIsSubscribed)
            {
                if (AudienceSubscribeType != null)
                {
                    ExampleUtils.ShowLoading();
                    Engine.OnLiveMixUnsubscribed = delegate (RCRTCMediaType type, int code, String message)
                    {
                        Engine.OnLiveMixUnsubscribed = null;
                        RunOnMainThread.Enqueue(() =>
                        {
                            ExampleUtils.HideLoading();
                            AudienceIsSubscribed = false;
                            AudienceSubscribeButton.transform.Find("Text").GetComponent<Text>().text = "订阅";

                            if (code != 0)
                            {
                                ExampleUtils.ShowToast(message);
                                return;
                            }

                            if (AudienceSubscribeType != null)
                            {
                                if (AudienceSubscribeType.Value != RCRTCMediaType.AUDIO)
                                {
                                    Engine.RemoveLiveMixView();
                                    AudienceVideoPanel.videoView.DestroySurface();
                                }

                                AudienceSubscribeType = null;
                            }
                        });
                    };

                    var ret = Engine.UnsubscribeLiveMix(AudienceSubscribeType.Value);
                    if (ret != 0)
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast("UnsubscribeLiveMix Error " + ret);
                        AudienceIsSubscribed = false;
                        AudienceSubscribeButton.transform.Find("Text").GetComponent<Text>().text = "订阅";
                    }
                }
            }
            else
            {
                ExampleUtils.ShowLoading();
                RCRTCMediaType subscribeType = AudienceUISelectedMediaType;
                Engine.OnLiveMixSubscribed = delegate (RCRTCMediaType type, int code, String message)
                {
                    Engine.OnLiveMixSubscribed = null;
                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        if (code != 0)
                        {
                            ExampleUtils.ShowToast(message);
                            return;
                        }

                        AudienceSubscribeType = subscribeType;
                        AudienceIsSubscribed = true;
                        AudienceSubscribeButton.transform.Find("Text").GetComponent<Text>().text = "取消订阅";
                        if (subscribeType != RCRTCMediaType.AUDIO)
                        {
                            Engine.SetLiveMixView(AudienceVideoPanel.videoView);
                        }
                    });
                };

                if (subscribeType != RCRTCMediaType.AUDIO)
                {
                    var isTiny = AudienceTinyStream.isOn;
                    var ret = Engine.SubscribeLiveMix(AudienceUISelectedMediaType, isTiny);
                    if (ret != 0)
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast("Subscribe Live Mix Error " + ret);
                    }
                }
                else
                {
                    var ret = Engine.SubscribeLiveMix(RCRTCMediaType.AUDIO);
                    if (ret != 0)
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast("Subscribe Live Mix Error " + ret);
                    }
                }
            }
        }

        public void OnClickAudienceSwitchAudioOutput()
        {
#if UNITY_STANDALONE_WIN
            RCRTCDevice[] list = Engine.GetSpeakerList();
            Debug.Log("获取到扬声器列表" + list.Length);
            if (list.Length > 0)
            {
                ReloadDeviceList(list, CurrentSpeakers, false, true, (device, value) => {
                    if (CurrentSpeakers.Contains(device))
                    {
                        if (!value)
                        {
                            Engine.EnableSpeaker(device, false);
                        }
                    }
                    else
                    {
                        foreach (var item in CurrentSpeakers)
                        {
                            Engine.EnableSpeaker(item, false);
                        }
                        Engine.EnableSpeaker(device, value);
                    }
                    CurrentSpeakers.Clear();
                    if (value)
                    {
                        CurrentSpeakers.Add(device);
                    }
                    /*if (value)
                    {
                        CurrentSpeakers.Add(device);
                    }
                    else
                    {
                        CurrentSpeakers.Remove(device);
                    }*/
                });
            }
            else
            {
                ExampleUtils.ShowToast("没有可用的扬声器");
            }
#else
            int ret = Engine.EnableSpeaker(!Speaker);
            if (ret == 0)
            {
                Speaker = !Speaker;
                AudienceCanvasSwitchAudioOutputInfo.text = Speaker ? "扬声器" : "听筒";
            }
#endif
        }

        public void OnClickAudienceTypeToggle(bool valueChanged)
        {
            if (AudienceVideoToggle.isOn || AudienceAudioVideoToggle.isOn)
            {
                AudienceTinyStream.isOn = false;
                AudienceTinyStream.gameObject.SetActive(true);
            }
            else
            {
                AudienceTinyStream.gameObject.SetActive(false);
            }

            if (AudienceAudioToggle.isOn)
            {
                AudienceUISelectedMediaType = RCRTCMediaType.AUDIO;
            }
            else if (AudienceVideoToggle.isOn)
            {
                AudienceUISelectedMediaType = RCRTCMediaType.VIDEO;
            }
            else if (AudienceAudioVideoToggle.isOn)
            {
                AudienceUISelectedMediaType = RCRTCMediaType.AUDIO_VIDEO;
            }
        }

        public void OnClickAudienceMuteVideo(bool value)
        {
            Engine.MuteLiveMixStream(RCRTCMediaType.VIDEO, value);
        }

        public void OnClickAudienceMuteAudio(bool value)
        {
            Engine.MuteLiveMixStream(RCRTCMediaType.AUDIO, value);
        }

        private bool InnerCdnIsSubscribed;

        public void OnClickLiveMixInnerCdnSubscribe()
        {
            ExampleUtils.ShowLoading();
            if (InnerCdnIsSubscribed)
            {
                Engine.OnLiveMixInnerCdnStreamUnsubscribed = delegate (int code, string errMsg)
                {
                    Engine.OnLiveMixInnerCdnStreamSubscribed = null;
                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        if (code == 0)
                        {
                            Engine.RemoveLiveMixInnerCdnStreamView();
                            ExampleUtils.ShowToast("取消订阅成功");
                            AudienceCanvas.transform.Find("SubscribeCDN/Text").GetComponent<Text>().text = "订阅";
                            InnerCdnIsSubscribed = false;
                        }
                        else
                        {
                            ExampleUtils.ShowToast($"{errMsg} code:{code}");
                        }
                    });
                };
                int ret = Engine.UnsubscribeLiveMixInnerCdnStream();
                if (ret != 0)
                {
                    Engine.OnLiveMixInnerCdnStreamSubscribed = null;
                    ExampleUtils.HideLoading();
                    ExampleUtils.ShowToast($"取消订阅发生错误 code:{ret}");
                }
            }
            else
            {
                Engine.OnLiveMixInnerCdnStreamSubscribed = delegate (int code, string errMsg)
                {
                    Engine.OnLiveMixInnerCdnStreamSubscribed = null;
                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        if (code == 0)
                        {
                            ExampleUtils.ShowToast("订阅成功");
                            AudienceCanvas.transform.Find("SubscribeCDN/Text").GetComponent<Text>().text = "取消订阅";
                            InnerCdnIsSubscribed = true;
                        }
                        else
                        {
                            ExampleUtils.ShowToast($"{errMsg} code:{code}");
                        }
                    });
                };
                int ret = Engine.SubscribeLiveMixInnerCdnStream();
                if (ret != 0)
                {
                    Engine.OnLiveMixInnerCdnStreamSubscribed = null;
                    ExampleUtils.HideLoading();
                    ExampleUtils.ShowToast($"订阅发生错误 code:{ret}");
                }
                else
                {
                    RCRTCView view = AudienceCanvas.transform.Find("CDNPanel").GetComponent<RCRTCView>();
                    Engine.SetLiveMixInnerCdnStreamView(view);
                }
            }
        }

        public void OnChangeLiveMixInnerCdnVideoFps(int value)
        {
            Engine.SetLocalLiveMixInnerCdnVideoFps((RCRTCVideoFps)value);
        }

        public void OnChangeLiveMixInnerCdnVideoResolution(int value)
        {
            Engine.SetLocalLiveMixInnerCdnVideoResolution(GetResolutionWidth((RCRTCVideoResolution)value), GetResolutionHeight((RCRTCVideoResolution)value));
        }

        public void OnClickLiveMixInnerCdnMuteVideo(bool value)
        {
            Engine.MuteLiveMixInnerCdnStream(value);
        }

        public void OnClickSwitchHost()
        {
            Engine.OnLiveRoleSwitched = delegate (RCRTCRole current, int code, string errMsg)
            {
                Engine.OnLiveRoleSwitched = null;
                RunOnMainThread.Enqueue(() =>
                {
                    ExampleUtils.HideLoading();
                    if (code != 0)
                    {
                        ExampleUtils.ShowToast($"切换失败 code:{code} msg:{errMsg}");
                    }
                    else
                    {
                        SwitchRole(current);
                    }
                });
            };
            ExampleUtils.ShowLoading();
            int ret = Engine.SwitchLiveRole(RCRTCRole.LIVE_BROADCASTER);
            if (ret != 0)
            {
                ExampleUtils.HideLoading();
                Engine.OnLiveRoleSwitched = null;
                ExampleUtils.ShowToast($"切换失败 code:{ret}");
            }
        }
        #endregion

        #region Message
        public void OnClickMessageAction()
        {
#if UNITY_STANDALONE_WIN
            ExampleUtils.ShowToast("Windows 暂不支持此功能");
            return;
#endif
            ExampleUtils.ShowLoading();
            if (!InChatRoom)
            {
                imEngine.OnChatRoomJoined = delegate (int code, string targetId) {
                    imEngine.OnChatRoomJoined = null;
                    if (code == (int)RCIMErrorCode.SUCCESS)
                    {
                        RunOnMainThread.Enqueue(() =>
                        {
                            InChatRoom = true;
                            MessageCanvasActionInfo.text = InChatRoom ? "离开聊天室" : "加入聊天室";
                            ExampleUtils.HideLoading();
                        });
                    }
                    else
                    {
                        RunOnMainThread.Enqueue(() =>
                        {
                            ExampleUtils.HideLoading();
                            ExampleUtils.ShowToast("IM Join Chat Room Error, Code = " + code);
                        });
                    }
                };
                imEngine?.JoinChatRoom(RoomId, 0, true);
            }
            else
            {
                imEngine.OnChatRoomLeft = delegate (int code, string targetId) {
                    imEngine.OnChatRoomLeft = null;
                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        InChatRoom = false;
                        MessageCanvasActionInfo.text = "加入聊天室";
                    });
                };
                imEngine?.LeaveChatRoom(RoomId);
            }
        }

        public void OnClickMessageClose()
        {
            foreach (GameObject message in Messages)
            {
                Destroy(message);
            }
            Messages.Clear();

            if (!InChatRoom)
            {
                MessageCanvas.SetActive(false);
            }
            else
            {
                ExampleUtils.ShowLoading();
                CloseMessagePanel = true;
                imEngine.OnChatRoomLeft = delegate (int code, string targetId) {
                    imEngine.OnChatRoomLeft = null;
                    if (code == (int)RCIMErrorCode.SUCCESS)
                    {
                        RunOnMainThread.Enqueue(() =>
                        {
                            InChatRoom = false;
                            MessageCanvasActionInfo.text = InChatRoom ? "离开聊天室" : "加入聊天室";
                            if (CloseMessagePanel)
                            {
                                MessageCanvas.SetActive(false);
                            }
                            CloseMessagePanel = false;
                            ExampleUtils.HideLoading();
                        });
                    }
                    else
                    {
                        RunOnMainThread.Enqueue(() =>
                        {
                            CloseMessagePanel = false;
                            ExampleUtils.HideLoading();
                            ExampleUtils.ShowToast("IM Left Chat Room Error, Code = " + code);
                        });
                    }
                };
                imEngine?.LeaveChatRoom(RoomId);
            }
        }

        public void OnClickMessageSend()
        {
            if (!InChatRoom)
            {
                ExampleUtils.ShowToast("请先加入聊天室");
                return;
            }
            String content = MessageCanvasMessageInput.text;
            if (String.IsNullOrEmpty(content))
            {
                ExampleUtils.ShowToast("请先输入消息");
                return;
            }
            var textMsg = imEngine?.CreateTextMessage(RCIMConversationType.CHATROOM, RoomId, "", $"{User.Name}, {content}");
            MessageCanvasMessageInput.text = "";
            imEngine?.SendMessage(textMsg);
        }
        #endregion

        #region AudioEffect
        public void OnClickAudioEffectClose()
        {
            int ret = Engine.StopAllAudioEffects();
            if (ret != 0)
            {
                ExampleUtils.ShowToast("Stop All Audio Effects Error " + ret);
            }

            var ids = AudioEffects.Keys;
            foreach (int id in ids)
            {
                ret = Engine.ReleaseAudioEffect(id);
                if (ret != 0)
                {
                    ExampleUtils.ShowToast("Release Audio Effect " + id + " Error " + ret);
                }
            }

            var effects = AudioEffects.Values;
            foreach (GameObject effect in effects)
            {
                Destroy(effect);
                // DestroyImmediate(effect);
            }
            AudioEffects.Clear();

            AudioEffectCanvas.SetActive(false);
        }

        private void OnClickAudioEffectPlay(GameObject item, int id)
        {
            int volume = (int)item.transform.Find("Volume").GetComponent<Slider>().value;
            int count = (int)item.transform.Find("Count").GetComponent<Slider>().value;
            int ret = Engine.PlayAudioEffect(id, volume, count);
            if (ret != 0)
            {
                ExampleUtils.ShowToast("Play Audio Effect " + id + " Error " + ret);
            }
        }

        private void OnClickAudioEffectStop(int id)
        {
            int ret = Engine.StopAudioEffect(id);
            if (ret != 0)
            {
                ExampleUtils.ShowToast("Stop Audio Effect " + id + " Error " + ret);
            }
        }
        #endregion

        #region AudioMix
        public void OnClickAudioMixOpen()
        {
#if UNITY_STANDALONE_WIN
            ExampleUtils.ShowToast("Windows 暂不支持此功能");
            return;
#endif
            AudioMixCanvas.SetActive(true);
        }

        public void OnClickAudioMixClose()
        {
            OnClickAudioMixStop();
            AudioMixCanvas.SetActive(false);
        }

        public void OnAudioMixModeNoneSelectStateChanged(bool selected)
        {
            if (selected)
            {
                AudioMixingMode = RCRTCAudioMixingMode.NONE;
            }
        }

        public void OnAudioMixModeMixSelectStateChanged(bool selected)
        {
            if (selected)
            {
                AudioMixingMode = RCRTCAudioMixingMode.MIX;
            }
        }

        public void OnAudioMixModeReplaceSelectStateChanged(bool selected)
        {
            if (selected)
            {
                AudioMixingMode = RCRTCAudioMixingMode.REPLACE;
            }
        }

        public void OnClickAudioMixPlay()
        {
            int mixingVolume = (int)AudioMixCanvasMixVolume.value;
            int publishVolume = (int)AudioMixCanvasPublishVolume.value;
            bool playback = AudioMixCanvasPlayback.isOn;
            int playbackVolume = (int)AudioMixCanvasPlaybackVolume.value;
#if UNITY_ANDROID
            String path = "file:///android_asset/" + AudioMixFilePath;
#else
            String path = "file://" + Path.Combine(Application.streamingAssetsPath, AudioMixFilePath);
#endif
            int ret = Engine.StartAudioMixing(path, AudioMixingMode, playback, 1);
            if (ret != 0)
            {
                ExampleUtils.ShowToast("Start Audio Mixing Error " + ret);
            }
            ret = Engine.AdjustAudioMixingVolume(mixingVolume);
            if (ret != 0)
            {
                ExampleUtils.ShowToast("Adjust Audio Mixing Volume Error " + ret);
            }
            ret = Engine.AdjustAudioMixingPublishVolume(publishVolume);
            if (ret != 0)
            {
                ExampleUtils.ShowToast("Adjust Audio Mixing Publish Volume Error " + ret);
            }
            ret = Engine.AdjustAudioMixingPlaybackVolume(playbackVolume);
            if (ret != 0)
            {
                ExampleUtils.ShowToast("Adjust Audio Mixing Playback Volume Error " + ret);
            }
        }

        public void OnClickAudioMixStop()
        {
            int ret = Engine.StopAudioMixing();
            if (ret != 0)
            {
                ExampleUtils.ShowToast("Stop Audio Mixing Error " + ret);
            }
        }
        #endregion

        #region 通用
        public void OnClickSwitchCamera()
        {
#if UNITY_STANDALONE_WIN
            if (CurrentCamera != null)
            {
                RCRTCDevice[] list = Engine.GetCameraList();
                Debug.Log("获取到摄像头列表" + list.Length);
                if (list.Length > 1)
                {
                    List<RCRTCDevice> devices = new List<RCRTCDevice>
                    {
                        CurrentCamera
                    };
                    ReloadDeviceList(list, devices, true, false, (device, value) => {
                        int ret = Engine.SwitchCamera(device);
                        CurrentCamera = device;
                        Debug.Log("切换摄像头结果" + ret);
                    });
                }
                else
                {
                    ExampleUtils.ShowToast("没有可切换的摄像头");
                }
            }
            else
            {
                ExampleUtils.ShowToast("还没开启摄像头");
            }
#else
            Engine.SwitchCamera();
#endif
        }
        public void OnCameraOrientationChanged(int value)
        {
#if UNITY_STANDALONE_WIN
            ExampleUtils.ShowToast("Windows暂不支持");
#else
            switch (value)
            {
                case 0:
                    Engine.SetCameraCaptureOrientation(RCRTCCameraCaptureOrientation.PORTRAIT);
                    break;
                case 1:
                    Engine.SetCameraCaptureOrientation(RCRTCCameraCaptureOrientation.PORTRAIT_UPSIDE_DOWN);
                    break;
                case 2:
                    Engine.SetCameraCaptureOrientation(RCRTCCameraCaptureOrientation.LANDSCAPE_LEFT);
                    break;
                case 3:
                    Engine.SetCameraCaptureOrientation(RCRTCCameraCaptureOrientation.LANDSCAPE_RIGHT);
                    break;
                default:
                    break;
            }
#endif
        }
        #endregion

        #endregion

        #region EventAction
        private void OnVideoConfigChanged(RCRTCVideoConfig config)
        {
            Engine.SetVideoConfig(config);
        }

        private void OnTinyVideoConfigChanged(RCRTCVideoConfig config)
        {
            Engine.SetVideoConfig(config, true);
        }

        private void OnCustomStreamVideoConfigChanged(RCRTCVideoConfig config)
        {
            Engine.SetCustomStreamVideoConfig(StreamTag(), config);
        }

        private void OnClickSubscribeAudioSwitch(String id, Button audio, bool custom)
        {
            ExampleUtils.ShowLoading();
            bool selected;
            UserSubscribeAudioStates.TryGetValue(id, out selected);
            if (!selected)
            {
                if (custom)
                {
                    Engine.OnCustomStreamSubscribed = delegate (string userId, string tag, RCRTCMediaType type, int code,
                                                          string errMsg)
                    {
                        Engine.OnCustomStreamSubscribed = null;
                        RunOnMainThread.Enqueue(() =>
                        {
                            ExampleUtils.HideLoading();
                            if (code != 0)
                            {
                                ExampleUtils.ShowToast(errMsg);
                            }
                            else
                            {
                                UserSubscribeAudioStates[$"{userId}#{tag}"] = true;
                                audio.transform.Find("Text").GetComponent<Text>().text = "取消订阅音频";
                            }
                        });
                    };
                    string[] array = id.Split('#');
                    int ret = Engine.SubscribeCustomStream(array[0], array[1], RCRTCMediaType.AUDIO, false);
                    if (ret != 0)
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast($"订阅失败 code:{ret}");
                    }
                }
                else
                {
                    Engine.OnSubscribed = delegate (String remoteUserId, RCRTCMediaType type, int code, String message)
                    {
                        Engine.OnSubscribed = null;

                        RunOnMainThread.Enqueue(() =>
                        {
                            ExampleUtils.HideLoading();
                            if (code != 0)
                            {
                                ExampleUtils.ShowToast(message);
                            }
                            else
                            {
                                UserSubscribeAudioStates[remoteUserId] = true;
                                audio.transform.Find("Text").GetComponent<Text>().text = "取消订阅音频";
                            }
                        });
                    };
                    int ret = Engine.Subscribe(id, RCRTCMediaType.AUDIO);
                    if (ret != 0)
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast("Error" + ret);
                    }
                }
            }
            else
            {
                if (custom)
                {
                    Engine.OnCustomStreamUnsubscribed = delegate (string userId, string tag, RCRTCMediaType type, int code,
                                                            string errMsg)
                    {
                        Engine.OnCustomStreamUnsubscribed = null;
                        RunOnMainThread.Enqueue(() =>
                        {
                            ExampleUtils.HideLoading();
                            if (code != 0)
                            {
                                ExampleUtils.ShowToast(errMsg);
                            }
                            else
                            {
                                UserSubscribeAudioStates[$"{userId}#{tag}"] = false;
                                audio.transform.Find("Text").GetComponent<Text>().text = "订阅音频";
                            }
                        });
                    };
                    string[] array = id.Split('#');
                    int ret = Engine.UnsubscribeCustomStream(array[0], array[1], RCRTCMediaType.AUDIO);
                    if (ret != 0)
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast($"取消订阅失败 code:{ret}");
                    }
                }
                else
                {
                    Engine.OnUnsubscribed = delegate (String remoteUserId, RCRTCMediaType type, int code, String message)
                    {
                        Engine.OnUnsubscribed = null;

                        RunOnMainThread.Enqueue(() =>
                        {
                            ExampleUtils.HideLoading();
                            if (code != 0)
                            {
                                ExampleUtils.ShowToast(message);
                            }
                            else
                            {
                                UserSubscribeAudioStates[remoteUserId] = false;
                                audio.transform.Find("Text").GetComponent<Text>().text = "订阅音频";
                            }
                        });
                    };
                    int ret = Engine.Unsubscribe(id, RCRTCMediaType.AUDIO);
                    if (ret != 0)
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast("Error" + ret);
                    }
                }
            }
        }

        private void OnClickSubscribeVideoSwitch(String id, Button video, GameObject userItem, bool custom)
        {
            ExampleUtils.ShowLoading();
            bool selected;
            UserSubscribeVideoStates.TryGetValue(id, out selected);
            if (!selected)
            {
                bool isTiny = userItem.transform.Find("Panel/TinyStream").GetComponent<Toggle>().isOn;
                bool isMirror = userItem.transform.Find("Panel/Mirror").GetComponent<Toggle>().isOn;
                int option = userItem.transform.Find("Panel/DisplayOption").GetComponent<Dropdown>().value;
                RCRTCViewFitType fitType = getViewFitType(option);
                if (custom)
                {
                    Engine.OnCustomStreamSubscribed = delegate (string userId, string tag, RCRTCMediaType type, int code,
                                                          string errMsg)
                    {
                        Engine.OnCustomStreamSubscribed = null;
                        RunOnMainThread.Enqueue(() =>
                        {
                            ExampleUtils.HideLoading();
                            if (code != 0)
                            {
                                ExampleUtils.ShowToast(errMsg);
                            }
                            else
                            {
                                UserSubscribeVideoStates[$"{userId}#{tag}"] = true;
                                video.transform.Find("Text").GetComponent<Text>().text = "取消订阅视频";
                                var view = userItem.transform.Find("Panel").GetComponent<RCRTCView>();
                                view.Mirror = isMirror;
                                view.FitType = fitType;
                                Engine.SetRemoteCustomStreamView(userId, tag, view);
                            }
                        });
                    };
                    string[] array = id.Split('#');
                    int ret = Engine.SubscribeCustomStream(array[0], array[1], RCRTCMediaType.VIDEO, isTiny);
                    if (ret != 0)
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast($"订阅失败 code:{ret}");
                    }
                }
                else
                {
                    Engine.OnSubscribed = delegate (String remoteUserId, RCRTCMediaType type, int code, String message)
                    {
                        Engine.OnSubscribed = null;

                        RunOnMainThread.Enqueue(() =>
                        {
                            ExampleUtils.HideLoading();
                            if (code != 0)
                            {
                                ExampleUtils.ShowToast(message);
                            }
                            else
                            {
                                UserSubscribeVideoStates[remoteUserId] = true;
                                video.transform.Find("Text").GetComponent<Text>().text = "取消订阅视频";
                                var view = userItem.transform.Find("Panel").GetComponent<RCRTCView>();
                                view.Mirror = isMirror;
                                view.FitType = fitType;
                                Engine.SetRemoteView(id, view);
                            }
                        });
                    };

                    int ret = Engine.Subscribe(id, RCRTCMediaType.VIDEO, isTiny);
                    if (ret != 0)
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast("Error" + ret);
                    }
                }
            }
            else
            {
                if (custom)
                {
                    Engine.OnCustomStreamUnsubscribed = delegate (string userId, string tag, RCRTCMediaType type, int code,
                                                          string errMsg)
                    {
                        Engine.OnCustomStreamUnsubscribed = null;

                        RunOnMainThread.Enqueue(() =>
                        {
                            ExampleUtils.HideLoading();
                            if (code != 0)
                            {
                                ExampleUtils.ShowToast(errMsg);
                            }
                            else
                            {
                                UserSubscribeVideoStates[$"{userId}#{tag}"] = false;
                                video.transform.Find("Text").GetComponent<Text>().text = "订阅视频";
                                Engine.RemoveRemoteCustomStreamView(userId, tag);
                            }
                        });
                    };
                    string[] array = id.Split('#');
                    int ret = Engine.UnsubscribeCustomStream(array[0], array[1], RCRTCMediaType.VIDEO);
                    if (ret != 0)
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast($"取消订阅失败 code:{ret}");
                    }
                }
                else
                {
                    Engine.OnUnsubscribed = delegate (String remoteUserId, RCRTCMediaType type, int code, String message)
                    {
                        Engine.OnUnsubscribed = null;

                        RunOnMainThread.Enqueue(() =>
                        {
                            ExampleUtils.HideLoading();
                            if (code != 0)
                            {
                                ExampleUtils.ShowToast(message);
                            }
                            else
                            {
                                UserSubscribeVideoStates[remoteUserId] = false;
                                video.transform.Find("Text").GetComponent<Text>().text = "订阅视频";
                                Engine.RemoveRemoteView(id);
                            }
                        });
                    };
                    int ret = Engine.Unsubscribe(id, RCRTCMediaType.VIDEO);
                    if (ret != 0)
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast("Error" + ret);
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        #region 连接页面
        private void GenerateToken()
        {
            ExampleUtils.ShowLoading();
            TimeSpan time = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            String current = Convert.ToInt64(time.TotalMilliseconds).ToString();
            String id = ExampleConfig.Prefix + current;
            String url = ExampleConfig.Host + "/token/" + id;
            String json = "{\"key\":\"" + ExampleConfig.AppKey + "\"}";
            Post(url, json, (bool error, String result) =>
            {
                ExampleUtils.HideLoading();
                if (!error)
                {
                    Debug.Log("result:" + result);
                    User = JsonUtility.FromJson<LoginData>(result);
                    User.userId = ExampleConfig.Prefix + current;
                    ConnectCanvasTokenInput.text = User.Token;
                }
                else
                {
                    ExampleUtils.ShowToast(result);
                }
            });
        }

        private void ConnectIM(string token)
        {
            if (imEngine != null)
            {
                imEngine.Disconnect(false);
                imEngine.Destroy();
                imEngine = null;
            }

            RCIMEngineOptions options = new RCIMEngineOptions();
            options.naviServer = ExampleConfig.NavServer;
            options.fileServer = ExampleConfig.FileServer;
#if UNITY_ANDROID
            options.enablePush = mPushEnabled;
            options.enableIPC = mDisableIPC;
#endif
            imEngine = RCIMEngine.Create(ExampleConfig.AppKey, options);
#if !UNITY_EDITOR
            imEngine.OnMessageSent += OnSendMessageSent;
            imEngine.OnMessageReceived += OnIMReceivedMessage;
#endif
            imEngine.OnConnected = delegate (int code, string userId) {
                User.Name = userId;
                imEngine.OnConnected = null;
                if (code == (int)RCIMErrorCode.SUCCESS)
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        SaveTokenInfo();
                        ChangeToConnected();
                    });
                }
                else
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast("IM Connect Error, Code = " + code);
                    });
                }
            };
            imEngine.Connect(token, 30);
        }

        private void StopEchoTest()
        {
            if (EchoTest)
            {
                int ret = Engine.StopEchoTest();
                if (ret == 0)
                {
                    EchoTest = false;
                    EchoTestDeltaTime = 0;
                    Text button = GameObject.Find("/EchoTest/Start/Text").GetComponent<Text>();
                    button.text = "开始检测";
                    Text countDown = GameObject.Find("/EchoTest/CountDown").GetComponent<Text>();
                    countDown.text = "";
                    Text tip = GameObject.Find("/EchoTest/Tip").GetComponent<Text>();
                    tip.text = "";
                    Engine.Destroy();
                    Engine = null;
                }
                else
                {
                    ExampleUtils.ShowToast("停止失败");
                }
            }
        }
        #endregion

        #region 模式切换
        private void InitSelectedRoomMode(RoomStreamConfigPrefab.RoomMode roomMode)
        {
            if (roomMode == RoomStreamConfigPrefab.RoomMode.MeetingRoom)
            {
                Role = RCRTCRole.MEETING_MEMBER;
                ConnectedCanvasModeGroup.transform.Find("MeetingMode").GetComponent<Toggle>().isOn = true;
            }
            else if (roomMode == RoomStreamConfigPrefab.RoomMode.HostRoom)
            {
                Role = RCRTCRole.LIVE_BROADCASTER;
                ConnectedCanvasModeGroup.transform.Find("HostMode").GetComponent<Toggle>().isOn = true;
            }
            else if (roomMode == RoomStreamConfigPrefab.RoomMode.AudienceRoom)
            {
                Role = RCRTCRole.LIVE_AUDIENCE;
                ConnectedCanvasModeGroup.transform.Find("AudienceMode").GetComponent<Toggle>().isOn = true;
            }
        }

        private void ChangeToUnconnect()
        {
            ConnectCanvas.SetActive(true);
            ConnectedCanvas.SetActive(false);
            MeetingCanvas.SetActive(false);
            HostCanvas.SetActive(false);
            AudienceCanvas.SetActive(false);

            Connected = false;
        }

        private void ChangeToConnected()
        {
            ConnectCanvas.SetActive(false);
            ConnectedCanvas.SetActive(true);
            InitSelectedRoomMode(ConnectedRoomStreamConfig.RoomType);
            MeetingCanvas.SetActive(false);
            HostCanvas.SetActive(false);
            AudienceCanvas.SetActive(false);

            Connected = true;

            var users = Users.Values;
            foreach (GameObject user in users)
            {
                Destroy(user);
                // DestroyImmediate(user);
            }
            Users.Clear();

            UserSubscribeAudioStates.Clear();
            UserSubscribeVideoStates.Clear();

            var statses = Statses.Values;
            foreach (GameObject stats in statses)
            {
                Destroy(stats);
                // DestroyImmediate(stats);
            }
            Statses.Clear();

            var cdns = CDNs.Values;
            foreach (GameObject cdn in cdns)
            {
                Destroy(cdn);
                // DestroyImmediate(cdn);
            }
            CDNs.Clear();

            foreach (GameObject cdn in SelectedCDNs)
            {
                Destroy(cdn);
                // DestroyImmediate(cdn);
            }
            SelectedCDNs.Clear();
        }

        private void ChangeToMeeting(String id)
        {
            Microphone = false;
            Camera = false;
            PublishAudio = false;

            Speaker = false;

            ConnectCanvas.SetActive(false);
            MeetingCanvas.SetActive(true);
            HostCanvas.SetActive(false);
            AudienceCanvas.SetActive(false);

            MeetingCanvasTitle.text = "会议号:" + id;
            MeetingVideoPanel.SetUserId(User.Id);

            MeetingCanvasEnableMicrophoneSwitchInfo.text = Microphone ? "关闭麦克风" : "开启麦克风";
            MeetingCanvasEnableCameraSwitchInfo.text = Camera ? "关闭摄像头" : "开启摄像头";
            MeetingCanvasPublishAudioSwitchInfo.text = PublishAudio ? "取消发送音频" : "发送音频";

            MeetingCanvasSwitchAudioOutputInfo.text = Speaker ? "扬声器" : "听筒";

#if UNITY_STANDALONE_WIN
            MeetingCanvasEnableMicrophoneSwitchInfo.text = "麦克风列表";
            MeetingCanvasSwitchAudioOutputInfo.text = "扬声器列表";
#endif

            Engine.SetStatsListener(new StatsListenerProxy((RCRTCNetworkStats stats) =>
            {
                RunOnMainThread.Enqueue(() =>
                {
                    ChangeNetworkStats(MeetingCanvasStatsTable, stats);
                });
            }, (RCRTCLocalAudioStats stats) =>
            {
                RunOnMainThread.Enqueue(() =>
                {
                    ChangeLocalAudioStats(MeetingCanvasStatsTable, stats);
                });
            }, (stats) =>
            {
                RunOnMainThread.Enqueue(() => { ChangeLocalVideoStats(MeetingCanvasStatsTable, stats); });
            }, (String userId, RCRTCRemoteAudioStats stats) =>
            {
                RunOnMainThread.Enqueue(() =>
                {
                    ChangeRemoteUserAudioStats(MeetingCanvasUserList, userId, stats);
                });
            }, (userId, stats) =>
            {
                RunOnMainThread.Enqueue(() => { ChangeRemoteUserVideoStats(MeetingCanvasUserList, userId, stats); });
            }));

#if UNITY_STANDALONE_WIN
            foreach (var item in CurrentMicrophones)
            {
                Engine.EnableMicrophone(item, false, true);
            }
            foreach (var item in CurrentSpeakers)
            {
                Engine.EnableSpeaker(item, false);
            }
            CurrentMicrophones.Clear();
            CurrentSpeakers.Clear();
#else
            Engine.EnableMicrophone(Microphone);
            Engine.EnableSpeaker(Speaker);
#endif
        }

        private void ChangeToHost(String id)
        {
            Microphone = false;
            Camera = false;
            PublishAudio = false;

            Speaker = false;

            InChatRoom = false;
            CloseMessagePanel = false;

            ConnectCanvas.SetActive(false);
            MeetingCanvas.SetActive(false);
            HostCanvas.SetActive(true);
            AudienceCanvas.SetActive(false);

            HostCanvasTitle.text = "房间号:" + id;
            HostVideoPanel.SetUserId(User.Id);

            HostCanvasEnableMicrophoneSwitchInfo.text = Microphone ? "关闭麦克风" : "开启麦克风";
            HostCanvasPublishAudioSwitchInfo.text = PublishAudio ? "取消发送音频" : "发送音频";

            HostCanvasSwitchAudioOutputInfo.text = Speaker ? "扬声器" : "听筒";

#if UNITY_STANDALONE_WIN
            HostCanvasEnableMicrophoneSwitchInfo.text = "麦克风列表";
            HostCanvasSwitchAudioOutputInfo.text = "扬声器列表";
#endif

            Engine.SetStatsListener(new StatsListenerProxy((RCRTCNetworkStats stats) =>
            {
                RunOnMainThread.Enqueue(() =>
                {
                    ChangeNetworkStats(HostCanvasStatsTable, stats);
                });
            }, (RCRTCLocalAudioStats stats) =>
            {
                RunOnMainThread.Enqueue(() =>
                {
                    ChangeLocalAudioStats(HostCanvasStatsTable, stats);
                });
            }, (stats) =>
            {
                RunOnMainThread.Enqueue(() => { ChangeLocalVideoStats(HostCanvasStatsTable, stats); });
            }, (String userId, RCRTCRemoteAudioStats stats) =>
            {
                RunOnMainThread.Enqueue(() =>
                {
                    ChangeRemoteUserAudioStats(HostCanvasUserList, userId, stats);
                });
            }, (userId, stats) =>
            {
                RunOnMainThread.Enqueue(() => { ChangeRemoteUserVideoStats(HostCanvasUserList, userId, stats); });
            }));

#if UNITY_STANDALONE_WIN
            foreach (var item in CurrentMicrophones)
            {
                Engine.EnableMicrophone(item, false, true);
            }
            foreach (var item in CurrentSpeakers)
            {
                Engine.EnableSpeaker(item, false);
            }
            CurrentMicrophones.Clear();
            CurrentSpeakers.Clear();
#else
            Engine.EnableMicrophone(Microphone);
            Engine.EnableSpeaker(Speaker);
#endif
        }

        private void ChangeToAudience(String id)
        {
            Speaker = false;

            InChatRoom = false;
            CloseMessagePanel = false;
            AudienceIsSubscribed = false;
            InnerCdnIsSubscribed = false;

            ConnectCanvas.SetActive(false);
            MeetingCanvas.SetActive(false);
            HostCanvas.SetActive(false);
            AudienceCanvas.SetActive(true);
            AudienceSubscribeButton.transform.Find("Text").GetComponent<Text>().text = "订阅";

            AudienceCanvasTitle.text = "房间号:" + id;

            AudienceCanvasSwitchAudioOutputInfo.text = Speaker ? "扬声器" : "听筒";

#if UNITY_STANDALONE_WIN
            AudienceCanvasSwitchAudioOutputInfo.text = "扬声器列表";
#endif

            Engine.SetStatsListener(new LiveStatsListenerProxy((RCRTCNetworkStats stats) =>
            {
                RunOnMainThread.Enqueue(() =>
                {
                    ChangeNetworkStats(AudienceCanvasStatsTable, stats);
                });
            }, (RCRTCRemoteAudioStats stats) =>
            {
                RunOnMainThread.Enqueue(() =>
                {
                    ChangeRemoteAudioStats(AudienceCanvasStatsTable, stats);
                });
            }, (stats) =>
            {
                RunOnMainThread.Enqueue(() => { ChangeRemoteVideoStats(AudienceCanvasStatsTable, stats); });
            }));

#if UNITY_STANDALONE_WIN
            foreach (var item in CurrentSpeakers)
            {
                Engine.EnableSpeaker(item, false);
            }
            CurrentSpeakers.Clear();
#else
            Engine.EnableSpeaker(Speaker);
#endif
        }

        private void resetAudienceUI()
        {
            AudienceUISelectedMediaType = RCRTCMediaType.AUDIO;
            AudienceAudioToggle.isOn = true;
            AudienceVideoToggle.isOn = false;
            AudienceAudioVideoToggle.isOn = false;
            AudienceTinyStream.isOn = false;
            AudienceTinyStream.gameObject.SetActive(false);
            AudienceCanvas.transform.Find("SubscribeCDN/Text").GetComponent<Text>().text = "订阅";
            AudienceCanvas.transform.Find("MuteCDN").GetComponent<Toggle>().isOn = false;
            GameObject.Find("Audience/SEI").GetComponent<Text>().text = "SEI:";
        }

        private void resetMeetingUI()
        {
            Microphone = false;
            MeetingCanvasEnableMicrophoneSwitchInfo.text = Microphone ? "关闭麦克风" : "开启麦克风";

            PublishAudio = false;
            MeetingCanvasPublishAudioSwitchInfo.text = PublishAudio ? "取消发布音频" : "发布音频";

            Speaker = false;
            MeetingCanvasSwitchAudioOutputInfo.text = Speaker ? "扬声器" : "听筒";

            Camera = false;
            MeetingCanvasEnableCameraSwitchInfo.text = Camera ? "关闭摄像头" : "开启摄像头";

            PublishVideo = false;
            MeetingPublishVideoText.text = PublishVideo ? "取消发布视频" : "发布视频";

#if UNITY_STANDALONE_WIN
            MeetingCanvasEnableMicrophoneSwitchInfo.text = "麦克风列表";
            MeetingCanvasSwitchAudioOutputInfo.text = "扬声器列表";
#endif
        }

        private void resetHostUI()
        {
            Microphone = false;
            HostCanvasEnableMicrophoneSwitchInfo.text = Microphone ? "关闭麦克风" : "开启麦克风";

            PublishAudio = false;
            HostCanvasPublishAudioSwitchInfo.text = PublishAudio ? "取消发布音频" : "发布音频";

            Speaker = false;
            HostCanvasSwitchAudioOutputInfo.text = Speaker ? "扬声器" : "听筒";

            Camera = false;
            HostCanvasEnableCameraSwitchInfo.text = Camera ? "关闭摄像头" : "开启摄像头";

            PublishVideo = false;
            HostPublishVideoText.text = PublishVideo ? "取消发布视频" : "发布视频";

            Sei = false;
            SeiDeltaTime = 0;
            SeiCountDown = 0;
            SeiCanvas.transform.Find("Switch").GetComponent<Toggle>().isOn = false;
            SeiCanvas.transform.Find("Start/Text").GetComponent<Text>().text = "发送";

            filePath = null;
            GameObject.Find("/Host/CustomStream/Text").GetComponent<Text>().text = "已选文件：";

            JoinedSubRooms.Clear();
            BandedSubRooms.Clear();
            JoinableSubRooms.Clear();
            ReloadSubRoomList();
#if UNITY_STANDALONE_WIN
            HostCanvasEnableMicrophoneSwitchInfo.text = "麦克风列表";
            HostCanvasSwitchAudioOutputInfo.text = "扬声器列表";
#endif
        }

        private void SwitchRole(RCRTCRole role)
        {
            Engine.SetStatsListener(null);
            var statses = Statses.Values;
            foreach (GameObject stats in statses)
            {
                Destroy(stats);
            }
            Statses.Clear();

            var cdns = CDNs.Values;
            foreach (GameObject cdn in cdns)
            {
                Destroy(cdn);
            }
            CDNs.Clear();

            foreach (GameObject cdn in SelectedCDNs)
            {
                Destroy(cdn);
            }
            SelectedCDNs.Clear();
            switch (role)
            {
                case RCRTCRole.MEETING_MEMBER:
                    Engine.OnUserJoined = MeetingOnUserJoined;
                    Engine.OnUserOffline = MeetingOnUserOffline;
                    Engine.OnUserLeft = MeetingOnUserLeft;

                    Engine.OnRemotePublished = MeetingOnRemotePublished;
                    Engine.OnRemoteUnpublished = MeetingOnRemoteUnpublished;
                    break;
                case RCRTCRole.LIVE_BROADCASTER:
                    Engine.OnSubRoomBanded = OnSubRoomBanded;
                    Engine.OnSubRoomDisband = OnSubRoomDisband;
                    Engine.OnJoinSubRoomRequestReceived = OnJoinSubRoomRequestReceived;
                    Engine.OnJoinSubRoomRequestResponseReceived = OnJoinSubRoomRequestResponseReceived;
                    Engine.OnRemoteCustomStreamPublished = OnRemoteCustomStreamPublished;
                    Engine.OnRemoteCustomStreamUnpublished = OnRemoteCustomStreamUnpublished;
                    Engine.OnSeiReceived = OnSeiReceived;
                    Engine.OnRemoteLiveRoleSwitched = OnRemoteLiveRoleSwitched;
                    ChangeToHost(RoomId);
                    resetHostUI();
                    break;
                case RCRTCRole.LIVE_AUDIENCE:
                    Engine.OnRemoteLiveMixPublished = AudienceOnRemoteLiveMixPublished;
                    Engine.OnRemoteLiveMixUnpublished = AudienceOnRemoteLiveMixUnpublished;
                    Engine.OnLiveMixSeiReceived = OnLiveMixSeiReceived;
                    ReloadHostUserList();
                    ChangeToAudience(RoomId);
                    resetAudienceUI();
                    break;
            }
        }
        #endregion

        #region 网络状态
        private void RemoveVideoStats(GameObject table)
        {
            var video = table.transform.Find("Video").gameObject;
            if (video != null)
            {
                Destroy(video);
                Statses.Remove("Video");
            }
        }

        private void RemoveAudioStats(GameObject table)
        {
            var audio = table.transform.Find("Audio").gameObject;
            if (audio != null)
            {
                Destroy(audio);
                Statses.Remove("Audio");
            }
        }
        private void ChangeNetworkStats(GameObject table, RCRTCNetworkStats stats)
        {
            Transform network = table.transform.Find("Network");
            if (network == null)
            {
                GameObject item = GameObject.Instantiate(NetworkStatsPrefab, table.transform) as GameObject;
                item.name = "Network";
                if (Statses.Values.Count > 0)
                {
                    RectTransform last = table.transform.GetChild(Statses.Values.Count - 1).gameObject.GetComponent<RectTransform>();
                    var y = last.localPosition.y - last.rect.height - 10;
                    RectTransform current = item.GetComponent<RectTransform>();
                    current.localPosition = new Vector3(last.localPosition.x, y, last.localPosition.z);
                    RectTransform rect = table.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, current.rect.height - y);
                }
                if (Statses.ContainsKey("Network"))
                {
                    Statses.Remove("Network");
                }
                Statses.Add("Network", item);
                network = item.transform;
            }
            network.Find("Type").GetComponent<Text>().text = "网络类型：" + stats.Type;
            network.Find("Ip").GetComponent<Text>().text = "IP：" + stats.Ip;
            network.Find("Send").GetComponent<Text>().text = "上行：" + stats.SendBitrate + "kbps";
            network.Find("Receive").GetComponent<Text>().text = "下行：" + stats.ReceiveBitrate + "kbps";
            network.Find("Rtt").GetComponent<Text>().text = "往返：" + stats.Rtt + "ms";
        }

        private void ChangeLocalAudioStats(GameObject table, RCRTCLocalAudioStats stats)
        {
            Transform audio;
            if (!Statses.ContainsKey("Audio"))
            {
                GameObject item = GameObject.Instantiate(AudioStatsPrefab, table.transform) as GameObject;
                item.name = "Audio";
                if (Statses.Values.Count > 0)
                {
                    RectTransform last = table.transform.GetChild(Statses.Values.Count - 1).gameObject.GetComponent<RectTransform>();
                    var y = last.localPosition.y - last.rect.height - 10;
                    RectTransform current = item.GetComponent<RectTransform>();
                    current.localPosition = new Vector3(last.localPosition.x, y, last.localPosition.z);
                    RectTransform rect = table.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, current.rect.height - y);
                }
                Statses.Add("Audio", item);
                audio = item.transform;
            }
            else
            {
                audio = table.transform.Find("Audio");
            }
            audio.Find("Bitrate").GetComponent<Text>().text = "码率：" + stats.Bitrate + "kbps";
            audio.Find("PackageLostRate").GetComponent<Text>().text = "丢包率：" + stats.PackageLostRate + "%";
        }

        private void ChangeLocalVideoStats(GameObject table, RCRTCLocalVideoStats stats)
        {
            Transform video;
            if (!Statses.ContainsKey("Video"))
            {
                GameObject item = GameObject.Instantiate(VideoStats, table.transform) as GameObject;
                VideoStatsPrefab videoStatsPrefab = item.GetComponent<VideoStatsPrefab>();
                videoStatsPrefab.DontShowTiny = !ConnectedRoomStreamConfig.TinyEnabled;
                item.name = "Video";
                if (Statses.Values.Count > 0)
                {
                    RectTransform last = table.transform.GetChild(Statses.Values.Count - 1).gameObject.GetComponent<RectTransform>();
                    var y = last.localPosition.y - last.rect.height - 10;
                    RectTransform current = item.GetComponent<RectTransform>();
                    current.localPosition = new Vector3(last.localPosition.x, y, last.localPosition.z);
                    RectTransform rect = table.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, current.rect.height - y);
                }
                Statses.Add("Video", item);
                video = item.transform;
            }
            else
            {
                video = table.transform.Find("Video");
            }

            if (stats.Tiny)
            {
                video.Find("TinyBitrate").GetComponent<Text>().text = stats.Bitrate + "kbps";
                video.Find("TinyResolution").GetComponent<Text>().text = stats.Width + "X" + stats.Height;
                video.Find("TinyFps").GetComponent<Text>().text = stats.Fps + "fps";
                video.Find("TinyPackageLostRate").GetComponent<Text>().text = "丢包率：" + stats.PackageLostRate + "%";
            }
            else
            {
                video.Find("Bitrate").GetComponent<Text>().text = stats.Bitrate + "kbps";
                video.Find("Resolution").GetComponent<Text>().text = stats.Width + "X" + stats.Height;
                video.Find("Fps").GetComponent<Text>().text = stats.Fps + "fps";
                video.Find("PackageLostRate").GetComponent<Text>().text = "丢包率：" + stats.PackageLostRate + "%";
            }
        }

        private void ChangeRemoteAudioStats(GameObject table, RCRTCRemoteAudioStats stats)
        {
            Transform audio;
            if (!Statses.ContainsKey("Audio"))
            {
                GameObject item = GameObject.Instantiate(AudioStatsPrefab, table.transform) as GameObject;
                item.name = "Audio";
                if (Statses.Values.Count > 0)
                {
                    RectTransform last = table.transform.GetChild(Statses.Values.Count - 1).gameObject.GetComponent<RectTransform>();
                    var y = last.localPosition.y - last.rect.height - 10;
                    RectTransform current = item.GetComponent<RectTransform>();
                    current.localPosition = new Vector3(last.localPosition.x, y, last.localPosition.z);
                    RectTransform rect = table.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, current.rect.height - y);
                }
                Statses.Add("Audio", item);
                audio = item.transform;
            }
            else
            {
                audio = table.transform.Find("Audio");
            }
            audio.Find("Bitrate").GetComponent<Text>().text = "码率：" + stats.Bitrate + "kbps";
            audio.Find("PackageLostRate").GetComponent<Text>().text = "丢包率：" + stats.PackageLostRate + "%";
        }

        private void ChangeRemoteVideoStats(GameObject table, RCRTCRemoteVideoStats stats)
        {
            Transform video;
            if (!Statses.ContainsKey("Video"))
            {
                GameObject item = GameObject.Instantiate(VideoStats, table.transform) as GameObject;
                item.name = "Video";
                if (Statses.Values.Count > 0)
                {
                    RectTransform last = table.transform.GetChild(Statses.Values.Count - 1).gameObject.GetComponent<RectTransform>();
                    var y = last.localPosition.y - last.rect.height - 10;
                    RectTransform current = item.GetComponent<RectTransform>();
                    current.localPosition = new Vector3(last.localPosition.x, y, last.localPosition.z);
                    RectTransform rect = table.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, current.rect.height - y);
                }
                Statses.Add("Video", item);
                video = item.transform;
                var videoStats = item.GetComponent<VideoStatsPrefab>();
                videoStats.DontShowTiny = true;
            }
            else
            {
                video = table.transform.Find("Video");
            }

            video.Find("Bitrate").GetComponent<Text>().text = stats.Bitrate + "kbps";
            video.Find("Resolution").GetComponent<Text>().text = stats.Width + "X" + stats.Height;
            video.Find("Fps").GetComponent<Text>().text = stats.Fps + "fps";
            video.Find("PackageLostRate").GetComponent<Text>().text = "丢包率：" + stats.PackageLostRate + "%";
        }
        #endregion

        #region CDN
        private void LoadCDNs()
        {
            ExampleUtils.ShowLoading();
            String url = ExampleConfig.Host + "/cdns";
            Get(url, (bool error, String result) =>
            {
                if (!error)
                {
                    Dictionary<String, String> cdns = JsonConvert.DeserializeObject<Dictionary<String, String>>(result);
                    foreach (String key in cdns.Keys)
                    {
                        String value;
                        if (cdns.TryGetValue(key, out value))
                        {
                            CDN cdn = new CDN(key, value);
                            AddCDN(cdn);
                        }
                    }
                    ConfigCDNCanvasDialog.SetActive(true);
                }
                else
                {
                    ExampleUtils.ShowToast("Load CDN List Error: " + result);
                }
                ExampleUtils.HideLoading();
            });
        }

        private void AddCDN(CDN cdn)
        {
            GameObject item = GameObject.Instantiate(CDNSelectorPrefab, ConfigCDNCanvasDialogCDNList.transform) as GameObject;
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                LoadCND(cdn.id);
            });
            item.transform.Find("Text").GetComponent<Text>().text = cdn.name;
            RectTransform rect = ConfigCDNCanvasDialogCDNList.GetComponent<RectTransform>();
            RectTransform current = item.GetComponent<RectTransform>();
            if (CDNs.Values.Count > 0)
            {
                RectTransform last = ConfigCDNCanvasDialogCDNList.transform.GetChild(CDNs.Values.Count - 1).gameObject.GetComponent<RectTransform>();
                item.GetComponent<RectTransform>().localPosition = new Vector3(last.localPosition.x, last.localPosition.y - last.rect.height - 10, last.localPosition.z);
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.rect.height + current.rect.height + 10);
            }
            else
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, current.rect.height);
            }
            CDNs.Add(cdn, item);
        }

        private void LoadCND(String id)
        {
            String sessionId = Engine.GetSessionId();
            if (String.IsNullOrEmpty(sessionId))
            {
                ExampleUtils.ShowToast("Get Session Id Null, Please try later.");
                return;
            }
            ExampleUtils.ShowLoading();
            String url = ExampleConfig.Host + "/cdn/" + id + "/sealLive/" + sessionId;
            Get(url, (bool error, String result) =>
            {
                if (!error)
                {
                    Debug.Log("CDN = " + result);

                    CDNInfo cdn = JsonUtility.FromJson<CDNInfo>(result);

                    Engine.OnLiveCdnAdded = delegate (String liveUrl, int code, String message)
                    {
                        Engine.OnLiveCdnAdded = null;

                        RunOnMainThread.Enqueue(() =>
                        {
                            ExampleUtils.HideLoading();
                            if (code != 0)
                            {
                                ExampleUtils.ShowToast("Add Live Cdn Error: " + message);
                            }
                            else
                            {
                                AddCDN(cdn);
                                ConfigCDNCanvasDialog.SetActive(false);
                            };
                        });
                    };
                    int ret = Engine.AddLiveCdn(cdn.push);
                    if (ret != 0)
                    {
                        ExampleUtils.HideLoading();
                        ExampleUtils.ShowToast("Add Live Cdn Error " + ret);
                    }
                }
                else
                {
                    ExampleUtils.HideLoading();
                    ExampleUtils.ShowToast("Load CDN Url Error: " + result);
                }
            });
        }

        private void AddCDN(CDNInfo cdn)
        {
            GameObject item = GameObject.Instantiate(CDNItemPrefab, ConfigCDNCanvasCDNList.transform) as GameObject;
            item.name = cdn.push;
            item.transform.Find("Delete").GetComponent<Button>().onClick.AddListener(() =>
            {
                RemoveCDN(item);
            });
            item.transform.Find("Info").GetComponent<Text>().text = "RTMP: " + cdn.rtmp + "\nHSL: " + cdn.hls + "\nFLV: " + cdn.flv;
            RectTransform rect = ConfigCDNCanvasCDNList.GetComponent<RectTransform>();
            RectTransform current = item.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(current);
            float height = LayoutUtility.GetPreferredSize(current, 1);
            if (SelectedCDNs.Count > 0)
            {
                RectTransform last = ConfigCDNCanvasCDNList.transform.GetChild(SelectedCDNs.Count - 1).gameObject.GetComponent<RectTransform>();
                item.GetComponent<RectTransform>().localPosition = new Vector3(last.localPosition.x, last.localPosition.y - last.rect.height - 10, last.localPosition.z);
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.rect.height + height + 10);
            }
            else
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
            }
            SelectedCDNs.Add(item);
        }

        private void RemoveCDN(GameObject cdn)
        {
            ExampleUtils.ShowLoading();
            Engine.OnLiveCdnRemoved = delegate (String url, int code, String message)
            {
                Engine.OnLiveCdnRemoved = null;

                RunOnMainThread.Enqueue(() =>
                {
                    ExampleUtils.HideLoading();
                    if (code != 0)
                    {
                        ExampleUtils.ShowToast("Remove Live Cdn Error: " + message);
                    }
                    else
                    {
                        SelectedCDNs.Remove(cdn);
                        Destroy(cdn);

                        float height = 0;
                        foreach (GameObject child in SelectedCDNs)
                        {
                            RectTransform transform = child.GetComponent<RectTransform>();
                            transform.localPosition = new Vector3(transform.localPosition.x, -height, transform.localPosition.z);
                            height += transform.rect.height + 10;
                        }
                        if (height > 0)
                        {
                            RectTransform rect = ConfigCDNCanvasCDNList.GetComponent<RectTransform>();
                            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
                        }
                    }
                });
            };
            int ret = Engine.RemoveLiveCdn(cdn.name);
            if (ret != 0)
            {
                ExampleUtils.HideLoading();
                ExampleUtils.ShowToast("Remove Live Cdn Error " + ret);
            }
        }
        #endregion

        #region 自定义视频合流布局

        public GameObject CustomVideoLayoutList;
        public GameObject CustomVideoLayoutListItem;

        private readonly IList<RCRTCCustomLayout> _videoCustomLayouts = new List<RCRTCCustomLayout>();

        private IList<GameObject> _videoCustomLayoutsGameObjects = new List<GameObject>();

        private void RefreshCustomVideoLayoutList()
        {
            RunOnMainThread.Enqueue(() =>
            {
                CustomVideoLayoutList =
                    GameObject.Find("/HostMixVideoCustomLayoutConfig/Background/CustomVideoLayoutConfigList/Viewport/Content");

                var customVideoLayoutListItems =
                    (from Transform child in CustomVideoLayoutList.transform select child.gameObject).ToList();
                customVideoLayoutListItems.ForEach(Destroy);
                _videoCustomLayoutsGameObjects.Clear();

                if (_videoCustomLayouts == null || _videoCustomLayouts.Count == 0)
                    return;

                foreach (var item in _videoCustomLayouts)
                {
                    AddCustomVideoLayoutListItem(item);
                }
            });
        }

        private void AddCustomVideoLayoutListItem(RCRTCCustomLayout layout)
        {
            if (layout == null)
                return;

            GameObject item =
                GameObject.Instantiate(CustomVideoLayoutListItem, CustomVideoLayoutList.transform) as GameObject;
            item.transform.Find("LayoutInfo").GetComponent<Text>().text =
                $" User: {layout.GetUserId()} {Environment.NewLine} Position: [{layout.GetX()}, {layout.GetY()}] Width: {layout.GetWidth()} Height: {layout.GetHeight()}";
            item.transform.Find("Remove").GetComponent<Button>().onClick.AddListener(() =>
            {
                _videoCustomLayouts.Remove(layout);
                RefreshCustomVideoLayoutList();
            });

            RectTransform rect = CustomVideoLayoutList.GetComponent<RectTransform>();
            RectTransform current = item.GetComponent<RectTransform>();
            if (_videoCustomLayoutsGameObjects.Count > 0)
            {
                RectTransform last = CustomVideoLayoutList.transform.GetChild(_videoCustomLayoutsGameObjects.Count - 1)
                    .gameObject
                    .GetComponent<RectTransform>();
                item.GetComponent<RectTransform>().localPosition = new Vector3(last.localPosition.x,
                    last.localPosition.y - last.rect.height - 10, last.localPosition.z);
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.rect.height + current.rect.height + 10);
            }
            else
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, current.rect.height);
            }

            _videoCustomLayoutsGameObjects.Add(item);
        }

        #endregion

        #region 自定义音频合流

        public GameObject CustomAudioMixList;
        public GameObject CustomAudioMixListItem;

        private readonly IList<String> _audioMixUserList = new List<String>();

        private IList<GameObject> _audioMixUserListsGameObjects = new List<GameObject>();

        private void RefreshCustomAudioMixUserList()
        {
            RunOnMainThread.Enqueue(() =>
            {
                CustomAudioMixList =
                    GameObject.Find("/HostMixAudioCustomConfig/Background/CustomLiveMixAudioUserList/Viewport/Content");

                var customVideoLayoutListItems =
                    (from Transform child in CustomAudioMixList.transform select child.gameObject).ToList();
                customVideoLayoutListItems.ForEach(Destroy);
                _audioMixUserListsGameObjects.Clear();

                if (_audioMixUserList == null || _audioMixUserList.Count == 0)
                    return;

                foreach (var item in _audioMixUserList)
                {
                    AddCustomAudioMixUserListItem(item);
                }
            });
        }

        private void AddCustomAudioMixUserListItem(string audioMixUser)
        {
            if (String.IsNullOrEmpty(audioMixUser))
                return;

            GameObject item =
                GameObject.Instantiate(CustomAudioMixListItem, CustomAudioMixList.transform) as GameObject;
            item.transform.Find("LayoutInfo").GetComponent<Text>().text =
                $" User: {audioMixUser}";
            item.transform.Find("Remove").GetComponent<Button>().onClick.AddListener(() =>
            {
                _audioMixUserList.Remove(audioMixUser);
                RefreshCustomAudioMixUserList();
            });

            RectTransform rect = CustomAudioMixList.GetComponent<RectTransform>();
            RectTransform current = item.GetComponent<RectTransform>();
            if (_audioMixUserListsGameObjects.Count > 0)
            {
                RectTransform last = CustomAudioMixList.transform.GetChild(_audioMixUserListsGameObjects.Count - 1)
                    .gameObject
                    .GetComponent<RectTransform>();
                item.GetComponent<RectTransform>().localPosition = new Vector3(last.localPosition.x,
                    last.localPosition.y - last.rect.height - 10, last.localPosition.z);
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.rect.height + current.rect.height + 10);
            }
            else
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, current.rect.height);
            }

            _audioMixUserListsGameObjects.Add(item);
        }

        #endregion

        #region 跨房间连麦
        private void ResponseRequest(string roomId, string userId, bool agree)
        {
            int ret = Engine.ResponseJoinSubRoomRequest(roomId, userId, agree, true, null);
            if (ret == 0 && agree)
            {
                JoinSubRoom(roomId);
            }
            else
            {
                ExampleUtils.ShowToast($"响应加入房间请求错误 code:{ret}");
            }
        }

        private void JoinSubRoom(string roomId)
        {
            ExampleUtils.ShowLoading();
            Engine.OnSubRoomJoined = delegate (string roomId, int code, string errMsg)
            {
                Engine.OnSubRoomJoined = null;
                RunOnMainThread.Enqueue(() =>
                {
                    ExampleUtils.HideLoading();
                    if (code != 0)
                    {
                        ExampleUtils.ShowToast($"加入{roomId}子房间失败, code:{code} message:{errMsg}");
                    }
                    else
                    {
                        if (!JoinedSubRooms.Contains(roomId))
                        {
                            JoinedSubRooms.Add(roomId);
                            ReloadSubRoomList();
                        }
                        ExampleUtils.ShowToast($"加入{roomId}子房间成功");
                    }
                });
            };
            int ret = Engine.JoinSubRoom(roomId);
            if (ret != 0)
            {
                Engine.OnSubRoomJoined = null;
                ExampleUtils.HideLoading();
                ExampleUtils.ShowToast($"加入{roomId}子房间失败, code:{ret}");
            }
        }

        private void LeaveSubRoom(string roomId, bool disband)
        {
            ExampleUtils.ShowLoading();
            Engine.OnSubRoomLeft = delegate (string roomId, int code, string errMsg)
            {
                Engine.OnSubRoomLeft = null;
                RunOnMainThread.Enqueue(() =>
                {
                    ExampleUtils.HideLoading();
                    if (code != 0)
                    {
                        ExampleUtils.ShowToast($"离开{roomId}子房间失败, code:{code} message:{errMsg}");
                    }
                    else
                    {
                        if (disband)
                        {
                            if (BandedSubRooms.Contains(roomId))
                            {
                                BandedSubRooms.Remove(roomId);
                            }
                        }
                        if (JoinedSubRooms.Contains(roomId))
                        {
                            JoinedSubRooms.Remove(roomId);
                        }
                        ReloadSubRoomList();
                        ExampleUtils.ShowToast($"离开{roomId}子房间成功");
                    }
                });
            };
            int ret = Engine.LeaveSubRoom(roomId, disband);
            if (ret != 0)
            {
                Engine.OnSubRoomLeft = null;
                ExampleUtils.HideLoading();
                ExampleUtils.ShowToast($"离开{roomId}子房间失败, code:{ret}");
            }
        }

        private void ReloadSubRoomList()
        {
            JoinableSubRooms.Clear();
            BandedSubRooms.ForEach((roomId) => {
                if (!JoinedSubRooms.Contains(roomId))
                {
                    JoinableSubRooms.Add(roomId);
                }
            });
            Transform joined = SubRoomCanvas.transform.Find("List/Joined");
            Transform joinable = SubRoomCanvas.transform.Find("List/SubRooms");
            for (int i = 0; i < joined.childCount; i++)
            {
                Destroy(joined.GetChild(i).gameObject);
            }
            for (int i = 0; i < joinable.childCount; i++)
            {
                Destroy(joinable.GetChild(i).gameObject);
            }
            foreach (var item in JoinedSubRooms)
            {
                GameObject cell = Instantiate(JoinedSubRoomPrefab, joined);
                cell.transform.Find("Info").GetComponent<Text>().text = item;
                cell.transform.Find("Leave").GetComponent<Button>().onClick.AddListener(() => {
                    LeaveSubRoom(item, false);
                });
                cell.transform.Find("Destory").GetComponent<Button>().onClick.AddListener(() => {
                    LeaveSubRoom(item, true);
                });
            }
            foreach (var item in JoinableSubRooms)
            {
                GameObject cell = Instantiate(SubRoomPrefab, joinable);
                cell.transform.Find("Info").GetComponent<Text>().text = item;
                cell.transform.Find("Join").GetComponent<Button>().onClick.AddListener(() =>
                {
                    JoinSubRoom(item);
                });
            }
        }
        #endregion

        #region 自定义流
        private string StreamTag()
        {
            return User.Id.Replace(ExampleConfig.Prefix, "custom");
        }
        #endregion

        private bool CheckMicrophonePermission()
        {
#if UNITY_STANDALONE_WIN
            if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                Application.RequestUserAuthorization(UserAuthorization.Microphone);
                return false;
            }
#endif
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
                return false;
            }
#endif
            return true;
        }

        private bool CheckCameraPermission()
        {
#if UNITY_STANDALONE_WIN
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                Application.RequestUserAuthorization(UserAuthorization.WebCam);
                return false;
            }
#endif
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
                return false;
            }
#endif
            return true;
        }

        private bool? _enableCamera(RCRTCView view)
        {
            if (!CheckCameraPermission())
            {
                ExampleUtils.ShowToast("没有摄像头权限");
                return null;
            }
#if UNITY_STANDALONE_WIN
            if (CurrentCamera != null)
            {
                int ret = Engine.EnableCamera(CurrentCamera, !Camera, true);
                Debug.Log("开启摄像头结果" + ret);
                if (ret == 0)
                {
                    Camera = !Camera;
                    if (Camera)
                    {
                        view.gameObject.SetActive(true);
                        Engine.SetLocalView(view);
                    }
                    else
                    {
                        Engine.RemoveLocalView();
                        view.DestroySurface();
                        CurrentCamera = null;
                    }
                    return Camera;
                }
            }
            else
            {
                RCRTCDevice[] list = Engine.GetCameraList();
                Debug.Log("获取到摄像头列表" + list.Length);
                if (list.Length > 0)
                {
                    int ret = Engine.EnableCamera(list[0], !Camera, true);
                    Debug.Log("开启摄像头结果" + ret);
                    if (ret == 0)
                    {
                        Camera = !Camera;
                        if (Camera)
                        {
                            view.gameObject.SetActive(true);
                            Engine.SetLocalView(view);
                            CurrentCamera = list[0];
                        }
                        else
                        {
                            Engine.RemoveLocalView();
                            view.DestroySurface();
                            CurrentCamera = null;
                        }
                        return Camera;
                    }
                }
                else
                {
                    ExampleUtils.ShowToast("没有可用的摄像头");
                }
            }
#else
            int ret = Engine.EnableCamera(!Camera);
            if (ret == 0)
            {
                Camera = !Camera;
                if (Camera)
                {
                    view.gameObject.SetActive(true);
                    Engine.SetLocalView(view);
                }
                else
                {
                    Engine.RemoveLocalView();
                    view.DestroySurface();
                }

                return Camera;
            }
#endif
            return null;
        }

        private void AddUserItemToUserList(String id, GameObject list, bool custom)
        {
            if (!Users.ContainsKey(id))
            {
                GameObject item = GameObject.Instantiate(UserItemPrefab, list.transform) as GameObject;
                item.name = id;
                item.transform.Find("Panel/UserId").GetComponent<Text>().text = id;
                Button audio = item.transform.Find("SubsribeAudioSwitch").GetComponent<Button>();
                audio.onClick.AddListener(() =>
                {
                    OnClickSubscribeAudioSwitch(id, audio, custom);
                });
                Button video = item.transform.Find("SubsribeVideoSwitch").GetComponent<Button>();
                video.onClick.AddListener(() =>
                {
                    OnClickSubscribeVideoSwitch(id, video, item, custom);
                });

                Toggle mirrorToggle = item.transform.Find("Panel/Mirror").GetComponent<Toggle>();
                mirrorToggle.onValueChanged.AddListener((changed) =>
                {
                    bool isSubscribed;
                    UserSubscribeVideoStates.TryGetValue(id, out isSubscribed);
                    if (isSubscribed)
                    {
                        var view = item.transform.Find("Panel").GetComponent<RCRTCView>();
                        view.Mirror = mirrorToggle.isOn;
                    }
                });
                Dropdown displayOption = item.transform.Find("Panel/DisplayOption").GetComponent<Dropdown>();
                displayOption.onValueChanged.AddListener((value) =>
                {
                    bool isSubscribed;
                    UserSubscribeVideoStates.TryGetValue(id, out isSubscribed);
                    if (isSubscribed)
                    {
                        var view = item.transform.Find("Panel").GetComponent<RCRTCView>();
                        view.FitType = getViewFitType(value);
                    }
                });
                if (Users.Values.Count > 0)
                {
                    RectTransform last = list.transform.GetChild(Users.Values.Count - 1).gameObject.GetComponent<RectTransform>();
                    item.GetComponent<RectTransform>().localPosition = new Vector3(last.localPosition.x, last.localPosition.y - last.rect.height - 10, last.localPosition.z);
                    RectTransform rect = list.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, (Users.Values.Count + 1) * (last.rect.height + 10));
                }
                Users.Add(id, item);
                UserSubscribeAudioStates.Add(id, false);
                UserSubscribeVideoStates.Add(id, false);
            }
        }

        private void RemoveUserItemFromUserList(String id, GameObject list)
        {
            GameObject user;
            if (Users.TryGetValue(id, out user))
            {
                RectTransform position = user.GetComponent<RectTransform>();

                Destroy(user);
                Users.Remove(id);
                UserSubscribeAudioStates.Remove(id);
                UserSubscribeVideoStates.Remove(id);

                if (Users.Values.Count > 0)
                {
                    int index = 0;
                    for (int i = 0; i < list.transform.childCount; i++)
                    {
                        Transform child = list.transform.GetChild(i);
                        if (child.gameObject.name != id)
                        {
                            child.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(position.localPosition.x, (position.rect.height + 10) * index * -1, position.localPosition.z);
                            index++;
                        }
                    }
                    RectTransform rect = list.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, Users.Values.Count * (position.rect.height + 10));
                }
            }
        }

        private void ReloadHostUserList()
        {
            foreach (var item in Users)
            {
                GameObject UserItem = item.Value;
                Button video = UserItem.transform.Find("SubsribeVideoSwitch").GetComponent<Button>();
                video.transform.Find("Text").GetComponent<Text>().text = "订阅视频";
                Button audio = UserItem.transform.Find("SubsribeAudioSwitch").GetComponent<Button>();
                audio.transform.Find("Text").GetComponent<Text>().text = "订阅音频";
            }
            UserSubscribeAudioStates.Clear();
            UserSubscribeVideoStates.Clear();
        }

        private void AddMessage(String message)
        {
            GameObject item = GameObject.Instantiate(MessagePrefab, MessageCanvasMessageList.transform) as GameObject;
            item.GetComponent<Text>().text = message;
            RectTransform rect = MessageCanvasMessageList.GetComponent<RectTransform>();
            RectTransform current = item.GetComponent<RectTransform>();
            if (Messages.Count > 0)
            {
                RectTransform last = MessageCanvasMessageList.transform.GetChild(Messages.Count - 1).gameObject.GetComponent<RectTransform>();
                item.GetComponent<RectTransform>().localPosition = new Vector3(last.localPosition.x, last.localPosition.y - last.rect.height - 10, last.localPosition.z);
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.rect.height + current.rect.height + 10);
            }
            else
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, current.rect.height);
            }
            Messages.Add(item);
        }

        private void AddAudioEffect(int id)
        {
            if (!AudioEffects.ContainsKey(id))
            {
                GameObject item = GameObject.Instantiate(AudioEffectPrefab, AudioEffectCanvasEffectList.transform) as GameObject;
                item.transform.Find("Title").GetComponent<Text>().text = AudioEffectNames[id];
                Button play = item.transform.Find("Play").GetComponent<Button>();
                play.onClick.AddListener(() =>
                {
                    OnClickAudioEffectPlay(item, id);
                });
                Button stop = item.transform.Find("Stop").GetComponent<Button>();
                stop.onClick.AddListener(() =>
                {
                    OnClickAudioEffectStop(id);
                });
                if (AudioEffects.Values.Count > 0)
                {
                    RectTransform last = AudioEffectCanvasEffectList.transform.GetChild(AudioEffects.Values.Count - 1).gameObject.GetComponent<RectTransform>();
                    item.GetComponent<RectTransform>().localPosition = new Vector3(last.localPosition.x, last.localPosition.y - last.rect.height - 10, last.localPosition.z);
                    RectTransform rect = AudioEffectCanvasEffectList.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, (AudioEffects.Values.Count + 1) * (last.rect.height + 10));
                }
                AudioEffects.Add(id, item);
            }
        }

        private RCRTCViewFitType getViewFitType(int value)
        {
            if (value == 0)
            {
                return RCRTCViewFitType.CENTER;
            }
            else if (value == 1)
            {
                return RCRTCViewFitType.COVER;
            }
            else if (value == 2)
            {
                return RCRTCViewFitType.FILL;
            }

            return RCRTCViewFitType.CENTER;
        }

        private void ChangeRemoteUserPublishState(GameObject list, String id, RCRTCMediaType type, bool published)
        {
            Debug.Log($"UserId {id} MediaType {type} Published {published}");
            Transform user = list.transform.Find(id);
            if (user == null)
            {
                return;
            }

            if (type == RCRTCMediaType.AUDIO || type == RCRTCMediaType.AUDIO_VIDEO)
            {
                Button audio = user.Find("SubsribeAudioSwitch").GetComponent<Button>();
                audio.interactable = published;
                Text audioInfo = audio.transform.Find("Text").GetComponent<Text>();
                audioInfo.text = "订阅音频";
                UserSubscribeAudioStates[id] = false;
            }

            if (type == RCRTCMediaType.VIDEO || type == RCRTCMediaType.AUDIO_VIDEO)
            {
                Button video = user.Find("SubsribeVideoSwitch").GetComponent<Button>();
                video.interactable = published;
                Text videoInfo = video.transform.Find("Text").GetComponent<Text>();
                videoInfo.text = "订阅视频";
                UserSubscribeVideoStates[id] = false;
            }
        }

        private void ChangeRemoteUserAudioStats(GameObject list, String userId, RCRTCRemoteAudioStats stats)
        {
            Transform user = list.transform.Find(userId);
            if (user == null)
            {
                return;
            }
            Text audioVolume = user.Find("AudioVolume").GetComponent<Text>();
            Text audioBitrate = user.Find("AudioBitrate").GetComponent<Text>();
            Text audioPackageLostRate = user.Find("AudioPackageLostRate").GetComponent<Text>();
            audioVolume.text = "音量：" + stats.Volume;
            audioBitrate.text = "音频码率：" + stats.Bitrate + "kbps";
            audioPackageLostRate.text = "音频丢包率：" + stats.PackageLostRate + "%";
        }

        private void ChangeRemoteUserVideoStats(GameObject list, String userId, RCRTCRemoteVideoStats stats)
        {
            Transform user = list.transform.Find(userId);
            if (user == null)
            {
                return;
            }

            Text videoBitrate = user.Find("VideoBitrate").GetComponent<Text>();
            Text videoFps = user.Find("VideoFps").GetComponent<Text>();
            Text videoPackageLostRate = user.Find("VideoPackageLostRate").GetComponent<Text>();
            Text videoResolution = user.Find("VideoSize").GetComponent<Text>();

            videoBitrate.text = "视频码率：" + stats.Bitrate + "kbps";
            videoFps.text = "视频帧率：" + stats.Fps + "fps";
            videoResolution.text = "视频分辨率：" + stats.Width + "X" + stats.Height;
            videoPackageLostRate.text = "视频丢包率：" + stats.PackageLostRate + "%";
        }

        private int GetResolutionWidth(RCRTCVideoResolution resolution)
        {
            var array = resolution.ToString().Split('_');
            if (array.Length == 3)
            {
                return Int32.Parse(array[1]);
            }

            return 0;
        }

        private int GetResolutionHeight(RCRTCVideoResolution resolution)
        {
            var array = resolution.ToString().Split('_');
            if (array.Length == 3)
            {
                return Int32.Parse(array[2]);
            }

            return 0;
        }

        #endregion

        #region Net
        private void Post(String url, String json, Action<bool, String> callback)
        {
            StartCoroutine(_Post(url, json, callback));
        }

        private IEnumerator _Post(String url, String json, Action<bool, String> callback)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                request.SetRequestHeader("content-type", "application/json;charset=utf-8");
                request.downloadHandler = new DownloadHandlerBuffer();
                yield return request.SendWebRequest();

                String result = "";
                if (request.isNetworkError || request.isHttpError)
                {
                    result = request.error;
                }
                else
                {
                    result = request.downloadHandler.text;
                }

                callback?.Invoke(request.isNetworkError || request.isHttpError, result);
            }
        }

        private void Get(String url, Action<bool, String> callback)
        {
            StartCoroutine(_Get(url, callback));
        }

        private IEnumerator _Get(String url, Action<bool, String> callback)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, "GET"))
            {
                request.SetRequestHeader("content-type", "application/json;charset=utf-8");
                request.downloadHandler = new DownloadHandlerBuffer();
                yield return request.SendWebRequest();

                String result = "";
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    result = request.error;
                }
                else
                {
                    result = request.downloadHandler.text;
                }

                callback?.Invoke(result.Length == 0, result);
            }
        }
        #endregion

        #region IM Callbacks
        private void OnIMReceivedMessage(RCIMMessage message, int left, bool offline, bool hasPackage)
        {
            RunOnMainThread.Enqueue(() =>
            {
                AddMessage(String.Format("{0}: {1}", message.messageType, message.ToString()));
            });
        }

        private void OnSendMessageSent(int code, RCIMMessage message)
        {
            RunOnMainThread.Enqueue(() =>
            {
                if (code == 0)
                {
                    AddMessage(String.Format("{0} (我): {1}", message.messageType, message.ToString()));
                }
                else
                {
                    ExampleUtils.ShowToast("消息发送失败！");
                }

            });
        }
        #endregion

        #region RTC Callbacks

        private void MeetingOnUserJoined(String roomId, String userId)
        {
            RunOnMainThread.Enqueue(() =>
            {
                AddUserItemToUserList(userId, MeetingCanvasUserList, false);
            });
        }

        private void MeetingOnUserOffline(String roomId, String userId)
        {
            RunOnMainThread.Enqueue(() =>
            {
                RemoveUserItemFromUserList(userId, MeetingCanvasUserList);
            });
        }

        private void MeetingOnUserLeft(String roomId, String userId)
        {
            RunOnMainThread.Enqueue(() =>
            {
                RemoveUserItemFromUserList(userId, MeetingCanvasUserList);
            });
        }

        private void MeetingOnRemotePublished(String roomId, String userId, RCRTCMediaType type)
        {
            RunOnMainThread.Enqueue(() =>
            {
                ChangeRemoteUserPublishState(MeetingCanvasUserList, userId, type, true);
            });
        }

        private void MeetingOnRemoteUnpublished(String roomId, String userId, RCRTCMediaType type)
        {
            RunOnMainThread.Enqueue(() =>
            {
                ChangeRemoteUserPublishState(MeetingCanvasUserList, userId, type, false);
            });
        }

        private void OnUserJoined(String roomId, String userId)
        {
            RunOnMainThread.Enqueue(() =>
            {
                AddUserItemToUserList(userId, HostCanvasUserList, false);
                if (roomId != RoomId)
                {
                    //更新跨房间的用户信息
                    if (!JoinedSubRooms.Contains(roomId))
                    {
                        JoinedSubRooms.Add(roomId);
                        ReloadSubRoomList();
                    }
                }
            });
        }

        private void OnUserOffline(String roomId, String userId)
        {
            RunOnMainThread.Enqueue(() =>
            {
                RemoveUserItemFromUserList(userId, HostCanvasUserList);
            });
        }

        private void OnUserLeft(String roomId, String userId)
        {
            RunOnMainThread.Enqueue(() =>
            {
                RemoveUserItemFromUserList(userId, HostCanvasUserList);
            });
        }

        private void OnRemotePublished(String roomId, String userId, RCRTCMediaType type)
        {
            RunOnMainThread.Enqueue(() =>
            {
                ChangeRemoteUserPublishState(HostCanvasUserList, userId, type, true);
            });
        }

        private void OnRemoteUnpublished(String roomId, String userId, RCRTCMediaType type)
        {
            RunOnMainThread.Enqueue(() =>
            {
                ChangeRemoteUserPublishState(HostCanvasUserList, userId, type, false);
            });
        }

        private void AudienceOnRemoteLiveMixPublished(RCRTCMediaType type)
        {
            RunOnMainThread.Enqueue(() =>
            {
                AudienceSubscribeButton.interactable = true;
            });
        }

        private void AudienceOnRemoteLiveMixUnpublished(RCRTCMediaType type)
        {
            RunOnMainThread.Enqueue(() =>
            {
                AudienceSubscribeButton.transform.Find("Text").GetComponent<Text>().text = "订阅";
                AudienceSubscribeButton.interactable = false;
            });
        }

        private void OnJoinSubRoomRequestReceived(string roomId, string userId, string extra)
        {
            var agree = new ExampleUtils.AlertAction()
            {
                title = "同意",
                action = () => {
                    ResponseRequest(roomId, userId, true);
                },
            };
            var degree = new ExampleUtils.AlertAction()
            {
                title = "拒绝",
                action = () => {
                    ResponseRequest(roomId, userId, false);
                },
            };
            ExampleUtils.ShowAlert("收到连麦请求", $"来自房间{roomId}的{userId}邀请你一起连麦，是否同意?", new[] { agree, degree });
        }

        private void OnJoinSubRoomRequestResponseReceived(string roomId, string userId, bool agree,
                                                                      string extra)
        {
            if (agree)
            {
                ExampleUtils.ShowToast($"{roomId}的{userId}同意了你的加入申请,正在加入...");
                JoinSubRoom(roomId);
            }
            else
            {
                ExampleUtils.ShowToast($"{roomId}的{userId}拒绝了你的加入申请");
            }
        }

        private void OnSubRoomBanded(string roomId)
        {
            Debug.Log($"OnSubRoomBanded {roomId}");
            if (!BandedSubRooms.Contains(roomId))
            {
                BandedSubRooms.Add(roomId);
                ReloadSubRoomList();
            }
        }

        private void OnSubRoomDisband(string roomId, string userId)
        {
            Debug.Log($"OnSubRoomDisband {roomId}");
            if (BandedSubRooms.Contains(roomId))
            {
                BandedSubRooms.Remove(roomId);
                ReloadSubRoomList();
            }
        }

        private void OnRemoteCustomStreamPublished(string roomId, string userId, string tag,
                                                               RCRTCMediaType type)
        {
            AddUserItemToUserList($"{userId}#{tag}", HostCanvasUserList, true);
            RunOnMainThread.Enqueue(() =>
            {
                ChangeRemoteUserPublishState(HostCanvasUserList, $"{userId}#{tag}", type, true);
            });
        }

        private void OnRemoteCustomStreamUnpublished(string roomId, string userId, string tag,
                                                               RCRTCMediaType type)
        {
            RemoveUserItemFromUserList($"{userId}#{tag}", HostCanvasUserList);
        }

        private void OnSeiReceived(string roomId, string userId, string sei)
        {
            GameObject obj = GameObject.Find("SEI/SEI");
            if (obj != null)
            {
                obj.GetComponent<Text>().text = $"roomId:{roomId}, userId:{userId}, sei:{sei}";
            }
        }

        private void OnLiveMixSeiReceived(string sei)
        {
            GameObject obj = GameObject.Find("Audience/SEI");
            if (obj != null)
            {
                obj.GetComponent<Text>().text = $"SEI:{sei.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "")}";
            }
        }

        private void OnRemoteLiveRoleSwitched(string roomId, string userId, RCRTCRole role)
        {
            if (roomId == RoomId)
            {
                if (HostCanvas.activeSelf)
                {
                    if (role == RCRTCRole.LIVE_AUDIENCE)
                    {
                        OnUserLeft(roomId, userId);
                    }
                    else if (role == RCRTCRole.LIVE_BROADCASTER)
                    {
                        OnUserJoined(roomId, userId);
                    }
                }
            }
        }
        #endregion

        public GameObject NetworkStatsPrefab;
        public GameObject AudioStatsPrefab;
        public GameObject VideoStats;
        public GameObject UserItemPrefab;
        public GameObject MessagePrefab;
        public GameObject CDNSelectorPrefab;
        public GameObject CDNItemPrefab;
        public GameObject AudioEffectPrefab;
        public GameObject DeviceItemPrefab;
        public GameObject JoinedSubRoomPrefab;
        public GameObject SubRoomPrefab;

        private static readonly ConcurrentQueue<Action> RunOnMainThread = new ConcurrentQueue<Action>();

        private GameObject ConnectCanvas;
        private InputField ConnectCanvasAppKeyInput;
        private InputField ConnectCanvasNaviServerInput;
        private InputField ConnectCanvasFileServerInput;
        private InputField ConnectCanvasMediaServerInput;
        private InputField ConnectCanvasTokenInput;

        private GameObject ConnectedCanvas;
        private GameObject ConnectedCanvasModeGroup;
        private Text ConnectedCanvasJoinText;
        private RoomStreamConfigPrefab ConnectedRoomStreamConfig;

        private GameObject MeetingCanvas;
        private Text MeetingCanvasTitle;
        private Text MeetingCanvasEnableMicrophoneSwitchInfo;
        private Text MeetingCanvasEnableCameraSwitchInfo;
        private Text MeetingCanvasPublishAudioSwitchInfo;
        private Text MeetingCanvasSwitchAudioOutputInfo;
        private GameObject MeetingCanvasStatsTable;
        private GameObject MeetingCanvasUserList;
        private Text MeetingPublishVideoText;
        private VideoPanelPrefab MeetingVideoPanel;
        private VideoConfigPrefab MeetingVideoConfigPanel;

        private GameObject HostCanvas;
        private Text HostCanvasTitle;
        private VideoPanelPrefab HostVideoPanel;
        private Text HostCanvasEnableMicrophoneSwitchInfo;
        private Text HostCanvasEnableCameraSwitchInfo;
        private Text HostCanvasPublishAudioSwitchInfo;
        private Text HostCanvasSwitchAudioOutputInfo;
        private Text HostPublishVideoText;
        private GameObject HostCanvasStatsTable;
        private GameObject HostCanvasUserList;
        private VideoConfigPrefab HostVideoConfigPanel;

        private GameObject AudienceCanvas;
        private Text AudienceCanvasTitle;
        private VideoPanelPrefab AudienceVideoPanel;
        private Button AudienceSubscribeButton;
        private Text AudienceCanvasSwitchAudioOutputInfo;
        private GameObject AudienceCanvasStatsTable;
        private Toggle AudienceAudioToggle;
        private Toggle AudienceVideoToggle;
        private Toggle AudienceAudioVideoToggle;
        private Toggle AudienceTinyStream;
        private RCRTCMediaType AudienceUISelectedMediaType;
        private RCRTCMediaType? AudienceSubscribeType;
        private bool AudienceIsSubscribed;

        private GameObject MessageCanvas;
        private Text MessageCanvasActionInfo;
        private GameObject MessageCanvasMessageList;
        private InputField MessageCanvasMessageInput;

        private GameObject ConfigCDNCanvas;
        private GameObject ConfigCDNCanvasCDNList;

        private GameObject ConfigCDNCanvasDialog;
        private GameObject ConfigCDNCanvasDialogCDNList;

        private GameObject AudioEffectCanvas;
        private GameObject AudioEffectCanvasEffectList;

        private GameObject AudioMixCanvas;
        private Slider AudioMixCanvasMixVolume;
        private Slider AudioMixCanvasPublishVolume;
        private Toggle AudioMixCanvasPlayback;
        private Slider AudioMixCanvasPlaybackVolume;

        private GameObject HostMixConfig;
        private GameObject HostMixBgConfig;
        private GameObject HostMixVideoStreamConfig;
        private GameObject HostMixTinyVideoStreamConfig;
        private GameObject HostMixVideoCustomLayoutConfig;
        private GameObject HostMixAddVideoCustomLayout;
        private GameObject HostMixAudioConfig;
        private GameObject HostMixAudioCustomConfig;

        private GameObject DeviceListCanvas;
        private GameObject DeviceList;

        private GameObject SubRoomCanvas;

        private GameObject SeiCanvas;

        private String RoomId;

        private RCRTCRole Role;
        private LoginData User;
        private bool Connected;

        private RCIMEngine imEngine = null;
        private RCRTCEngine Engine = null;

        private bool Speaker;
        private bool Microphone;
        private bool Camera;
        private bool PublishAudio;
        private bool PublishVideo;
        private bool InChatRoom;
        private bool CloseMessagePanel;
        private bool PublishCustomStream;
        private RCRTCAudioMixingMode AudioMixingMode = RCRTCAudioMixingMode.MIX;

        private Dictionary<String, GameObject> Users;
        private Dictionary<String, bool> UserSubscribeAudioStates;
        private Dictionary<String, bool> UserSubscribeVideoStates;
        private Dictionary<String, GameObject> Statses;
        private List<GameObject> Messages;
        private Dictionary<CDN, GameObject> CDNs;
        private List<GameObject> SelectedCDNs;
        private Dictionary<int, GameObject> AudioEffects;
        private List<Config> SavedInfos;
        private List<String> JoinedSubRooms = new List<string>();
        private List<String> JoinableSubRooms = new List<string>();
        private List<String> BandedSubRooms = new List<string>();

        private static readonly String[] AudioEffectNames = {
            "反派大笑",
            "狗子叫声",
            "胜利号角",
        };
        private const String AudioEffectFilePathFormat = "effect_{0}.mp3";

        private const String AudioMixFilePath = "music_0.mp3";

        private Toggle PushEnabled;
        private Toggle IpcDisEnabled;
#if UNITY_ANDROID
        private bool mPushEnabled = false;
        private bool mDisableIPC = true;
#endif
#if UNITY_STANDALONE_WIN
        //设备
        private RCRTCDevice CurrentCamera;
        private List<RCRTCDevice> CurrentMicrophones;
        private List<RCRTCDevice> CurrentSpeakers;

        private void ReloadDeviceList(RCRTCDevice[] list, List<RCRTCDevice> devices,  bool single, bool cancelable, Action<RCRTCDevice,bool> action)
        {
            DeviceListCanvas.SetActive(true);
            int childCount = DeviceList.transform.childCount;
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    Transform tf = DeviceList.transform.GetChild(i);
                    Destroy(tf.gameObject);
                }
            }
            RectTransform last = null;
            foreach (var model in list)
            {
                GameObject item = GameObject.Instantiate(DeviceItemPrefab, DeviceList.transform) as GameObject;
                item.name = model.index.ToString();
                item.transform.Find("Name").GetComponent<Text>().text = "设备名称："+model.name;
                item.transform.Find("ID").GetComponent<Text>().text = "设备ID：" + model.id;
                Toggle toggle = item.transform.Find("CheckBox").GetComponent<Toggle>();
                if (devices != null)
                {
                    toggle.isOn = devices.Contains(model);
                    if (!cancelable && toggle.isOn)
                    {
                        toggle.interactable = false;
                    }
                }
                toggle.onValueChanged.AddListener((changed) =>
                {
                    Debug.Log(changed);
                    action(model, changed);
                    if (single)
                    {
                        DeviceListCanvas.SetActive(false);
                    }
                });
                if (last != null)
                {
                    item.GetComponent<RectTransform>().localPosition = new Vector3(last.localPosition.x, last.localPosition.y - last.rect.height - 10, last.localPosition.z);
                }
                last = item.GetComponent<RectTransform>();
            }
            RectTransform rect = DeviceList.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, list.Length * (last.rect.height + 10));
        }

        private void OnCameraLiseChange()
        {
            Debug.Log("摄像头有改变");
        }

        private void OnMicrophoneLiseChange()
        {
            Debug.Log("麦克风有改变");
        }

        private void OnSpeakerLiseChange()
        {
            Debug.Log("扬声器有改变");
        }

        private void OnCameraChange(RCRTCDevice camera, int code, string errMsg)
        {
            Debug.Log("OnCameraChange" + camera.name);
        }
#else
        private void OnCameraChange(RCRTCCamera camera, int code, string errMsg)
        {
            Debug.Log("OnCameraChange" + camera);
        }
#endif
    }

}