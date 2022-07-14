public class PlayerData : BaseData
{
    public event EventDataHandler<CharacterData> RefreshEvent;

    CharacterData character;
    string _id = "C0000";

    public PlayerData() {
        character = DataManager.I.GetCharacterData(_id);
        character.RefreshEvent += Update;
    }

    ~PlayerData() {

    }

    void Update(CharacterData data) {
        RefreshEvent?.Invoke(data);
    }

}
