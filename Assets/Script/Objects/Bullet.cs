using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage { get; } = 1;
    public float speed = 0.1f;

    void Start()
    {
        
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.position += transform.forward * speed;
        }
    }

    void OnCollisionEnter(Collision collision) {
        //Debug.Log(collision.transform.name);
        if (collision.transform.tag == "Bullet")
        {
            return;
        }
        ObjectPool.I.Push(gameObject);

    }

}
