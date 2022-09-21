using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    PlayerHealth target;
    [SerializeField] int damage = 20;

    void Start()
    {
        target = FindObjectOfType<PlayerHealth>();
    }

    public void AttackHitEvent()
    {
        Debug.Log("attacking");
        if (target == null) return;
        target.DecreaseHealth(damage);
        GetComponent<AudioPlayer>().PlayByName("Attack");
    }
}
