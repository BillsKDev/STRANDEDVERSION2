using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class HealthUI : MonoBehaviour
{
    private Health playerHealth;
    [SerializeField] UnityEvent Dead;

    [SerializeField]
    private Image healthFillBar;
    [SerializeField]
    private TextMeshProUGUI healthText;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerMovement>().GetComponent<Health>();
        playerHealth.OnHealthChanged += HandleTookHit;
    }
    
    private void HandleTookHit(int currentHealth, int maxHealth)
    {
        healthText.text = string.Format("{0}/{1}", currentHealth, maxHealth);
        healthFillBar.fillAmount = (float)currentHealth / (float)maxHealth;
        FindObjectOfType<AudioManager>().Play("PlayerHit"); 

        if (currentHealth == 0)
        {
            Dead.Invoke();
        }
    }

    public void StartDying() => StartCoroutine(Die());

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("IslandScene");
    }
}