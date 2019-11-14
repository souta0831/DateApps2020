using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BalletBase : MonoBehaviour
{
    
    [SerializeField] protected float _speed=10;
    [SerializeField] protected float DestroyTime=300;

    protected GameObject _targetObject=null;
    protected float _accuracy;
    protected Rigidbody _rigidBody;


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

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
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
    private void FixedUpdate()
    {
        FixedMove();
    }
    protected virtual void Initialize() { }

    protected virtual void Move()
    {
    }
    protected virtual void FixedMove() { }
    void Delete()
    {
        DestroyTime-=Time.deltaTime;
        if (DestroyTime <= 0)
        {
            Destroy(this.gameObject);
        }


    }

}
