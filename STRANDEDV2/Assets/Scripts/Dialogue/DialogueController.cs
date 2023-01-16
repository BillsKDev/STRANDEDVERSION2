using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;
using System.Text;

public class DialogueController : ToggleablePanel
{
    Story _story;

    [SerializeField] TMP_Text _storyText;
    [SerializeField] Button[] _choiceButtons;

    public void StartDialog(TextAsset dialog)
    {
        _story = new Story(dialog.text);
        RefreshView();
        Show();
    }
    
    void RefreshView()
    {
        StringBuilder storyText = new StringBuilder();
        while (_story.canContinue)
        {
            storyText.AppendLine(_story.Continue());
            HandleTags();
        }

        _storyText.SetText(storyText);

        if (_story.currentChoices.Count == 0)
            Hide();
        else
            ShowChoiceButtons();
    }

    void ShowChoiceButtons()
    {
        for (int i = 0; i < _choiceButtons.Length; i++)
        {
            var button = _choiceButtons[i];
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(i < _story.currentChoices.Count);
            if (i < _story.currentChoices.Count)
            {
                var choice = _story.currentChoices[i];
                button.GetComponentInChildren<TMP_Text>().SetText(choice.text);
                button.onClick.AddListener(() =>
                {
                    _story.ChooseChoiceIndex(choice.index);
                    RefreshView();
                });
            }
        }
    }

    void HandleTags()
    {
        foreach (var tag in _story.currentTags)
        {
            if (tag.StartsWith("E."))
            {
                string eventName = tag.Remove(0, 2);
                GameEvent.RaiseEvent(eventName);
            }
        }
    }
}