using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Settings")]
    [SerializeField] private int maxSlots = 20;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject inventoryItemPrefab;

    [HideInInspector] public InventorySlot[] toolbarSlots;

    private List<InventorySlot> slots = new();
    private int selectedSlotIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterSlots(InventorySlot[] newSlots)
    {
        slots.Clear();
        slots.AddRange(newSlots);
        toolbarSlots = newSlots;
        Debug.Log($"InventoryManager: Registered {slots.Count} slots");
    }

    public void SetSelectedSlot(int index)
    {
        if (index < 0 || index >= slots.Count) return;
        selectedSlotIndex = index;
    }

    // Returns the item in the currently selected slot
    public Item GetSelectedItem()
    {
        if (toolbarSlots == null || selectedSlotIndex >= toolbarSlots.Length) return null;
        InventoryItem item = toolbarSlots[selectedSlotIndex].GetComponentInChildren<InventoryItem>();
        return item != null ? item.item : null;
    }

    // Returns the InventoryItem component in the selected slot
    public InventoryItem GetSelectedInventoryItem()
    {
        if (toolbarSlots == null || selectedSlotIndex >= toolbarSlots.Length) return null;
        return toolbarSlots[selectedSlotIndex].GetComponentInChildren<InventoryItem>();
    }

    public bool AddItem(Item item, int amount = 1)
    {
        if (item == null)
        {
            Debug.LogError("InventoryManager: Item is null!");
            return false;
        }

        Debug.Log($"InventoryManager: Trying to add {item.name} x{amount}, slots available: {slots.Count}");

        // Try stacking into existing slot first
        foreach (InventorySlot slot in slots)
        {
            InventoryItem existing = slot.GetComponentInChildren<InventoryItem>();
            if (existing != null && existing.item == item && existing.CanStack(amount))
            {
                existing.AddAmount(amount);
                Debug.Log($"InventoryManager: Stacked {item.name}");
                return true;
            }
        }

        // Find empty slot
        foreach (InventorySlot slot in slots)
        {
            InventoryItem existing = slot.GetComponentInChildren<InventoryItem>();
            if (existing == null)
            {
                SpawnItem(item, amount, slot.transform);
                Debug.Log($"InventoryManager: Placed {item.name} in slot {slot.name}");
                return true;
            }
        }

        Debug.LogWarning($"InventoryManager: All {slots.Count} slots full!");
        return false;
    }

    private void SpawnItem(Item item, int amount, Transform slot)
    {
        if (inventoryItemPrefab == null)
        {
            Debug.LogError("InventoryManager: inventoryItemPrefab is not assigned!");
            return;
        }

        GameObject itemObj = Instantiate(inventoryItemPrefab, slot);
        InventoryItem inventoryItem = itemObj.GetComponent<InventoryItem>();

        if (inventoryItem == null)
        {
            Debug.LogError("InventoryManager: InventoryItem component missing from prefab!");
            return;
        }
        if (inventoryItem.itemImage == null)
        {
            Debug.LogError("InventoryManager: itemImage not assigned on prefab!");
            return;
        }

        inventoryItem.Initialize(item, amount);
    }
}