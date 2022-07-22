using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level
{
    public string sceneName;
    public GameObject gate;
    public List<EnemyData> enemies = new List<EnemyData>();

    public Level() {
        GlobalMessenger.AddListener(EventMsg.KilledTheEnemy, CheckEnemiseState);
    }

    ~Level() {
        GlobalMessenger.RemoveListener(EventMsg.KilledTheEnemy, CheckEnemiseState);
    }


    void CheckEnemiseState() {
        bool isAlive = false;
        foreach (var enemy in enemies)
        {
            if (enemy.CurrentState != EnemyActionState.Dead)
            {
                isAlive = true;
                return;
            }
        }

        if (!isAlive)
        {
            gate.SetActive(true);
        }
    }
}
