using UnityEngine;

public class CharacterData : BaseData
{
    public CharacterBaseData BaseData { get; }
    public int NowHp { get; private set; }
    public int NowMp { get; private set; }
    public int NowAtk { get; private set; }
    public int NowDef { get; private set; }
    public float NowMoveSpeed { get; private set; }
    public float NowAtkSpeed { get; private set; }
    public float NowViewRadius { get; private set; }
    public Vector3 NowPos { get; private set; }

    public event EventDataHandler<CharacterData> refreshEvent;

    public CharacterData(){}

    public CharacterData(CharacterBaseData baseData)
    {
        NowHp = baseData.hp;
        NowMp = baseData.mp;
        NowAtk = baseData.atk;
        NowDef = baseData.def;
        NowMoveSpeed = baseData.moveSpeed;
        NowAtkSpeed = baseData.atkSpeed;
        NowViewRadius = baseData.viewRadius;
    }

    void Update() {
        refreshEvent.Invoke(this);
    }


}
