using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private string nextLevel = "";
    [SerializeField]
    private float angleSpeed = 1.0f;
    [SerializeField]
    private List<Transform> gateFx = new List<Transform>();

    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < gateFx.Count; i++)
        {
            gateFx[i].RotateAround(transform.position, Vector3.up, angleSpeed);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player")
        {
            LevelManager.I.LoadScene(nextLevel);
        }
        
    }


}
