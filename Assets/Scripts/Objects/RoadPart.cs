using System;
using UnityEngine;

public class RoadPart : MonoBehaviour
{
    [field:SerializeField] public Transform FinishPoint { get; private set; }
    [field:SerializeField] public MoveUnit MoveUnit { get; private set; }

    public Action<RoadPart> onFinish;

    private void Start()
    {
        MovementController.instance.AddMoveUnit(MoveUnit);
    }
}

