using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance is null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance is null)
                {
                    Debug.LogError(typeof(T) + "がシーンに存在しません。");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        Check();
    }

    void Check()
    {
        if (instance == null)
        {
            instance = this as T;
            return;
        }
        else if (Instance == this)
        {
            return;
        }
        Destroy(this);
    }
}
