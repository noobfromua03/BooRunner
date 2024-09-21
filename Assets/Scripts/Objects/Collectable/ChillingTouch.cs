using UnityEngine;

public class ChillingTouch : MonoBehaviour, IPoolObject
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
        MovementController.instance.TeleportToPosition(MoveUnit, 2f);
    }

    public void ActionHandler()
    {
        PlayerData.Instance.ChillingTouchON();
        actionDone = true;
        gameObject.SetActive(false);
        AudioManager.Instance.PlayAudioByType(SoundType.CatchBooster);
    }
    public bool ActionDone()
        => actionDone;

    public bool ActiveStatus()
        => gameObject.activeSelf;
}

