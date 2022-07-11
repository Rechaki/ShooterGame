using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public string CurrentLevel { get; private set; }

    void Awake() {
        CurrentLevel = "0_0";
    }

    public void LoadScene(string levelName) {
        if (levelName == "Clear")
        {
            EventMsgManager.Launch(EventMsg.GameClear);
            return;
        }
        CurrentLevel = levelName;
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        //EventMsgManager.Check();

    }
}
