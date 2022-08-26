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

#if !UNITY_EDITOR
#if !UNITY_ANDROID
            RCIMClient.Instance.Init(ExampleConfig.AppKey);
#endif
            RCIMClient.Instance.OnSendMessageSucceed += Instance_OnSendMessageSucceed;
            RCIMClient.Instance.OnSendMessageFailed += Instance_OnSendMessageFailed;
            RCIMClient.Instance.OnMessageReceived += OnIMReceivedMessage;
#endif

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
            MeetingAudioSource = GameObject.Find("Meeting").GetComponent<AudioSource>();
            MeetingCanvasPlayBackgroundMusicSwitchButton = GameObject.Find("/Meeting/PlayBackgroundMusicSwitch")
                .GetComponent<Button>().gameObject;
            MeetingCanvasPlayBackgroundMusicSwitchInfo =
                GameObject.Find("/Meeting/PlayBackgroundMusicSwitch/Text").GetComponent<Text>();
            MeetingCanvasSwitchAudioOutputInfo =
                GameObject.Find("/Meeting/SwitchAudioOutput/Text").GetComponent<Text>();
            MeetingCanvasStatsTable = GameObject.Find("/Meeting/StatsTable/Viewport/Content");
            MeetingCanvasUserList = GameObject.Find("/Meeting/UserList/Viewport/Content");
            MeetingPublishVideoText = GameObject.Find("/Meeting/PublishVideoSwitch/Text").GetComponent<Text>();
            MeetingVideoConfigPanel = GameObject.Find("/Meeting/VideoConfigPanel").GetComponent<VideoConfigPrefab>();
            MeetingVideoConfigPanel.OnValueChanged = OnVideoConfigChanged;

            HostCanvas = GameObject.Find("Host").GetComponent<Canvas>().gameObject;
            HostCanvasTitle = GameObject.Find("/Host/Title").GetComponent<Text>();
            HostVideoPanel = GameObject.Find("/Host").GetComponent<VideoPanelPrefab>();
            HostCanvasEnableMicrophoneSwitchInfo =
                GameObject.Find("/Host/EnableMicrophoneSwitch/Text").GetComponent<Text>();
            HostCanvasEnableCameraSwitchInfo = GameObject.Find("/Host/EnableCameraSwitch/Text").GetComponent<Text>();
            HostCanvasPublishAudioSwitchInfo = GameObject.Find("/Host/PublishAudioSwitch/Text").GetComponent<Text>();
            HostCanvasSwitchAudioOutputInfo = GameObject.Find("/Host/SwitchAudioOutput/Text").GetComponent<Text>();
            HostPublishVideoText = GameObject.Find("/Host/PublishVideoSwitch/Text").GetComponent<Text>();
            HostCanvasConfigCDN = GameObject.Find("/Host/ConfigCDN").GetComponent<Button>();
            HostCanvasStatsTable = GameObject.Find("/Host/StatsTable/Viewport/Content");
            HostCanvasUserList = GameObject.Find("/Host/UserList/Viewport/Content");
            HostVideoConfigPanel = GameObject.Find("/Host/VideoConfigPanel").GetComponent<VideoConfigPrefab>();
            HostVideoConfigPanel.OnValueChanged = OnVideoConfigChanged;

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
            AudienceLiveStreamText = GameObject.Find("/Audience/LiveStreamText").GetComponent<Text>();
            AudienceLiveStreamText.text = string.Empty;
            initAudienceUIState();

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

            LoadingCanvas = GameObject.Find("Loading").GetComponent<Canvas>().gameObject;

            Toast = GameObject.Find("/Toast/Toast").GetComponent<CanvasGroup>();
            ToastInfo = GameObject.Find("/Toast/Toast/Info").GetComponent<Text>();

            HostMixConfig = GameObject.Find("HostMixConfig").GetComponent<Canvas>().gameObject;
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

            HostMixConfig.SetActive(false);
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
            LoadingCanvas.SetActive(false);
            MeetingCanvasPlayBackgroundMusicSwitchButton.SetActive(false);
            DeviceListCanvas.SetActive(false);
            Toast.alpha = 0.0f;

            Connected = false;

            Users = new Dictionary<String, GameObject>();
            UserSubscribeAudioStates = new Dictionary<string, bool>();
            UserSubscribeVideoStates = new Dictionary<string, bool>();
            Statses = new Dictionary<string, GameObject>();
            Messages = new List<GameObject>();
            CDNs = new Dictionary<CDN, GameObject>();
            SelectedCDNs = new List<GameObject>();
            AudioEffects = new Dictionary<int, GameObject>();
            _videoCustomLayouts.Clear();
            PushEnabled = GameObject.Find("/Connect/PushEnabled").GetComponent<Toggle>();
            IpcDisEnabled = GameObject.Find("/Connect/IpcDisEnabled").GetComponent<Toggle>();
            InitButton = GameObject.Find("/Connect/Init").GetComponent<Button>();
#if !UNITY_ANDROID
            PushEnabled.gameObject.SetActive(false);
            IpcDisEnabled.gameObject.SetActive(false);
            InitButton.gameObject.SetActive(false);
#else
            PushEnabled.isOn = mPushEnabled;
            IpcDisEnabled.isOn = mDisableIPC;
#endif
#if UNITY_STANDALONE_WIN
            CurrentMicrophones = new List<RCRTCDevice>();
            CurrentSpeakers = new List<RCRTCDevice>();
#endif
        }

        private void Instance_OnSendMessageFailed(RCErrorCode errorCode, RCMessage message)
        {
            if (errorCode != RCErrorCode.Succeed)
            {
                ShowToast("发送消息失败，Error " + errorCode);
            }
        }

        private void Instance_OnSendMessageSucceed(RCMessage message)
        {
            RunOnMainThread.Enqueue(() =>
            {
                AddMessage(String.Format("{0} (我): {1}", message.ObjectName, message));
            });
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
        }

#if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            Engine?.LeaveRoom();
            Engine?.Destroy();
            RCIMClient.Instance.Destroy();
        }
#endif

        private void OnDestroy()
        {
            Engine?.LeaveRoom();
            Engine?.Destroy();
            RCIMClient.Instance.Destroy();
        }

        #region UI Event

        public void OnClickHostMixConfig()
        {
            HostCanvas.SetActive(false);
            HostMixConfig.SetActive(true);
        }

        public void OnClickCloseHostLiveMixConfig()
        {
            HostCanvas.SetActive(true);
            HostMixConfig.SetActive(false);
        }

        public void OnHostMixVideoModeCustomSelectStateChanged(bool selected)
        {
            if (selected)
            {
                HostMixConfig.SetActive(false);
                HostMixVideoCustomLayoutConfig.SetActive(true);
                Engine?.SetLiveMixLayoutMode(RCRTCLiveMixLayoutMode.CUSTOM);
            }
        }

        public void OnHostMixVideoModeFloatSelectStateChanged(bool selected)
        {
            if (selected)
            {
                Engine?.SetLiveMixLayoutMode(RCRTCLiveMixLayoutMode.SUSPENSION);
            }
        }

        public void OnHostMixVideoModeAutoSelectStateChanged(bool selected)
        {
            if (selected)
            {
                Engine?.SetLiveMixLayoutMode(RCRTCLiveMixLayoutMode.ADAPTIVE);
            }
        }

        public void OnHostMixVideoRenderModeWholeSelectStateChanged(bool selected)
        {
            if (selected)
            {
                Engine?.SetLiveMixRenderMode(RCRTCLiveMixRenderMode.WHOLE);
            }
        }

        public void OnHostMixVideoRenderModeCropSelectStateChanged(bool selected)
        {
            if (selected)
            {
                Engine?.SetLiveMixRenderMode(RCRTCLiveMixRenderMode.CROP);
            }
        }
        public void OnClickHostLiveMixVideoStreamConfig()
        {
            HostMixConfig.SetActive(false);
            HostMixVideoStreamConfig.SetActive(true);
        }

        public void OnClickHostLiveMixVideoStreamClose()
        {
            HostMixVideoStreamConfig.SetActive(false);
            HostMixConfig.SetActive(true);
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
            HostMixConfig.SetActive(true);
        }

        public void OnClickHostVideoStreamConfigCancel()
        {
            HostMixVideoStreamConfig.SetActive(false);
            HostMixConfig.SetActive(true);
        }
        
        public void OnClickHostTinyVideoStreamConfig()
        {
            HostMixConfig.SetActive(false);
            HostMixTinyVideoStreamConfig.SetActive(true);
        }

        public void OnClickHostTinyVideoStreamConfigConfirm()
        {
            var config = GameObject.Find("/HostMixTinyVideoStreamConfig/Background/TinyVideoStreamConfig")
                .GetComponent<VideoStreamConfig>();

            Engine?.SetLiveMixVideoBitrate(config.BitRateValue, true);
            Engine?.SetLiveMixVideoFps(config.FPS, true);
            Engine?.SetLiveMixVideoResolution(GetResolutionWidth(config.Resolution),
                GetResolutionHeight(config.Resolution), true);

            HostMixConfig.SetActive(true);
            HostMixTinyVideoStreamConfig.SetActive(false);
        }

        public void OnClickHostTinyVideoStreamConfigCancel()
        {
            HostMixConfig.SetActive(true);
            HostMixTinyVideoStreamConfig.SetActive(false);
        }

        public void OnClickHostVideoCustomLayoutConfig()
        {
            HostMixConfig.SetActive(false);
            HostMixVideoCustomLayoutConfig.SetActive(true); 
            RefreshCustomVideoLayoutList();
        }
        
        public void OnClickHostVideoCustomLayoutConfigConfirm()
        {
            Engine?.SetLiveMixCustomLayouts(_videoCustomLayouts);
            HostMixConfig.SetActive(true);
            HostMixVideoCustomLayoutConfig.SetActive(false);
        }

        public void OnClickHostVideoCustomLayoutConfigCancel()
        {
            HostMixConfig.SetActive(true);
            HostMixVideoCustomLayoutConfig.SetActive(false);
        }

        public void OnClickHostVideoCustomLayoutAdd()
        {
            HostMixVideoCustomLayoutConfig.SetActive(false);
            HostMixAddVideoCustomLayout.SetActive(true);

            var userOptions = GameObject
                .Find("/HostMixAddVideoCustomLayout/Background/SelectUserDropdown").GetComponent<Dropdown>();
            OnSelectUserDropdownValueChanged(0);
            userOptions.options.Clear();
            var optionDatas = new List<Dropdown.OptionData> {new Dropdown.OptionData(RCIMClient.Instance.GetCurrentUserID())};
            optionDatas.AddRange(Users.Select(item => new Dropdown.OptionData(item.Key)));
            
            userOptions.AddOptions(optionDatas);
            userOptions.SetValueWithoutNotify(0);
        }

        #region 自定义视频合流布局

        public GameObject CustomVideoLayoutList;
        public GameObject CustomVideoLayoutListItem;
        
        private readonly IList<RCRTCCustomLayout> _videoCustomLayouts = new List<RCRTCCustomLayout>();

        private IList<GameObject> _videoCustomLayoutsGameObjects = new List<GameObject>();


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
        
        public void OnClickHostVideoCustomLayoutAddCancel()
        {
            HostMixAddVideoCustomLayout.SetActive(false);
            HostMixVideoCustomLayoutConfig.SetActive(true);
        }
        public void OnClickHostAudioConfig()
        {
            HostMixConfig.SetActive(false);
            HostMixAudioConfig.SetActive(true);
        }

        public void OnClickHostAudioConfigConfirm()
        {
            var liveMixAudioConfig = GameObject.Find("/HostMixAudioConfig/Background/LiveMixAudioBitrate")
                .GetComponent<LiveMixAudioBitrate>();
            if (liveMixAudioConfig == null)
                return;
            Engine?.SetLiveMixAudioBitrate(liveMixAudioConfig.BitRateValue);
            HostMixConfig.SetActive(true);
            HostMixAudioConfig.SetActive(false);
        }

        public void OnClickHostAudioConfigCancel()
        {
            HostMixConfig.SetActive(true);
            HostMixAudioConfig.SetActive(false);
        }

        #region 自定义音频合流

        public GameObject CustomAudioMixList;
        public GameObject CustomAudioMixListItem;
        
        private readonly IList<String> _audioMixUserList = new List<String>();

        private IList<GameObject> _audioMixUserListsGameObjects = new List<GameObject>();
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
            if ( String.IsNullOrEmpty(audioMixUser))
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

        public void OnClickHostAudioCustomConfig()
        {
            HostMixConfig.SetActive(false);
            HostMixAudioCustomConfig.SetActive(true);

            var userOptions = GameObject
                .Find("/HostMixAudioCustomConfig/Background/SelectUserDropdown").GetComponent<Dropdown>();

            userOptions.options.Clear();
            var optionDatas = new List<Dropdown.OptionData>()
            {
                new Dropdown.OptionData(RCIMClient.Instance.GetCurrentUserID())
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

            HostMixConfig.SetActive(true);
            HostMixAudioCustomConfig.SetActive(false);
        }

        public void OnClickHostAudioCustomConfigCancel()
        {
            HostMixConfig.SetActive(true);
            HostMixAudioCustomConfig.SetActive(false);
        }
        
        public void OnClickConnect()
        {
            if (Connected)
            {
                RCIMClient.Instance.Disconnect();
                ChangeToUnconnect();
            }
            else
            {
                if (String.IsNullOrEmpty(ConnectCanvasTokenInput.text))
                {
                    ShowToast("请输入Token!");
                    return;
                }

                ConnectIM(ConnectCanvasTokenInput.text);  
            }
        }

        public void OnClickGenerate()
        {
            CheckConfigInput();
            GenerateTokenAndLogin(false);
        }

        private void CheckConfigInput()
        {
            if (!string.IsNullOrEmpty(ConnectCanvasAppKeyInput.text))
            {
                ExampleConfig.AppKey = ConnectCanvasAppKeyInput.text;
            }

            if (!string.IsNullOrEmpty(ConnectCanvasNaviServerInput.text))
            {
                ExampleConfig.NavServer = ConnectCanvasNaviServerInput.text;
            }
                
            if (!string.IsNullOrEmpty(ConnectCanvasFileServerInput.text))
            {
                ExampleConfig.FileServer = ConnectCanvasFileServerInput.text;
            }
                
            if (!string.IsNullOrEmpty(ConnectCanvasMediaServerInput.text))
            {
                ExampleConfig.MediaServer = ConnectCanvasMediaServerInput.text;
            }
            
        }

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

        public void OnClickJoin()
        {
            if (String.IsNullOrEmpty(RoomId))
            {
                ShowToast("房间号不能为空！");
                return;
            }

            ShowLoading();
            RCRTCRoomSetup.Builder roomSetupBuilder = RCRTCRoomSetup.Builder.Create();
            RCRTCEngineSetup.Builder engineSetupBuilder = RCRTCEngineSetup.Builder.Create();
            RCRTCVideoSetup.Builder videoSetupBuilder = RCRTCVideoSetup.Builder.Create();
       
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
#if UNITY_STANDALONE_WIN
            Engine = RCRTCEngine.Create(engineSetupBuilder.Build(), RCIMClient.Instance.Handler());
            Engine.OnCameraListChanged = OnCameraLiseChange;
            Engine.OnMicrophoneListChanged = OnMicrophoneLiseChange;
            Engine.OnSpeakerListChanged = OnSpeakerLiseChange;
            Engine.OnCameraSwitched = OnCameraChange;
#else
            Engine = RCRTCEngine.Create(engineSetupBuilder.Build());
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
                    Engine.OnUserJoined = HostOnUserJoined;
                    Engine.OnUserOffline = HostOnUserOffline;
                    Engine.OnUserLeft = HostOnUserLeft;

                    Engine.OnRemotePublished = HostOnRemotePublished;
                    Engine.OnRemoteUnpublished = HostOnRemoteUnpublished;
                    break;
                case RCRTCRole.LIVE_AUDIENCE:
                    Engine.OnUserJoined = AudienceOnUserJoined;
                    Engine.OnUserOffline = AudienceOnUserOffline;
                    Engine.OnUserLeft = AudienceOnUserLeft;

                    Engine.OnRemoteLiveMixPublished = AudienceOnRemoteLiveMixPublished;
                    Engine.OnRemoteLiveMixUnpublished = AudienceOnRemoteLiveMixUnpublished;
                    break;
            }
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("!","1");
            Engine.OnRoomJoined = delegate (int code, String message)
            {
                Engine.OnRoomJoined = null;

                RunOnMainThread.Enqueue(() =>
                {
                    HideLoading();
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
                        ShowToast(message);
                    }
                });
            };
           
            RCRTCRoomSetup setup = roomSetupBuilder.WithRole(Role).Build();
            int ret = Engine.JoinRoom(RoomId, setup);
            if (ret != 0)
            {
                HideLoading();
                ShowToast("Error " + ret);
            }
        }

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
                    ReloadDeviceList(list, devices, true, false, (device,value) => {
                        int ret = Engine.SwitchCamera(device);
                        CurrentCamera = device;
                        Debug.Log("切换摄像头结果" + ret);
                    });
                }
                else
                {
                    ShowToast("没有可切换的摄像头");
                }
            }
            else
            {
                ShowToast("还没开启摄像头");
            }
#else
        Engine.SwitchCamera();
#endif
        }

        public void OnClickLeaveMeeting()
        {
            ShowLoading();

            if (MeetingAudioSource.isPlaying)
            {
                MeetingAudioSource.Stop();
            }

            Engine.OnRoomLeft = delegate (int code, String message)
            {
                Engine.OnRoomLeft = null;

                RunOnMainThread.Enqueue(() =>
                {
                    HideLoading();
                    ChangeToConnected();
                    if (code != 0)
                    {
                        ShowToast(message);
                    }
                    Engine.Destroy();
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
                ShowToast("没有麦克风权限");
                return;
            }
#if UNITY_STANDALONE_WIN
            RCRTCDevice[] list = Engine.GetMicrophoneList();
            Debug.Log("获取到麦克风列表" + list.Length);
            if (list.Length > 0)
            {
                ReloadDeviceList(list, CurrentMicrophones, true, true,(device, value) => {
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
                ShowToast("没有可用的麦克风");
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
            ShowLoading();
            if (!PublishAudio)
            {
                Engine.OnPublished = delegate (RCRTCMediaType type, int code, String message)
                {
                    Engine.OnPublished = null;

                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        if (code != 0)
                        {
                            ShowToast(message);
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
                    HideLoading();
                    ShowToast("Error " + ret);
                }
            }
            else
            {
                Engine.OnUnpublished = delegate (RCRTCMediaType type, int code, String message)
                {
                    Engine.OnUnpublished = null;

                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        if (code != 0)
                        {
                            ShowToast(message);
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
                    HideLoading();
                    ShowToast("Error" + ret);
                }
            }
        }

        public void OnClickMeetingPlayBackgroundMusic()
        {
            PlayBackgroundMusic = !PlayBackgroundMusic;
            if (PlayBackgroundMusic)
            {
                MeetingAudioSource.Play();
            }
            else
            {
                MeetingAudioSource.Stop();
            }
            MeetingCanvasPlayBackgroundMusicSwitchInfo.text = PlayBackgroundMusic ? "停止背景音乐" : "播放背景音乐";
        }

        public void OnClickMeetingSwitchAudioOutput()
        {
#if UNITY_STANDALONE_WIN
            RCRTCDevice[] list = Engine.GetSpeakerList();
            Debug.Log("获取到扬声器列表" + list.Length);
            if (list.Length > 0)
            {
                ReloadDeviceList(list, CurrentSpeakers, false, true,(device, value) => {
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
                ShowToast("没有可用的扬声器");
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
            ShowToast("Windows 暂不支持此功能");
            return;
#endif
            Engine.OnAudioEffectCreated = delegate (int id, int code, String message)
            {
                RunOnMainThread.Enqueue(() =>
                {
                    if (code != 0)
                    {
                        ShowToast("Create Audio Effect " + id + " Error: " + message);
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
                    ShowToast("Create Audio Effect " + file + " Error " + ret);
                }
            }
            AudioEffectCanvas.SetActive(true);
        }

        public void OnClickMeetingPublishVideo()
        {
            ShowLoading();
            if (!PublishVideo)
            {
                Engine.OnPublished = delegate(RCRTCMediaType type, int code, String errMsg)
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        if (code != 0)
                        {
                            string toast = $"Publish video error: {errMsg}";
                            ShowToast(toast);
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
                    HideLoading();
                    ShowToast("Error" + ret);
                }
                RCRTCVideoConfig config = MeetingVideoConfigPanel.VideoConfig;
                Engine.SetVideoConfig(config);
                Debug.Log($"SetVideoConfig {config}");
            }
            else
            {
                Engine.OnUnpublished = delegate(RCRTCMediaType type, int code, string msg)
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        if (code != 0)
                        {
                            string toast = $"UnPublish video error: {msg}";
                            ShowToast(toast);
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
                    HideLoading();
                    ShowToast("Error" + ret);
                }
            }
        }

        public void OnClickLeaveHost()
        {
            ShowLoading();
            Engine.OnRoomLeft = delegate (int code, String message)
            {
                Engine.OnRoomLeft = null;

                RunOnMainThread.Enqueue(() =>
                {
                    HideLoading();
                    ChangeToConnected();
                    if (code != 0)
                    {
                        ShowToast(message);
                    }
                    Engine.Destroy();
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
                ShowToast("没有麦克风权限");
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
                ShowToast("没有可用的麦克风");
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
            ShowLoading();
            if (!PublishAudio)
            {
                Engine.OnPublished = delegate (RCRTCMediaType type, int code, String message)
                {
                    Engine.OnPublished = null;

                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        if (code != 0)
                        {
                            ShowToast(message);
                        }
                        else
                        {
                            PublishAudio = true;
                            HostCanvasPublishAudioSwitchInfo.text = PublishAudio ? "取消发布音频" : "发布音频";
                            HostCanvasConfigCDN.interactable = PublishAudio;
                        }
                    });
                };
                int ret = Engine.Publish(RCRTCMediaType.AUDIO);
                if (ret != 0)
                {
                    HideLoading();
                    ShowToast("Error " + ret);
                }
            }
            else
            {
                Engine.OnUnpublished = delegate (RCRTCMediaType type, int code, String message)
                {
                    Engine.OnUnpublished = null;

                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        if (code != 0)
                        {
                            ShowToast(message);
                        }
                        else
                        {
                            PublishAudio = false;
                            HostCanvasPublishAudioSwitchInfo.text = PublishAudio ? "取消发布音频" : "发布音频";
                            HostCanvasConfigCDN.interactable = PublishAudio;
                            RemoveAudioStats(HostCanvasStatsTable);
                        }
                    });
                };
                int ret = Engine.Unpublish(RCRTCMediaType.AUDIO);
                if (ret != 0)
                {
                    HideLoading();
                    ShowToast("Error" + ret);
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
                ShowToast("没有可用的扬声器");
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
            ShowLoading();
            if (!PublishVideo)
            {
                Engine.OnPublished = delegate(RCRTCMediaType type, int code, String errMsg)
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        if (code != 0)
                        {
                            string toast = $"Publish video error: {errMsg}";
                            ShowToast(toast);
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
                    HideLoading();
                    ShowToast("Error" + ret);
                }
                RCRTCVideoConfig config = HostVideoConfigPanel.VideoConfig;
                Engine.SetVideoConfig(config);
                Debug.Log($"SetVideoConfig {config}");
            }
            else
            {
                Engine.OnUnpublished = delegate(RCRTCMediaType type, int code, string msg)
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        if (code != 0)
                        {
                            string toast = $"UnPublish video error: {msg}";
                            ShowToast(toast);
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
                    HideLoading();
                    ShowToast("Error" + ret);
                }
            }
        }
        
        public void OnClickLeaveAudience()
        {
            ShowLoading();
            Engine.OnRoomLeft = delegate (int code, String message)
            {
                Engine.OnRoomLeft = null;

                RunOnMainThread.Enqueue(() =>
                {
                    HideLoading();
                    ChangeToConnected();
                    if (code != 0)
                    {
                        ShowToast(message);
                    }
                    Engine.Destroy();
                });
            };
            Engine.SetStatsListener(null);
            Engine.LeaveRoom();
        }
        
        public void OnClickAudienceSubscribe()
        {
            if (AudienceIsSubscribed)
            {
                _AudienceUnsubscribe();
                return;
            }
            
            ShowLoading();
            RCRTCMediaType subscribeType = AudienceUISelectedMediaType;
            Engine.OnLiveMixSubscribed = delegate (RCRTCMediaType type, int code, String message)
            {
                Engine.OnLiveMixSubscribed = null;
                RunOnMainThread.Enqueue(() =>
                {
                    HideLoading();
                    if (code != 0)
                    {
                        ShowToast(message);
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
                    HideLoading();
                    ShowToast("Subscribe Live Mix Error " + ret);
                } 
            }
            else
            {
                var ret = Engine.SubscribeLiveMix(RCRTCMediaType.AUDIO);
                if (ret != 0)
                {
                    HideLoading();
                    ShowToast("Subscribe Live Mix Error " + ret);
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
                ShowToast("没有可用的扬声器");
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

        public void OnClickMessageAction()
        {
#if UNITY_STANDALONE_WIN
            ShowToast("Windows 暂不支持此功能");
            return;
#endif
            ShowLoading();
            if (!InChatRoom)
            {
                RCIMClient.Instance.JoinChatRoom(RoomId, (code) =>
                 {
                     if (code == RCErrorCode.Succeed)
                     {
                         RunOnMainThread.Enqueue(() =>
                         {
                             InChatRoom = true;
                             MessageCanvasActionInfo.text = InChatRoom ? "离开聊天室" : "加入聊天室";
                             HideLoading();
                         });
                     }
                     else
                     {
                         RunOnMainThread.Enqueue(() =>
                         {
                             HideLoading();
                             ShowToast("IM Join Chat Room Error, Code = " + code);
                         });
                     }
                 }, -1);
            }
            else
            {
                RCIMClient.Instance.QuitChatRoom(RoomId, (code) =>
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        InChatRoom = false;
                        MessageCanvasActionInfo.text = "加入聊天室";
                    });
                });
            }
        }

        public void OnClickMessageClose()
        {
            foreach (GameObject message in Messages)
            {
                Destroy(message);
                // DestroyImmediate(message);
            }
            Messages.Clear();

            if (!InChatRoom)
            {
                MessageCanvas.SetActive(false);
            }
            else
            {
                ShowLoading();
                CloseMessagePanel = true;
                RCIMClient.Instance.QuitChatRoom(RoomId, (code) =>
                {
                    if (code == RCErrorCode.Succeed)
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
                            HideLoading();
                        });
                    }
                    else
                    {
                        RunOnMainThread.Enqueue(() =>
                        {
                            CloseMessagePanel = false;
                            HideLoading();
                            ShowToast("IM Left Chat Room Error, Code = " + code);
                        });
                    }
                });
            }
        }

        public void OnClickMessageSend()
        {
            if (!InChatRoom)
            {
                ShowToast("请先加入聊天室");
                return;
            }
            String content = MessageCanvasMessageInput.text;
            if (String.IsNullOrEmpty(content))
            {
                ShowToast("请先输入消息");
                return;
            }
            var textMsg = new RCTextMessage($"{User.Name}, {content}");
            MessageCanvasMessageInput.text = "";
            RCIMClient.Instance.SendMessage(RCConversationType.ChatRoom, RoomId, textMsg);
        }

        public void OnClickCDNAdd()
        {
            ShowCDNDialog();
        }

        public void OnClickAudioEffectClose()
        {
            int ret = Engine.StopAllAudioEffects();
            if (ret != 0)
            {
                ShowToast("Stop All Audio Effects Error " + ret);
            }

            var ids = AudioEffects.Keys;
            foreach (int id in ids)
            {
                ret = Engine.ReleaseAudioEffect(id);
                if (ret != 0)
                {
                    ShowToast("Release Audio Effect " + id + " Error " + ret);
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

        public void OnClickAudioMixOpen()
        {
#if UNITY_STANDALONE_WIN
            ShowToast("Windows 暂不支持此功能");
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
                ShowToast("Start Audio Mixing Error " + ret);
            }
            ret = Engine.AdjustAudioMixingVolume(mixingVolume);
            if (ret != 0)
            {
                ShowToast("Adjust Audio Mixing Volume Error " + ret);
            }
            ret = Engine.AdjustAudioMixingPublishVolume(publishVolume);
            if (ret != 0)
            {
                ShowToast("Adjust Audio Mixing Publish Volume Error " + ret);
            }
            ret = Engine.AdjustAudioMixingPlaybackVolume(playbackVolume);
            if (ret != 0)
            {
                ShowToast("Adjust Audio Mixing Playback Volume Error " + ret);
            }
        }

        public void OnClickAudioMixStop()
        {
            int ret = Engine.StopAudioMixing();
            if (ret != 0)
            {
                ShowToast("Stop Audio Mixing Error " + ret);
            }
        }

        public void OnClickDeviceListClose()
        {
            DeviceListCanvas.SetActive(false);
        }

#endregion

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
        
        private void initAudienceUIState()
        {
            AudienceUISelectedMediaType = RCRTCMediaType.AUDIO;
            AudienceAudioToggle.isOn = true;
            AudienceVideoToggle.isOn = false;
            AudienceAudioVideoToggle.isOn = false;
            AudienceTinyStream.isOn = false;
            AudienceTinyStream.gameObject.SetActive(false);
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

#if UNITY_STANDALONE_WIN
            HostCanvasEnableMicrophoneSwitchInfo.text = "麦克风列表";
            HostCanvasSwitchAudioOutputInfo.text = "扬声器列表";
#endif
        }

        private void OnVideoConfigChanged(RCRTCVideoConfig config)
        {
            Engine.SetVideoConfig(config);
        }

        private void _AudienceUnsubscribe()
        {
            if (AudienceSubscribeType != null)
            {
                ShowLoading();
                Engine.OnLiveMixUnsubscribed = delegate (RCRTCMediaType type, int code, String message)
                {
                    Engine.OnLiveMixUnsubscribed = null;
                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        AudienceIsSubscribed = false;
                        AudienceSubscribeButton.transform.Find("Text").GetComponent<Text>().text = "订阅"; 
                    
                        if (code != 0)
                        {
                            ShowToast(message);
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
                    HideLoading();
                    ShowToast("UnsubscribeLiveMix Error " + ret);
                    AudienceIsSubscribed = false;
                    AudienceSubscribeButton.transform.Find("Text").GetComponent<Text>().text = "订阅"; 
                }
            }
        }
        
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

            PlayBackgroundMusic = false;

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
            MeetingCanvasPlayBackgroundMusicSwitchInfo.text = PlayBackgroundMusic ? "停止背景音乐" : "播放背景音乐";

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
            },  (String userId, RCRTCRemoteAudioStats stats) =>
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

            HostCanvasConfigCDN.interactable = false;

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
                RunOnMainThread.Enqueue(()=>{ ChangeRemoteVideoStats(AudienceCanvasStatsTable, stats);});
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

        private void ChangeNetworkStats(GameObject table, RCRTCNetworkStats stats)
        {
            Transform network;
            if (!Statses.ContainsKey("Network"))
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
                Statses.Add("Network", item);
                network = item.transform;
            }
            else
            {
                network = table.transform.Find("Network");
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
       
        private bool? _enableCamera(RCRTCView view)
        {
            if (!CheckCameraPermission())
            {
                ShowToast("没有摄像头权限");
                return null;
            }

            // 90
            // Engine.SetCameraCaptureOrientation(RCRTCCameraCaptureOrientation.PORTRAIT_UPSIDE_DOWN);
            // 0
            // Engine.SetCameraCaptureOrientation(RCRTCCameraCaptureOrientation.LANDSCAPE_RIGHT);
            // 360
            // Engine.SetCameraCaptureOrientation(RCRTCCameraCaptureOrientation.LANDSCAPE_LEFT);
            // 270
            // Engine.SetCameraCaptureOrientation(RCRTCCameraCaptureOrientation.PORTRAIT);
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
                    ShowToast("没有可用的摄像头");
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
        private void AddUserItemToUserList(String id, GameObject list)
        {
            if (!Users.ContainsKey(id))
            {
                GameObject item = GameObject.Instantiate(UserItemPrefab, list.transform) as GameObject;
                item.name = id;
                item.transform.Find("Panel/UserId").GetComponent<Text>().text = id;
                Button audio = item.transform.Find("SubsribeAudioSwitch").GetComponent<Button>();
                audio.onClick.AddListener(() =>
                {
                    OnClickSubscribeAudioSwitch(id, audio);
                });
                Button video = item.transform.Find("SubsribeVideoSwitch").GetComponent<Button>();
                video.onClick.AddListener(() =>
                {
                    OnClickSubscribeVideoSwitch(id, video, item);
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

        private void OnClickSubscribeAudioSwitch(String id, Button audio)
        {
            ShowLoading();
            bool selected;
            UserSubscribeAudioStates.TryGetValue(id, out selected);
            if (!selected)
            {
                Engine.OnSubscribed = delegate (String remoteUserId, RCRTCMediaType type, int code, String message)
                {
                    Engine.OnSubscribed = null;

                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        if (code != 0)
                        {
                            ShowToast(message);
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
                    HideLoading();
                    ShowToast("Error" + ret);
                }
            }
            else
            {
                Engine.OnUnsubscribed = delegate (String remoteUserId, RCRTCMediaType type, int code, String message)
                {
                    Engine.OnUnsubscribed = null;

                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        if (code != 0)
                        {
                            ShowToast(message);
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
                    HideLoading();
                    ShowToast("Error" + ret);
                }
            }
        }

        private void OnClickSubscribeVideoSwitch(String id, Button video, GameObject userItem)
        {
            ShowLoading();
            bool selected;
            UserSubscribeVideoStates.TryGetValue(id, out selected);
            if (!selected)
            {
                bool isTiny = userItem.transform.Find("Panel/TinyStream").GetComponent<Toggle>().isOn;
                bool isMirror = userItem.transform.Find("Panel/Mirror").GetComponent<Toggle>().isOn;
                int option = userItem.transform.Find("Panel/DisplayOption").GetComponent<Dropdown>().value;
                RCRTCViewFitType fitType = getViewFitType(option);
                Engine.OnSubscribed = delegate (String remoteUserId, RCRTCMediaType type, int code, String message)
                {
                    Engine.OnSubscribed = null;

                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        if (code != 0)
                        {
                            ShowToast(message);
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
                    HideLoading();
                    ShowToast("Error" + ret);
                }
            }
            else
            {
                Engine.OnUnsubscribed = delegate (String remoteUserId, RCRTCMediaType type, int code, String message)
                {
                    Engine.OnUnsubscribed = null;

                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        if (code != 0)
                        {
                            ShowToast(message);
                        }
                        else
                        {
                            UserSubscribeVideoStates[remoteUserId] = false;
                            video.transform.Find("Text").GetComponent<Text>().text = "订阅视频";
                            var view = userItem.transform.Find("Panel").GetComponent<RCRTCView>();
                            Engine.RemoveRemoteView(id);
                        }
                    });
                };
                int ret = Engine.Unsubscribe(id, RCRTCMediaType.VIDEO);
                if (ret != 0)
                {
                    HideLoading();
                    ShowToast("Error" + ret);
                }
            }
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

        private void RemoveUserItemFromUserList(String id, GameObject list)
        {
            GameObject user;
            if (Users.TryGetValue(id, out user))
            {
                RectTransform position = user.GetComponent<RectTransform>();

                Destroy(user);
                // DestroyImmediate(user);
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

        private void ShowCDNDialog()
        {
#if UNITY_STANDALONE_WIN
            ShowToast("Windows 暂不支持此功能");
            return;
#endif
            if (CDNs.Values.Count > 0)
            {
                ConfigCDNCanvasDialog.SetActive(true);
            }
            else
            {
                LoadCDNs();
            }
        }

        private void LoadCDNs()
        {
            ShowLoading();
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
                    ShowToast("Load CDN List Error: " + result);
                }
                HideLoading();
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
                ShowToast("Get Session Id Null, Please try later.");
                return;
            }
            ShowLoading();
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
                            HideLoading();
                            if (code != 0)
                            {
                                ShowToast("Add Live Cdn Error: " + message);
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
                        HideLoading();
                        ShowToast("Add Live Cdn Error " + ret);
                    }
                }
                else
                {
                    HideLoading();
                    ShowToast("Load CDN Url Error: " + result);
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
            ShowLoading();
            Engine.OnLiveCdnRemoved = delegate (String url, int code, String message)
            {
                Engine.OnLiveCdnRemoved = null;

                RunOnMainThread.Enqueue(() =>
                {
                    HideLoading();
                    if (code != 0)
                    {
                        ShowToast("Remove Live Cdn Error: " + message);
                    }
                    else
                    {
                        SelectedCDNs.Remove(cdn);
                        Destroy(cdn);
                        // DestroyImmediate(cdn);

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
                HideLoading();
                ShowToast("Remove Live Cdn Error " + ret);
            }
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

        private void OnClickAudioEffectPlay(GameObject item, int id)
        {
            int volume = (int)item.transform.Find("Volume").GetComponent<Slider>().value;
            int count = (int)item.transform.Find("Count").GetComponent<Slider>().value;
            int ret = Engine.PlayAudioEffect(id, volume, count);
            if (ret != 0)
            {
                ShowToast("Play Audio Effect " + id + " Error " + ret);
            }
        }

        private void OnClickAudioEffectStop(int id)
        {
            int ret = Engine.StopAudioEffect(id);
            if (ret != 0)
            {
                ShowToast("Stop Audio Effect " + id + " Error " + ret);
            }
        }

        public void ShowLoading()
        {
            LoadingCount++;
            if (!LoadingCanvas.activeInHierarchy)
            {
                LoadingCanvas.SetActive(true);
            }
        }

        public void HideLoading()
        {
            LoadingCount--;
            if (LoadingCount <= 0 && LoadingCanvas.activeInHierarchy)
            {
                LoadingCount = 0;
                LoadingCanvas.SetActive(false);
            }
        }

        public void ShowToast(String toast)
        {
            ShowToast(toast, 2);
        }

        public void ShowToast(String toast, int duration)
        {
            if (CurrentToast != null)
            {
                StopCoroutine(CurrentToast);
            }
            CurrentToast = MakeToast(toast, duration);
            StartCoroutine(CurrentToast);
        }

        private IEnumerator MakeToast(String toast, int duration)
        {
            ToastInfo.text = toast;
            yield return fadeInAndOut(true, 0.5f);

            float counter = 0;
            while (counter < duration)
            {
                counter += Time.deltaTime;
                yield return null;
            }

            yield return fadeInAndOut(false, 0.5f);
        }

        private IEnumerator fadeInAndOut(bool fadeIn, float duration)
        {
            float from, to;
            if (fadeIn)
            {
                from = 0f;
                to = 1f;
            }
            else
            {
                from = 1f;
                to = 0f;
            }

            float counter = 0f;
            while (counter < duration)
            {
                counter += Time.deltaTime;
                float alpha = Mathf.Lerp(from, to, counter / duration);
                Toast.alpha = alpha;
                yield return null;
            }
        }
        private void GenerateTokenAndLogin(bool doLogin)
        {
            ShowLoading();
            TimeSpan time = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            String current = Convert.ToInt64(time.TotalMilliseconds).ToString();
            String id = ExampleConfig.Prefix + current;
            String url = ExampleConfig.Host + "/token/" + id;
            String json = "{\"key\":\"" + ExampleConfig.AppKey + "\"}";
            Post(url, json, (bool error, String result) =>
            {
                if (!error)
                {
                    Debug.Log("result:"+ result);
                    User = JsonUtility.FromJson<LoginData>(result);
                    User.Name = ExampleConfig.Prefix + current;
                    ConnectCanvasTokenInput.text = User.Token;
                    if (doLogin)
                    {
                        ConnectIM(User.Token); 
                    }
                    else
                    {
                        HideLoading();
                    }
                }
                else
                {
                    HideLoading();
                    ShowToast(result);
                }
            });
        }

        private void ConnectIM(string token)
        {
            RCIMClient.Instance.Connect(token, (code, userId) =>
            {
                if (code == RCErrorCode.Succeed)
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        ChangeToConnected();
                    });
                }
                else
                {
                    RunOnMainThread.Enqueue(() =>
                    {
                        HideLoading();
                        ShowToast("IM Connect Error, Code = " + code);
                    });
                }
            });
        }

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

#region IM Callbacks

        private void OnIMReceivedMessage(RCMessage msg, int left)
        {
            RunOnMainThread.Enqueue(() =>
            {
                AddMessage(String.Format("{0}: {1}", msg.ObjectName, msg));
            });
        }

#endregion

#region RTC Callbacks

        private void MeetingOnUserJoined(String id)
        {
            RunOnMainThread.Enqueue(() =>
            {
                AddUserItemToUserList(id, MeetingCanvasUserList);
            });
        }

        private void MeetingOnUserOffline(String id)
        {
            RunOnMainThread.Enqueue(() =>
            {
                RemoveUserItemFromUserList(id, MeetingCanvasUserList);
            });
        }

        private void MeetingOnUserLeft(String id)
        {
            RunOnMainThread.Enqueue(() =>
            {
                RemoveUserItemFromUserList(id, MeetingCanvasUserList);
            });
        }

        private void MeetingOnRemotePublished(String id, RCRTCMediaType type)
        {
            RunOnMainThread.Enqueue(() =>
            {
                ChangeRemoteUserPublishState(MeetingCanvasUserList, id, type, true);
            });
        }

        private void MeetingOnRemoteUnpublished(String id, RCRTCMediaType type)
        {
            RunOnMainThread.Enqueue(() =>
            {
                ChangeRemoteUserPublishState(MeetingCanvasUserList, id, type, false);
            });
        }

        private void HostOnUserJoined(String id)
        {
            RunOnMainThread.Enqueue(() =>
            {
                AddUserItemToUserList(id, HostCanvasUserList);
            });
        }

        private void HostOnUserOffline(String id)
        {
            RunOnMainThread.Enqueue(() =>
            {
                RemoveUserItemFromUserList(id, HostCanvasUserList);
            });
        }

        private void HostOnUserLeft(String id)
        {
            RunOnMainThread.Enqueue(() =>
            {
                RemoveUserItemFromUserList(id, HostCanvasUserList);
            });
        }

        private void HostOnRemotePublished(String id, RCRTCMediaType type)
        {
            RunOnMainThread.Enqueue(() =>
            {
                ChangeRemoteUserPublishState(HostCanvasUserList, id, type, true);
            });
        }

        private void HostOnRemoteUnpublished(String id, RCRTCMediaType type)
        {
            RunOnMainThread.Enqueue(() =>
            {
                ChangeRemoteUserPublishState(HostCanvasUserList, id, type, false);
            });
        }

        private void AudienceOnUserJoined(String id)
        {
            Debug.Log($"AudienceOnUserJoined {id}");
        }

        private void AudienceOnUserOffline(String id)
        {
            Debug.Log($"AudienceOnUserOffline {id}");
        }

        private void AudienceOnUserLeft(String id)
        {
            Debug.Log($"AudienceOnUserLeft {id}");
        }

        private string toString(RCRTCMediaType type)
        {
            if (type == RCRTCMediaType.AUDIO) return "AUDIO";
            if (type == RCRTCMediaType.VIDEO) return "VIDEO";
            if (type == RCRTCMediaType.AUDIO_VIDEO) return "AUDIO_VIDEO";
            return string.Empty;
        }
        
        private void AudienceOnRemoteLiveMixPublished(RCRTCMediaType type)
        {
            RunOnMainThread.Enqueue(() =>
            {
                // AudienceLiveStreamText.text = toString(type);
                AudienceSubscribeButton.interactable = true;
            });
            
        }
        
        private void AudienceOnRemoteLiveMixUnpublished(RCRTCMediaType type)
        {
           RunOnMainThread.Enqueue(() =>
           {
               AudienceLiveStreamText.text = string.Empty;
               AudienceSubscribeButton.transform.Find("Text").GetComponent<Text>().text = "订阅";
               AudienceSubscribeButton.interactable = false;
           }); 
        }
#endregion

#region Private Methods

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

        public void OnClickInit()
        {
#if UNITY_ANDROID
            RCIMClient.Instance.EnablePush(mPushEnabled);
            RCIMClient.Instance.DisableIPC(mDisableIPC);
            RCIMClient.Instance.Init(ExampleConfig.AppKey);
            GameObject.Find("/Connect/Init/Text").GetComponent<Text>().text = "已初始化";
            InitButton.enabled = false;
#endif
        }
        public GameObject NetworkStatsPrefab;
        public GameObject AudioStatsPrefab;
        public GameObject VideoStats;
        public GameObject UserItemPrefab;
        public GameObject MessagePrefab;
        public GameObject CDNSelectorPrefab;
        public GameObject CDNItemPrefab;
        public GameObject AudioEffectPrefab;
        public GameObject DeviceItemPrefab;

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
        private AudioSource MeetingAudioSource;
        private GameObject MeetingCanvasPlayBackgroundMusicSwitchButton;
        private Text MeetingCanvasPlayBackgroundMusicSwitchInfo;
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
        private Button HostCanvasConfigCDN;
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
        private Text AudienceLiveStreamText;
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

        private GameObject LoadingCanvas;
        private int LoadingCount;

        private GameObject HostMixConfig;
        private GameObject HostMixVideoStreamConfig;
        private GameObject HostMixTinyVideoStreamConfig;
        private GameObject HostMixVideoCustomLayoutConfig;
        private GameObject HostMixAddVideoCustomLayout;
        private GameObject HostMixAudioConfig;
        private GameObject HostMixAudioCustomConfig;

        private GameObject DeviceListCanvas;
        private GameObject DeviceList;

        private CanvasGroup Toast;
        private Text ToastInfo;
        private IEnumerator CurrentToast;

        private String RoomId;

        private RCRTCRole Role;
        private LoginData User;
        private bool Connected;

        private RCRTCEngine Engine = null;

        private bool PlayBackgroundMusic;

        private bool Speaker;
        private bool Microphone;
        private bool Camera;
        private bool PublishAudio;
        private bool PublishVideo;
        private bool InChatRoom;
        private bool CloseMessagePanel;
        private RCRTCAudioMixingMode AudioMixingMode = RCRTCAudioMixingMode.MIX;

        private Dictionary<String, GameObject> Users;
        private Dictionary<String, bool> UserSubscribeAudioStates;
        private Dictionary<String, bool> UserSubscribeVideoStates;
        private Dictionary<String, GameObject> Statses;
        private List<GameObject> Messages;
        private Dictionary<CDN, GameObject> CDNs;
        private List<GameObject> SelectedCDNs;
        private Dictionary<int, GameObject> AudioEffects;

        private static readonly String[] AudioEffectNames = {
            "反派大笑",
            "狗子叫声",
            "胜利号角",
        };
        private const String AudioEffectFilePathFormat = "effect_{0}.mp3";

        private const String AudioMixFilePath = "music_0.mp3";

        private Toggle PushEnabled;
        private Toggle IpcDisEnabled;
        private Button InitButton;
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
                        OnClickDeviceListClose();
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

    public class LoginData
    {
        public String Name;

        [SerializeField]
        private String userId;
        public String Id { get { return userId; } private set { Id = value; } }

        [SerializeField]
        private String token;
        public String Token { get { return token; } private set { Token = value; } }
    }

    public class Message
    {
        public Message(String name, String content)
        {
            this.name = name;
            this.content = content;
        }

        public String name;
        public String content;
    }

    public class CDN
    {
        public CDN(String id, String name)
        {
            this.id = id;
            this.name = name;
        }
        public String id;
        public String name;
    }

    public class CDNInfo
    {
        public String push;
        public String rtmp;
        public String hls;
        public String flv;
    }

    public class StatsListenerProxy : RCRTCStatsListener
    {
        public StatsListenerProxy(OnNetworkStatsDelegate ons, OnLocalAudioStatsDelegate olas, OnLocalVideoStatsDelegate olvs, OnRemoteAudioStatsDelegate oras, OnRemoteVideoStatsDelegate orvs)
        {
            ONSDelegate = ons;
            OLASDelegate = olas;
            OLVSDelegate = olvs;
            ORASDelegate = oras;
            ORVSDelegate = orvs;
        }

        public void OnNetworkStats(RCRTCNetworkStats stats)
        {
            ONSDelegate.Invoke(stats);
        }

        public void OnLocalAudioStats(RCRTCLocalAudioStats stats)
        {
            OLASDelegate.Invoke(stats);
        }

        public void OnLocalVideoStats(RCRTCLocalVideoStats stats)
        {
            OLVSDelegate.Invoke(stats);
        }

        public void OnRemoteAudioStats(String userId, RCRTCRemoteAudioStats stats)
        {
            ORASDelegate.Invoke(userId, stats);
        }

        public void OnRemoteVideoStats(String userId, RCRTCRemoteVideoStats stats)
        {
            ORVSDelegate.Invoke(userId, stats);
        }

        public void OnLiveMixAudioStats(RCRTCRemoteAudioStats stats)
        {
        }

        public void OnLiveMixVideoStats(RCRTCRemoteVideoStats stats)
        {
        }

        public void OnLocalCustomAudioStats(String tag, RCRTCLocalAudioStats stats)
        {
        }

        public void OnLocalCustomVideoStats(String tag, RCRTCLocalVideoStats stats)
        {
        }

        public void OnRemoteCustomAudioStats(String userId, String tag, RCRTCRemoteAudioStats stats)
        {
        }

        public void OnRemoteCustomVideoStats(String userId, String tag, RCRTCRemoteVideoStats stats)
        {
        }

        private readonly OnNetworkStatsDelegate ONSDelegate;
        private readonly OnLocalAudioStatsDelegate OLASDelegate;
        private readonly OnLocalVideoStatsDelegate OLVSDelegate;
        private readonly OnRemoteAudioStatsDelegate ORASDelegate;
        private readonly OnRemoteVideoStatsDelegate ORVSDelegate;
    }

    public class LiveStatsListenerProxy : RCRTCStatsListener
    {
        public LiveStatsListenerProxy(OnNetworkStatsDelegate ons, OnLiveMixAudioStatsDelegate oras, OnLiveMixVideoStatsDelegate orvs)
        {
            ONSDelegate = ons;
            OLMASDelegate = oras;
            OLMVSDelegate = orvs;
        }

        public void OnNetworkStats(RCRTCNetworkStats stats)
        {
            ONSDelegate.Invoke(stats);
        }

        public void OnLocalAudioStats(RCRTCLocalAudioStats stats)
        {
        }

        public void OnLocalVideoStats(RCRTCLocalVideoStats stats)
        {
        }

        public void OnRemoteAudioStats(String userId, RCRTCRemoteAudioStats stats)
        {
        }

        public void OnRemoteVideoStats(String userId, RCRTCRemoteVideoStats stats)
        {
        }

        public void OnLiveMixAudioStats(RCRTCRemoteAudioStats stats)
        {
            OLMASDelegate.Invoke(stats);
        }

        public void OnLiveMixVideoStats(RCRTCRemoteVideoStats stats)
        {
            OLMVSDelegate.Invoke(stats);
        }

        public void OnLocalCustomAudioStats(String tag, RCRTCLocalAudioStats stats)
        {
        }

        public void OnLocalCustomVideoStats(String tag, RCRTCLocalVideoStats stats)
        {
        }

        public void OnRemoteCustomAudioStats(String userId, String tag, RCRTCRemoteAudioStats stats)
        {
        }

        public void OnRemoteCustomVideoStats(String userId, String tag, RCRTCRemoteVideoStats stats)
        {
        }

        private readonly OnNetworkStatsDelegate ONSDelegate;
        private readonly OnLiveMixAudioStatsDelegate OLMASDelegate;
        private readonly OnLiveMixVideoStatsDelegate OLMVSDelegate;
    }

    public delegate void OnNetworkStatsDelegate(RCRTCNetworkStats stats);
    public delegate void OnLocalAudioStatsDelegate(RCRTCLocalAudioStats stats);
    public delegate void OnLocalVideoStatsDelegate(RCRTCLocalVideoStats stats);
    public delegate void OnRemoteAudioStatsDelegate(String userId, RCRTCRemoteAudioStats stats);
    public delegate void OnRemoteVideoStatsDelegate(String userId, RCRTCRemoteVideoStats stats);
    public delegate void OnLiveMixAudioStatsDelegate(RCRTCRemoteAudioStats stats);
    public delegate void OnLiveMixVideoStatsDelegate(RCRTCRemoteVideoStats stats);

}