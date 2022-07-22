using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    private Dictionary<int, Queue<GameObject>> _poolDic = new Dictionary<int, Queue<GameObject>>();
    private Dictionary<GameObject, int> _poppedIdDic = new Dictionary<GameObject, int>();

    public GameObject Pop(GameObject prefab) {
        int id = prefab.GetInstanceID();
        GameObject go = null;
        Queue<GameObject> objects;
        if (_poolDic.TryGetValue(id, out objects) && objects.Count > 0)
        {
            go = objects.Dequeue();
        }
        else
        {
            if (objects == null)
            {
                objects = new Queue<GameObject>();
                _poolDic.Add(id, objects);
            }

            if (objects.Count == 0)
            {
                go = Instantiate<GameObject>(prefab);
            }
            //objects.Enqueue(go);
        }

        _poppedIdDic.Add(go, id);
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
        if (_poppedIdDic.TryGetValue(go, out id))
        {
            _poppedIdDic.Remove(go);
            Queue<GameObject> objects;
            if (_poolDic.TryGetValue(id, out objects))
            {
                objects.Enqueue(go);
                
            }
            else
            {
                _poolDic[id] = new Queue<GameObject>();
                _poolDic[id].Enqueue(go);
            }

        }
    }

    public void ClearCachePool() {
        foreach (var item in _poolDic)
        {
            foreach (var go in item.Value)
            {
                Destroy(go);
            }
            item.Value.Clear();
        }
        _poolDic.Clear();

        foreach (var item in _poppedIdDic)
        {
            Destroy(item.Key);
        }
        _poppedIdDic.Clear();
    }

}
