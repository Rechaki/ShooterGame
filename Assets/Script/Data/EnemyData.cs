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
    public EnemyActionState CurrentState { get; private set; }

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
        CurrentState = EnemyActionState.Idle;
    }

    ~EnemyData() {
        RefreshEvent = null;
    }

    public void Damage(int damage) {
        NowHp -= damage;
        if (NowHp <= 0)
        {
            ToDeadState();
        }
    }

    public void ToIdleState() {
        if (CurrentState == EnemyActionState.Dead) return;
        CurrentState = EnemyActionState.Idle;
        Update();
    }

    public void ToAttckState() {
        if (CurrentState == EnemyActionState.Dead) return;
        CurrentState = EnemyActionState.Attack;
        Update();
    }

    public void ToBackState() {
        if (CurrentState == EnemyActionState.Dead) return;
        CurrentState = EnemyActionState.Back;
        Update();
    }

    public void ToDeadState() {
        CurrentState = EnemyActionState.Dead;
        Update();
    }

    void Update() {
        RefreshEvent?.Invoke(this);
    }

}
