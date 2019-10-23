using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ロックオンカーソルを制御するクラス
/// </summary>
public class LockonCursor : MonoBehaviour
{
    // カーソルのRectTransform
    private RectTransform[] rectTransform=new RectTransform[(int)LockOnState_.Num];

    // カーソルのImage
    public Image[] _lockon_image = new Image[(int)LockOnState_.Num];

    // ロックオン対象のTransform
    GameObject LockonTarget;

    void Start()
    {

        for (int i = 0; i < 2; i++)
        {
            rectTransform[i] = _lockon_image[i].GetComponent<RectTransform>();

            _lockon_image[i].enabled = false;
        }
    }

    void Update()
    {

        if (LockonTarget != null)
        {
            Vector3 targetPoint = Camera.main.WorldToScreenPoint(LockonTarget.transform.position);
            for (int i = 0; i < 2; i++)
            {
                rectTransform[i].Rotate(0, 0, 1f);
                rectTransform[i].position = targetPoint;
            }
        }

    }


    public void OnLockonRady(GameObject target)
    {
        _lockon_image[(int)LockOnState_.Ready].enabled = true;
        LockonTarget = target;
    }
    public void OnLockonStart()
    {
        _lockon_image[(int)LockOnState_.Ready].enabled = false;
        _lockon_image[(int)LockOnState_.LockOn].enabled = true;
        Debug.Log("ロックオン");
    }
    public void OnLockonEnd()
    {
        for (int i = 0; i < 2; i++)
        {
            _lockon_image[i].enabled = false;
        }
        LockonTarget = null;
    }
    public GameObject GetLockONTarget()
    {
        return LockonTarget;
    }
    public enum LockOnState_
    {
        Ready,
        LockOn,
        Num,
    }

}
