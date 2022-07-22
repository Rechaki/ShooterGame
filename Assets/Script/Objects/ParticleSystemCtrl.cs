using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemCtrl : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _particleSystem;

    void Update() {
        if (_particleSystem == null)
        {
            return;
        }

        if (gameObject.activeSelf && _particleSystem.isStopped)
        {
            ObjectPool.I.Push(gameObject);
        }
    }

    public void Play() {
        _particleSystem.Play();
    }
}
