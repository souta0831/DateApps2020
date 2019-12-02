using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection : MonoBehaviour
{
    [SerializeField]
    private float m_reflectionPower = 1.5f;

    [SerializeField]
    private Transform m_reflectionVectorObject = default;
    [SerializeField]
    private string m_changeTagName = "ReflectBullet";//◆A.タグ切り替わりませんQ.タグ名のスペルミスを直してみて下さい
 
    private void OnTriggerEnter(Collider other)
    {
        var rigitbody = other.gameObject.GetComponent<Rigidbody>();
        //rigitbody.velocity = -rigitbody.velocity * m_reflectionPower;
        rigitbody.velocity = rigitbody.velocity.magnitude * m_reflectionVectorObject.forward * m_reflectionPower;

        var ColliiderGameObject = other.gameObject;
        other.gameObject.tag = m_changeTagName;
;        ColliiderGameObject.transform.eulerAngles = m_reflectionVectorObject.eulerAngles;
        //ColliiderGameObject.transform.eulerAngles = new Vector3(ColliiderGameObject.transform.eulerAngles.x, 180.0f + ColliiderGameObject.transform.eulerAngles.y, ColliiderGameObject.transform.eulerAngles.z);
    }
}
