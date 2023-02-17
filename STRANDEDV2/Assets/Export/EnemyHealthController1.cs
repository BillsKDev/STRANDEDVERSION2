using UnityEngine;

public class EnemyHealthController1 : MonoBehaviour
{
    public int currentHealth = 5;
    public EnemyController Enemy;

    public void DamageEnemy(int damageAmount)
    {
        currentHealth -= damageAmount;
        

        if(Enemy != null)
            Enemy.GetShot();

        if(currentHealth <= 0)
        {
            FindObjectOfType<AudioManager>().Play("ZombieDeath2");

            Destroy(gameObject);
        }
    }
}
