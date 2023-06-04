using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableLinker : MonoBehaviour
{
    [SerializeField] UnityEvent OnEnableCallbacks;
    [SerializeField] UnityEvent OnDisableCallbacks;
    private void OnEnable()
    {
        OnEnableCallbacks.Invoke();
    }
    private void OnDisable()
    {
        OnDisableCallbacks.Invoke();
    }
}
