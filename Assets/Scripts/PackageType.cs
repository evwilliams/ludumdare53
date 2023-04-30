using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class PackageType : ScriptableObject
{
    public Color color;
    
    [FormerlySerializedAs("sprite")] 
    public Sprite destinationSprite;
}
