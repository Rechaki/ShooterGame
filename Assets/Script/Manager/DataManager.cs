using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public PlayerData playerData => _playerData;

    Dictionary<string, CharacterBaseData> _characterDic = new Dictionary<string, CharacterBaseData>();
    PlayerData _playerData;

    void Start() {

    }

    public void Init() {
        CharacterDataInit();
    }

    public void Load() {

    }

    public void Unload() {

    }

    public CharacterData GetCharacterData(string id)
    {
        CharacterData data = new CharacterData();
        CharacterBaseData baseData;
        if (_characterDic.TryGetValue(id, out baseData))
        {
            data = new CharacterData(baseData);
        }
        else
        {
            Debug.LogError($"No data found for id: {id}");
        }

        return data;
    }

    void CharacterDataInit() {
        var data = ResourceManager.I.ReadFile(AssetPath.CHARACTER_DATA);
        foreach (var item in data)
        {
            if (item.Length > 0)
            {
                CharacterBaseData character = new CharacterBaseData();
                character.id = item[0];
                character.hp = int.Parse(item[1]);
                character.mp = int.Parse(item[2]);
                character.lv = int.Parse(item[3]);
                character.atk = int.Parse(item[4]);
                character.def = int.Parse(item[5]);
                character.moveSpeed = float.Parse(item[6]);
                character.atkSpeed = float.Parse(item[7]);
                character.viewRadius = float.Parse(item[8]);
                _characterDic.Add(character.id, character);
            }
        }
    }

}
