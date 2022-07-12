using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClearPanel : MonoBehaviour
{
    public Button restartBtn;

    private void Awake() {
        restartBtn.onClick.AddListener(OnClikck);
    }

    private void OnClikck() {
        EventMsgManager.Launch(EventMsg.GameRestart);
        LevelManager.I.LoadScene("0_0");
    }
}
