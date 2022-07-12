using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Object = UnityEngine.Object;

public delegate void AssetsLoadCallback(string name, UnityEngine.Object obj);

public class ResourceManager : Singleton<ResourceManager>
{
    Dictionary<string, AssetData> _loadedList = new Dictionary<string, AssetData>();
    Dictionary<string, AssetData> _unloadList = new Dictionary<string, AssetData>();
    Dictionary<int, AssetData> _instanceIDList = new Dictionary<int, AssetData>();

    char delimiter = ',';

    public T Load<T>(string path) where T : Object {
        AssetData assetData = null;
        if (_loadedList.TryGetValue(path, out assetData))
        {
            assetData.refCount++;
            return assetData.asset as T;
        }
        assetData = new AssetData();
        assetData.refCount = 1;
        assetData.asset = Resources.Load<T>(path);
        _loadedList.Add(path, assetData);
        return assetData.asset as T;
    }

    public List<string[]> ReadFile(string filepath)
    {
        TextAsset csvFile = Resources.Load(filepath) as TextAsset;
        List<string[]> data = new List<string[]>();
        StringReader sr = new StringReader(csvFile.text);
        bool isFirstLineSkip = true;
        while (sr.Peek() >= 0)
        {
            string line = sr.ReadLine();
            if (isFirstLineSkip)
            {
                isFirstLineSkip = false;
                continue;
            }
            data.Add(line.Split(delimiter));
        }

        return data;

    }

}