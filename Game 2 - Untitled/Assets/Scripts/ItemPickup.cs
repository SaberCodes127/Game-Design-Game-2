using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemPickup : MonoBehaviour
{
    public Item item;
    public int amount = 1;
    

    private void Awake()
    {
        // Ensure the item can rest on the ground and still be picked up.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = false;

        // Set the sprite to display the item
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (item != null && item.sprite != null)
        {
            spriteRenderer.sprite = item.sprite;
        }
        else
        {
            Debug.LogWarning($"ItemPickup: Item or sprite is null for {gameObject.name}");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"ItemPickup: Collision with {collision.collider.gameObject.name}, tag: {collision.collider.tag}");

        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log($"ItemPickup: Player detected, item: {item?.name}, amount: {amount}");

            if (InventoryManager.Instance != null)
            {
                Debug.Log("ItemPickup: InventoryManager found");

                if (InventoryManager.Instance.AddItem(item, amount))
                {
                    Debug.Log("ItemPickup: Item added successfully, destroying pickup");
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning("ItemPickup: Failed to add item to inventory");
                }
            }
            else
            {
                Debug.LogError("ItemPickup: InventoryManager.Instance is null!");
            }
        }
    }
}