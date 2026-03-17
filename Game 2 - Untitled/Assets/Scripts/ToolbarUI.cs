using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ToolbarUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform toolbarParent;
    public int numberOfSlots = 6;

    [Header("Selection")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    private InventorySlot[] slots;
    private int selectedIndex = 0;

    private void Start()
    {
        if (toolbarParent == null)
            toolbarParent = transform;

        for (int i = 0; i < numberOfSlots; i++)
        {
            if (toolbarParent.childCount <= i)
            {
                GameObject slot = Instantiate(slotPrefab, toolbarParent);
                slot.name = $"Slot_{i}";
            }
        }

        slots = GetComponentsInChildren<InventorySlot>();
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.RegisterSlots(slots);
            Debug.Log($"ToolbarUI: Registered {slots.Length} slots");
        }
        else
        {
            Debug.LogError("ToolbarUI: InventoryManager.Instance is null!");
        }

        SelectSlot(0);
    }

    private void Update()
    {
        HandleKeyboardInput();
        HandleScrollInput();
    }

    private void HandleKeyboardInput()
    {
        // New Input System equivalents for number keys 1-6
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectSlot(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectSlot(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SelectSlot(2);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) SelectSlot(3);
        if (Keyboard.current.digit5Key.wasPressedThisFrame) SelectSlot(4);
        if (Keyboard.current.digit6Key.wasPressedThisFrame) SelectSlot(5);
    }

    private void HandleScrollInput()
    {
        if (Mouse.current == null) return;

        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll > 0f)
            SelectSlot((selectedIndex - 1 + slots.Length) % slots.Length);
        else if (scroll < 0f)
            SelectSlot((selectedIndex + 1) % slots.Length);
    }

    public void SelectSlot(int index)
    {
        if (slots == null || slots.Length == 0) return;

        SetSlotHighlight(selectedIndex, false);
        selectedIndex = index;
        SetSlotHighlight(selectedIndex, true);

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.SetSelectedSlot(selectedIndex);

        Debug.Log($"ToolbarUI: Selected slot {selectedIndex}");
    }

    private void SetSlotHighlight(int index, bool selected)
    {
        if (index < 0 || index >= slots.Length) return;

        Image slotImage = slots[index].GetComponent<Image>();
        if (slotImage != null)
            slotImage.color = selected ? selectedColor : normalColor;
    }

    public int GetSelectedIndex() => selectedIndex;
    public InventorySlot GetSelectedSlot() => slots[selectedIndex];
}