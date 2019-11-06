using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_LockOn : MonoBehaviour
{
    [SerializeField]
    private LockonCursor _lockonCursor = null;

    [SerializeField]
    private List<GameObject> _hitColliderList = new List<GameObject>();
    private GameObject _lockOnGameObject = null;

    private float _lockOnRange = 10.0f;
   
    // Start is called before the first frame update
    void Start()
    {
        _lockonCursor = GetComponent<LockonCursor>();
        _lockOnRange = GetComponent<SphereCollider>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        //ロックオン中
        if (_lockOnGameObject!=null)
        {
            //ロック切り替え
            if (InputController.GetButtonDown(Button.L1))
            {
                TagetChange();                
            }

            if (Vector3.Distance(this.transform.position, _lockOnGameObject.transform.position) > _lockOnRange)
            {
                LockOnExit();
            }
            return;
        }

        //ロックオン　
        if (InputController.GetButtonDown(Button.L1))
        {
            LockOnStart();
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        _hitColliderList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        _hitColliderList.Remove(other.gameObject);
    }

    public void LockOnStart()
    {
        if (_hitColliderList.Count <= 0)
        {
            return;
        }

        _lockOnGameObject = _hitColliderList[0];
        _lockonCursor.OnLockonRady(_hitColliderList[0].transform);
        _lockonCursor.OnLockonStart();
    }

    public void LockOnExit()
    {
        _lockonCursor.OnLockonEnd();
        _lockOnGameObject = null;
    }

    public void TagetChange()
    {
        if (_hitColliderList.Count==1)
        {
            LockOnExit();
            return;
        }

        var changeObject = _hitColliderList[0];
        _hitColliderList.RemoveAt(0);
        _hitColliderList.Add(changeObject);
        _lockonCursor.OnLockonRady(_hitColliderList[0].transform);
    }

    public GameObject NowLockOnGameObject()
    {
        return _lockOnGameObject;
    }
}
