using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] GameObject Door;
    [SerializeField] GameObject Enemies;

    void Update()
    {
        if (gameObject.activeSelf)
            if (HaveEnemiesBeenDefeated())
                if (Door != null)
                    Door.GetComponent<Animator>().SetBool("Open", true);
    }

    private bool HaveEnemiesBeenDefeated() => GetEnemyHealth() <= 0f;

    private float GetEnemyHealth()
    {
        float TotalHealth = 0f;
        if (Enemies != null)
        {
            for (int i = 0; i < Enemies.transform.childCount; i++)
            {
                GameObject Enemy = Enemies.transform.GetChild(i).gameObject;
                EnemyHealth Health = Enemy.GetComponent<EnemyHealth>();
                if (Health != null)
                    TotalHealth += Health.GetCurrentHitPoints();
            }
        }
        return TotalHealth;
    }
}
