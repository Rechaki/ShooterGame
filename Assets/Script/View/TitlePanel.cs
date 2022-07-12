using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitlePanel : UIPanel
{
    public Button startBtn;

    protected override void Show() {
        Debug.Log("Show");
        startBtn.onClick.AddListener(StartBtnClick);
    }

    protected override void Hide() {

    }

    private void StartBtnClick() {
        LevelManager.I.LoadScene("0_1");
    }
}
