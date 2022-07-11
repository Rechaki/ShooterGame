using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public bool isGameOver { get; private set; }

	private bool m_inited = false;


    private void Awake() {
		if (!this.m_inited)
		{
			this.Init();
		}
	}

	public void Init() {
		Application.targetFrameRate = 60;

		m_inited = true;
		isGameOver = false;

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
		UIManager.Instance.Open(AssetPath.GAME_CLEAR_PANEL);
	}

	private void GameOver() {
		isGameOver = true;
		UIManager.Instance.Open(AssetPath.GAME_OVER_PANEL);
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
