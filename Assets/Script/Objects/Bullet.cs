using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage { get; private set; } = 1;
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

    public void SetDamage(int damage) {
        Damage = damage;
    }

    void OnCollisionEnter(Collision collision) {
        //Debug.Log(collision.transform.name);
        if (collision.transform.tag != "Bullet")
        {
            ObjectPool.I.Push(gameObject);
        }

    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.tag != "Bullet")
        {
            ObjectPool.I.Push(gameObject);
        }
    }

}
