using BunnyHouse.Core;
using BunnyHouse.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BunnyHouse.UI
{
    /// <summary>
    /// Updates a slider based on a resource
    /// </summary>
    public class UIBar : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private Resource resource;

        private void Update()
        {
            if (resource.HasMaxValue)
            {
                float res = resource.Get();
                if (res > resource.MaxValue)
                {
                    resource.Set(resource.MaxValue);
                    res = resource.MaxValue;
                }
                slider.value = res;
                slider.maxValue = resource.MaxValue;
            }
        }
    }
}
