﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : UIPanel
{
    public Button restartBtn;

    protected override void Hide() {
        throw new System.NotImplementedException();
    }

    protected override void Show() {
        throw new System.NotImplementedException();
    }

    private void Awake() {
        restartBtn.onClick.AddListener(OnClikck);
    }

    private void OnClikck() {
        EventMsgManager.Launch(EventMsg.GameRestart);
        LevelManager.Instance.LoadScene(LevelManager.Instance.CurrentLevel);
    }

}
