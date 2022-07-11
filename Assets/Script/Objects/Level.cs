using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level : MonoBehaviour
{
    public Transform startPosition;
    public GameObject gate;
    public List<Enemy> enemies = new List<Enemy>();
    public string uiPath = AssetPath.MAIN_UI_PANEL;

    private bool m_enemyIsAlive = true;

    void Start() {
        gate.SetActive(false);
        //ActionOwner owner = new ActionOwner
        //{
        //    component = transform,
        //    action = CheckEnemiseState
        //};
        //EventMsgManager.Add(EventMsg.KilledTheEnemy, owner);
        UIManager.Instance.Open(uiPath);

    }

    void Update()
    {
        
    }

    void CheckEnemiseState() {
        m_enemyIsAlive = false;
        foreach (var enemy in enemies)
        {
            if (enemy.state != Enemy.State.Dead)
            {
                m_enemyIsAlive = true;
                return;
            }
        }

        if (!m_enemyIsAlive)
        {
            gate.SetActive(true);
        }
    }
}
