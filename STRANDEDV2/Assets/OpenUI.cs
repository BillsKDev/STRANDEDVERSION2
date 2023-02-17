using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OpenUI : MonoBehaviour
{
    bool click = false;
    public UnityEvent onClick;
    public UnityEvent onLeave;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onClick.Invoke();
            click = true;
        }
    }

    private void Update()
    {
        if (ToggleablePanel.AnyVisible == true)
        {
            onLeave.Invoke();
        }
    }
}
