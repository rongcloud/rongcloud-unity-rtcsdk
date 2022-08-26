using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiveMixAudioBitrate : MonoBehaviour
{
    public GameObject AudioBitrateObject;
    
    public enum BitrateEnum
    {
        bitrate_16kbps,
        bitrate_32kbps,
        bitrate_48kbps,
    }
    
    public BitrateEnum Bitrate { get; private set; } = BitrateEnum.bitrate_32kbps;

    public int BitRateValue
    {
        get
        {
            if (Bitrate == BitrateEnum.bitrate_16kbps) return 16;
            if (Bitrate == BitrateEnum.bitrate_32kbps) return 32;
            if (Bitrate == BitrateEnum.bitrate_48kbps) return 48;

            return 48;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        if (AudioBitrateObject != null)
        {
            AudioBitrateObject.transform.Find("BitrateOptions").GetComponent<Dropdown>().value = (int) Bitrate;
            AudioBitrateObject.transform.Find("BitrateOptions").GetComponent<Dropdown>().onValueChanged
                .AddListener(OnBitrateDropDownChanged);

        }
    }

    private void OnBitrateDropDownChanged(int value)
    {
        Bitrate = (BitrateEnum) value;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}