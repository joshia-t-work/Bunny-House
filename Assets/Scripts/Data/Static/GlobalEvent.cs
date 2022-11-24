using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.Data.Events
{
    [CreateAssetMenu(fileName = "Global Event", menuName = "SO/Global Event")]
    public class GlobalEvent : ScriptableObject
    {
        List<System.Action> actions = new List<System.Action>();
        public void AddListener(System.Action action)
        {
            actions.Add(action);
        }
        public void RemoveListener(System.Action action)
        {
            actions.Remove(action);
        }
        public void Invoke()
        {
            foreach (System.Action action in actions)
            {
                action();
            }
        }
    }
}