using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    void Start() {
        In();
    }

    void OnDestroy() {
        Out();
    }

    public void In() {
        //UIManager.Instance.Open(this);
        Show();
    }

    public void Out() {
        Hide();
    }

    protected abstract void Show();
    protected abstract void Hide();
}
