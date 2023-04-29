using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform holster;

    public bool HasPackage()
    {
        return holster.childCount > 0;
    }

    public void TryPickup(Transform package)
    {
        if (!HasPackage())
        {
            package.transform.position = holster.transform.position;
            package.parent = holster;    
        }
    }

    public void TryDropoff()
    {
        if (!HasPackage())
            return;
        
        for (int i = holster.childCount - 1; i >= 0; i--)
        {
            Debug.Log($"Dropping off: {holster.GetChild(i).gameObject.name}");
            Destroy(holster.GetChild(i).gameObject);
        }
    }
}
