using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    private Dictionary<int, Queue<GameObject>> m_poolDic = new Dictionary<int, Queue<GameObject>>();
    private Dictionary<GameObject, int> m_poppedIdDic = new Dictionary<GameObject, int>();

    public void ClearCachePool() {
        foreach (var item in m_poolDic)
        {
            foreach (var go in item.Value)
            {
                Destroy(go);
            }
            item.Value.Clear();
        }
        m_poolDic.Clear();
        m_poppedIdDic.Clear();
    }

    public GameObject Pop(GameObject prefab) {
        int id = prefab.GetInstanceID();
        GameObject go = null;
        Queue<GameObject> objects;
        if (m_poolDic.TryGetValue(id, out objects) && objects.Count > 0)
        {
            go = objects.Dequeue();
        }
        else
        {
            if (objects == null)
            {
                objects = new Queue<GameObject>();
                m_poolDic.Add(id, objects);
            }

            if (objects.Count == 0)
            {
                go = Instantiate<GameObject>(prefab);
            }
            //objects.Enqueue(go);
        }

        m_poppedIdDic.Add(go, id);
        return go;
    }

    public void Push(GameObject go) {
        if (go == null)
        {
            return;
        }

        go.SetActive(false);
        go.transform.parent = transform;

        int id;
        if (m_poppedIdDic.TryGetValue(go, out id))
        {
            m_poppedIdDic.Remove(go);
            Queue<GameObject> objects;
            if (m_poolDic.TryGetValue(id, out objects))
            {
                objects.Enqueue(go);
                
            }
            else
            {
                m_poolDic[id] = new Queue<GameObject>();
                m_poolDic[id].Enqueue(go);
            }

        }
    }

}
