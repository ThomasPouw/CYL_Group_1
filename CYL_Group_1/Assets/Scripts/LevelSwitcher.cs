using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] LevelGenerator Generator;

    private bool IsPressed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsPressed && Generator != null)
        {
            IsPressed = true;
            Generator.GetNextFloor();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        IsPressed = false;
    }
}
