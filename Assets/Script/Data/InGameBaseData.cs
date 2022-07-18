using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateData
{
    public bool isClear;
    public bool isGameOver;
}

public struct CharacterBaseData
{
    public string id;
    public int hp;
    public int mp;
    public int lv;
    public int atk;
    public int def;
    public float moveSpeed;
    public float atkSpeed;
    public float viewRadius;
}

public struct EnemyBaseData
{
    public string id;
    public EnemyType type;
    public int hp;
    public int mp;
    public int lv;
    public int atk;
    public int def;
    public float moveSpeed;
    public float atkSpeed;
    public float turnSpeed;
    public float viewRadius;
    public int viewAngle;
}
