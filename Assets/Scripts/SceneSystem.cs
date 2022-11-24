using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BunnyHouse.Core
{
    public class SceneSystem : MonoSingleton
    {
        public static GameObject Canvas;
        public static Slider LoadingBar;
        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private Slider loadingBar;
        public override void MonoAwake()
        {
            Canvas = canvas;
            LoadingBar = loadingBar;
        }
        public static async void LoadScene(string scene)
        {
            Canvas.SetActive(true);
            await TaskSystem.StopAll();
            Singleton.Instance.StartCoroutine(LoadSceneAsync(scene));
        }
        public static async void LoadScene(int scene)
        {
            Canvas.SetActive(true);
            await TaskSystem.StopAll();
            Singleton.Instance.StartCoroutine(LoadSceneAsync(scene));
        }
        static IEnumerator LoadSceneAsync(string scene)
        {
            Canvas.SetActive(true);
            LoadingBar.value = 0f;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                LoadingBar.value = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                yield return null;
            }
            Canvas.SetActive(false);
        }
        static IEnumerator LoadSceneAsync(int scene)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
