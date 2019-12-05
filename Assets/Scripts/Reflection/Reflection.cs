using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection : MonoBehaviour
{
    public enum ReflectionMode
    {
        NomalMode,
        RandamMode
    }

    [SerializeField]
    private float m_reflectionPower = 1.5f;

    [SerializeField]
    private Transform m_reflectionTagetObject = default;

    [SerializeField]
    private GameObject m_HitInstanceObject = null;

    [SerializeField]
    private string m_changeTagName = "ReflectBullet";//◆A.タグ切り替わりませんQ.タグ名のスペルミスを直してみて下さい

    private ReflectionMode m_reflectionMode = ReflectionMode.NomalMode;

    private void OnTriggerEnter(Collider other)
    {
        var ColliiderGameObject = other.gameObject;
        ColliiderGameObject.tag = m_changeTagName;

        switch (m_reflectionMode)
        {
            case ReflectionMode.NomalMode:
                NomalReflection(ColliiderGameObject);
                break;

            case ReflectionMode.RandamMode:
                Debug.Log("乱反射");
                RandamReflection(ColliiderGameObject);
                break;
        }            
    }

    private void NomalReflection(GameObject other)
    {
        var rigitbody = other.GetComponent<Rigidbody>();

        other.transform.rotation = Quaternion.LookRotation(m_reflectionTagetObject.transform.position, Vector3.up);

        rigitbody.velocity = rigitbody.velocity.magnitude * other.transform.forward * m_reflectionPower;
        rigitbody.useGravity = false;

        var hitObject = Instantiate(m_HitInstanceObject);
        hitObject.transform.position = other.transform.position;
    }

    private void RandamReflection(GameObject other)
    {
        var rigitbody = other.GetComponent<Rigidbody>();

        other.transform.eulerAngles = 90.0f * Vector3.left;

        rigitbody.velocity = other.transform.forward * m_reflectionPower;//rigitbody.velocity.magnitude * 
        rigitbody.useGravity = true;

        var hitObject = Instantiate(m_HitInstanceObject);
        hitObject.transform.position = other.transform.position;
    }

    public void ModeChange(ReflectionMode mode)
    {
        m_reflectionMode = mode;
    }

}
