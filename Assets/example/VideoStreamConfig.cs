using System;
using System.Collections;
using System.Collections.Generic;
using cn_rongcloud_rtc_unity;
using UnityEngine;
using UnityEngine.UI;

public class VideoStreamConfig : MonoBehaviour
{
    public GameObject VideoStreamConfigObject;

    public enum BitrateEnum
    {
        bitrate_2000kbps,
        bitrate_2200kbps,
        bitrate_3500kbps,
        bitrate_4400kbps,
        bitrate_6000kbps,
        bitrate_8000kbps,
    }

    public BitrateEnum Bitrate { get; private set; } = BitrateEnum.bitrate_2200kbps;

    public int BitRateValue
    {
        get
        {
            if (Bitrate == BitrateEnum.bitrate_2000kbps) return 2000;
            if (Bitrate == BitrateEnum.bitrate_2200kbps) return 2200;
            if (Bitrate == BitrateEnum.bitrate_3500kbps) return 3500;
            if (Bitrate == BitrateEnum.bitrate_4400kbps) return 4400;
            if (Bitrate == BitrateEnum.bitrate_6000kbps) return 2000;
            if (Bitrate == BitrateEnum.bitrate_8000kbps) return 2000;

            return 2200;
        }
    }

    public RCRTCVideoFps FPS { get; private set; } = RCRTCVideoFps.FPS_24;
    public RCRTCVideoResolution Resolution { get; private set; } = RCRTCVideoResolution.RESOLUTION_720_1280;

    // Start is called before the first frame update
    void Start()
    {
        if (VideoStreamConfigObject != null)
        {
            VideoStreamConfigObject.transform.Find("BitrateOptions").GetComponent<Dropdown>().value = (int) Bitrate;
            VideoStreamConfigObject.transform.Find("BitrateOptions").GetComponent<Dropdown>().onValueChanged
                .AddListener(OnBitrateDropDownChanged);

            VideoStreamConfigObject.transform.Find("FpsOptions").GetComponent<Dropdown>().value = (int) FPS;
            VideoStreamConfigObject.transform.Find("FpsOptions").GetComponent<Dropdown>().onValueChanged
                .AddListener(OnFPSDropDownChanged);

            VideoStreamConfigObject.transform.Find("ResolutionOptions").GetComponent<Dropdown>().value =
                (int) Resolution;
            VideoStreamConfigObject.transform.Find("ResolutionOptions").GetComponent<Dropdown>().onValueChanged
                .AddListener(OnResolutionDropDownChanged);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnBitrateDropDownChanged(int value)
    {
        Bitrate = (BitrateEnum) value;
    }

    private void OnFPSDropDownChanged(int value)
    {
        FPS = (RCRTCVideoFps) value;
    }

    private void OnResolutionDropDownChanged(int value)
    {
        Resolution = (RCRTCVideoResolution) value;
    }
}