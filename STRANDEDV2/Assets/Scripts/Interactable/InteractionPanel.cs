using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class InteractionPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _beforeText;
    [SerializeField] TMP_Text _interactingText;
    [SerializeField] TMP_Text _completedInteractionText;
    [SerializeField] Image _progressBarFilledImage;
    [SerializeField] GameObject _progressBar;

    void OnEnable()
    {
        _beforeText.enabled = false;
        _completedInteractionText.enabled = false;

        //Interactable.InteractablesInRangeChanged += UpdateHintTextState;
        FindObjectOfType<InteractionManager>().CurrentInteractableChanged += UpdateInteractionText;
        Interactable.AnyInteractionComplete += ShowCompletedInspectionText;
    }

    void UpdateInteractionText(Interactable interactable)
    {
        if (interactable == null)
            _beforeText.enabled = false;
        else
        {
            var interactionType = interactable.InteractionType;
            _beforeText.SetText($"{interactionType.Hotkey} - {interactionType.BeforeInteraction}");
            _beforeText.enabled = true;
            _interactingText.SetText(interactionType.DuringInteraction);
        }
    }

    void ShowCompletedInspectionText(Interactable inspectable, string message)
    {
        _completedInteractionText.SetText(message);
        _completedInteractionText.enabled = true;
        float msgTime = message.Length / 5f;
        msgTime = Mathf.Clamp(msgTime, 3f, 15f);
        StartCoroutine(FadeCompletedText(msgTime));
    }

    IEnumerator FadeCompletedText(float msgTime)
    {
        _completedInteractionText.alpha = 1f;
        while (_completedInteractionText.alpha > 0)
        {
            yield return null;
            _completedInteractionText.alpha -= Time.deltaTime / msgTime;
        }
        _completedInteractionText.enabled = false;
    }

    void OnDisable() => Interactable.InteractablesInRangeChanged -= UpdateHintTextState;

    void UpdateHintTextState(bool enableHint) => _beforeText.enabled = enableHint;

    void Update()
    {
        if (InteractionManager.Interacting)
        {
            _progressBarFilledImage.fillAmount = InteractionManager.InteractionProgress;
            _progressBar.SetActive(true);
        }
        else if (_progressBar.activeSelf) { 
        _progressBar.SetActive(false);
    }
    }
}