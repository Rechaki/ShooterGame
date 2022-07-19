using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public Level CurrentLevel { get; private set; }

    public void Init() {
        CurrentLevel = new Level();
        CurrentLevel.sceneName = SceneManager.GetActiveScene().name;
    }

    public void SetGate(GameObject gate)
    {
        CurrentLevel.gate = gate;
    }

    public void AddEnemy(EnemyData enemy)
    {
        CurrentLevel.enemies.Add(enemy);
    }

    public void LoadScene(string levelName) {
        if (levelName == "Clear")
        {
            EventMessenger.Launch(EventMsg.GameClear);
            return;
        }
        CurrentLevel.sceneName = levelName;
        CurrentLevel.gate = null;
        CurrentLevel.enemies.Clear();
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        //EventMsgManager.Check();

    }
}
