using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    float _nowLeep=0;
    public float _scrollSpeed { get; set; }
    public Vector3 _spawnPos { get; set; }
    public Vector3 _endPos { get; set; }
    public float _startLeep = 0;
    void Start()
    {
        _nowLeep= _startLeep;
    }

    // Update is called once per frame
    void Update()
    {
        _nowLeep += _scrollSpeed / 100;
        transform.position = Vector3.Lerp(_spawnPos, -_spawnPos, _nowLeep);
        if (_nowLeep >= 1.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
