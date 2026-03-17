using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image itemImage;
    public TextMeshProUGUI countText;

    [HideInInspector] public Item item;
    [HideInInspector] public int amount;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public int siblingIndexAfterDrag;

    private Canvas rootCanvas;

    private void Awake()
    {
        if (itemImage == null)
            itemImage = GetComponent<Image>();
        if (itemImage == null)
            itemImage = GetComponentInChildren<Image>();

        rootCanvas = GetComponentInParent<Canvas>();
    }

    public void Initialize(Item newItem, int newAmount)
    {
        item = newItem;
        amount = newAmount;
        itemImage.sprite = newItem.sprite;
        itemImage.enabled = true;
        itemImage.color = Color.white;

        // Fit perfectly inside parent slot on spawn
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        RefreshCount();
    }

    public bool CanStack(int incoming)
    {
        if (!item.stackable) return false;
        return amount + incoming <= item.maxStackSize;
    }

    public void AddAmount(int incoming)
    {
        amount += incoming;
        RefreshCount();
    }

    private void RefreshCount()
    {
        if (countText != null)
            countText.text = item.stackable && amount > 1 ? amount.ToString() : "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        siblingIndexAfterDrag = transform.GetSiblingIndex();
        itemImage.raycastTarget = false;
        transform.SetParent(rootCanvas.transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootCanvas.transform as RectTransform,
            eventData.position,
            rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out Vector2 localPoint
        );
        (transform as RectTransform).localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemImage.raycastTarget = true;

        // If not dropped onto a valid slot, snap back to original slot
        if (transform.parent == rootCanvas.transform)
        {
            transform.SetParent(parentAfterDrag);
            transform.SetSiblingIndex(siblingIndexAfterDrag);
            FitToParent();
        }
    }

    // Forces item back to fill its slot perfectly
    private void FitToParent()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;
    }
}