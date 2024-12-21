using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    private static readonly object lockObj = new object();

    // The instance property ensures that only one instance is created
    public static T Instance
    {
        get
        {
            // If instance is null, create a new one or find the existing one
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<T>();

                        if (instance == null)
                        {
                            GameObject obj = new GameObject(typeof(T).Name);
                            instance = obj.AddComponent<T>();
                            DontDestroyOnLoad(obj); // Ensure the singleton persists between scene loads
                        }
                    }
                }
            }
            return instance;
        }
    }

    // Removed redundant Awake method
}
