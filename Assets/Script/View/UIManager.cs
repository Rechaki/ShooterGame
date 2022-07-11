using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Stack<UIPanel> m_windows = new Stack<UIPanel>();

    public void Open(string path) {
        var uiroot = GameObject.FindGameObjectWithTag("UIRoot");
        if (uiroot == null)
        {
            Debug.LogError("Can not find UIRoot!");
        }
        else
        {
            var prefab = ResourceManager.Instance.Load<GameObject>(path);
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
        m_windows.Push(panel);
    }

    public void CloseAll() {
        while (m_windows.Count > 0)
        {
            m_windows.Pop().Out();
        }
    }

    public void CloseOthers() {
        while (m_windows.Count > 1)
        {
            m_windows.Pop().Out();
        }
    }

    public void BackToPrevUI() {
        m_windows.Pop().Out();
    }

    public UIPanel TopUI() {
        return m_windows.Peek();
    }

}
