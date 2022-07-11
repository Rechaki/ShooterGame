using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameStateData
{
    public bool isClear;
    public bool isGameOver;
}

public struct CharacterBaseData
{
    public int hp;
    public int mp;
    public int lv;
    public int atk;
    public int def;
    public int moveSpeed;
    public int atkSpeed;
    public int viewRadius;
}
