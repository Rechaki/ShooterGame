using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIPanel : UIPanel
{
    public Image hp;

    protected override void Show() {
        Debug.Log("Show");
        var playerData = DataManager.I.PlayerData;
        playerData.RefreshEvent += Refresh;
        hp.fillAmount = 1;
    }

    protected override void Hide() {
        if (DataManager.I.PlayerData != null)
        {
            DataManager.I.PlayerData.RefreshEvent -= Refresh;
        }
    }

    void Refresh(CharacterData data) {
        hp.fillAmount = (float)data.NowHp / data.HP;
    }
}
