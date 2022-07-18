using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private string _id = "E0000";
    [Header("最小追いかける距離")]
    [SerializeField]
    private float _minChaseDist = 3.0f;       
    [Header("最大追いかける距離")]
    [SerializeField]
    private float _maxChaseDist = 11.0f;      
    [Header("離れた最大距離")]
    [SerializeField]
    private float _maxLeaveDist = 3.0f;
    [SerializeField]
    private Transform _firePoint;

    public enum State
    {
        Idle,
        Attack,
        Back,
        Dead,
    }

    private GameObject _player = null;
    private GameObject _bulletPrefab;
    private GameObject _deadVFXPrefab;
    private Vector3 _startPoint;          
    private Quaternion _startDirection;   
    private State _state = State.Idle;
    private float _timer = 0;
    private float _moveSpeed;
    private float _turnSpeed;
    private float hp;
    private float _atkSpeed;
    private float _viewRadius;
    private int _viewAngle;
    private int _viewRayNum;

    void Start() {
        _bulletPrefab = ResourceManager.I.Load<GameObject>(AssetPath.ENEMY_BULLET);
        _deadVFXPrefab = ResourceManager.I.Load<GameObject>(AssetPath.ENEMY_DEAD_VFX);
        _startPoint = transform.position;
        _startDirection = transform.rotation;
    }

    void Refresh(EnemyData data) {
        _moveSpeed = data.NowMoveSpeed;
        _atkSpeed = data.NowAtkSpeed;
        _turnSpeed = data.NowTurnSpeed;
        _viewRadius = data.NowViewRadius;
        _viewAngle = data.NowViewAngle;
        _viewRayNum = data.NowViewAngle / 5;
        HPCheck(data.NowHp);
    }

    void Update() {
        if (GameManager.I.isGameOver)
        {
            return;
        }
        if (_state == State.Dead)
        {
            return;
        }

        if (_state == State.Attack)
        {
            AttackAction();
        }
        else if (_state == State.Back)
        {
            BackAction();
        }
        SetFireView();
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag != "Bullet")
        {
            return;
        }
        if (_state == State.Idle)
        {
            _state = State.Attack;
        }
    }

    void HPCheck(int hp) {
        if (hp <= 0)
        {
            _state = State.Dead;
            if (_deadVFXPrefab != null)
            {
                //GameObject vfxObj = ObjectPool.I.Pop(_deadVFXPrefab);
                //vfxObj.transform.position = transform.position;
                //vfxObj.transform.forward = other.transform.forward;
                //vfxObj.SetActive(true);
                //ParticleSystemCtrl vfx = _deadVFXPrefab.GetComponent<ParticleSystemCtrl>();
                //vfx.Play();
            }
            gameObject.SetActive(false);
            EventMessenger.Launch(EventMsg.KilledTheEnemy);
        }
    }

    void IdleAction() {

    }

    void AttackAction() {
        if (_player != null)
        {
            float distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            float distanceToStartPoint = Vector3.Distance(_startPoint, transform.position);
            if (distanceToStartPoint >= _maxLeaveDist)
            {
                _state = State.Back;
                return;
            }
            else if (distanceToPlayer <= _minChaseDist)
            {
                transform.position += _player.transform.forward.normalized * _moveSpeed * Time.deltaTime;
            }
            else if (distanceToPlayer >= _maxChaseDist)
            {
                _state = State.Back;
                return;
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
    }

    void BackAction() {
        if (IsInPosition(_startPoint))
        {
            _state = State.Idle;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _startDirection, _turnSpeed);
            return;
        }
        MoveToPosition(_startPoint);
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
            Vector3 rayPos = Quaternion.Euler(0, (_viewAngle / _viewRayNum) * i, 0) * farLeftRayPos; ;
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
                if (hit.transform.tag == "Player")
                {
                    _player = hit.transform.gameObject;
                    _state = State.Attack;
                }
            }
        }
    }

}