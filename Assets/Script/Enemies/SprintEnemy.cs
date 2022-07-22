using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintEnemy : MonoBehaviour
{
    [SerializeField]
    string _id = "E0001";
    [Header("スプリントスピード")]
    [SerializeField]
    float _sprintSpeed = 15.0f;

    public List<Transform> movePos = new List<Transform>();
    public int Atk { get; private set; }

    EnemyData _enemyData;
    GameObject _deadVFX;
    Vector3 _targetPos;
    float _viewRadius;
    float _moveSpeed;
    float _turnSpeed;
    int m_index = 0;

    void Start() {
        _deadVFX = ResourceManager.I.Load<GameObject>(AssetPath.SPRINT_DEAD_VFX);
        _enemyData = DataManager.I.GetEnemyData(_id);
        _enemyData.RefreshEvent += Refresh;
        Refresh(_enemyData);

        if (movePos.Count != 0)
        {
            m_index = 0;
            _targetPos = movePos[m_index].position;
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
        switch (_enemyData.CurrentState)
        {
            case EnemyActionState.Idle:
                IdleAction();
                break;
            case EnemyActionState.Attack:
                transform.position += transform.forward * _sprintSpeed * Time.deltaTime;
                break;
            case EnemyActionState.Dead:

                break;
            default:
                break;
        }

        SetFireView();
    }

    void OnDestroy() {
        _enemyData.RefreshEvent -= Refresh;
    }

    void Refresh(EnemyData data) {
        Atk = data.NowAtk;
        _moveSpeed = data.NowMoveSpeed;
        _turnSpeed = data.NowTurnSpeed;
        _viewRadius = data.NowViewRadius;
    }

    void SetFireView() {
        Vector3 farRayPos =  transform.forward * _viewRadius;
        Vector3 rayPos = Quaternion.Euler(0, 0, 0) * farRayPos;
        Ray ray = new Ray(transform.position, rayPos);
        RaycastHit hit = new RaycastHit();
        int mask = LayerMask.GetMask("Player", "Default");
        Physics.Raycast(ray, out hit, _viewRadius, mask);
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
                _enemyData.ToAttckState();
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Bullet")
        {
            return;
        }
        if (_deadVFX != null)
        {
            GameObject vfxObj = ObjectPool.I.Pop(_deadVFX);
            vfxObj.transform.position = transform.position;
            vfxObj.transform.forward = other.transform.forward;
            vfxObj.SetActive(true);
            ParticleSystemCtrl vfx = _deadVFX.GetComponent<ParticleSystemCtrl>();
            vfx.Play();
        }
        gameObject.SetActive(false);
        _enemyData.ToDeadState();
    }

    void IdleAction() {
        if (m_index != -1)
        {
            if (Vector3.Distance(_targetPos, transform.position) < 0.5f)
            {
                m_index++;
                if (m_index == movePos.Count)
                {
                    m_index = 0;
                }
                _targetPos = movePos[m_index].position;
            }
            if (IsFacingToPlayer())
            {
                transform.position += (_targetPos - transform.position).normalized * _moveSpeed * Time.deltaTime;
            }
            else
            {
                RotateToPos(_targetPos);
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
        transform.Rotate(cross, Mathf.Min(_turnSpeed, Mathf.Abs(angle)));
    }

    bool IsFacingToPlayer() {
        Vector3 distance = _targetPos - transform.position;
        distance.y = 0;
        if (Vector3.Angle(transform.forward, distance) < 1)
        {
            return true;
        }
        return false;
    }


}
