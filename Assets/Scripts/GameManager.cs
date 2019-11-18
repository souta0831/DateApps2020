using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//進行管理クラス
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Camera _MainCamera;
    static private Camera _mainCamera= default;
    static private Camera _nowCamera=default;
    static private int _now_disply = 0;
    public Camera MainCamera
    {
        get { return _mainCamera; }
    }
    void Awake()
    {
        InputController.SetDefault();
        //子供からカメラを取得する
        _mainCamera = _MainCamera;
        //初期カメラをメインカメラに設定   
        GameManager.ChangeCamera(_mainCamera);

    }

    // Update is called once per frame
    void Update()
    {
        InputController.Update();
        CameraCheck();
    }
    void CameraCheck()
    {
        if (_nowCamera == null)
        {
            ChangeCamera(null);
        }
    }
    public static void ChangeCamera(Camera camera)
    {
        if (camera == null)
        {
            if(_nowCamera!=null) _nowCamera.enabled = false;
            _nowCamera = _mainCamera.GetComponent<Camera>();//_mainCamera;
            _nowCamera.enabled = true;

            return;
        }
        if (camera!=_nowCamera)
        {
            if(_nowCamera!=null) _nowCamera.enabled = false;
            //カメラを取得する
            _nowCamera = camera.GetComponent<Camera>();
            _nowCamera.enabled = true;

            return;
        }


    }
    
}
