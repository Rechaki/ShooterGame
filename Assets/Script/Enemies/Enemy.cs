using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("攻撃の当たり判定の半径")]
    public int viewRadius = 10;
    [Header("視界内レイ数")]
    public int viewRayNum = 40;             
    [Header("視界角度")]
    public int viewAngle = 120;             
    [Header("移動スピード")] 
    public float moveSpeed = 2.0f;          
    [Header("向きを変わるスピード")]
    public float turnSpeed = 2.0f;          
    [Header("最小追いかける距離")]
    public float minChaseDist = 3.0f;       
    [Header("最大追いかける距離")]
    public float maxChaseDist = 11.0f;      
    [Header("離れた最大距離")]
    public float maxLeaveDist = 2.0f;
    [Space]
    public float fireCd = 0.5f;
    public int hp = 10;
    public State state = State.Idle;
    public Transform firePoint;

    public enum State
    {
        Idle,
        Attack,
        Back,
        Dead,
    }

    private GameObject m_player = null;
    private GameObject m_bulletPrefab;
    private GameObject m_deadVFXPrefab;
    private Vector3 m_startPoint;          
    private Quaternion m_startDirection;   
    private float m_timer = 0;

    void Start() {
        m_bulletPrefab = ResourceManager.Instance.Load<GameObject>(AssetPath.ENEMY_BULLET);
        m_deadVFXPrefab = ResourceManager.Instance.Load<GameObject>(AssetPath.ENEMY_DEAD_VFX);
        m_startPoint = transform.position;
        m_startDirection = transform.rotation;
    }

    void Update() {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        if (state == State.Dead)
        {
            return;
        }

        if (state == State.Attack)
        {
            if (m_player != null)
            {
                float distanceToPlayer = Vector3.Distance(m_player.transform.position, transform.position);
                float distanceToStartPoint = Vector3.Distance(m_startPoint, transform.position);
                if (distanceToStartPoint >= maxLeaveDist)
                {
                    state = State.Back;
                    return;
                }
                else if (distanceToPlayer <= minChaseDist)
                {
                    transform.position += m_player.transform.forward.normalized * moveSpeed * Time.deltaTime;
                }
                else if (distanceToPlayer >= maxChaseDist)
                {
                    state = State.Back;
                    return;
                }
                else
                {
                    MoveToPosition(m_player.transform.position);
                }

                if (IsFacingToPlayer())
                {
                    Fire();
                }
                else
                {
                    RotateToPlayer();
                }
            }
        }
        else if (state == State.Back)
        {
            if (IsInPosition(m_startPoint))
            {
                state = State.Idle;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, m_startDirection, turnSpeed);
                return;
            }
            MoveToPosition(m_startPoint);
        }
        SetFireView();
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag != "Bullet")
        {
            return;
        }
        //Destroy(other);
        //Debug.Log(other.transform.name);
        if (state == State.Idle)
        {
            state = State.Attack;
        }
        if (hp > 0)
        {
            hp -= 1;
            if (hp <= 0)
            {
                if (m_deadVFXPrefab != null)
                {
                    GameObject vfxObj = ObjectPool.Instance.Pop(m_deadVFXPrefab);
                    vfxObj.transform.position = transform.position;
                    vfxObj.transform.forward = other.transform.forward;
                    vfxObj.SetActive(true);
                    ParticleSystemCtrl vfx = m_deadVFXPrefab.GetComponent<ParticleSystemCtrl>();
                    vfx.Play();
                }
                gameObject.SetActive(false);
                state = State.Dead;
                EventMsgManager.Launch(EventMsg.KilledTheEnemy);
            }
        }
    }

    void Fire() {
        if (m_timer > fireCd)
        {
            m_timer = 0;
            var bulletObject = Instantiate(m_bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.transform.forward = transform.forward;
        }
        m_timer += Time.deltaTime;
    }

    bool IsFacingToPlayer() {
        if (m_player == null)
        {
            return false;
        }
        Vector3 distance = m_player.transform.position - transform.position;
        distance.y = 0;
        if (Vector3.Angle(transform.forward, distance) < 1)
        {
            return true;
        }
        return false;
    }

    void RotateToPlayer() {
        if (m_player == null)
        {
            return;
        }
        Vector3 v = m_player.transform.position - transform.position;
        v.y = 0;
        Vector3 cross = Vector3.Cross(transform.forward, v);
        cross.x = 0;
        cross.z = 0;
        float angle = Vector3.Angle(transform.forward, v);
        transform.Rotate(cross, Mathf.Min(turnSpeed, Mathf.Abs(angle)));
    }

    bool IsInPosition(Vector3 pos) {
        Vector3 v = pos - transform.position;
        v.y = 0;
        return v.magnitude < 0.1f;
    }

    void MoveToPosition(Vector3 pos) {
        Vector3 v = pos - transform.position;
        v.y = 0;
        transform.position += v.normalized * moveSpeed * Time.deltaTime;
    }

    void SetFireView() {
        Vector3 farLeftRayPos = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * viewRadius;
        for (int i = 0; i <= viewRayNum; i++)
        {
            Vector3 rayPos = Quaternion.Euler(0, (viewAngle / viewRayNum) * i, 0) * farLeftRayPos; ;
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
                if (hit.transform.tag == "Player")
                {
                    m_player = hit.transform.gameObject;
                    state = State.Attack;
                }
            }
        }
    }

}