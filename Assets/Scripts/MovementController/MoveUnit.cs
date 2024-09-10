using UnityEngine;

[System.Serializable]
public class MoveUnit
{
    public MoveType MoveType;
    public Rigidbody Rb;
    public Transform Transform;
    [Range(0, 2)] public int PointNumber;
    [Range(0, 1000)] public float SpeedX;
    [Range(0, 100)] public float SpeedY;
    [Range(0, 100)] public float SpeedZ;
    public bool jump;
    public bool isPatrol;
    public int currentPatrolLine;
}
    
public enum MoveType
{
    Transform = 0,
    Rigidbody = 1
}

