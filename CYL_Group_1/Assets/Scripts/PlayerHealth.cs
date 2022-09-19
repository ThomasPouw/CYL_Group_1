using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int hitpoints = 100;
    int maxHp;
    [SerializeField] GameObject DamageOverlay;

    private void Start()
    {
        maxHp = hitpoints;
        HideDamageOverlay();
    }

    public void DecreaseHealth(int points)
    {
        hitpoints -= points;

        UpdateHealthBar();

        if (hitpoints <= 0)
        {
            GetComponent<DeathHandler>().HandleDeath();
        }

        ShowDamageOverlay();
    }

    internal void IncreaseHealth(int increaseAmmount)
    {
        hitpoints += increaseAmmount;

        if (hitpoints > maxHp) hitpoints = maxHp;

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        Debug.Log("Health is: " + hitpoints);
    }

    private void ShowDamageOverlay()
    {
        DamageOverlay.SetActive(true);
        Invoke("HideDamageOverlay", 0.5f);
    }

    private void HideDamageOverlay()
    {
        DamageOverlay.SetActive(false);
    }
}
