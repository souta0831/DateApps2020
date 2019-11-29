using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection : MonoBehaviour
{
    [SerializeField]
    private float m_reflectionPower = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        var rigitbody = other.gameObject.GetComponent<Rigidbody>();
        rigitbody.velocity = -rigitbody.velocity * m_reflectionPower;

        var ColliiderGameObject = other.gameObject;
        ColliiderGameObject.transform.eulerAngles = new Vector3(ColliiderGameObject.transform.eulerAngles.x, 180.0f+ColliiderGameObject.transform.eulerAngles.y, ColliiderGameObject.transform.eulerAngles.z);


    }
}
