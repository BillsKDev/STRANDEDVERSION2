using System.Collections.Generic;
using UnityEngine;

public class ToggleablePanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    static HashSet<ToggleablePanel> _visiblePanels = new HashSet<ToggleablePanel>();
    public static bool AnyVisible => _visiblePanels.Count > 0;
    public bool IsVisible => _canvasGroup.alpha > 0;

    [SerializeField] private KeyCode _hotKey;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    void Update()
    {
        if (Input.GetKeyDown(_hotKey))
            ToggleState();
        else if (Input.GetKeyDown(KeyCode.Escape))
            Hide();
    }

    void ToggleState()
    {
        if (IsVisible)
            Hide();
        else
            Show();
    }


    protected void Show()
    {
        _visiblePanels.Add(this);
        _canvasGroup.alpha = 0.9f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    public void Hide()
    {
        _visiblePanels.Remove(this);
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }
}