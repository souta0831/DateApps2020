using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {

    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            if (instance == null)
            {
                Debug.Assert(instance, "An instance is null " + typeof(T).Name);
            }

            return instance;
        }
    }

    public static bool IsNullInstance
    {
        get { return instance == null; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            var thisInstance = this as T;
            if (instance != thisInstance)
            {
                Destroy(this.gameObject);
                return;
            }
        }
        instance = this as T;
        DontDestroyOnLoad(this.gameObject);
    }

#if UNITY_EDITOR
    public static void SetInstance(T singletonMonoBehaviour)
    {
        instance = singletonMonoBehaviour;
    }
#endif
}
