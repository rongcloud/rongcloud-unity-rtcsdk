using cn_rongcloud_rtc_unity;
using UnityEngine;
using UnityEngine.UI;

namespace cn_rongcloud_rtc_unity_example
{
    public class VideoConfigPrefab : MonoBehaviour
    {
        public delegate void ConfigValueChanged(RCRTCVideoConfig videoConfig);
        
        public enum MinBitrateValue
        {
            bitrate_300kbps,
            bitrate_500kbps,
            bitrate_700kbps,
            bitrate_900kbps,
            bitrate_1200kbps,
            bitrate_1500kbps
        }

        public enum MaxBitrateValue
        {
            bitrate_2000kbps,
            bitrate_2200kbps,
            bitrate_3500kbps,
            bitrate_4400kbps,
            bitrate_6000kbps,
            bitrate_8000kbps
        }

        public ConfigValueChanged OnValueChanged;
        
        [SerializeField] public GameObject VideoConfigObject;
        private RCRTCVideoConfig _videoConfig;

        public RCRTCVideoFps ffps;

        public MinBitrateValue minBitRate;

        public MaxBitrateValue maxBitRate;

        public RCRTCVideoResolution resolution;

        public RCRTCVideoConfig VideoConfig => _videoConfig;

        public RCRTCVideoFps FPS => ffps;

        public RCRTCVideoResolution Resolution => resolution;
        public int MinBitRate
        {
            get
            {
                if (minBitRate == MinBitrateValue.bitrate_300kbps) return 300;
                if (minBitRate == MinBitrateValue.bitrate_500kbps) return 500;
                if (minBitRate == MinBitrateValue.bitrate_700kbps) return 700;
                if (minBitRate == MinBitrateValue.bitrate_900kbps) return 900;
                if (minBitRate == MinBitrateValue.bitrate_1200kbps) return 1200;
                if (minBitRate == MinBitrateValue.bitrate_1500kbps) return 1500;
                return 700;
            }
        }

        public int MaxBitRate
        {
            get
            {
                if (maxBitRate == MaxBitrateValue.bitrate_2000kbps) return 2000;
                if (maxBitRate == MaxBitrateValue.bitrate_2200kbps) return 2200;
                if (maxBitRate == MaxBitrateValue.bitrate_3500kbps) return 3500;
                if (maxBitRate == MaxBitrateValue.bitrate_4400kbps) return 4400;
                if (maxBitRate == MaxBitrateValue.bitrate_6000kbps) return 2000;
                if (maxBitRate == MaxBitrateValue.bitrate_8000kbps) return 2000;
                return 2200;
            }
        }

        public void Start()
        {
            if (VideoConfigObject != null)
            {
                VideoConfigObject.transform.Find("FpsOptions").GetComponent<Dropdown>().value = (int)FPS;
                VideoConfigObject.transform.Find("FpsOptions").GetComponent<Dropdown>().onValueChanged.AddListener(OnDropDownFps);
                VideoConfigObject.transform.Find("ResolutionOptions").GetComponent<Dropdown>().value = (int) resolution;
                VideoConfigObject.transform.Find("ResolutionOptions").GetComponent<Dropdown>().onValueChanged.AddListener(OnDropDownResolution);
                VideoConfigObject.transform.Find("MinBitrateOptions").GetComponent<Dropdown>().value = (int) minBitRate;
                VideoConfigObject.transform.Find("MinBitrateOptions").GetComponent<Dropdown>().onValueChanged.AddListener(OnDropDownMinBitrate);
                VideoConfigObject.transform.Find("MaxBitrateOptions").GetComponent<Dropdown>().value = (int) maxBitRate;
                VideoConfigObject.transform.Find("MaxBitrateOptions").GetComponent<Dropdown>().onValueChanged.AddListener(OnDropDownMaxBitrate);
            }

            _videoConfig.SetResolution(Resolution);
            _videoConfig.SetMaxBitrate(MaxBitRate);
            _videoConfig.SetMinBitrate(MinBitRate);
            _videoConfig.SetFps(FPS);
        }

        private void OnDropDownFps(int value)
        {
            ffps = (RCRTCVideoFps)value;
            _videoConfig.SetFps(FPS);

            if (OnValueChanged != null)
            {
                OnValueChanged(_videoConfig);
            }
        }
        
        private void OnDropDownResolution(int value)
        {
            this.resolution = (RCRTCVideoResolution)value;
            _videoConfig.SetResolution(this.Resolution);
            if (OnValueChanged != null)
            {
                OnValueChanged(_videoConfig);
            }
        }
        
        private void OnDropDownMinBitrate(int value)
        {
            minBitRate = (MinBitrateValue) value;
            _videoConfig.SetMinBitrate(MinBitRate);
            if (OnValueChanged != null)
            {
                OnValueChanged(_videoConfig);
            }
        }
        
        private void OnDropDownMaxBitrate(int value)
        {
            maxBitRate = (MaxBitrateValue) value;
            _videoConfig.SetMaxBitrate(MaxBitRate);
            if (OnValueChanged != null)
            {
                OnValueChanged(_videoConfig);
            }
        }
    }
}