using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    string _id = "E0000";
    [Header("最小追いかける距離")]
    [SerializeField]
    float _minChaseDist = 3.0f;       
    [Header("最大追いかける距離")]
    [SerializeField]
    float _maxChaseDist = 11.0f;      
    [Header("離れた最大距離")]
    [SerializeField]
    float _maxLeaveDist = 3.0f;
    [SerializeField]
    Transform _firePoint;
    
    EnemyData _enemyData;
    GameObject _player = null;
    GameObject _bulletPrefab;
    GameObject _deadVFXPrefab;
    Vector3 _contactPoint;
    Vector3 _startPoint;          
    Quaternion _startDirection;   
    float _timer = 0;
    float _moveSpeed;
    float _turnSpeed;
    float _atkSpeed;
    float _viewRadius;
    int _viewAngle;
    int _viewRayNum;

    void Start() {
        _bulletPrefab = ResourceManager.I.Load<GameObject>(AssetPath.ENEMY_BULLET);
        _deadVFXPrefab = ResourceManager.I.Load<GameObject>(AssetPath.ENEMY_DEAD_VFX);
        _startPoint = transform.position;
        _startDirection = transform.rotation;

        _enemyData = DataManager.I.GetEnemyData(_id);
        _enemyData.RefreshEvent += Refresh;
        Refresh(_enemyData);
        LevelManager.I.AddEnemy(_enemyData);
    }

    void Refresh(EnemyData data) {
        _moveSpeed = data.NowMoveSpeed;
        _atkSpeed = data.NowAtkSpeed;
        _turnSpeed = data.NowTurnSpeed;
        _viewRadius = data.NowViewRadius;
        _viewAngle = data.NowViewAngle;
        _viewRayNum = data.NowViewAngle / 5;
        _player = data.Player;
    }

    void Update() {
        if (GameManager.I.isGameOver)
        {
            return;
        }
        switch (_enemyData.CurrentState)
        {
            case EnemyData.State.Idle:
                IdleAction();
                break;
            case EnemyData.State.Attack:
                AttackAction();
                break;
            case EnemyData.State.Back:
                BackAction();
                break;
            case EnemyData.State.Dead:
                DeadAction();
                break;
            default:
                break;
        }

        SetFireView();
    }

    void OnDestroy()
    {
        _enemyData.RefreshEvent -= Refresh;
    }

    void OnCollisionEnter(Collision collision) {
        _contactPoint = collision.transform.forward;
        _enemyData.CheckCollision(collision);
    }

    void IdleAction() {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _startDirection, _turnSpeed);
    }

    void AttackAction() {
        if (_player != null)
        {
            float distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            float distanceToStartPoint = Vector3.Distance(_startPoint, transform.position);
            if (distanceToStartPoint >= _maxLeaveDist)
            {
                _enemyData.ToBackState();
            }
            else if (distanceToPlayer <= _minChaseDist)
            {
                transform.position += _player.transform.forward.normalized * _moveSpeed * Time.deltaTime;
            }
            else if (distanceToPlayer >= _maxChaseDist)
            {
                _enemyData.ToBackState();
            }
            else
            {
                MoveToPosition(_player.transform.position);
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
        else
        {
            //float angle = Vector3.Angle(_contactPoint, transform.forward);
            //transform.Rotate(Vector3.up, Mathf.Min(_turnSpeed, Mathf.Abs(angle)));
            _enemyData.ToBackState();
        }
    }

    void BackAction() {
        if (IsInPosition(_startPoint))
        {
            _enemyData.ToIdleState();
            return;
        }
        MoveToPosition(_startPoint);
    }

    void DeadAction()
    {
        gameObject.SetActive(false);
        GameObject vfxObj = ObjectPool.I.Pop(_deadVFXPrefab);
        vfxObj.transform.position = transform.position;
        vfxObj.transform.forward = _contactPoint;
        vfxObj.SetActive(true);
        ParticleSystemCtrl vfx = _deadVFXPrefab.GetComponent<ParticleSystemCtrl>();
        vfx.Play();
        GlobalMessenger.Launch(EventMsg.KilledTheEnemy);
    }

    void Fire() {
        if (_timer > _atkSpeed)
        {
            _timer = 0;
            var bulletObject = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.transform.forward = transform.forward;
        }
        _timer += Time.deltaTime;
    }

    bool IsFacingToPlayer() {
        if (_player == null)
        {
            return false;
        }
        Vector3 distance = _player.transform.position - transform.position;
        distance.y = 0;
        if (Vector3.Angle(transform.forward, distance) < 1)
        {
            return true;
        }
        return false;
    }

    void RotateToPlayer() {
        if (_player == null)
        {
            return;
        }
        Vector3 v = _player.transform.position - transform.position;
        v.y = 0;
        Vector3 cross = Vector3.Cross(transform.forward, v);
        cross.x = 0;
        cross.z = 0;
        float angle = Vector3.Angle(transform.forward, v);
        transform.Rotate(cross, Mathf.Min(_turnSpeed, Mathf.Abs(angle)));
    }

    bool IsInPosition(Vector3 pos) {
        Vector3 v = pos - transform.position;
        v.y = 0;
        return v.magnitude < 0.1f;
    }

    void MoveToPosition(Vector3 pos) {
        Vector3 v = pos - transform.position;
        v.y = 0;
        transform.position += v.normalized * _moveSpeed * Time.deltaTime;
    }

    void SetFireView() {
        Vector3 farLeftRayPos = Quaternion.Euler(0, -_viewAngle / 2, 0) * transform.forward * _viewRadius;
        for (int i = 0; i <= _viewRayNum; i++)
        {
            Vector3 rayPos = Quaternion.Euler(0, (_viewAngle / _viewRayNum) * i, 0) * farLeftRayPos;
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
                _enemyData.CheckRayHitObject(hit);
            }
        }
    }

}