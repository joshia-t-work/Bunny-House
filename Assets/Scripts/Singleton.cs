using BunnyHouse.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BunnyHouse.Core
{
    public class Singleton : MonoBehaviour
    {
        public static bool isUIOverride;
        public static Singleton Instance;
        static TaskSystem TaskSystem;
        static SceneSystem SceneSystem;
        static DataSystem DataSystem;
        List<MonoSingleton> monoSingletons = new List<MonoSingleton>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                TaskSystem = GetComponent<TaskSystem>();
                monoSingletons.Add(TaskSystem);
                SceneSystem = GetComponent<SceneSystem>();
                monoSingletons.Add(SceneSystem);
                DataSystem = GetComponent<DataSystem>();
                monoSingletons.Add(DataSystem);
            }
            else
            {
                Destroy(gameObject);
            }
            for (int i = 0; i < monoSingletons.Count; i++)
            {
                monoSingletons[i].MonoAwake();
            }
        }
        private void Update()
        {
            for (int i = 0; i < monoSingletons.Count; i++)
            {
                monoSingletons[i].MonoUpdate();
            }
            var view = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            var isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
            isUIOverride = EventSystem.current.IsPointerOverGameObject() || (isOutside);
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                for (int i = 0; i < monoSingletons.Count; i++)
                {
                    monoSingletons[i].MonoOnApplicationPause();
                }
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                for (int i = 0; i < monoSingletons.Count; i++)
                {
                    monoSingletons[i].MonoOnApplicationPause();
                }
            }
        }
    }
}
