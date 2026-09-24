using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints = new();
    [SerializeField] private List<SO_InteractableCollectableData> itemTypes = new();

    private void Start()
    {
        List<Transform> validSpawnPoints = GetValidSpawnPoints();
        List<SO_InteractableCollectableData> validItemTypes = GetValidItemTypes();

        if (validSpawnPoints.Count == 0 || validItemTypes.Count == 0)
        {
            return;
        }

        Shuffle(validSpawnPoints);

        for (int i = 0; i < validSpawnPoints.Count; i += validItemTypes.Count)
        {
            Shuffle(validItemTypes);
            int batchSize = Mathf.Min(validItemTypes.Count, validSpawnPoints.Count - i);

            for (int j = 0; j < batchSize; j++)
            {
                Spawn(validSpawnPoints[i + j], validItemTypes[j]);
                Debug.Log(validSpawnPoints[i + j], validItemTypes[j]);
            }
        }
    }

    private List<Transform> GetValidSpawnPoints()
    {
        List<Transform> valid = new();
        HashSet<Transform> seen = new();

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            if (spawnPoint == null)
            {
                Debug.LogWarning($"{nameof(CollectableSpawner)} skipped null spawn point at index {i}.", this);
            }
            else if (!seen.Add(spawnPoint))
            {
                Debug.LogWarning($"{nameof(CollectableSpawner)} skipped duplicate spawn point '{spawnPoint.name}'.", this);
            }
            else
            {
                valid.Add(spawnPoint);
                Debug.Log(spawnPoint);
            }
        }

        return valid;
    }

    private List<SO_InteractableCollectableData> GetValidItemTypes()
    {
        List<SO_InteractableCollectableData> valid = new();
        HashSet<SO_InteractableCollectableData> seen = new();

        for (int i = 0; i < itemTypes.Count; i++)
        {
            SO_InteractableCollectableData itemType = itemTypes[i];
            if (itemType == null)
            {
                Debug.LogWarning($"{nameof(CollectableSpawner)} skipped null item type at index {i}.", this);
            }
            else if (!seen.Add(itemType))
            {
                Debug.LogWarning($"{nameof(CollectableSpawner)} skipped duplicate item type '{itemType.name}'.", this);
            }
            else if (itemType.GetCollectablePrefab() == null)
            {
                Debug.LogWarning($"{nameof(CollectableSpawner)} skipped '{itemType.name}' because it has no prefab.", this);
            }
            else if (itemType.GetCollectablePrefab().GetComponent<InteractableCollectable>() == null)
            {
                Debug.LogWarning($"{nameof(CollectableSpawner)} skipped '{itemType.name}' because its prefab has no {nameof(InteractableCollectable)} component.", this);
            }
            else
            {
                valid.Add(itemType);
            }
        }

        return valid;
    }

    private void Spawn(Transform spawnPoint, SO_InteractableCollectableData itemType)
    {
        GameObject spawnedObject = Instantiate(itemType.GetCollectablePrefab(), spawnPoint);
        spawnedObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        spawnedObject.name = itemType.GetCollectableName();
        spawnedObject.GetComponent<InteractableCollectable>().Initialize(itemType);
        Debug.Log(itemType);
    }

    private static void Shuffle<T>(IList<T> items)
    {
        for (int i = items.Count - 1; i > 0; i--)
        {
            int swapIndex = Random.Range(0, i + 1);
            (items[i], items[swapIndex]) = (items[swapIndex], items[i]);
            Debug.Log("Shuffled item at index " + i + ": " + items[swapIndex]);
        }
    }
}
