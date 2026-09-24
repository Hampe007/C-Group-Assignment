using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarriedCollectableList : IEnumerable<SO_InteractableCollectableData>
{
    [SerializeField] private List<InteractableCollectable> items = new();

    public int Count => items.Count;

    public SO_InteractableCollectableData this[int index] => GetData(items[index]);

    public void Add(InteractableCollectable collectable) { items.Add(collectable); }

    public bool Contains(InteractableCollectable collectable) { return items.Contains(collectable); }

    public void RemoveAt(int index)
    {
        InteractableCollectable removed = items[index];
        items.RemoveAt(index);
        if (removed != null) { UnityEngine.Object.Destroy(removed.gameObject); }
    }

    public bool Remove(SO_InteractableCollectableData data)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (GetData(items[i]) != data) { continue; }
            RemoveAt(i);
            return true;
        }
        return false;
    }

    public void RestoreAll()
    {
        foreach (InteractableCollectable item in items)
        {
            if (item != null) { item.Restore(); }
        }
        items.Clear();
    }

    public IEnumerator<SO_InteractableCollectableData> GetEnumerator()
    {
        foreach (InteractableCollectable item in items)
        {
            SO_InteractableCollectableData data = GetData(item);
            if (data != null) { yield return data; }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    private static SO_InteractableCollectableData GetData(InteractableCollectable item)
    {
        return item == null ? null : item.GetInteractableData() as SO_InteractableCollectableData;
    }
}
