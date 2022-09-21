using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hitpoints = 100f;

    private bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }

    public float GetCurrentHitPoints() => Math.Max(hitpoints, 0);

    public void ReduceHealth(float points)
    {
        if (IsDead()) return;

        BroadcastMessage("OnDamageTaken");

        hitpoints -= points;

        if (hitpoints <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        GetComponent<Animator>().SetTrigger("die");
        EnemyAI enAi = GetComponent<EnemyAI>();
        if (enAi) enAi.enabled = false;
        EnemyRanged enRa = GetComponent<EnemyRanged>();
        if (enRa) enRa.enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
    }
}
