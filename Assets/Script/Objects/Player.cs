using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    public List<Friend> friends = new List<Friend>();

    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private Transform _firePoint;
    [SerializeField]
    private float _moveSpeed;

    private GameObject _bulletPrefab;
    private GameObject _deadVFXPrefab;
    private Vector3 _lastPos = Vector3.zero;
    private float _time = 0.5f;
    private Queue<Vector3> _posQueue = new Queue<Vector3>();

    private const float SPACE = 2.0f;
    private const float LOOK_AT_DES = 10.0f;

    void Start() {
        _bulletPrefab = ResourceManager.Instance.Load<GameObject>(AssetPath.PLAYER_BULLET);
        _deadVFXPrefab = ResourceManager.Instance.Load<GameObject>(AssetPath.PLAYER_DEAD_VFX);
        _lastPos = transform.position;

        InputManager.Instance.MoveEvent += Move;
        InputManager.Instance.LookAtEvent += LookAt;
        InputManager.Instance.FireEvent += Fire;
        //ActionOwner owner = new ActionOwner
        //{
        //    component = transform,
        //    action = Dead
        //};
        //EventMsgManager.Add(EventMsg.GameOver, owner);
    }

    void Update() {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }

        SetFriend();

        _time += Time.deltaTime;

    }

    void OnDestroy() {
        InputManager.Instance.MoveEvent -= Move;
        InputManager.Instance.LookAtEvent -= LookAt;
    }

    void OnCollisionEnter(Collision collision) {
        //Debug.Log(collision.transform.tag);
        if (collision.transform.tag == "Bullet")
        {
            //Debug.Log("!!!!!!");
            EventMsgManager.Launch(EventMsg.Damage);
        }
    }

    private void Move(Vector2 v) {
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

    private void LookAt(Vector2 v) {
        switch (InputManager.Instance.DevicesType)
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

    private void SetFriend() {
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

    private void Fire(float value) {
        if (value > 0 && _time > 0.2)
        {
            _time = 0;
            GameObject bulletObject = ObjectPool.Instance.Pop(_bulletPrefab);
            bulletObject.transform.position = _firePoint.position;
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.transform.forward = transform.forward;
            bullet.gameObject.SetActive(true);
        }
    }

    private void Dead() {
        if (_deadVFXPrefab != null)
        {
            GameObject vfxObj = ObjectPool.Instance.Pop(_deadVFXPrefab);
            vfxObj.transform.position = transform.position;
            vfxObj.transform.forward = -transform.forward;
            vfxObj.SetActive(true);
            ParticleSystemCtrl vfx = _deadVFXPrefab.GetComponent<ParticleSystemCtrl>();
            vfx.Play();
        }
        gameObject.SetActive(false);
    }

}
