using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemCtrl : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_particleSystem;

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (m_particleSystem == null)
        {
            return;
        }

        if (gameObject.activeSelf && m_particleSystem.isStopped)
        {
            ObjectPool.I.Push(gameObject);
        }
    }

    public void Play() {
        m_particleSystem.Play();
    }
}
