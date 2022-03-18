using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private static InteractableObject instance;
    public static InteractableObject Instance { get { return instance; } }

    public void SetAsCurrent()
    {
        instance = this;
    }

    public void CleanCurrent()
    {
        instance = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Interaction();
        }
    }
}
