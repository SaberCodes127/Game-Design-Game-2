using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [Header("UI")]
    public Image image;
    
    [HideInInspector] public Item item;

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = item.sprite;
    }
}