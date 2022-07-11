public class PlayerData : BaseData
{
    public int nowHp;
    public int maxHp;

    public event EventDataHandler<PlayerData> refreshEvent;

    public PlayerData() {
        nowHp = 10;
        maxHp = 10;
        EventMsgManager.AddListener(EventMsg.Damage, Damage);
    }

    ~PlayerData() {
        EventMsgManager.RemoveListener(EventMsg.Damage, Damage);
    }

    void Update() {
        refreshEvent?.Invoke(this);
    }

    void Damage() {
        nowHp--;
        Update();
    }
}
