using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_LockOn : MonoBehaviour
{
    [SerializeField]
    private LockonCursor _lockonCursor = null;

    [SerializeField]
    private EnemyManager _enemyManager = null;

    [SerializeField]
    private float _lockOnRange =10.0f;

    private GameObject _lockOnGameObject = null;

    private bool _isLockOn = false;
    private int _lockOnNum = 0;

    // Update is called once per frame
    void Update()
    {
        if (_isLockOn)
        {
            if (InputController.GetButtonDown(Button.L1) ||
                _lockOnRange <=  _enemyManager.GetEnemyList()[_lockOnNum].GetPlayerDistance())
            {
                Debug.Log("ロックオン解除");
              _lockonCursor.OnLockonEnd();
                _isLockOn = false;
            }
            return;
        }

        if (_lockOnRange >=  _enemyManager.GetEnemyList()[_lockOnNum].GetPlayerDistance())
        {
          _lockonCursor.OnLockonRady( _enemyManager.GetEnemyList()[_lockOnNum].gameObject);
        }
        //ロックオン　
        if (InputController.GetButtonDown(Button.L1))
        {
          _lockonCursor.OnLockonStart();
            _isLockOn = true;
        }
        //ロック切り替え
        if (InputController.GetButtonDown(Button.RightStick))
        {
            int _lockOnIndex = _lockOnNum+1 < _enemyManager.GetEnemyList().Count ? _lockOnNum + 1 : 0;
            _lockOnNum = _lockOnRange >= _enemyManager.GetEnemyList()[_lockOnIndex].GetPlayerDistance() ? _lockOnIndex : _lockOnNum;
        }
    }
}
