using UnityEngine;
using System.Collections;

public class CloudEnemy : MonoBehaviour
{
    [Header("高さ")]
    [SerializeField]
    float _height = 5;
    [Header("移動スピード")]
    [SerializeField]
    float _moveSpeed = 2.0f;
    [Header("逃げるスピード")]
    [SerializeField]
    float _escapeSpeed = 2.0f;
    [Header("向きを変わるスピード")]
    [SerializeField]
    float _turnSpeed = 1.0f;
    [Header("攻撃距離")]
    [SerializeField]
    float _attackDistance = 10.0f;
    [Header("逃げる距離")]
    [SerializeField]
    float _escapeDistance = 20.0f;
    [Header("移動範囲オブジェクト")]
    [SerializeField]
    BoxCollider _area;
    [Header("弾")]
    [SerializeField]
    GameObject _bulletPrefab;
    [Header("弾数")]
    [SerializeField]
    int _bulletNum = 2;
    [Header("弾と弾のスペース")]
    [SerializeField]
    float _bulletSpace = 2;
    [Header("発射冷却時間")]
    [SerializeField]
    float _fireCd = 0.5f;
    [Header("弾生成ポジション")]
    [SerializeField]
    Transform _firePoint;
    [Header("弾モード")]
    [SerializeField]
    FireMode fireMode = FireMode.Vertical;

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

    Vector3 _startPoint;          
    Vector3 _targetPos = Vector3.zero;
    int _viewAngle = 360;
    float _timer = 0;
    float _minXPos;
    float _maxXPos;
    float _minZPos;
    float _maxZPos;

    void Start() {
        _startPoint = transform.position;
        _timer = _fireCd;
        float width = _area.size.x;
        float length = _area.size.z;
        _minXPos = _area.transform.position.x - width / 2;
        _maxXPos = _area.transform.position.x + width / 2;
        _minZPos = _area.transform.position.z - length / 2;
        _maxZPos = _area.transform.position.z + length / 2;

        _targetPos = new Vector3(Random.Range(_minXPos, _maxXPos), transform.position.y, Random.Range(_minZPos, _maxZPos));
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
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * _height), Color.red);
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            float distance = Vector3.Distance(transform.position, hit.point);
            if (distance < _height)
            {
                _targetPos = new Vector3(_targetPos.x, _height + hit.point.y, _targetPos.z);
            }
            else
            {
                _targetPos = new Vector3(_targetPos.x, _height, _targetPos.z);
            }
        }

    }

    void IdleAction()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.position) < _escapeDistance)
            {
                state = State.Attack;
            }
        }

        if (IsFacingToTarget(_targetPos))
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _moveSpeed);
        }
        else
        {
            RotateToTarget(_targetPos);
        }

        if (Vector3.Distance(transform.position, _targetPos) <= 0.5f)
        {
            _targetPos = new Vector3(Random.Range(_minXPos, _maxXPos), transform.position.y, Random.Range(_minZPos, _maxZPos));
        }
    }

    void AttackAction()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.position) > _attackDistance)
            {
                if (IsFacingToTarget(player.position))
                {
                    _targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _moveSpeed);
                    Fire();
                }
                else
                {
                    RotateToTarget(player.position);
                }
            }
            else
            {
                _targetPos = new Vector3(Random.Range(_minXPos, _maxXPos), transform.position.y, Random.Range(_minZPos, _maxZPos));
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
            if (Vector3.Distance(transform.position, _targetPos) <= 0.5f)
            {
                state = State.Idle;
            }


            transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _escapeSpeed);
        }
        else
        {
            state = State.Idle;
        }
        
    }

    void CanNotEscape()
    {
        if (IsFacingToTarget(_targetPos))
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _moveSpeed);
        }
        else
        {
            RotateToTarget(_targetPos);
        }

        if (Vector3.Distance(transform.position, _targetPos) <= 0.5f)
        {
            _targetPos = new Vector3(Random.Range(_minXPos, _maxXPos), transform.position.y, Random.Range(_minZPos, _maxZPos));
        }
    }

    void Fire() {
        if (_timer > _fireCd)
        {
            _timer = 0;
            switch (fireMode)
            {
                case FireMode.Horizontal:
                    for (int i = 0; i < _bulletNum; i++)
                    {
                        var bulletObject = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
                        Bullet bullet = bulletObject.GetComponent<Bullet>();
                        var v = (player.position - transform.position).normalized;
                        float offsetX = (_bulletSpace * (-_bulletNum / 2 + i)) / 10;
                        bullet.transform.forward = new Vector3(v.x + offsetX, v.y, v.z);
                    }
                    break;
                case FireMode.Vertical:
                    for (int i = 0; i < _bulletNum; i++)
                    {
                        var bulletObject = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
                        Bullet bullet = bulletObject.GetComponent<Bullet>();
                        bullet.transform.forward = (player.position - transform.position).normalized;
                        bullet.transform.position += bullet.transform.forward * _bulletSpace * i; 
                    }
                    break;
                default:
                    break;
            }
        }
        _timer += Time.deltaTime;
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
        transform.Rotate(cross, Mathf.Min(_turnSpeed, Mathf.Abs(angle)));
    }

}