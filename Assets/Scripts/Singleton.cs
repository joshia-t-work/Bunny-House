using BunnyHouse.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BunnyHouse.Core
{
    public class Singleton : MonoBehaviour
    {
        /// <summary>
        /// Checks if there is an object under mouse
        /// </summary>
        public static bool isUIOverride()
        {
            var view = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            var isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
            bool isOverride = EventSystem.current.IsPointerOverGameObject(-1) || (isOutside);
            for (int i = 0; i < Input.touchCount; i++)
            {
                isOverride = isOverride || EventSystem.current.IsPointerOverGameObject(i);
            }
            return isOverride;
        }
        public static Singleton Instance;
        List<MonoSingleton> monoSingletons = new List<MonoSingleton>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                monoSingletons = new List<MonoSingleton>(GetComponents<MonoSingleton>());
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
