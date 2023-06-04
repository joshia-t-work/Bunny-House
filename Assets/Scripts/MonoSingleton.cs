using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BunnyHouse.Core
{
    /// <summary>
    /// Singleton base class, used to differentiate scripts located in the main singleton
    /// </summary>
    public class MonoSingleton : MonoBehaviour
    {
        public virtual void MonoAwake()
        {

        }
        public virtual void MonoUpdate()
        {

        }
        public virtual void MonoOnApplicationPause()
        {

        }
    }
}
