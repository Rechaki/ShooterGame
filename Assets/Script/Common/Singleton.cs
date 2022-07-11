using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
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
                    m_instance = instance;
                }

                m_instance = instance;
            }

            return m_instance;
        }
    }
}