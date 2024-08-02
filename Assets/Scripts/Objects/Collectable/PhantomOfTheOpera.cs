using UnityEngine;

public class PhantomOfTheOpera : MonoBehaviour, IPoolObject
{
    [field: SerializeField] public MoveUnit MoveUnit { get; private set; }
    [SerializeField] private PoolObjectType objectType;

    public PoolObjectType ObjectType { get => objectType; }

    public bool actionDone;

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
        MovementController.instance.TeleportToPosition(MoveUnit, 2.1f);
    }

    public void ActionHandler()
    {
        PlayerData.instance.PhantomOfTheOperaON();
        actionDone = true;
        gameObject.SetActive(false);
        // додати відповідні жахаючі звуки
        // а ще поворот до того, кого лякає
    }
    public bool ActionDone()
        => actionDone;
}

