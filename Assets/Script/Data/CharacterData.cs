using UnityEngine;

public class CharacterData : BaseData
{
    public int HP => _baseData.hp;
    public int MP => _baseData.mp;
    public int ATK => _baseData.atk;
    public int Def => _baseData.def;
    public int NowHp { get; private set; }
    public int NowMp { get; private set; }
    public int NowAtk { get; private set; }
    public int NowDef { get; private set; }
    public float NowMoveSpeed { get; private set; }
    public float NowAtkSpeed { get; private set; }
    public float NowViewRadius { get; private set; }
    public Vector3 NowPos { get; private set; }

    public event EventDataHandler<CharacterData> RefreshEvent;

    CharacterBaseData _baseData;

    public CharacterData(){}

    public CharacterData(CharacterBaseData baseData)
    {
        _baseData = baseData;
        NowHp = baseData.hp;
        NowMp = baseData.mp;
        NowAtk = baseData.atk;
        NowDef = baseData.def;
        NowMoveSpeed = baseData.moveSpeed;
        NowAtkSpeed = baseData.atkSpeed;
        NowViewRadius = baseData.viewRadius;
    }

    ~CharacterData() {

    }

    public void Damage(int damage) {
        NowHp -= damage;
        Update();
    }

    void Update() {
        RefreshEvent?.Invoke(this);
    }


}
