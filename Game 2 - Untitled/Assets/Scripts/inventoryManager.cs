using UnityEngine;

public class inventoryManager : MonoBehaviour
{
    public GameObject inventoryItemPrefab;
    public InventorySlot[] inventorySlots;
   public void AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item == null)
            {
                SpawnNewItem(item, inventorySlots[i]);
                return;
            }
        }
        
    }
    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }
}
