using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;

namespace cn_rongcloud_rtc_unity_example
{
    public class ExampleUtils : MonoBehaviour
    {
        public GameObject Loading;
        public CanvasGroup Toast;
        public Text ToastInfo;
        public GameObject Alert;
        public GameObject AlertBtn;

        private int LoadingCount;
        private IEnumerator CurrentToast;

        private static ExampleUtils utils;

        private void Start()
        {
            utils = this;
        }

        public static void ShowLoading()
        {
            utils._ShowLoading();
        }

        public static void HideLoading()
        {
            utils._HideLoading();
        }

        public static void ShowToast(String toast, int duration = 2)
        {
            utils._ShowToast(toast, duration);
        }

        public static void ShowAlert(String title, String info, AlertAction[] actions)
        {
            utils._ShowAlert(title, info, actions);
        }

        public static void PickImage(Action<string> action)
        {
#if UNITY_IOS
            ShowLoading();
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                HideLoading();
                if (path != null)
                {
                    Debug.Log("Image path: " + path);
#if UNITY_ANDROID
                    action($"file://{path}");
#else
                    action(path);
#endif
                }
            });

            if (permission != NativeGallery.Permission.Granted)
            {
                ShowToast("没有权限");
                HideLoading();
            }
            Debug.Log("Permission result: " + permission);
#else
            ShowLoading();
            FileBrowser.SetFilters(false, new FileBrowser.Filter("Images", ".jpg", ".png"));
            FileBrowser.ShowLoadDialog((string[] paths) =>
            {
                HideLoading();
                if (paths.Length > 0)
                {
                    Debug.Log("File path: " + paths[0]);
                    action(paths[0]);
                }
            }, () =>
            {
                HideLoading();
            }, FileBrowser.PickMode.Files);    
#endif
        }

        public static void PickVideo(Action<string,int> action)
        {
#if UNITY_IOS
            ShowLoading();
            NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
            {
                HideLoading();
                if (path != null)
                {
                    Debug.Log("Video path: " + path);
                    var property = NativeGallery.GetVideoProperties(path);
#if UNITY_ANDROID
                    action($"file://{path}", (int)property.duration / 1000);
#else
                    action(path, (int)property.duration / 1000);
#endif
                }
            });

            if (permission != NativeGallery.Permission.Granted)
            {
                ShowToast("没有权限");
                HideLoading();
            }
            Debug.Log("Permission result: " + permission);
#else
            ShowLoading();
            FileBrowser.SetFilters(false, new FileBrowser.Filter("Videos", ".mp4", ".mkv", ".mov"));
            FileBrowser.ShowLoadDialog((string[] paths) =>
            {
                HideLoading();
                if (paths.Length > 0)
                {
                    Debug.Log("File path: " + paths[0]);
                    action(paths[0], 15);
                }
            }, () =>
            {
                HideLoading();
            }, FileBrowser.PickMode.Files);
#endif
        }

        public static void PickAudio(Action<string, int> action)
        {
            ShowLoading();
            FileBrowser.SetFilters(false, new FileBrowser.Filter("Audios", ".mp3", ".aac", ".amr"));
            FileBrowser.ShowLoadDialog((string[] paths) =>
            {
                HideLoading();
                if (paths.Length > 0)
                {
                    Debug.Log("File path: " + paths[0]);
                    action(paths[0], 15);
                }
            }, () =>
            {
                HideLoading();
            }, FileBrowser.PickMode.Files);
        }

        public static void PickFile(Action<string> action)
        {
            ShowLoading();
            FileBrowser.SetFilters(true);
            FileBrowser.ShowLoadDialog((string[] paths) =>
            {
                HideLoading();
                if (paths.Length > 0)
                {
                    Debug.Log("File path: " + paths[0]);
                    action(paths[0]);
                }
            }, () =>
            {
                HideLoading();
            }, FileBrowser.PickMode.Files);
        }

        public static void PickFile(string type, Action<string> action)
        {
            ShowLoading();
            FileBrowser.SetFilters(false, new FileBrowser.Filter("Custom", type));
            FileBrowser.ShowLoadDialog((string[] paths) =>
            {
                HideLoading();
                if (paths.Length > 0)
                {
                    Debug.Log("File path: " + paths[0]);
                    action(paths[0]);
                }
            }, () =>
            {
                HideLoading();
            }, FileBrowser.PickMode.Files);
        }

        public class AlertAction
        {
            public string title;
            public Action action;
        }

        private void _ShowAlert(string title, string info, AlertAction[] actions)
        {
            Alert.transform.Find("View/Title").GetComponent<Text>().text = title;
            Alert.transform.Find("View/Text").GetComponent<Text>().text = info;
            Transform tf = Alert.transform.Find("View/Btns").GetComponent<Transform>();
            int count = tf.childCount;
            for (int i = 0; i < count; i++)
            {
                Destroy(tf.GetChild(i).gameObject);
            }
            foreach (var item in actions)
            {
                GameObject obj = Instantiate(AlertBtn, tf) as GameObject;
                obj.SetActive(true);
                obj.GetComponentInChildren<Text>().text = item.title;
                Button btn = obj.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    _AlertActionClick(item);
                });
            }
            Alert.SetActive(true);
        }

        private void _AlertActionClick(AlertAction action)
        {
            action.action();
            Alert.SetActive(false);
        }

        private void _ShowLoading()
        {
            LoadingCount++;
            if (!Loading.activeInHierarchy)
            {
                Loading.SetActive(true);
            }
        }

        private void _HideLoading()
        {
            LoadingCount--;
            if (LoadingCount <= 0 && Loading.activeInHierarchy)
            {
                LoadingCount = 0;
                Loading.SetActive(false);
            }
        }

        private void _ShowToast(String toast, int duration)
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
    }
}