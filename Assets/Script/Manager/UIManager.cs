using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    string _theBeginningUI;

    Stack<UIPanel> _windows = new Stack<UIPanel>();

    void Start() {
        if (!string.IsNullOrEmpty(_theBeginningUI))
        {
            Open(_theBeginningUI);
        }
    }

    void OnDestroy() {
        _windows.Clear();
    }

    public void Open(string path) {
        var uiroot = GameObject.FindGameObjectWithTag("UIRoot");
        if (uiroot == null)
        {
            Debug.LogError("Can not find UIRoot!");
        }
        else
        {
            var prefab = ResourceManager.I.Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError("Can not load:" + path);
            }
            else
            {
                var uiPanel = GameObject.Instantiate<GameObject>(prefab);
                uiPanel.transform.parent = uiroot.transform;
                uiPanel.transform.localPosition = Vector3.zero;
                uiPanel.transform.localScale = Vector3.one;
                var uiRect = uiPanel.GetComponent<RectTransform>();
                uiRect.sizeDelta = Vector2.zero;
                Open(prefab.GetComponent<UIPanel>());
            }
        }
    }

    public void Open(UIPanel panel) {
        _windows.Push(panel);
    }

    public void CloseAll() {
        while (_windows.Count > 0)
        {
            _windows.Pop().Out();
        }
    }

    public void CloseOthers() {
        while (_windows.Count > 1)
        {
            _windows.Pop().Out();
        }
    }

    public void BackToPrevUI() {
        _windows.Pop().Out();
    }

    public UIPanel TopUI() {
        return _windows.Peek();
    }

}
