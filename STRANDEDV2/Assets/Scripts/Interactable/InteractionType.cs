using UnityEngine;

[CreateAssetMenu(menuName = "Interaction Type")]

public class InteractionType : ScriptableObject
{
    public KeyCode Hotkey = KeyCode.E;
    public string BeforeInteraction;
    public string DuringInteraction;
    public string CompletedInteraction;
    public bool IsDefault;
}