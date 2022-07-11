using UnityEngine;

public class CharacterData : BaseData
{
    public CharacterBaseData baseData;
    public int nowHp;
    public int nowMp;
    public int nowAtk;
    public int nowDef;
    public float nowMoveSpeed;
    public float nowAtkSpeed;
    public float nowViewRadius;
    public Vector3 nowPos;

    public event EventDataHandler<CharacterData> refreshEvent;

    void Update() {
        refreshEvent.Invoke(this);
    }


}
