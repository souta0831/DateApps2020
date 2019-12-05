using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using State;
//キャラクターの状態
public enum PlayerStateID
{
    IDLE,
    RUN,
    SLIDING,
    SLIDINGC_END,
    JUMP,
    WALLRUN
}

    //ステートクラス
    public abstract class PlayerState : StateBase
    {
    }
    //待機クラス
    public class PlayerStateIdle : PlayerState {

    }
    //走りクラス
    public class PlayerStateRun : PlayerState {
    }
    //スライディングクラス
    public class PlayerStateSliding : PlayerState {

    }
    public class PlayerStateWallRun : PlayerState
    {

    }
public class PlayerStateRushAttack : PlayerState
{

}
public class PlayerStateDamage : PlayerState
{

}

public class PlayerStateJump : PlayerState
    {

    }



