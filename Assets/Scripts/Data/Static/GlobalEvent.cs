using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.Data.Events
{
    /// <summary>
    /// SO for listening and invoking calls
    /// </summary>
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
            List<System.Action> tempActions = new List<System.Action>(actions);
            foreach (System.Action action in tempActions)
            {
                action();
            }
        }
    }
}