using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] protected float _exp;
    [SerializeField] protected float _totalExp;
    [SerializeField] protected int _level;
    [SerializeField] protected int _statePoint;

    public float Exp { get { return _exp; } set { _exp = value; } }
    public float TotalExp { get { return _totalExp; } set { _totalExp = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    public int StatePoint { get { return _statePoint; } set { _statePoint = value; } }
}
