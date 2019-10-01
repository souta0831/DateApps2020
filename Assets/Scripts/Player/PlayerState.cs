using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//キャラクターの状態
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
        public abstract string GetStateName();
    }
    //状態クラス

    //待機クラス
    public class PlayerStateIdle : PlayerState {

        public override string GetStateName()
        {
            return "State:Idle";
        }
    }
    //走りクラス
    public class PlayerStateRun : PlayerState {
        public override string GetStateName()
        {
            return "State:Run" ;
        }
    }
    //スライディングクラス
    public class PlayerStateSliding : PlayerState {

        public override string GetStateName()
        {
            return "State:Sliding";
        }
    }


}

