using System;
using UnityEngine;
using cn_rongcloud_rtc_unity;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace cn_rongcloud_rtc_unity_example
{
    public class VideoPanelPrefab : MonoBehaviour
    {
        [SerializeField] public RCRTCView videoView;
        private Toggle mirrorToggle;
        private Dropdown fitOptions;
        [SerializeField] public bool isMirror;
        public RCRTCViewFitType fitType;

        public void Awake()
        {
           if (videoView != null)
           {
               videoView.Mirror = isMirror;
               videoView.FitType = fitType;
               mirrorToggle = videoView.transform.Find("Mirror").GetComponent<Toggle>();
               mirrorToggle.isOn = isMirror;
               mirrorToggle.onValueChanged.AddListener(OnClickedMirror);
        
               fitOptions = videoView.transform.Find("FitOptions").GetComponent<Dropdown>();
               fitOptions.onValueChanged.AddListener(OnDropdownValueChanged);
               fitOptions.value = toInt(fitType);

               videoView.transform.Find("UserId").GetComponent<Text>().text = string.Empty;
           }
        }

        public void SetUserId(string userId)
        {
            videoView.transform.Find("UserId").GetComponent<Text>().text = userId;
        }
        
        private void OnClickedMirror(bool changed)
        {
            isMirror = mirrorToggle.isOn; 
            videoView.Mirror = isMirror;
        }

        private void OnDropdownValueChanged(int value)
        {
            fitType = getViewFitType(value);
            videoView.FitType = fitType;
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
        
        private int toInt(RCRTCViewFitType type)
        {
            if (type == RCRTCViewFitType.CENTER)
            {
                return 0;
            }
            else if (type == RCRTCViewFitType.COVER)
            {
                return 1;
            }
            else if (type == RCRTCViewFitType.FILL)
            {
                return 2;
            }

            return 0;
        }
    }
}