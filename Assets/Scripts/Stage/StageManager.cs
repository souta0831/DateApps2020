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
        [System.NonSerialized]
        public Stage _stageScript;
    };
    //同時に生成するステージの数
    [SerializeField]
    private float _existingStageNum=3;
    //スタート時に存在してるステージ
    [SerializeField]
    private _Stage _startStage;
    //生成するステージ
    [SerializeField]
    private List<_Stage> _stageRandomList=new List<_Stage>();
    //現在存在するステージ
    private List<_Stage> _StageArray = new List<_Stage>();
    private float _scrollSpeed;
    private float _bufferSpeed;
    public float ScrollSpeed
    {
        get { return _scrollSpeed; }
        set { _scrollSpeed = value; }
    }
    void Start()
    {
        //ゲームオブジェクトからスクリプトを取得
        _startStage._stageScript = _startStage._gameObject.GetComponent<Stage>();
        _StageArray.Add(_startStage);

        for (int i = 0; i < _stageRandomList.Count; i++)
        {
            _Stage stage = _stageRandomList[i];
            stage._stageScript = stage._gameObject.GetComponent<Stage>();
            _stageRandomList[i] = stage;
        }
    }
    void Update()
    {
        InstanceStage();
    }
    void InstanceStage()
    {
        if (_StageArray.Count<= _existingStageNum)
        {
            _Stage stage=_stageRandomList[Random.Range(0,_stageRandomList.Count)];
            stage._gameObject = Instantiate(stage._gameObject,_StageArray[_StageArray.Count-1]._stageScript.SpawnPos);
            _StageArray.Add(stage);
        }
    }
    void SpeedUpdate()
    {
        _StageArray[1]._gameObject.transform.position-=new Vector3(0,0,ScrollSpeed);
    }
}
