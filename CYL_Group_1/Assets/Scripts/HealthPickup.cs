using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] int increaseAmmount = 30;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<PlayerHealth>().IncreaseHealth(increaseAmmount);
            Destroy(gameObject);
        }
    }
}

