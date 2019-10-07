using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballet : MonoBehaviour
{
    [SerializeField] float Speed;
    Vector3 _angle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        transform.position += _angle * Speed * Time.deltaTime;

    }
    public void SetAngle(Vector3 angle) {
        _angle = angle;
    }
}
