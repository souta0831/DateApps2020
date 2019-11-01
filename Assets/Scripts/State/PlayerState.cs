using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

namespace Player_State {
    //ステートクラス
    public abstract class PlayerState : StateBase
    {
        public abstract PlayerStateID GetState();
    }
    //待機クラス
    public class PlayerStateIdle : PlayerState {

        public override PlayerStateID GetState()
        {
            return PlayerStateID.IDLE;
        }
    }
    //走りクラス
    public class PlayerStateRun : PlayerState {
        public override PlayerStateID GetState()
        {
            return PlayerStateID.RUN;
        }
    }
    //スライディングクラス
    public class PlayerStateSliding : PlayerState {

        public override PlayerStateID GetState()
        {
            return PlayerStateID.SLIDING;
        }
    }
    public class PlayerStateWallRun : PlayerState
    {

        public override PlayerStateID GetState()
        {
            return PlayerStateID.WALLRUN;
        }
    }

    public class PlayerStateJump : PlayerState
    {

        public override PlayerStateID GetState()
        {
            return PlayerStateID.JUMP;
        }
    }

}

