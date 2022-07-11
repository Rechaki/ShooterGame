using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 0.1f;

    float m_time = 0.0f;


    void Start()
    {
        
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.position += transform.forward * speed;
            m_time += Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision) {
        //Debug.Log(collision.transform.name);
        if (collision.transform.tag == "Bullet")
        {
            return;
        }
        ObjectPool.Instance.Push(gameObject);

    }

}
