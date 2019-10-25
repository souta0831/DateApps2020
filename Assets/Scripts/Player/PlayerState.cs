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

namespace PlayerState {
    //ステート管理クラス
    public class StateProcessor
    {
        private PlayerState _State;
        public PlayerState State
        {
            set { _State = value; }
            get { return _State; }
        }
        public void Execute()
        {
            State.Execute();
        }
        

    }
    //ステートクラス
    public abstract class PlayerState {

        public delegate void executeState();
        public executeState execDelegate;
        public virtual void Execute()
        {
            if (execDelegate != null) execDelegate();
        }
        public abstract PlayerStateID GetState();
    }
    //状態クラス

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

