using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cn_rongcloud_rtc_unity
{
    public class HostMorePanel : MonoBehaviour
    {
        public GameObject thirdCDN;
        public GameObject innerCDN;
        public GameObject mixConfig;

        /// <summary>
        /// 是否发布
        /// </summary>
        [HideInInspector]
        public bool Published
        {
            set
            {
                thirdCDN.transform.GetComponent<Button>().interactable = value;
                innerCDN.transform.GetComponent<Button>().interactable = value;
                mixConfig.transform.GetComponent<Button>().interactable = value;
            }
        }

    }
}

