using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BalletBase : MonoBehaviour
{
    
    [SerializeField] protected float Speed;
    [SerializeField] protected int DestroyTime;
    protected GameObject _targetObject=null;
    protected float _accuracy;

    private bool _isJust=true;
    //パラメーター
    public GameObject TargetObject
    {
        set { _targetObject = value; }
    }
    public float Accuracy
    {
        set { _accuracy = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isJust)
        {
            Initialize();
            _isJust = false;
        }
        Move();
        Delete();
    }
    protected virtual void Initialize() { }

    protected virtual void Move()
    {
    }
    void Delete()
    {
        DestroyTime--;
        if (DestroyTime <= 0)
        {
            Destroy(this.gameObject);
        }


    }

}
