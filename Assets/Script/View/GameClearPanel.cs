using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClearPanel : UIPanel
{
    public Button restartBtn;

    protected override void Show() {
        restartBtn.onClick.AddListener(OnClikck);
    }

    protected override void Hide() {

    }

    private void OnClikck() {
        GlobalMessenger.Launch(EventMsg.GameRestart);
        LevelManager.I.LoadScene("0_0");
    }
}
