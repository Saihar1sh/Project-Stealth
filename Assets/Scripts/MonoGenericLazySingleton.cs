using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoGenericLazySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //flag to avoid creating singleton in some cases. Like when closing the game
    private static bool singletonDestroyed = false;

    private static T instance;

    public static T Instance
    {
        get
        {
            if (singletonDestroyed)
            {
                Debug.LogError("Singleton is already destroyed. Returning null");
                return null;
            }

            if (!instance)
            {
                new GameObject(typeof(T).ToString()).AddComponent<T>();
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (!instance && !singletonDestroyed)
        {
            instance = this as T;
        }
        else
            if (instance != this)
            Destroy(this);
    }
    protected virtual void OnDestroy()
    {
        if (instance != this)
            return;

        singletonDestroyed = true;
        instance = null;
    }
}
