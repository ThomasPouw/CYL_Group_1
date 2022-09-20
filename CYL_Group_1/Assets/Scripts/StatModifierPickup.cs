using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifierPickup : MonoBehaviour
{
    [SerializeField] StatModifier modifier;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StatModifierHandler.instance.AddStatModifier(modifier);
            Destroy(gameObject);
        }
    }
}
