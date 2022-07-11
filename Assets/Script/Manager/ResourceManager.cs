using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public delegate void AssetsLoadCallback(string name, UnityEngine.Object obj);

public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<string, AssetData> m_loadedList = new Dictionary<string, AssetData>();
    private Dictionary<string, AssetData> m_unloadList = new Dictionary<string, AssetData>();
    private Dictionary<int, AssetData> m_instanceIDList = new Dictionary<int, AssetData>();

    public T Load<T>(string path) where T : Object {
        AssetData assetData = null;
        if (m_loadedList.TryGetValue(path, out assetData))
        {
            assetData.refCount++;
            return assetData.asset as T;
        }
        assetData = new AssetData();
        assetData.refCount = 1;
        assetData.asset = Resources.Load<T>(path);
        m_loadedList.Add(path, assetData);
        return assetData.asset as T;
    }

}