using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
public enum BossStateID
{

}

//ステートクラス
public abstract class BossState : StateBase
{
}
//落下
public class BossStateFall : BossState { }
//直進
public class BossStateChase: BossState { }
//上昇
public class BossStateRise : BossState { }