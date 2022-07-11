using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public PlayerData playerData => _playerData;

    PlayerData _playerData;

    void Start() {
        Init();
    }

    public void Init() {
        PlayerInit();
    }

    public void Load() {

    }

    public void Unload() {

    }

    void PlayerInit() {
        _playerData = new PlayerData();
        //_playerData.nowHp = 10;
        //_playerData.maxHp = 10;
    }
}
