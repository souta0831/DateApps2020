using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_LockOn : MonoBehaviour
{
    [SerializeField]
    private LockonCursor _lockonCursor = default;

    [SerializeField]
    private List<GameObject> _hitColliderList = new List<GameObject>();

    [SerializeField]
    private GameObject _lockOnGameObject = null;
   
    // Start is called before the first frame update
    void Start()
    {
        _lockonCursor = GetComponent<LockonCursor>();
    }

    // Update is called once per frame
    void Update()
    {
        NullCheck();

        if (_lockOnGameObject!=null)
        {
            //ロック切り替え
            if (InputController.GetButtonDown(Button.L1))
            {
                TagetChange();                
            }
            return;
        }

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
        if (_lockOnGameObject == other.gameObject)
        {
            LockOnExit();
        }
        _hitColliderList.Remove(other.gameObject);
    }

    private void NullCheck()
    {
        foreach (var num in _hitColliderList)
        {
            if (num == null)
            {
                if (_lockOnGameObject == num.gameObject)
                {
                    LockOnExit();
                }
                _hitColliderList.Remove(num.gameObject);
            }
        }
    }

    public void LockOnStart()
    {
        //近くに対象がいない場合ロックオン開始をしない
        if (_hitColliderList.Count <= 0)
        {
            return;
        }

        _lockOnGameObject = _hitColliderList[0];
        _lockonCursor.OnLockonStart(_lockOnGameObject.transform);
    }

    public void LockOnExit()
    {
        _lockonCursor.OnLockonEnd();
        _lockOnGameObject = null;
    }

    public void TagetChange()
    {
        //他の対象がいない場合ロックオン解除
        if (_hitColliderList.Count==1)
        {
            LockOnExit();
            return;
        }

        var changeObject = _hitColliderList[0];
        _hitColliderList.RemoveAt(0);
        _hitColliderList.Add(changeObject);
        _lockOnGameObject = _hitColliderList[0];
        _lockonCursor.OnLockonStart(_hitColliderList[0].transform);
    }

    public GameObject NowLockOnGameObject()
    {
        return _lockOnGameObject;
    }
}
