using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GuidePanels : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] UnityEvent newPanel;

    public void ShowTipFew()
    {
        panel.SetActive(true);
        FindObjectOfType<AudioManager>().Play("QuestGuide");
        StartCoroutine(ShowPanel());
    }

    public void ShowTipLong()
    {
        panel.SetActive(true);
        FindObjectOfType<AudioManager>().Play("QuestGuide");
        StartCoroutine(ShowPanelLong());
    }

    public void ShowTip()
    {
        panel.SetActive(true);
        FindObjectOfType<AudioManager>().Play("QuestGuide");
    }

    IEnumerator ShowPanel()
    {
        yield return new WaitForSeconds(10f);
        panel.SetActive(false);
        newPanel.Invoke();
    }

    IEnumerator ShowPanelLong()
    {
        yield return new WaitForSeconds(20f);
        panel.SetActive(false);
        newPanel.Invoke();
    }
}
