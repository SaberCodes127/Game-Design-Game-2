using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject pickupPrefab;

    [Header("Item")]
    public Item itemToSpawn;
    public int spawnAmount = 1;
    public float spawnRadius = 1f;

    public void SpawnItem()
    {
        if (pickupPrefab == null)
        {
            Debug.LogWarning("ItemSpawner: pickupPrefab is not assigned.");
            return;
        }

        Vector3 spawnPos = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;
        GameObject pickup = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);

        ItemPickup pickupComponent = pickup.GetComponent<ItemPickup>();
        if (pickupComponent != null)
        {
            pickupComponent.item = itemToSpawn;
            pickupComponent.amount = spawnAmount;
        }
    }

    private void Start()
    {
        // Auto-spawn on start for testing
        SpawnItem();
    }
}