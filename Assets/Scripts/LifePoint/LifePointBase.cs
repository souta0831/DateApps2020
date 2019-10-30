using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System; // .NET 4.xモードで動かす場合は必須

public class LifePointBase : MonoBehaviour
{
    [SerializeField]
    private IntReactiveProperty _nowLifePoint = new IntReactiveProperty(0);
    private int _maxLifePoint = 100;

    public IObservable<int> OnPointChanged
    {
        get { return _nowLifePoint; }
    }

    public void PointSet(int _newPoint)
    {
        _nowLifePoint.Value = Mathf.Min(_newPoint,_maxLifePoint);
    }

    public void MaxPointSet(int _newMaxPoint)
    {
        _maxLifePoint = _newMaxPoint;
    }

    public void AddPoint(int _addPoint)
    {
        _nowLifePoint.Value = Mathf.Min(_nowLifePoint.Value + _addPoint, _maxLifePoint);
    }

    public int GetNowPoint()
    {
        return _nowLifePoint.Value;
    }
    public int GetMaxPoint()
    {
        return _maxLifePoint;
    }
}
