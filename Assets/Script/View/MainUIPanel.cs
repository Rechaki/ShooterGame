using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIPanel : UIPanel
{
    public Image hp;

    protected override void Show() {
        Debug.Log("Show");
        var playerData = DataManager.Instance.playerData;
        playerData.refreshEvent += Refresh;
        hp.fillAmount = 1;
        //ActionOwner owner = new ActionOwner
        //{
        //    component = transform,
        //    action = () => {
        //        hp.fillAmount = (float)playerData.nowHp / playerData.maxHp;
        //    }
        //};
        //EventMsgManager.Instance.Add(EventMsg.HPHasChanged, owner);
    }

    protected override void Hide() {
        DataManager.Instance.playerData.refreshEvent -= Refresh;
    }

    void Refresh(PlayerData data) {
        hp.fillAmount = (float)data.nowHp / data.maxHp;
    }
}
