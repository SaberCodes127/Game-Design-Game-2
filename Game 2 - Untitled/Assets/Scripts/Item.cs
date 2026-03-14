using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Seed,
        Tool,
        Weapon
    }

    public enum ActionType
    {
        None,
        Water,
        Plant,
        Attack
    }

    public string itemName;
    public Sprite sprite; // Changed from icon to sprite to match existing assets
    public ItemType type;
    public ActionType actionType;
    public bool stackable = true;
    public int maxStackSize = 99;
}