using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolObject
{
    [field: SerializeField] public MoveUnit MoveUnit { get; private set; }

    [SerializeField] private PoolObjectType objectType;
    public PoolObjectType ObjectType { get => objectType; }

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
    }

    public void TeleportToPosition()
    {
        MovementController.instance.TeleportToPosition(MoveUnit);
    }
}
