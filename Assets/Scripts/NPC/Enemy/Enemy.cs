using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Stagger,
    Patrol
}

public class Enemy : MonoBehaviour, IDamageable
{


    public EnemyState currentState;

    public float maxHealth = 2f;
    public float currHealth;
    //public string enemyName;
    public int baseAttack;
    public GameObject deathFX;
    [HideInInspector]
    public FireProjectiles shooter;
    Collectables newItem;


    protected virtual void Awake()
    {
        ChangeState(EnemyState.Idle);
        currHealth = maxHealth;
        shooter = GetComponent<FireProjectiles>();
        newItem = GetComponent<Collectables>();
    }

    protected virtual void OnEnable()
    {
        currHealth = maxHealth;
    }

    public void TakeDamage(float damageTaken, GameObject damageGiver)
    {
        currHealth -= damageTaken;
        if (currHealth <= 0f)
        {
            Destroy();
        }
        else
        {
            Knockback kB = GetComponent<Knockback>();
            if (kB!= null)
            {
                ChangeState(EnemyState.Stagger);
                StartCoroutine(kB.KnockBack(damageGiver.transform));
            }
        }
    }

    public virtual void Destroy()
    {
        gameObject.SetActive(false);
        GameObject deathEffect = Instantiate(deathFX, transform.position, Quaternion.identity);
        GameObject droppedItem = newItem.GetRandomItem();
        if(droppedItem != null)
            Instantiate(droppedItem, transform.position, Quaternion.identity);
        Destroy(deathEffect, 1f);
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
            currentState = newState;
    }

}
