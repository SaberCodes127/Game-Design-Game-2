using UnityEngine;

public class ToolbarUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform toolbarParent;
    public int numberOfSlots = 6;

    private void Start()
    {
        if (toolbarParent == null)
            toolbarParent = transform;

        // Create slots if they don't exist
        for (int i = 0; i < numberOfSlots; i++)
        {
            if (toolbarParent.childCount <= i)
            {
                GameObject slot = Instantiate(slotPrefab, toolbarParent);
                slot.name = $"Slot_{i}";
            }
        }

        // Assign slots to inventory manager
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.toolbarSlots = GetComponentsInChildren<InventorySlot>();
        }
    }
}