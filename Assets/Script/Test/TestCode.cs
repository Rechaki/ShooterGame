using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public float speed = 1;
    public float rayLength = 0.5f;
    public float minAngle = 0f;
    public float maxAngle = 45f;
    public Transform foot;

    private Vector3 m_hitNormal;

    void Update() {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (OnSlope())
        {
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                moveDir = Vector3.ProjectOnPlane(Vector3.back, m_hitNormal).normalized;
            }
            else
            {
                moveDir = Vector3.ProjectOnPlane(moveDir, m_hitNormal).normalized;
            }
            
        }
        this.transform.position += moveDir * speed * Time.deltaTime; 
    }

    bool OnSlope() {
        Ray ray = new Ray(foot.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            Debug.DrawRay(foot.position, hit.normal, Color.black, 0, false);
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            //Debug.Log(angle);
            if (angle > minAngle && angle < maxAngle)
            {
                m_hitNormal = hit.normal;
                return true;
            }
        }
        return false;
    }

}
