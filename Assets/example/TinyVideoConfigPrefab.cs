using cn_rongcloud_rtc_unity;
using UnityEngine;
using UnityEngine.UI;

namespace cn_rongcloud_rtc_unity_example
{
    public class TinyVideoConfigPrefab : MonoBehaviour
    {
        public delegate void ConfigValueChanged(RCRTCVideoConfig videoConfig);

        public enum MinBitrateValue
        {
            bitrate_50kbps,
            bitrate_100kbps,
            bitrate_200kbps,
            bitrate_300kbps,
        }

        public enum MaxBitrateValue
        {
            bitrate_300kbps,
            bitrate_500kbps,
            bitrate_800kbps,
            bitrate_1000kbps,
            bitrate_1200kbps,
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
                if (minBitRate == MinBitrateValue.bitrate_50kbps) return 50;
                if (minBitRate == MinBitrateValue.bitrate_100kbps) return 100;
                if (minBitRate == MinBitrateValue.bitrate_200kbps) return 200;
                if (minBitRate == MinBitrateValue.bitrate_300kbps) return 300;
                return 100;
            }
        }

        public int MaxBitRate
        {
            get
            {
                if (maxBitRate == MaxBitrateValue.bitrate_300kbps) return 300;
                if (maxBitRate == MaxBitrateValue.bitrate_500kbps) return 500;
                if (maxBitRate == MaxBitrateValue.bitrate_800kbps) return 800;
                if (maxBitRate == MaxBitrateValue.bitrate_1000kbps) return 1000;
                if (maxBitRate == MaxBitrateValue.bitrate_1200kbps) return 1200;
                return 500;
            }
        }

        public void Start()
        {
            if (VideoConfigObject != null)
            {
                VideoConfigObject.transform.Find("ResolutionOptions").GetComponent<Dropdown>().value = (int)resolution;
                VideoConfigObject.transform.Find("ResolutionOptions").GetComponent<Dropdown>().onValueChanged.AddListener(OnDropDownResolution);
                VideoConfigObject.transform.Find("MinBitrateOptions").GetComponent<Dropdown>().value = (int)minBitRate;
                VideoConfigObject.transform.Find("MinBitrateOptions").GetComponent<Dropdown>().onValueChanged.AddListener(OnDropDownMinBitrate);
                VideoConfigObject.transform.Find("MaxBitrateOptions").GetComponent<Dropdown>().value = (int)maxBitRate;
                VideoConfigObject.transform.Find("MaxBitrateOptions").GetComponent<Dropdown>().onValueChanged.AddListener(OnDropDownMaxBitrate);
            }

            _videoConfig.SetResolution(Resolution);
            _videoConfig.SetMaxBitrate(MaxBitRate);
            _videoConfig.SetMinBitrate(MinBitRate);
            _videoConfig.SetFps(FPS);
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
            minBitRate = (MinBitrateValue)value;
            _videoConfig.SetMinBitrate(MinBitRate);
            if (OnValueChanged != null)
            {
                OnValueChanged(_videoConfig);
            }
        }

        private void OnDropDownMaxBitrate(int value)
        {
            maxBitRate = (MaxBitrateValue)value;
            _videoConfig.SetMaxBitrate(MaxBitRate);
            if (OnValueChanged != null)
            {
                OnValueChanged(_videoConfig);
            }
        }
    }
}