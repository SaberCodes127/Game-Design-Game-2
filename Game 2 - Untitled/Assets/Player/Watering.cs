using UnityEngine;


/// Handles the player's watering action.  It expects to be passed the
/// GameObject that represents the planted seed.

public class Watering : MonoBehaviour
{
    public void WaterSeed(GameObject seed)
    {
        if (seed == null)
            return;

        Seed seedComponent = seed.GetComponent<Seed>();
        if (seedComponent != null)
        {
            seedComponent.Water();
        }
    }
}