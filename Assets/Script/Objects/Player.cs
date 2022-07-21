using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    public List<Friend> friends = new List<Friend>();

    [SerializeField]
    Rigidbody _rb;
    [SerializeField]
    Transform _firePoint;

    CharacterData _characterData;
    GameObject _bulletPrefab;
    GameObject _deadVFXPrefab;
    Vector3 _lastPos = Vector3.zero;
    float _time = 0.5f;
    float _moveSpeed;
    float _atkSpeed;
    Queue<Vector3> _posQueue = new Queue<Vector3>();

    const float SPACE = 2.0f;
    const float LOOK_AT_DES = 10.0f;

    void Start() {
        _bulletPrefab = ResourceManager.I.Load<GameObject>(AssetPath.PLAYER_BULLET);
        _deadVFXPrefab = ResourceManager.I.Load<GameObject>(AssetPath.PLAYER_DEAD_VFX);
        _lastPos = transform.position;

        InputManager.I.MoveEvent += Move;
        InputManager.I.LookAtEvent += LookAt;
        InputManager.I.FireEvent += Fire;
        DataManager.I.PlayerData.RefreshEvent += Refresh;
        //UIManager.I.Open(AssetPath.MAIN_UI_PANEL);

        Refresh(DataManager.I.PlayerData.CharacterData);
    }

    void Update() {
        SetFriend();

        _time += Time.deltaTime;

    }

    void OnDestroy() {
        InputManager.I.MoveEvent -= Move;
        InputManager.I.LookAtEvent -= LookAt;
        InputManager.I.FireEvent -= Fire;
        DataManager.I.PlayerData.RefreshEvent -= Refresh;
    }

    void OnCollisionEnter(Collision collision) {
        _characterData.CheckCollision(collision);
    }

    void Move(Vector2 v) {
        float x = v.x;
        float z = v.y;
        if (x * z != 0)
        {
            x = x * Mathf.Sqrt(1 - (z * z) / 2.0f);
            z = z * Mathf.Sqrt(1 - (x * x) / 2.0f);
        }
        Vector3 moveInput = new Vector3 (x, 0, z);
        _rb.velocity = moveInput * _moveSpeed;// * Time.deltaTime;
    }

    void LookAt(Vector2 v) {
        switch (InputManager.I.DevicesType)
        {
            case InputManager.Devices.Keyboard:
                Vector3 pos = new Vector3(v.x, v.y, 0);
                Ray ray = Camera.main.ScreenPointToRay(pos);
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    Vector3 point = ray.GetPoint(distance);
                    transform.LookAt(new Vector3(point.x, 1f, point.z));
                }
                break;
            case InputManager.Devices.Gamepad:
                Vector3 newPos = new Vector3(v.normalized.x, 0, v.normalized.y);
                transform.LookAt(transform.position + newPos);
                break;
            default:
                break;
        }
        
    }

    void Fire(float value) {
        if (value > 0 && _time > _atkSpeed)
        {
            _time = 0;
            GameObject bulletObject = ObjectPool.I.Pop(_bulletPrefab);
            bulletObject.transform.position = _firePoint.position;
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.transform.forward = transform.forward;
            bullet.gameObject.SetActive(true);
        }
    }

    void Refresh(CharacterData data) {
        _characterData = data;
        _moveSpeed = data.NowMoveSpeed;
        _atkSpeed = data.NowAtkSpeed;
        HPCheck(data.NowHp);
    }

    void HPCheck(int hp) {
        if (hp <= 0)
        {
            Dead();
            GlobalMessenger.Launch(EventMsg.GameOver);
        }
    }

    void Dead() {
        if (_deadVFXPrefab != null)
        {
            GameObject vfxObj = ObjectPool.I.Pop(_deadVFXPrefab);
            vfxObj.transform.position = transform.position;
            vfxObj.transform.forward = -transform.forward;
            vfxObj.SetActive(true);
            ParticleSystemCtrl vfx = _deadVFXPrefab.GetComponent<ParticleSystemCtrl>();
            vfx.Play();
        }
        gameObject.SetActive(false);
    }

    void SetFriend() {
        if (Vector3.Distance(_lastPos, transform.position) >= SPACE)
        {
            _lastPos = transform.position;
            _posQueue.Enqueue(_lastPos);
            if (_posQueue.Count > 10)
            {
                _posQueue.Dequeue();
            }
        }

        if (friends.Count > 0)
        {
            int index = 0;
            //Vector3 prevPos = transform.position;
            Vector3 velocityTemp = Vector3.zero;
            Vector3[] posList = _posQueue.ToArray();
            for (int i = posList.Length - 2; i > 0; i--)
            {
                if (index < friends.Count)
                {
                    Debug.DrawLine(transform.position, posList[i]);
                    friends[index].targetPos = posList[i];
                    index++;
                }

            }

        }
    }

}
