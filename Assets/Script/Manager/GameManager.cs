using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public bool isGameOver { get; private set; }

	bool _inited = false;

    void Awake() {
		if (!_inited)
		{
			Init();
		}
	}

    void OnDestroy() {
		GlobalMessenger.RemoveListener(EventMsg.GameOver, GameOver);
		GlobalMessenger.RemoveListener(EventMsg.GameClear, GameClear);
		GlobalMessenger.RemoveListener(EventMsg.GameRestart, Restart);
	}

    public void Init() {
		Application.targetFrameRate = 60;
		DataManager.I.Init();
		LevelManager.I.Init();

		_inited = true;
		isGameOver = false;
		//DontDestroyOnLoad(this);

		GlobalMessenger.AddListener(EventMsg.GameOver, GameOver);
		GlobalMessenger.AddListener(EventMsg.GameClear, GameClear);
		GlobalMessenger.AddListener(EventMsg.GameRestart, Restart);
	}

	private void GameClear() {
		UIManager.I.Open(AssetPath.GAME_CLEAR_PANEL);
	}

	private void GameOver() {
		isGameOver = true;
		UIManager.I.Open(AssetPath.GAME_OVER_PANEL);
	}

	private void Restart() {
		isGameOver = false;
	}

	public void SaveData() {
		//DataManager.SaveData();
	}

	public void DeleteSaveData() {
		//DataManager.DeleteSaveData();
	}

}
