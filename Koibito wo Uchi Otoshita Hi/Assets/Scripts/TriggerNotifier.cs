using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerNotifier : MonoBehaviour
{
    public delegate void TriggerEvent();
    public static event TriggerEvent OnEnterLockArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LockArea")) {
            OnEnterLockArea?.Invoke();
        }
    }
}
