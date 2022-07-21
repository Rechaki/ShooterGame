using UnityEngine;

public class EnemyData : BaseData
{

    public int HP => _baseData.hp;
    public int MP => _baseData.mp;
    public int ATK => _baseData.atk;
    public int Def => _baseData.def;
    public float MoveSpeed => _baseData.moveSpeed;
    public float AtkSpeed => _baseData.atkSpeed;
    public int NowHp { get; private set; }
    public int NowMp { get; private set; }
    public int NowAtk { get; private set; }
    public int NowDef { get; private set; }
    public float NowMoveSpeed { get; private set; }
    public float NowAtkSpeed { get; private set; }
    public float NowTurnSpeed { get; private set; }
    public float NowViewRadius { get; private set; }
    public int NowViewAngle { get; private set; }
    public State CurrentState { get; private set; }
    public GameObject Player { get; private set; }
    public int HashCode { get; private set; }

    public event EventDataHandler<EnemyData> RefreshEvent;

    public enum State
    {
        Idle,
        Attack,
        Back,
        Dead,
    }

    EnemyBaseData _baseData;

    public EnemyData() { }

    public EnemyData(EnemyBaseData baseData) {
        _baseData = baseData;
        NowHp = baseData.hp;
        NowMp = baseData.mp;
        NowAtk = baseData.atk;
        NowDef = baseData.def;
        NowMoveSpeed = baseData.moveSpeed;
        NowAtkSpeed = baseData.atkSpeed;
        NowTurnSpeed = baseData.turnSpeed;
        NowViewRadius = baseData.viewRadius;
        NowViewAngle = baseData.viewAngle;
        CurrentState = State.Idle;
        HashCode = GetHashCode();

        EventMessenger<Collision>.AddListener("CollisionOfEnemy" + HashCode, new Callback<Collision>(CheckCollision));
        EventMessenger<RaycastHit>.AddListener("RayHitObject" + HashCode, new Callback<RaycastHit>(CheckRayHitObject));
        EventMessenger.AddListener("EnemyReturnToStartPos", new Callback(ToBackState));
        EventMessenger.AddListener("EnemyToIdleState", new Callback(ToIdleState));
    }

    ~EnemyData() {
        EventMessenger<Collision>.RemoveListener("CollisionOfEnemy" + HashCode, CheckCollision);
        EventMessenger<RaycastHit>.RemoveListener("RayHitObject" + HashCode, CheckRayHitObject);

        RefreshEvent = null;
    }

    void CheckCollision(Collision collision) {
        Debug.Log(this.GetHashCode());
        if (collision.transform.tag == "Bullet")
        {
            var bullet = collision.transform.GetComponent<Bullet>();
            NowHp -= bullet.Damage;
            CurrentState = NowHp <= 0 ? State.Dead : State.Attack;
            Update();
        }
    }

    void CheckRayHitObject(RaycastHit hit)
    {
        if (hit.transform.tag == "Player")
        {
            Player = hit.transform.gameObject;
            CurrentState = State.Attack;
            Update();
        }
        else
        {
            Player = null;
        }
    }

    void ToIdleState()
    {
        CurrentState = State.Idle;
        Update();
    }

    void ToBackState()
    {
        CurrentState = State.Back;
        Update();
    }

    void Update() {
        RefreshEvent?.Invoke(this);
    }

}
