using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePointBase : MonoBehaviour
{
    private int _maxLifePoint = 100;
    private int _nowLifePoint = 100;

    public void PointSet(int _newPoint)
    {
        _nowLifePoint = Mathf.Min(_newPoint,_maxLifePoint);
    }

    public void MaxPointSet(int _newMaxPoint)
    {
        _maxLifePoint = _newMaxPoint;
    }

    public void AddPoint(int _addPoint)
    {
        _nowLifePoint += Mathf.Min(_nowLifePoint+_addPoint, _maxLifePoint);
    }

    public int GetNowPoint()
    {
        return _nowLifePoint;
    }
    public int GetMaxPoint()
    {
        return _maxLifePoint;
    }
}
