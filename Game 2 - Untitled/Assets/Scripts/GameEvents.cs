using System;

/// <summary>
/// Simple event hub for global game notifications. Other systems can subscribe
/// to the events they care about (death, respawn, level complete, etc.).
/// </summary>
public static class GameEvents
{
    /// <summary>
    /// Invoked when the player respawns (e.g. after dying).  Any object that needs
    /// to revert to its original state should listen for this event.
    /// </summary>
    public static event Action OnPlayerRespawn;

    /// <summary>
    /// Call this method when the player actually respawns.  It will forward the
    /// call to all subscribers.
    /// </summary>
    public static void PlayerRespawn()
    {
        OnPlayerRespawn?.Invoke();
    }
}
