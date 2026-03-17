using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    public Item item;
    public int amount = 1;

    [Header("Attract Settings")]
    public float attractRadius = 3f;     // Distance at which item starts moving toward player
    public float attractSpeed = 8f;      // How fast it moves toward player
    public float pickupRadius = 0.5f;    // Distance at which it actually gets picked up

    private bool isPickedUp = false;
    private bool isAttracting = false;
    private Transform playerTransform;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        rb.bodyType = RigidbodyType2D.Dynamic;

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = false;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (item != null && item.sprite != null)
            spriteRenderer.sprite = item.sprite;
        else
            Debug.LogWarning($"ItemPickup: Item or sprite is null for {gameObject.name}");
    }

    private void Start()
    {
        // Cache player transform at start
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
        else
            Debug.LogWarning("ItemPickup: No GameObject with tag 'Player' found!");
    }

    private void Update()
    {
        if (isPickedUp || playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // Start attracting when player is within attract radius
        if (distance <= attractRadius)
        {
            isAttracting = true;
            GetComponent<Collider2D>().isTrigger = true; // Disable physics so it flies freely
            rb.gravityScale = 0f;
        }

        // Pick up when close enough
        if (distance <= pickupRadius)
        {
            TryPickup();
        }
    }

    private void FixedUpdate()
    {
        if (!isAttracting || isPickedUp || playerTransform == null) return;

        // Move smoothly toward player
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * attractSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPickedUp || isAttracting) return;
        if (!collision.collider.CompareTag("Player")) return;
        TryPickup();
    }

    private void TryPickup()
    {
        if (isPickedUp) return;

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("ItemPickup: InventoryManager.Instance is null!");
            return;
        }

        if (InventoryManager.Instance.AddItem(item, amount))
        {
            isPickedUp = true;
            Destroy(gameObject);
        }
        else
        {
            // Inventory full — stop attracting and drop back down
            isAttracting = false;
            rb.gravityScale = 1f;
            GetComponent<Collider2D>().isTrigger = false;
            Debug.LogWarning($"ItemPickup: Inventory full — could not pick up {item?.name}");
        }
    }

    // Visualize radii in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}