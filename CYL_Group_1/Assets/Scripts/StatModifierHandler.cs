using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class StatModifierHandler : MonoBehaviour
{
    public static StatModifierHandler instance { get; private set; } 
    private List<StatModifier> activeModifiers = new List<StatModifier>();
    [SerializeField] Transform modifierUiParent;

    void Start()
    {
        instance = this;
    }

    public void AddStatModifier(StatModifier statMod)
    {
        activeModifiers.Add(statMod);

        GameObject newObject = new GameObject(statMod.name);
        newObject.transform.SetParent(modifierUiParent);
        TextMeshProUGUI newText = newObject.AddComponent<TextMeshProUGUI>();
        newText.text = $"x {statMod.amount} {statMod.name}";
        newText.color = Color.black;
    }

    public void RemoveStatModifier(StatModifier statMod)
    {
        activeModifiers.Remove(statMod);
    }

    public float GetModifier(ModType type)
    {
        List<StatModifier> mods = activeModifiers.Where(x => x.type == type).ToList();
        if (mods.Count == 0)
        {
            return 1f;
        }
        else
        {
            float am = 1f;
            foreach(StatModifier mod in mods)
            {
                am *= mod.amount;
            }
            return am;
        }
    }
}

[System.Serializable]
public class StatModifier
{
    [SerializeField] public ModType type;
    [SerializeField] public float amount;
    [SerializeField] public string name;
}

public enum ModType
{
    maxHealth,
    damage,
    speed,
    ammoClipSize
}
