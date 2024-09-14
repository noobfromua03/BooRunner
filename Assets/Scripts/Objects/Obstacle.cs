using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolObject
{
    [field: SerializeField] public MoveUnit MoveUnit { get; private set; }
    [field: SerializeField] public bool Rotatable { get; private set; } = true;

    [SerializeField] private PoolObjectType objectType;
    public PoolObjectType ObjectType { get => objectType; }

    public bool actionDone;

    private List<int> rotateVariants = new() { 0, 180 };


    private void Start()
    {
        MovementController.instance.AddMoveUnit(MoveUnit);
    }

    public void ChangeLine(int line)
    {
        MoveUnit.PointNumber = line;
        TeleportToPosition();
    }

    private void OnEnable()
    {
        TeleportToPosition();
        actionDone = false;
    }

    public void TeleportToPosition()
    {
        MovementController.instance.TeleportToPosition(MoveUnit);
    }

    public void ActionHandler()
    {
        PlayerData.Instance.RemoveLife(1);
        actionDone = true;
    }
    public bool ActionDone()
        => actionDone;
    public bool ActiveStatus()
        => gameObject.activeSelf;

    public void Rotate()
    {
        if (!Rotatable)
            return;
        var rotation = transform.rotation;
        rotation.y += rotateVariants[Random.Range(0, rotateVariants.Count)];
        transform.rotation = rotation;
    }
}
