using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T _instance;

    public static T I
    {
        get
        {
            if (_instance == null)
            {
                T instance = null;
                var type = typeof(T);

                instance = Object.FindObjectOfType(type) as T;

                if (instance == null)
                {
                    var obj = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(obj);
                    instance = obj.AddComponent(typeof(T)) as T;
                }
                else
                {
                    _instance = instance;
                }

                _instance = instance;
            }

            return _instance;
        }
    }
}