using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ロックオンカーソルを制御するクラス
/// </summary>
public class LockonCursor : MonoBehaviour
{
    [SerializeField]
    GameObject _lockon_image = default;

    [SerializeField]
    GameObject _canvas = default;

    private RectTransform _rectTransform = new RectTransform();

    private GameObject _image_obj = default;
    private Image _image = default;

    private Transform _lockonTarget = default;

    void Start()
    {    
            //生成
            _image_obj = Instantiate(_lockon_image);
            _image_obj.transform.SetParent(_canvas.transform, false);
            //イメージ取得
            _image = _image_obj.GetComponent<Image>();
            _rectTransform = _image.GetComponent<RectTransform>();
            _image.enabled = false;
    }

    void Update()
    {
        if (_lockonTarget == null)
        {
            return;
        }
            Vector3 targetPoint = Camera.main.WorldToScreenPoint(_lockonTarget.transform.position);
            _rectTransform.Rotate(0, 0, 1f);
            _rectTransform.position = targetPoint;
    }

    public void OnLockonStart(Transform target)
    {
        _image.enabled = true;
        _lockonTarget = target;
        Debug.Log("ロックオン");
    }
    public void OnLockonEnd()
    {
       _image.enabled = false;
        _lockonTarget = null;
    }
}
