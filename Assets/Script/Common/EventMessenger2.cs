﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public static class EventMessenger<T>
{
    private class EventData
    {
        public List<Callback<T>> callbacks = new List<Callback<T>>();
        public List<Callback<T>> temp = new List<Callback<T>>();
        public bool isInvoking;
    }

    private static Dictionary<string, EventData> _eventDic = new Dictionary<string, EventData>();

    public static void AddListener(string msg, Callback<T> handler) {
        Dictionary<string, EventData> obj = _eventDic;
        lock (obj)
        {
            EventData eventData;
            if (!_eventDic.TryGetValue(msg, out eventData))
            {
                eventData = new EventData();
                _eventDic.Add(msg, eventData);
            }
            eventData.callbacks.Add(handler);
        }
    }

    public static void RemoveListener(string msg, Callback<T> handler) {
        Dictionary<string, EventData> obj = _eventDic;
        lock (obj)
        {
            EventData eventData;
            if (_eventDic.TryGetValue(msg, out eventData))
            {
                int num = eventData.callbacks.IndexOf(handler);
                if (num >= 0)
                {
                    eventData.callbacks[num] = eventData.callbacks[eventData.callbacks.Count - 1];
                    eventData.callbacks.RemoveAt(eventData.callbacks.Count - 1);
                }
            }
        }
    }

    public static void Launch(string msg, T arg) {
        Dictionary<string, EventData> obj = _eventDic;
        lock (obj)
        {
            EventData eventData;
            if (_eventDic.TryGetValue(msg, out eventData))
            {
                if (eventData.isInvoking)
                {
                    throw new InvalidOperationException("Can not support Launch calls to the same eventType.");
                }
                eventData.isInvoking = true;
                eventData.temp.AddRange(eventData.callbacks);
                for (int i = 0; i < eventData.temp.Count; i++)
                {
                    try
                    {
                        eventData.temp[i](arg);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogException(exception);
                    }
                }
                eventData.temp.Clear();
                eventData.isInvoking = false;
            }
        }
    }

    public static void ClearALL() {
        _eventDic.Clear();
    }

}
