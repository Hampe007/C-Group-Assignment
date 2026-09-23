using UnityEngine;

/// <summary>
/// Applies a singleton pattern to a MonoBehaviour of type T.
/// </summary>
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
        DontDestroyOnLoad(this);

        OnSingletonAwake();
    }

    /// <summary>
    /// Replaces the Awake method for Monobehaviors which inherit from this class.
    /// </summary>
    protected virtual void OnSingletonAwake(){}
}
