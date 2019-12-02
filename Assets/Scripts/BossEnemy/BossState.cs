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
public class BossStateIdle : BossState { }
//直進
public class BossStateDown: BossState { }
//上昇
public class BossStateReturn : BossState { }