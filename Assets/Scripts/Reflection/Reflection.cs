using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection : MonoBehaviour
{
    [SerializeField]
    private float m_reflectionPower = 1.5f;

    [SerializeField]
    private Transform m_reflectionTagetObject = default;

    [SerializeField]
    private GameObject m_HitInstanceObject = null;

    [SerializeField]
    private string m_changeTagName = "ReflectBullet";//◆A.タグ切り替わりませんQ.タグ名のスペルミスを直してみて下さい
 
    private void OnTriggerEnter(Collider other)
    {
        var rigitbody = other.gameObject.GetComponent<Rigidbody>();
     
        var ColliiderGameObject = other.gameObject;
        ColliiderGameObject.tag = m_changeTagName;
        ColliiderGameObject.transform.rotation = Quaternion.LookRotation(m_reflectionTagetObject.transform.position, Vector3.up);

        rigitbody.velocity = rigitbody.velocity.magnitude * ColliiderGameObject.transform.forward * m_reflectionPower;
        var hitObject = Instantiate(m_HitInstanceObject);
        hitObject.transform.position = ColliiderGameObject.transform.position;
    }
}
