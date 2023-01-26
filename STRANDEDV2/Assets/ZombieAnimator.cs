using UnityEngine;
using UnityEngine.Events;

public class ZombieAnimator : MonoBehaviour
{
    public static int killCounter = 0;

    private void Awake()
    {
        GetComponent<Health>().OnDied += HandleDied;

        GetComponent<ZombieAttack>().OnAttack += HandleAttack;
    }

    private void HandleAttack()
    {
        int attackId = Random.Range(1, 3);
    }

    private void HandleDied()
    {
        gameObject.SetActive(false);
        killCounter++;
        FindObjectOfType<AudioManager>().Play("ZombieDeath");
    }

}