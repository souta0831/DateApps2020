using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBreakEfect : MonoBehaviour
{
   public enum PowerVector { 
        Up,
        Down,
    };
    Rigidbody _rigidbody;
    [SerializeField]
    private PowerVector _powerVector;
    [SerializeField]
    private float ForcePower=5.0f;
    [SerializeField]
    private float _randomPower=10.0f;
    [SerializeField]
    private float _deadTime = 3;
    private bool _isHit=false;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Vector3 force = Vector3.zero;
        switch (_powerVector)
        { 
            case PowerVector.Up:
                force += transform.up*Random.Range(-_randomPower, _randomPower) * ForcePower;
                force += -transform.right* Random.Range(-_randomPower, _randomPower) * ForcePower;
                break;
            case PowerVector.Down:
                force += transform.right * Random.Range(-_randomPower, _randomPower) * ForcePower;

                break;

        }
        force += -transform.forward * Random.Range(-0.1f, _randomPower) * ForcePower;
        _rigidbody.AddForce(force*Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isHit) _deadTime -= Time.deltaTime;
        if (_deadTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        _isHit = true;
    }
}
