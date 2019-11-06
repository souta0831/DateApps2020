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
    private RectTransform[] rectTransform = new RectTransform[(int)LockOnState_.Num];

    // カーソルのImage
    [SerializeField]
    GameObject[] _lockon_image = new GameObject[(int)LockOnState_.Num];

    [SerializeField]
    GameObject Canvas;

    GameObject[] _image_obj = new GameObject[(int)LockOnState_.Num];
    Image[] _image=new Image[(int)LockOnState_.Num];

    // ロックオン対象のTransform
    Transform LockonTarget;

    void Start()
    {

        for (int i = 0; i < 2; i++)
        {
            //生成
            _image_obj[i] = Instantiate(_lockon_image[i]);
            _image_obj[i].transform.SetParent(Canvas.transform, false);
            //イメージ取得
            _image[i] = _image_obj[i].GetComponent<Image>();
            rectTransform[i] = _image[i].GetComponent<RectTransform>();

            _image[i].enabled = false;
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

    public void OnLockonRady(Transform target)
    {
        _image[(int)LockOnState_.Ready].enabled = true;
        LockonTarget = target;
    }
    public void OnLockonStart()
    {
        _image[(int)LockOnState_.Ready].enabled = false;
        _image[(int)LockOnState_.LockOn].enabled = true;
        Debug.Log("ロックオン");
    }
    public void OnLockonEnd()
    {
        for (int i = 0; i < 2; i++)
        {
            _image[i].enabled = false;
        }
        LockonTarget = null;
    }
  
    public enum LockOnState_
    {
        Ready,
        LockOn,
        Num,
    }

}
