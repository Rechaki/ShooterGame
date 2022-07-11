using System.Collections;
using System.Collections.Generic;

public class SkillData
{
    public string id;                                                                       //ID
    public string name;                                                                     //名前
    public string description;                                                              //記述
    public float cd;                                                                        //CD
    public float distance;                                                                  //距離
    public float angle;                                                                     //角度
    public SkillBehavior attackType = SkillBehavior.Point;                                  //攻撃タイプ
    public List<BuffData> buffs = new List<BuffData>();                                     //Buff
    public string vfxName;                                                                  //エフェクト名前
    public string hitVfxName;                                                               //受けったエフェクト名前
    public string animationName;                                                            //アニメーション名前
}
