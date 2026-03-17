using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image backgroundImage;
    public Image itemImage;
    public TextMeshProUGUI countText;
    private Item currentItem;
    private int currentCount;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (droppedItem == null) return;

        // Swap if slot already has an item
        InventoryItem existingItem = GetComponentInChildren<InventoryItem>();
        if (existingItem != null && existingItem != droppedItem)
        {
            // Send existing item back to where the dropped item came from
            existingItem.transform.SetParent(droppedItem.parentAfterDrag);
            existingItem.transform.SetSiblingIndex(droppedItem.siblingIndexAfterDrag);
            FitToSlot(existingItem.GetComponent<RectTransform>());
        }

        // Move dropped item into this slot
        droppedItem.parentAfterDrag = transform;
        droppedItem.transform.SetParent(transform);
        droppedItem.transform.SetSiblingIndex(0);
        FitToSlot(droppedItem.GetComponent<RectTransform>());
    }

    // Forces the item to perfectly fill the slot
    private void FitToSlot(RectTransform rt)
    {
        if (rt == null) return;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;
    }

    private void Awake()
    {
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();

        if (itemImage == null)
        {
            Image[] images = GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                if (img != backgroundImage)
                {
                    itemImage = img;
                    break;
                }
            }
        }

        if (countText == null)
            countText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetItem(Item item, int count)
    {
        currentItem = item;
        currentCount = count;

        if (itemImage != null && item.sprite != null)
        {
            itemImage.sprite = item.sprite;
            itemImage.enabled = true;
        }

        if (countText != null)
        {
            if (item.stackable && count > 1)
            {
                countText.text = count.ToString();
                countText.enabled = true;
            }
            else
            {
                countText.enabled = false;
            }
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        currentCount = 0;
        if (itemImage != null) itemImage.enabled = false;
        if (countText != null) countText.enabled = false;
    }

    public Item GetItem() => currentItem;
    public int GetCount() => currentCount;
}