using UnityEngine;

[CreateAssetMenu(menuName = "Stone Data")]
public class StoneData : ScriptableObject
{
    public Sprite Sprite;
    public string Name;
    public string Description;
    public StoneType Type;

    public bool Equals(StoneData obj)
    {
        if (Name == obj.Name) return true;
        return false;
    }
}
