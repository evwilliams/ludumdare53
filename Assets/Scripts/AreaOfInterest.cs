using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfInterest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{name} entered");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"{name} exited");
    }
}
