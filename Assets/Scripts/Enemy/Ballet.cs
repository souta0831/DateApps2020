using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballet : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] int DestroyTime;
    Vector3 _angle; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Delete();
    }
    void Move()
    {
        transform.position += _angle * Speed * Time.deltaTime;

    }
    void Delete()
    {
        DestroyTime--;
        if (DestroyTime <= 0)
        {
            Destroy(this);
        }


    }

    public void SetAngle(Vector3 angle) {
        _angle = angle;
    }
}
