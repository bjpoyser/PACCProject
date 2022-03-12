using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NJL_PlayerAlertScript : MonoBehaviour
{
    
    void Start()
    {
        
    }

   
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

        }
    }
}
