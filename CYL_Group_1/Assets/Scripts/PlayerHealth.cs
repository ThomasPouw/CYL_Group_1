using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int hitpoints = 100;
    int modifiedMaxHitpoints
    {
        get { return (int)(maxHp * StatModifierHandler.instance.GetModifier(ModType.maxHealth)); }
        set { }
    }
    int maxHp;
    [SerializeField] GameObject DamageOverlay;
    [SerializeField] TextMeshProUGUI healthText;

    private void Start()
    {
        maxHp = hitpoints;
        HideDamageOverlay();
        InvokeRepeating(nameof(IncreaseHealth), 5, 5);
    }

    private void Update()
    {
        UpdateHealthBar();
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

    internal void IncreaseHealth(int increaseAmmount = 5)
    {
        hitpoints += increaseAmmount;

        if (hitpoints > modifiedMaxHitpoints) hitpoints = modifiedMaxHitpoints;
    }

    public void UpdateHealthBar()
    {
        healthText.text = $"{hitpoints} / {modifiedMaxHitpoints}";
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
