using UnityEngine;
using System.Collections;

public class CloudEnemy : MonoBehaviour
{
    [Header("高さ")]
    [SerializeField]
    private float height = 5;
    [Header("移動スピード")]
    [SerializeField]
    private float moveSpeed = 2.0f;
    [Header("逃げるスピード")]
    [SerializeField]
    private float escapeSpeed = 2.0f;
    [Header("向きを変わるスピード")]
    [SerializeField]
    private float turnSpeed = 1.0f;
    [Header("攻撃距離")]
    [SerializeField]
    private float attackDistance = 10.0f;
    [Header("逃げる距離")]
    [SerializeField]
    private float escapeDistance = 20.0f;
    [Header("移動範囲オブジェクト")]
    [SerializeField]
    private BoxCollider area;
    [Header("弾")]
    [SerializeField]
    private GameObject bulletPrefab;
    [Header("弾数")]
    [SerializeField]
    private int bulletNum = 2;
    [Header("弾と弾のスペース")]
    [SerializeField]
    private float bulletSpace = 2;
    [Header("発射冷却時間")]
    [SerializeField]
    private float fireCd = 0.5f;
    [Header("弾生成ポジション")]
    [SerializeField]
    public Transform firePoint;
    [Header("弾モード")]
    [SerializeField]
    private FireMode fireMode = FireMode.Vertical;
    [HideInInspector]
    public State state = State.Idle;
    [HideInInspector]
    public Transform player = null;

    public enum State
    {
        Idle,
        Attack,
        Escape,
        Dead,
    }

    public enum FireMode
    {
        Horizontal,
        Vertical,
    }

    private Vector3 m_startPoint;          
    private Vector3 m_targetPos = Vector3.zero;
    private int viewAngle = 360;
    private float m_timer = 0;
    private float m_minXPos;
    private float m_maxXPos;
    private float m_minZPos;
    private float m_maxZPos;
    private float m_offsetYTemp;

    void Start() {
        m_startPoint = transform.position;
        m_timer = fireCd;
        float width = area.size.x;
        float length = area.size.z;
        m_minXPos = area.transform.position.x - width / 2;
        m_maxXPos = area.transform.position.x + width / 2;
        m_minZPos = area.transform.position.z - length / 2;
        m_maxZPos = area.transform.position.z + length / 2;

        m_targetPos = new Vector3(Random.Range(m_minXPos, m_maxXPos), transform.position.y, Random.Range(m_minZPos, m_maxZPos));
    }

    void Update() {
        if (state == State.Dead)
            return;

        CheckHeight();

        switch (state)
        {
            case State.Idle:
                IdleAction();
                break;
            case State.Attack:
                AttackAction();
                break;
            case State.Escape:
                EscapeAction();
                break;
            default:
                break;
        }
    }

    void CheckHeight()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * height), Color.red);
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            float distance = Vector3.Distance(transform.position, hit.point);
            if (distance < height)
            {
                m_targetPos = new Vector3(m_targetPos.x, height + hit.point.y, m_targetPos.z);
            }
            else
            {
                m_targetPos = new Vector3(m_targetPos.x, height, m_targetPos.z);
            }
        }

    }

    void IdleAction()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.position) < escapeDistance)
            {
                state = State.Attack;
            }
        }

        if (IsFacingToTarget(m_targetPos))
        {
            transform.position = Vector3.MoveTowards(transform.position, m_targetPos, Time.deltaTime * moveSpeed);
        }
        else
        {
            RotateToTarget(m_targetPos);
        }

        if (Vector3.Distance(transform.position, m_targetPos) <= 0.5f)
        {
            m_targetPos = new Vector3(Random.Range(m_minXPos, m_maxXPos), transform.position.y, Random.Range(m_minZPos, m_maxZPos));
        }
    }

    void AttackAction()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.position) > attackDistance)
            {
                if (IsFacingToTarget(player.position))
                {
                    m_targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, m_targetPos, Time.deltaTime * moveSpeed);
                    Fire();
                }
                else
                {
                    RotateToTarget(player.position);
                }
            }
            else
            {
                m_targetPos = new Vector3(Random.Range(m_minXPos, m_maxXPos), transform.position.y, Random.Range(m_minZPos, m_maxZPos));
                state = State.Escape;
            }
        }
    }

    void EscapeAction()
    {
        if (player != null)
        {
            if (!IsFacingToTarget(player.position))
            {
                RotateToTarget(player.position);
            }
            if (Vector3.Distance(transform.position, m_targetPos) <= 0.5f)
            {
                state = State.Idle;
            }


            transform.position = Vector3.MoveTowards(transform.position, m_targetPos, Time.deltaTime * escapeSpeed);
        }
        else
        {
            state = State.Idle;
        }
        
    }

    void CanNotEscape()
    {
        if (IsFacingToTarget(m_targetPos))
        {
            transform.position = Vector3.MoveTowards(transform.position, m_targetPos, Time.deltaTime * moveSpeed);
        }
        else
        {
            RotateToTarget(m_targetPos);
        }

        if (Vector3.Distance(transform.position, m_targetPos) <= 0.5f)
        {
            m_targetPos = new Vector3(Random.Range(m_minXPos, m_maxXPos), transform.position.y, Random.Range(m_minZPos, m_maxZPos));
        }
    }

    void Fire() {
        if (m_timer > fireCd)
        {
            m_timer = 0;
            switch (fireMode)
            {
                case FireMode.Horizontal:
                    for (int i = 0; i < bulletNum; i++)
                    {
                        var bulletObject = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                        Bullet bullet = bulletObject.GetComponent<Bullet>();
                        var v = (player.position - transform.position).normalized;
                        float offsetX = (bulletSpace * (-bulletNum / 2 + i)) / 10;
                        bullet.transform.forward = new Vector3(v.x + offsetX, v.y, v.z);
                    }
                    break;
                case FireMode.Vertical:
                    for (int i = 0; i < bulletNum; i++)
                    {
                        var bulletObject = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                        Bullet bullet = bulletObject.GetComponent<Bullet>();
                        bullet.transform.forward = (player.position - transform.position).normalized;
                        bullet.transform.position += bullet.transform.forward * bulletSpace * i; 
                    }
                    break;
                default:
                    break;
            }
        }
        m_timer += Time.deltaTime;
    }

    bool IsFacingToTarget(Vector3 target) {
        Vector3 distance = target - transform.position;
        distance.y = 0;
        if (Vector3.Angle(transform.forward, distance) < 1)
        {
            return true;
        }
        return false;
    }

    void RotateToTarget(Vector3 target) {
        Vector3 v = target - transform.position;
        v.y = 0;
        Vector3 cross = Vector3.Cross(transform.forward, v);
        cross.x = 0;
        cross.z = 0;
        float angle = Vector3.Angle(transform.forward, v);
        transform.Rotate(cross, Mathf.Min(turnSpeed, Mathf.Abs(angle)));
    }

}