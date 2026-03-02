using UnityEngine;

public class Watering : MonoBehaviour
{
    public void WaterSeed(GameObject seed)
    {
        if (seed == null)
            return;

        // Check if the seed has a Seed component
        Seed seedComponent = seed.GetComponent<Seed>();
        if (seedComponent != null && !seedComponent.IsWatered)
        {
            seedComponent.IsWatered = true;
            ConvertSeedToVine(seed);
        }
    }

    private void ConvertSeedToVine(GameObject seed)
    {
        // Add vine component or change sprite/model
        Vine vineComponent = seed.AddComponent<Vine>();
        
        // Remove or disable seed component
        Seed seedComponent = seed.GetComponent<Seed>();
        if (seedComponent != null)
            Destroy(seedComponent);
    }
}