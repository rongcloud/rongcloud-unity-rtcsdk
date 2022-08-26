using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cn_rongcloud_rtc_unity_example
{
    public class RoomStreamConfigPrefab : MonoBehaviour
    {
        struct _Values
        {
            public bool isTiny;
            public bool isSrtp;
            public bool saveYUV;
            public bool isAudioOnly;
        }
        
        public enum RoomMode
        {
            MeetingRoom,
            HostRoom,
            AudienceRoom
        }
        
        public void Start()
        {
            values[(int) RoomMode.MeetingRoom] = new _Values();
            values[(int) RoomMode.HostRoom] = new _Values();
            values[(int) RoomMode.AudienceRoom] = new _Values();

            _Values value = new _Values();
            value.isSrtp = SrtpEnabled;
            value.isTiny = TinyEnabled;
            value.isAudioOnly = AudioOnlyMode;
            value.saveYUV = YuvDataSaved;
            values[(int) RoomType] = value;
            UpdatePanel();
        }

        public void Awake()
        {
            values[(int) RoomMode.MeetingRoom] = new _Values();
            values[(int) RoomMode.HostRoom] = new _Values();
            values[(int) RoomMode.AudienceRoom] = new _Values();
            
            _Values value = new _Values();
            value.isSrtp = SrtpEnabled;
            value.isTiny = TinyEnabled;
            value.isAudioOnly = AudioOnlyMode;
            value.saveYUV = YuvDataSaved;
            values[(int) RoomType] = value;
            UpdatePanel();
        }

        public void OnClickedTiny(bool changed)
        {
            TinyEnabled = StreamConfigObject.transform.Find("TinyEnabled").GetComponent<Toggle>().isOn;
            _Values value = values[(int) RoomType];
            value.isTiny = TinyEnabled;
            values[(int) RoomType] = value;
        }
       
        public void OnClickedSrtp(bool changed)
        {
            SrtpEnabled = StreamConfigObject.transform.Find("SRTP").GetComponent<Toggle>().isOn;
            _Values value = values[(int) RoomType];
            value.isSrtp = SrtpEnabled;
            values[(int) RoomType] = value;
        }
        
        public void OnClickedAudioOnly(bool changed)
        {
            AudioOnlyMode = StreamConfigObject.transform.Find("AudioOnly").GetComponent<Toggle>().isOn;
            _Values value = values[(int) RoomType];
            value.isAudioOnly = AudioOnlyMode;
            values[(int) RoomType] = value;
        }
        
        public void OnClickedYUVSaved(bool changed)
        {
            YuvDataSaved = StreamConfigObject.transform.Find("SaveYUV").GetComponent<Toggle>().isOn; 
            _Values value = values[(int) RoomType];
            value.saveYUV = YuvDataSaved;
            values[(int) RoomType] = value;
        }

        public void SetRoomMode(RoomMode mode)
        {
            RoomType = mode;
            UpdatePanel();
        }

        private void UpdatePanel()
        {
            TinyEnabled = values[(int) RoomType].isTiny;
            SrtpEnabled = values[(int) RoomType].isSrtp;
            YuvDataSaved = values[(int) RoomType].saveYUV;
            AudioOnlyMode = values[(int) RoomType].isAudioOnly;
            
            if (RoomType == RoomMode.MeetingRoom)
            {
                StreamConfigObject.transform.Find("SRTP").gameObject.SetActive(true);
                StreamConfigObject.transform.Find("SRTP").GetComponent<Toggle>().isOn = SrtpEnabled;
                StreamConfigObject.transform.Find("SaveYUV").gameObject.SetActive(false);
                StreamConfigObject.transform.Find("AudioOnly").gameObject.SetActive(false);
                StreamConfigObject.transform.Find("AudioVideoEnabled").gameObject.SetActive(false);
                StreamConfigObject.transform.Find("TinyEnabled").gameObject.SetActive(true);
                StreamConfigObject.transform.Find("TinyEnabled").GetComponent<Toggle>().isOn = TinyEnabled;
            }
            else if (RoomType == RoomMode.HostRoom)
            {
                StreamConfigObject.transform.Find("SRTP").gameObject.SetActive(true);
                StreamConfigObject.transform.Find("SRTP").GetComponent<Toggle>().isOn = SrtpEnabled;
                StreamConfigObject.transform.Find("SaveYUV").gameObject.SetActive(false);
                StreamConfigObject.transform.Find("SaveYUV").GetComponent<Toggle>().isOn = YuvDataSaved;
                StreamConfigObject.transform.Find("AudioOnly").gameObject.SetActive(true);
                StreamConfigObject.transform.Find("AudioOnly").GetComponent<Toggle>().isOn = AudioOnlyMode;
                StreamConfigObject.transform.Find("AudioVideoEnabled").gameObject.SetActive(true);
                StreamConfigObject.transform.Find("AudioVideoEnabled").GetComponent<Toggle>().isOn = !AudioOnlyMode;
                StreamConfigObject.transform.Find("TinyEnabled").gameObject.SetActive(true); 
                StreamConfigObject.transform.Find("TinyEnabled").GetComponent<Toggle>().isOn = TinyEnabled; 
            }
            else if (RoomType == RoomMode.AudienceRoom)
            {
                StreamConfigObject.transform.Find("SRTP").gameObject.SetActive(true);
                StreamConfigObject.transform.Find("SRTP").GetComponent<Toggle>().isOn = SrtpEnabled;
                StreamConfigObject.transform.Find("SaveYUV").gameObject.SetActive(false);
                StreamConfigObject.transform.Find("AudioOnly").gameObject.SetActive(false);
                StreamConfigObject.transform.Find("AudioVideoEnabled").gameObject.SetActive(false);
                StreamConfigObject.transform.Find("TinyEnabled").gameObject.SetActive(false);
            }
        }
        
        #region Var
        
        [SerializeField] public GameObject StreamConfigObject;

        [SerializeField] public bool TinyEnabled;
        [SerializeField] public bool SrtpEnabled;
        [SerializeField] public bool YuvDataSaved;
        [SerializeField] public bool AudioOnlyMode;

        public RoomMode RoomType;

        private Dictionary<int, _Values> values = new Dictionary<int, _Values>();

        #endregion
    }
}