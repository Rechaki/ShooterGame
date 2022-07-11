public enum ActorState
{
    None = 0,
    AttackImmune = 1,                   //攻撃が受けない
    Blind = 2,                          //攻撃できない
}

//スキルのタイプ
public enum SkillBehavior
{
    Hidden = 0,                         //スキルは利用できない
    Passive = 1,                        //パッシブスキル
    Target = 2,                         //目標を指定する必要があり
    NoTarget = 3,                       //目標を指定する必要がない
    Point = 4,                          //単一
    AOE = 5,                            //範囲
}

//スキルの事件
public enum SkillEvent
{
    OnSpawned = 0,                      //生成した時
    OnDied = 1,                         //死亡した時
    OnHit = 2,                          //目標を当たった時
    OnBeHit = 3,                        //当たられた時
    OnChannelStart = 4,                 //スキルを発動した時
    OnChannelInterrupted = 5,           //スキルの発動が中止られた時
    OnChannelSucceeded = 6,             //スキルの発動が成功した時
    OnTime = 7,                         //〇秒が経った時
}

//変更する属性
public enum ModifiedProperty
{
    None = 0,
    Hp = 1,
    MaxHp = 2,
    Atk = 3,
    MaxAtk = 4,
    Def = 5,
    MaxDef = 6,
    MoveSpeed = 7,
    MaxMoveSpeed = 8,
    CD = 10,
}

public enum ValueSource
{
    Set = 0,
    MySelf = 1,
    Others = 2,
}

//Buffのタイプ
public enum BuffAttributes
{
    None = 0,                           //デフォルト  
    Invulnerable = 1,                   //消さない
    Multiple = 2,                       //同じ種類の効果は重複できる
}