using UnityEngine;

/// <summary>
/// Applies a singleton pattern to a MonoBehavior of type T. This makes sure that there is only ever one instance of this Monobehavior which can referenced through T.Instance.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this as T;
    }
}
