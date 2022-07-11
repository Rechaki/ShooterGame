using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour
{
    public Transform firePoint;
    public Rigidbody rb;
    public Vector3 targetPos;
    public float fireCD = 0.2f;
    public int hp = 10;

    private GameObject m_bulletPrefab;
    private GameObject m_deadVFXPrefab;
    private float m_time = 0.5f;
    private bool m_rescued = false;

    void Start()
    {
        m_bulletPrefab = ResourceManager.Instance.Load<GameObject>(AssetPath.PLAYER_BULLET);
        m_deadVFXPrefab = ResourceManager.Instance.Load<GameObject>(AssetPath.PLAYER_DEAD_VFX);
        targetPos = Vector3.zero;
        m_rescued = false;
    }

    void Update()
    {
        if (m_rescued)
        {
            LookAtMouse();
            Vector3 velocityTemp = Vector3.zero;
            Debug.DrawLine(transform.position, targetPos, Color.white);

            if (Vector3.Distance(targetPos, transform.position) < 0.1)
            {
                velocityTemp = Vector3.zero;
                rb.velocity = velocityTemp;
            }
            else
            {
                if (!IsReachingPlayer())
                {
                    //velocityTemp = (targetPos - transform.position).normalized;
                    velocityTemp = (targetPos - transform.position).normalized * 6;
                    velocityTemp.y = rb.velocity.y;
                    rb.velocity = velocityTemp;
                }

            }

            m_time += Time.deltaTime;
            if (m_time > 0.2 && Input.GetMouseButton(0))
            {
                Fire();
                m_time = 0;
            }
        }

    }

    void OnCollisionEnter(Collision collision) {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            if (!m_rescued)
            {
                transform.rotation = Quaternion.identity;
                Player player = collision.transform.GetComponent<Player>();
                player.friends.Add(this);
                m_rescued = true;
            }
        }
        if (collision.transform.tag == "Bullet")
        {
            hp -= 1;
            if (hp <= 0)
            {
                if (m_deadVFXPrefab != null)
                {
                    GameObject vfxObj = ObjectPool.Instance.Pop(m_deadVFXPrefab);
                    vfxObj.transform.position = transform.position;
                    vfxObj.transform.forward = collision.transform.forward;
                    vfxObj.SetActive(true);
                    ParticleSystemCtrl vfx = m_deadVFXPrefab.GetComponent<ParticleSystemCtrl>();
                    vfx.Play();
                }
                gameObject.SetActive(false);
            }
        }

    }

    private void Fire() {
        GameObject bulletObject = ObjectPool.Instance.Pop(m_bulletPrefab);
        bulletObject.transform.position = firePoint.position;
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.transform.forward = transform.forward;
        bullet.gameObject.SetActive(true);
    }

    private void LookAtMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            transform.LookAt(new Vector3(point.x, 1f, point.z));
        }
    }

    private bool IsReachingPlayer() {
        Vector3 farRayPos = Quaternion.Euler(0, 360 / 2, 0) * transform.forward;
        for (int i = 0; i <= 36; i++)
        {
            Vector3 rayPos = Quaternion.Euler(0, (360 / 36) * i, 0) * farRayPos; ;
            Ray ray = new Ray(transform.position, rayPos);
            RaycastHit hit = new RaycastHit();
            int mask = LayerMask.GetMask("Player", "Default");
            Physics.Raycast(ray, out hit, 1, mask);
            Debug.DrawLine(transform.position, transform.position + rayPos, Color.yellow);

            if (hit.transform != null)
            {
                if (hit.transform.tag == "Player")
                {
                    return true;
                }
            }
        }

        return false;
    }

}
