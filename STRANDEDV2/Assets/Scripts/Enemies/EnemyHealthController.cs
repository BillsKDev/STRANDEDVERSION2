using System.Collections;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int currentHealth = 5;
    public EnemyController Enemy;
    [SerializeField] Animator animator;

    public void DamageEnemy(int damageAmount)
    {
        currentHealth -= damageAmount;

        if(Enemy != null)
            Enemy.GetShot();

        if(currentHealth <= 0)
        {
            FindObjectOfType<AudioManager>().Play("ZombieDeath");
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
