using UnityEngine;
using UnityEngine.Events;

public class TextCounter : MonoBehaviour
{
    [SerializeField] UnityEvent newPanel;

    void Update()
    {
        if (WoodCounter.wood == 2 && StoneCounter.stone == 1)
            newPanel.Invoke();
    }
}
