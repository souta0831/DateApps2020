using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    //ステージの構造体
    [System.Serializable]
    public struct _Stage
    {
        public GameObject _gameObject;
        public Stage _stageScript;
    };

    //生成座標
    [SerializeField]
    private Transform _spawnPos;
    //スタート時に存在してるステージ
    [SerializeField]
    private _Stage _startStage;
    //生成するステージ
    [SerializeField]
    private List<GameObject> _stageRandomList;
    //現在存在するステージ
    private _Stage[] _StageArray = new _Stage[3];
    private float _scrollSpeed;
    private float _bufferSpeed;
    public float ScrollSpeed
    {
        get { return _scrollSpeed; }
        set { _scrollSpeed = value; }
    }
    public Transform SpawnPos
    {
        get { return _spawnPos; }
    }
    void Start()
    {
        //ゲームオブジェクトからスクリプトを取得
        _startStage._stageScript = _startStage._gameObject.GetComponent<Stage>();
        //パラメータをセット
        _startStage._stageScript._startLeep = 0.5f;
        _StageArray[0] = _startStage;
    }
    void Update()
    {
        InstanceStage();
    }
    void InstanceStage()
    {
        for(int i=0;i<_StageArray.Length;i++)
        {
            if (_StageArray[i]._gameObject == null)
            {
                int random = Random.Range(0, _stageRandomList.Count);
                _StageArray[i]._gameObject = Instantiate(_stageRandomList[random], _spawnPos);
                Vector3 spawnPos = Vector3.Scale(_spawnPos.position, new Vector3(0, 0, i + 1));
                _StageArray[i]._stageScript = _StageArray[i]._gameObject.GetComponent<Stage>();
                _StageArray[i]._stageScript._scrollSpeed= _scrollSpeed;
                _StageArray[i]._stageScript._spawnPos = spawnPos;
                _StageArray[i]._stageScript._endPos = -_spawnPos.position;

            }
        }
    }
    void SpeedUpdate()
    {
        //スピードに変化があったらステージのスピードを更新する
        if (_bufferSpeed != _scrollSpeed)
        {
            for (int i = 0; i < _StageArray.Length; i++)
            {
                _StageArray[i]._stageScript._scrollSpeed = _scrollSpeed;
            }
        }
        _bufferSpeed = _scrollSpeed;
    }
}
