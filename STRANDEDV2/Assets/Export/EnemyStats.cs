using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField] private int damage;
    [SerializeField] public float attackSpeed;


    [SerializeField] private bool canAttack;

    private void Start()
    {
        InitVariables();
        
    }

    private void Update()
    {
        Debug.Log("Enemy Health is: " + health);
    }

    public void DealDamage(CharacterStats statsToDamage)
    {
        statsToDamage.TakeDamage(damage);
    }
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
    public override void InitVariables()
    {

        maxHealth = 25;
        health = maxHealth;
        isDead = false;

        damage = 10;
        attackSpeed = 1.5f;
        canAttack = true;
    }
}
