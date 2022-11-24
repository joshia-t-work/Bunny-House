using BunnyHouse.Core;
using BunnyHouse.Data.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BunnyHouse.UI
{
    public class UIChangeScene : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            SceneSystem.LoadScene(sceneName);
        }
    }
}