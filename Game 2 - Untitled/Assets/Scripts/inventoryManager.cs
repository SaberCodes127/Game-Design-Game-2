using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Toolbar Settings")]
    public int toolbarSize = 6;
    public InventorySlot[] toolbarSlots;

    private Dictionary<Item, int> inventory = new Dictionary<Item, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize toolbar slots if not set
        if (toolbarSlots == null || toolbarSlots.Length == 0)
        {
            toolbarSlots = GetComponentsInChildren<InventorySlot>();
        }
    }

    public bool AddItem(Item item, int amount = 1)
    {
        Debug.Log($"InventoryManager: Adding item {item?.name}, amount: {amount}, stackable: {item?.stackable}");

        if (item == null)
        {
            Debug.LogError("InventoryManager: Item is null!");
            return false;
        }

        if (item.stackable)
        {
            if (inventory.ContainsKey(item))
            {
                int oldCount = inventory[item];
                inventory[item] += amount;
                Debug.Log($"InventoryManager: Added {amount} to existing stack, old total: {oldCount}, new total: {inventory[item]}");
            }
            else
            {
                inventory[item] = amount;
                Debug.Log($"InventoryManager: Added new stack with {amount} items");
            }
        }
        else
        {
            // For non-stackable items, add multiple entries
            for (int i = 0; i < amount; i++)
            {
                inventory[item] = 1; // This is simplistic, might need better handling
                Debug.Log($"InventoryManager: Added non-stackable item");
            }
        }

        UpdateToolbarUI();
        return true;
    }

    public bool RemoveItem(Item item, int amount = 1)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= amount;
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
            UpdateToolbarUI();
            return true;
        }
        return false;
    }

    public bool HasItem(Item item, int amount = 1)
    {
        return inventory.ContainsKey(item) && inventory[item] >= amount;
    }

    public int GetItemCount(Item item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }

    private void UpdateToolbarUI()
    {
        Debug.Log($"InventoryManager: Updating toolbar UI, inventory count: {inventory.Count}, toolbar slots: {toolbarSlots?.Length ?? 0}");

        if (toolbarSlots == null || toolbarSlots.Length == 0)
        {
            Debug.LogWarning("InventoryManager: No toolbar slots assigned!");
            return;
        }

        // Simple toolbar update - assign first few items to slots
        int slotIndex = 0;
        foreach (var kvp in inventory)
        {
            if (slotIndex >= toolbarSlots.Length) break;

            Debug.Log($"InventoryManager: Setting slot {slotIndex} with item {kvp.Key.name}, count: {kvp.Value}");
            toolbarSlots[slotIndex].SetItem(kvp.Key, kvp.Value);
            slotIndex++;
        }

        // Clear remaining slots
        for (int i = slotIndex; i < toolbarSlots.Length; i++)
        {
            Debug.Log($"InventoryManager: Clearing slot {i}");
            toolbarSlots[i].ClearSlot();
        }
    }

    public Item GetSelectedItem()
    {
        // For now, return the first item. Later can add selection logic
        foreach (var kvp in inventory)
        {
            return kvp.Key;
        }
        return null;
    }
}