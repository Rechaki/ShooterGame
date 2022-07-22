using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public Level CurrentLevel => _level;

    Level _level = new Level();

    public void Init() {
        _level.sceneName = SceneManager.GetActiveScene().name;
    }

    public void SetGate(GameObject gate)
    {
        _level.gate = gate;
    }

    public void AddEnemy(EnemyData enemy)
    {
        _level.enemies.Add(enemy);
    }

    public void LoadScene(string levelName) {
        if (levelName == "Clear")
        {
            GlobalMessenger.Launch(EventMsg.GameClear);
            return;
        }
        _level.sceneName = levelName;
        _level.gate = null;
        _level.enemies.Clear();
        ObjectPool.I.ClearCachePool();
        SceneManager.LoadScene(levelName);

    }
}
