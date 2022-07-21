using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level
{
    public string sceneName;
    public GameObject gate;
    public List<EnemyData> enemies = new List<EnemyData>();

    bool _enemyIsAlive = true;

    public Level()
    {
        GlobalMessenger.AddListener(EventMsg.KilledTheEnemy, CheckEnemiseState);
    }

    ~Level()
    {
        GlobalMessenger.RemoveListener(EventMsg.KilledTheEnemy, CheckEnemiseState);
    }


    void CheckEnemiseState() {
        _enemyIsAlive = false;
        foreach (var enemy in enemies)
        {
            if (enemy.CurrentState != EnemyData.State.Dead)
            {
                _enemyIsAlive = true;
                return;
            }
        }

        if (!_enemyIsAlive)
        {
            gate.SetActive(true);
        }
    }
}
