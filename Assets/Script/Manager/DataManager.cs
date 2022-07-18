using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public PlayerData PlayerData { get; private set; }
    public bool GameOver { get; private set; }

    Dictionary<string, CharacterBaseData> _characterDic = new Dictionary<string, CharacterBaseData>();
    Dictionary<string, EnemyBaseData> _enemyDic = new Dictionary<string, EnemyBaseData>();
    GameStateData _stateData;

    public void Init() {
        Load();

        GameStateInit();
        PlayerDataInit();
    }

    public void Load() {
        CharacterBaseDataInit();
        EnemyDataInit();
    }

    public void Unload() {

    }

    public CharacterData GetCharacterData(string id) {
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

    public EnemyData GetEnemyData(string id) {
        EnemyData data = new EnemyData();
        EnemyBaseData baseData;
        if (_enemyDic.TryGetValue(id, out baseData))
        {
            data = new EnemyData(baseData);
        }
        else
        {
            Debug.LogError($"No data found for id: {id}");
        }
        return data;
    }

    void GameStateInit() {
        _stateData = new GameStateData();
        _stateData.isClear = false;
        _stateData.isGameOver = false;
        GameOver = _stateData.isGameOver;

        EventMessenger.AddListener(EventMsg.GameOver, SetGameOver);
    }

    void SetGameOver() {
        GameOver = true;
    }

    void PlayerDataInit() {
        PlayerData = new PlayerData();
    }

    void CharacterBaseDataInit() {
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

    void EnemyDataInit() {
        var data = ResourceManager.I.ReadFile(AssetPath.ENEMY_DATA);
        foreach (var item in data)
        {
            if (item.Length > 0)
            {
                EnemyBaseData enemy = new EnemyBaseData();
                enemy.id = item[0];
                enemy.type = (EnemyType)int.Parse(item[1]);
                enemy.hp = int.Parse(item[2]);
                enemy.mp = int.Parse(item[3]);
                enemy.lv = int.Parse(item[4]);
                enemy.atk = int.Parse(item[5]);
                enemy.def = int.Parse(item[6]);
                enemy.moveSpeed = float.Parse(item[7]);
                enemy.atkSpeed = float.Parse(item[8]);
                enemy.turnSpeed = float.Parse(item[9]);
                enemy.viewRadius = float.Parse(item[10]);
                enemy.viewAngle = int.Parse(item[11]);
                _enemyDic.Add(enemy.id, enemy);
            }
        }
    }

}
