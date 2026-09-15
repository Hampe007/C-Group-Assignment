using UnityEngine;

// TEMP SCRIPT!

public class TestDropOffProvider : MonoBehaviour, IDropOffProvider
{
    [SerializeField] private GameObject carriedItem;

    public bool HasItem() => carriedItem != null;

    public GameObject DropOffItem()
    {
        if (!HasItem())
            return null;

        GameObject item = carriedItem;
        carriedItem = null;

        return item;
    }
}