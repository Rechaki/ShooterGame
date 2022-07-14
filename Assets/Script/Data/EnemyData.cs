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
    public float NowViewAngle { get; private set; }

    public event EventDataHandler<EnemyData> RefreshEvent;

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
        EventMessenger<Collision>.AddListener(EventMsg.CollisionOfEnemy, CheckCollision);
    }

    ~EnemyData() {
        EventMessenger<Collision>.RemoveListener(EventMsg.CollisionOfEnemy, CheckCollision);
    }

    void CheckCollision(Collision collision) {
        if (collision.transform.tag == "Bullet")
        {
            var bullet = collision.transform.GetComponent<Bullet>();
            NowHp -= bullet.Damage;
            Update();
        }

        if (NowHp == 0)
        {
            EventMessenger.Launch(EventMsg.GameOver);
        }
    }

    void Update() {
        RefreshEvent?.Invoke(this);
    }

}
