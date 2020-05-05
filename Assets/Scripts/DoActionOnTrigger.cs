using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class DoActionOnTrigger : MonoBehaviour
{
    public UnityEvent Event;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Event?.Invoke();
    }
}
