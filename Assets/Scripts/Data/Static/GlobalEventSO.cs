using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.Data.Events
{
    /// <summary>
    /// SO for listening and invoking calls with an SO parameter
    /// </summary>
    [CreateAssetMenu(fileName = "Global Event SO", menuName = "SO/Global Event SO")]
    public class GlobalEventSO : ScriptableObject
    {
        List<System.Action<ScriptableObject>> actions = new List<System.Action<ScriptableObject>>();
        public void AddListener(System.Action<ScriptableObject> action)
        {
            actions.Add(action);
        }
        public void RemoveListener(System.Action<ScriptableObject> action)
        {
            actions.Remove(action);
        }
        public void Invoke(ScriptableObject scriptableObject)
        {
            List<System.Action<ScriptableObject>> tempActions = new List<System.Action<ScriptableObject>>(actions);
            foreach (System.Action<ScriptableObject> action in tempActions)
            {
                action(scriptableObject);
            }
        }
    }
}