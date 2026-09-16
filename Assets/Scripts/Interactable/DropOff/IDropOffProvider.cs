using UnityEngine;

public interface IDropOffProvider
{
    // Example: return carriedItem != null;
    bool HasItem();

    // Example: return the currently carried item and clear it from the pickup system.
    GameObject DropOffItem();
}