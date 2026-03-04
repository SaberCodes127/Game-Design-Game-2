using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
   [Header("Gameplay")]
    public ItemType type;
    public ActionType actionType;
    public Sprite sprite;

    [Header("Only UI")]
    public bool stackable = true;


}

public enum ItemType
{
    Seed,
    WateringCan,
    Weapon,
}

public enum ActionType
{
    None,
    Water,
    Attack,
}