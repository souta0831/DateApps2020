using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    Transform _spawnPos;
    public Transform SpawnPos
    {
        get { return _spawnPos; }
    }
    public void OnDead()
    {
        _spawnPos.DetachChildren();
        Destroy(this.gameObject);
    }
    // Update is called once per frame
}
