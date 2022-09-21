using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloser : MonoBehaviour
{
    private bool HasPlayed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!HasPlayed)
            GetComponent<Animator>().SetBool("Open", false);
    }
}
