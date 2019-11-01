using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステート管理クラス
public class StateProcessor
{
    private StateBase _State;
    private StateBase _bufferState;
    public StateBase State
    {
        set { _State = value; }
        get { return _State; }
    }
    public StateBase BufferState
    {
        set { _bufferState = value; }
        get { return _bufferState; }
    }
    public void Execute()
    {
        if (BufferState != null)
        {
            if (State != BufferState)
            {
                BufferState.EndExecute();
            }
        }

        State.Execute();
        BufferState = State;

    }
    public StateBase GetState() { return _State; }

}
//ステートクラス
public abstract class StateBase
{

    public delegate void executeState();
    public executeState execDelegate;
    public executeState execEndDelegate;
    public virtual void Execute()
    {
        if (execDelegate != null) execDelegate();
    }
    public virtual void EndExecute()
    {
        if (execEndDelegate != null) execEndDelegate();

    }
}
