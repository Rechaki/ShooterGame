public class PlayerData : BaseData
{
    public event EventDataHandler<CharacterData> RefreshEvent;
    public CharacterData CharacterData => data;

    CharacterData data;
    string _id = "C0000";

    public PlayerData() {
        data = DataManager.I.GetCharacterData(_id);
        data.RefreshEvent += Update;
    }

    ~PlayerData() {

    }

    void Update(CharacterData data) {
        RefreshEvent?.Invoke(data);
    }

}
