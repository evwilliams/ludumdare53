using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnPoint : MonoBehaviour
{
    public AreaOfInterest areaOfInterest;
    public bool Available => areaOfInterest == null;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

    public void SetAreaOfInterest(AreaOfInterest area)
    {
        areaOfInterest = area;
    }
}
