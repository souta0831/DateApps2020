using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private float _jumpPower = 6.0f;
    [SerializeField]
    private Camera _camera;

    private Rigidbody _rigidbody;
    private Animator _animator;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        Vector3 camForward = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = (camForward * z) + (_camera.transform.right * x);

        if (moveForward.magnitude > 0.01f) 
        {
            transform.position += moveForward * _speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(moveForward); 
        }

        if (Input.GetButtonDown("Jump"))
        {
            _rigidbody.AddForce(0.0f, _jumpPower, 0.0f, ForceMode.Impulse);
        }
    }
}
