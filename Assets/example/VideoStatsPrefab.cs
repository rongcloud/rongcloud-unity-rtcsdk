using System;
using UnityEngine;
using UnityEngine.UI;

namespace cn_rongcloud_rtc_unity_example
{
    public class VideoStatsPrefab : MonoBehaviour
    {
        [SerializeField] public GameObject VideoStats;

        private bool _dontShowTiny;

        [SerializeField]
        public bool DontShowTiny
        {
            get => _dontShowTiny;

            set
            {
                _dontShowTiny = value;
                CheckActiveTiny();
            }
        }

        public void Start()
        { 
            CheckActiveTiny();
        }

        private void CheckActiveTiny()
        {
            if (VideoStats != null)
            {
                bool active = !DontShowTiny;
                VideoStats.transform.Find("TinyBitrate").gameObject.SetActive(active);
                VideoStats.transform.Find("TinyResolution").gameObject.SetActive(active);
                VideoStats.transform.Find("TinyFps").gameObject.SetActive(active);
                VideoStats.transform.Find("TinyPackageLostRate").gameObject.SetActive(active);
                VideoStats.transform.Find("TinyText").gameObject.SetActive(active);

                if (DontShowTiny)
                {
                    VideoStats.transform.Find("Text").GetComponent<Text>().text = "码率：";
                }
            }
        }
    }
}