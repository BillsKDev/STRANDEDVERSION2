using System.Collections;
using UnityEngine;

public class FewGuidePanels : MonoBehaviour
{
    public GameObject panel;

    public void ShowTipFew()
    {
        panel.SetActive(true);
        FindObjectOfType<AudioManager>().Play("QuestGuide");
        StartCoroutine(ShowPanel());
    }

    IEnumerator ShowPanel()
    {
        yield return new WaitForSeconds(8f);
        panel.SetActive(false);
    }
}
