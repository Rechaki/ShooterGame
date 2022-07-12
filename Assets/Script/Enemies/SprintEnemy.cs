using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintEnemy : MonoBehaviour
{
    [Header("攻撃する当たり判定の半径")]
    public int viewRadius = 10;
    [Header("移動スピード")]
    public float moveSpeed = 2.0f;
    [Header("スプリントスピード")]
    public float sprintSpeed = 10.0f;
    [Header("向きを変わるスピード")]
    public float turnSpeed = 2.0f;

    public State state = State.Idle;
    public List<Transform> movePos = new List<Transform>();

    public enum State
    {
        Idle,
        Attack,
        Dead,
    }

    private GameObject m_deadVFX;
    private Vector3 m_targetPos;
    private int m_index = 0;

    void Start() {
        m_deadVFX = ResourceManager.I.Load<GameObject>(AssetPath.SPRINT_DEAD_VFX);
        if (movePos.Count != 0)
        {
            m_index = 0;
            m_targetPos = movePos[m_index].position;
        }
        else
        {
            m_index = -1;
        }

    }

    void Update() {
        if (GameManager.I.isGameOver)
        {
            return;
        }
        switch (state)
        {
            case State.Idle:
                MoveToPos();
                break;
            case State.Attack:
                transform.position += transform.forward * sprintSpeed * Time.deltaTime;
                break;
            case State.Dead:
                break;
            default:
                break;
        }

        SetFireView();
    }

    void SetFireView() {
        Vector3 farRayPos =  transform.forward * viewRadius;
        Vector3 rayPos = Quaternion.Euler(0, 0, 0) * farRayPos;
        Ray ray = new Ray(transform.position, rayPos);
        RaycastHit hit = new RaycastHit();
        int mask = LayerMask.GetMask("Player", "Default");
        Physics.Raycast(ray, out hit, viewRadius, mask);
        Vector3 pos = transform.position + rayPos;
        if (hit.transform != null)
        {
            pos = hit.point;
        }

        Debug.DrawLine(transform.position, pos, Color.red); ;

        if (hit.transform != null)
        {
            if (hit.transform.tag == "Player" || hit.transform.tag == "Friend")
            {
                state = State.Attack;
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Bullet")
        {
            return;
        }
        if (other.gameObject.tag == "Player")
        {
            EventMsgManager.Launch(EventMsg.Damage);
        }
        if (m_deadVFX != null)
        {
            GameObject vfxObj = ObjectPool.I.Pop(m_deadVFX);
            vfxObj.transform.position = transform.position;
            vfxObj.transform.forward = other.transform.forward;
            vfxObj.SetActive(true);
            ParticleSystemCtrl vfx = m_deadVFX.GetComponent<ParticleSystemCtrl>();
            vfx.Play();
        }
        gameObject.SetActive(false);
        state = State.Dead;
    }

    void MoveToPos() {
        if (m_index != -1)
        {
            if (Vector3.Distance(m_targetPos, transform.position) < 0.5f)
            {
                m_index++;
                if (m_index == movePos.Count)
                {
                    m_index = 0;
                }
                m_targetPos = movePos[m_index].position;
            }
            if (IsFacingToPlayer())
            {
                transform.position += (m_targetPos - transform.position).normalized * moveSpeed * Time.deltaTime;
            }
            else
            {
                RotateToPos(m_targetPos);
            }
        }
    }

    void RotateToPos(Vector3 pos) {
        Vector3 v = pos - transform.position;
        v.y = 0;
        Vector3 cross = Vector3.Cross(transform.forward, v);
        cross.x = 0;
        cross.z = 0;
        float angle = Vector3.Angle(transform.forward, v);
        transform.Rotate(cross, Mathf.Min(turnSpeed, Mathf.Abs(angle)));
    }

    bool IsFacingToPlayer() {
        Vector3 distance = m_targetPos - transform.position;
        distance.y = 0;
        if (Vector3.Angle(transform.forward, distance) < 1)
        {
            return true;
        }
        return false;
    }


}
