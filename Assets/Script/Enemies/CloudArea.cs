using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudArea : MonoBehaviour
{
    public CloudEnemy cloud;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.gameObject.tag == "Player")
        {
            cloud.player = other.transform;
            //cloud.state = Cloud.State.Attack;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name);
        if (other.gameObject.tag == "Player")
        {
            cloud.player = null;
            cloud.state = CloudEnemy.State.Idle;
        }
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            cloud.player = other.transform;
            //cloud.state = Cloud.State.Attack;
        }

    }

}
