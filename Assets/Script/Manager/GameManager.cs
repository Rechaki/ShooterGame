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

	public void Init() {
		Application.targetFrameRate = 60;
		DataManager.I.Init();
		LevelManager.I.Init();

		_inited = true;
		DontDestroyOnLoad(this);
		//isGameOver = false;

		//ActionOwner gamerClear = new ActionOwner
		//{
		//	component = transform,
		//	action = GameClear
		//};
		//EventMsgManager.Add(EventMsg.GameClear, gamerClear);

		//ActionOwner gamerOver = new ActionOwner
		//{
		//	component = transform,
		//	action = GameOver
		//};
		//EventMsgManager.Add(EventMsg.GameOver, gamerOver);

		//ActionOwner restart = new ActionOwner
		//{
		//	component = transform,
		//	action = Restart
		//};
		//EventMsgManager.Add(EventMsg.GameRestart, restart);
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
