using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image backgroundImage; // The slot background (always visible)
    public Image itemImage; // The item icon (overlays on top)
    public TextMeshProUGUI countText;
    private Item currentItem;
    private int currentCount;

    private void Awake()
    {
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();
        if (itemImage == null)
        {
            // Try to find itemImage as a child
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
        {
            countText = GetComponentInChildren<TextMeshProUGUI>();
            if (countText == null)
            {
                Debug.LogWarning($"InventorySlot: No TextMeshProUGUI found on {gameObject.name} or its children!");
            }
        }

        Debug.Log($"InventorySlot Awake: backgroundImage={backgroundImage}, itemImage={itemImage}, countText={countText}");
    }

    public void SetItem(Item item, int count)
    {
        currentItem = item;
        currentCount = count;

        Debug.Log($"InventorySlot: Setting item {item?.name}, count: {count}, stackable: {item?.stackable}");

        if (itemImage != null && item.sprite != null)
        {
            itemImage.sprite = item.sprite;
            itemImage.enabled = true;
            Debug.Log("InventorySlot: Item image set successfully");
        }
        else
        {
            Debug.LogWarning($"InventorySlot: itemImage is null or item.sprite is null. itemImage: {itemImage}, sprite: {item?.sprite}");
        }

        if (countText != null)
        {
            if (item.stackable && count > 1)
            {
                countText.text = count.ToString();
                countText.enabled = true;
                Debug.Log($"InventorySlot: Count text set to {count}");
            }
            else
            {
                countText.enabled = false;
                Debug.Log($"InventorySlot: Count text disabled (stackable: {item?.stackable}, count: {count})");
            }
        }
        else
        {
            Debug.LogWarning("InventorySlot: countText is null!");
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        currentCount = 0;

        if (itemImage != null)
            itemImage.enabled = false;

        if (countText != null)
            countText.enabled = false;
    }

    public Item GetItem()
    {
        return currentItem;
    }

    public int GetCount()
    {
        return currentCount;
    }
}