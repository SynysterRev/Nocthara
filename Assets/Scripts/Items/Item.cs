using UnityEngine;

public enum ItemType
{
    Equipment,
    Money
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Items/Item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public string ItemDescription;
    public Sprite ItemSprite;
    public ItemType ItemType;
    public int ItemPrice;
}
