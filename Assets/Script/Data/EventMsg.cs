public enum EventMsg
{
    CollisionOfPlayer,
    CollisionOfEnemy,
    RayHitObject,
    Damage,                     //プレイヤーHPの変化
    KilledTheEnemy,             //敵を倒れた
    GameClear,                  //ゲームクリア
    GameOver,                   //ゲームオーバー
    GameRestart,                //ゲームリスタート
}
