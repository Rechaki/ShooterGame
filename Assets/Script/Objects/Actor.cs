using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private int _hp = 0;
    private int _maxHp = 0;
    private int _atk = 0;
    private int _maxAtk = 0;
    private int _def = 0;
    private int _maxDef = 0;
    private int _moveSpeed = 0;
    private int _maxMoveSpeed = 0;
    private ActorState _state = ActorState.None;
    private List<SkillData> _skillDatas = new List<SkillData>();
    private List<BuffData> _buffs = new List<BuffData>();
    private Queue<BuffData> _buffsWaitingToAdd = new Queue<BuffData>();
    private List<float> _buufTimer = new List<float>();

    public int HP { get { return _hp; } }
    public int MaxHP { get { return _maxHp; } }
    public int ATK { get { return _atk; } }
    public int MaxATK { get { return _maxAtk; } }
    public int DEF { get { return _def; } }
    public int MaxDEF { get { return _maxDef; } }
    public int MoveSpeed { get { return _moveSpeed; } }
    public int MaxMoveSpeed { get { return _maxMoveSpeed; } }

    void Update() {
        StateUpdate();
    }

    public void StateUpdate() {
        foreach (var buff in _buffs)
        {
            foreach (var property in buff.modifiedProperties)
            {
                switch (property.property)
                {
                    case ModifiedProperty.None:
                        break;
                    case ModifiedProperty.Hp:
                        _hp += property.actualValue;
                        break;
                    case ModifiedProperty.MaxHp:
                        _maxHp += property.actualValue;
                        break;
                    case ModifiedProperty.Atk:
                        _atk += property.actualValue;
                        break;
                    case ModifiedProperty.MaxAtk:
                        _maxAtk += property.actualValue;
                        break;
                    case ModifiedProperty.Def:
                        _def += property.actualValue;
                        break;
                    case ModifiedProperty.MaxDef:
                        _maxDef += property.actualValue;
                        break;
                    case ModifiedProperty.MoveSpeed:
                        _moveSpeed += property.actualValue;
                        break;
                    case ModifiedProperty.MaxMoveSpeed:
                        _maxMoveSpeed += property.actualValue;
                        break;
                    case ModifiedProperty.CD:
                        break;
                    default:
                        break;
                }
            }
            buff.durationTime -= Time.deltaTime;
        }

        _buffs.RemoveAll(buff => (buff.durationTime < 0));
    }

    public void Execute(int index) {

    }

    public void Hurt(Actor sender, SkillData skill) {
        foreach (var newBuff in skill.buffs)
        {
            newBuff.owner = sender;
            bool isAdded = false;
            for (int i = 0; i < _buffs.Count; i++)
            {
                if (newBuff.id == _buffs[i].id)
                {
                    if (_buffs[i].attributes == BuffAttributes.Multiple)
                    {
                        _buufTimer[i] += newBuff.durationTime;
                        isAdded = true;
                        break;
                    }
                }
            }
            if (!isAdded)
            {
                _buffsWaitingToAdd.Enqueue(newBuff);
            }
        }

        SetBuffValue();

        while (_buffsWaitingToAdd.Count > 0)
        {
            var buff = _buffsWaitingToAdd.Dequeue();
            _buffs.Add(buff);
            _buufTimer.Add(buff.durationTime);
        }


    }

    private void SetBuffValue() {
        foreach (var buff in _buffsWaitingToAdd)
        {
            foreach (var property in buff.modifiedProperties)
            {
                property.actualValue = (int)GetPropertyValue(this, buff.owner, property);
            }
        }
    }

    private float GetPropertyValue(Actor myself, Actor other, PropertyData data) {
        float value = 0;
        switch (data.source)
        {
            case ValueSource.Set:
                value = data.value;
                break;
            case ValueSource.MySelf:
                value = GetBaseValue(myself, data) * (1 + data.value);
                break;
            case ValueSource.Others:
                value = GetBaseValue(other, data) * (1 + data.value);
                break;
            default:
                break;
        }
        return value;
    }

    private float GetBaseValue(Actor source, PropertyData data) {
        float value = 0;
        switch (data.sourceType)
        {
            case ModifiedProperty.None:
                break;
            case ModifiedProperty.Hp:
                value = source.HP;
                break;
            case ModifiedProperty.MaxHp:
                value = source.MaxHP;
                break;
            case ModifiedProperty.Atk:
                value = source.ATK;
                break;
            case ModifiedProperty.MaxAtk:
                value = source.MaxATK;
                break;
            case ModifiedProperty.Def:
                value = source.DEF;
                break;
            case ModifiedProperty.MaxDef:
                value = source.MaxDEF;
                break;
            case ModifiedProperty.MoveSpeed:
                value = source.MoveSpeed;
                break;
            case ModifiedProperty.MaxMoveSpeed:
                value = source.MaxMoveSpeed;
                break;
            case ModifiedProperty.CD:
                
                break;
            default:
                break;
        }
        return value;
    }

}
