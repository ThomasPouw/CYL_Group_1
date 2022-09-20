using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;
using UnityEngine.UI;

public class StatModifierHandler : MonoBehaviour
{
    public static StatModifierHandler instance { get; private set; }
    private List<StatModifier> activeModifiers = new List<StatModifier>();
    [SerializeField] Transform modifierUiParent;
    [SerializeField] Transform modifierPickerUiParent;
    [SerializeField] GameObject modifierPickerCanvas;
    [SerializeField] int maxModifiers = 5;
    [SerializeField] TextMeshProUGUI newModifierText;

    StatModifier nextStatModifier = null;

    void Start()
    {
        instance = this;
    }

    public void AddStatModifier(StatModifier statMod)
    {
        if (activeModifiers.Count >= maxModifiers)
        {
            ShowStatPicker(statMod);
            return;
        }

        activeModifiers.Add(statMod);

        {
            GameObject newObject = new GameObject(statMod.name);
            newObject.transform.SetParent(modifierUiParent);
            TextMeshProUGUI newText = newObject.AddComponent<TextMeshProUGUI>();
            newText.text = $"x {statMod.amount} {statMod.name}";
            newText.color = Color.black;

            statMod.AddGameObject(newObject);
        }

        {
            GameObject newObject = new GameObject(statMod.name);
            newObject.transform.SetParent(modifierPickerUiParent);
            Button button = newObject.AddComponent<Button>();
            newObject.AddComponent<Image>();
            GameObject textObject = new GameObject();
            textObject.transform.SetParent(newObject.transform);
            TextMeshProUGUI newText = textObject.AddComponent<TextMeshProUGUI>();
            newText.color = Color.black;
            button.onClick.AddListener(() => { AcceptNewStat(statMod); });
            button.transform.localScale = new Vector3(1, 1, 1);
            newText.transform.localScale = new Vector3(1, 1, 1);

            newText.text = $"x {statMod.amount} {statMod.name}";

            statMod.AddGameObject(newObject);
        }
    }

    public void RemoveStatModifier(StatModifier statMod)
    {
        statMod.DeleteAttachedGameObjects();

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
            foreach (StatModifier mod in mods)
            {
                am *= mod.amount;
            }
            return am;
        }
    }

    private void ShowStatPicker(StatModifier newStat)
    {
        nextStatModifier = newStat;

        newModifierText.text = $"x {newStat.amount} {newStat.name}"; ;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        modifierPickerCanvas.SetActive(true);
    }

    public void RejectNewStat()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        modifierPickerCanvas.SetActive(false);
    }

    public void AcceptNewStat(StatModifier removeStat)
    {
        RemoveStatModifier(removeStat);
        AddStatModifier(nextStatModifier);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        modifierPickerCanvas.SetActive(false);
    }
}

[System.Serializable]
public class StatModifier
{
    [SerializeField] public ModType type;
    [SerializeField] public float amount;
    [SerializeField] public string name;

    private List<GameObject> attachedGameObjects = new List<GameObject>();

    public void DeleteAttachedGameObjects()
    {
        foreach(GameObject go in attachedGameObjects)
        {
            GameObject.Destroy(go);
        }
    }

    public void AddGameObject(GameObject go)
    {
        attachedGameObjects.Add(go);
    }
}

public enum ModType
{
    maxHealth,
    damage,
    speed,
    ammoClipSize
}
