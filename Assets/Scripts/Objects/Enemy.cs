using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject
{
    [SerializeField] private PoolObjectType objectType;
    [SerializeField] private int boldLevel;
    [field: SerializeField] public MoveUnit MoveUnit { get; private set; }
    public PoolObjectType ObjectType { get => objectType; }
    public int BoldLevel { get => boldLevel; }

    public bool actionDone;

    private bool CanUseEssence => CompareBold(PlayerData.Instance.FearEssence) || PlayerData.Instance.IsTownLegend.Status;
    private void Start()
    {
        MovementController.instance.AddMoveUnit(MoveUnit);
        MoveUnit.currentPatrolLine = MoveUnit.PointNumber;
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

    public void GetPatrolLine(List<bool> lines, int line)
    {
        if (lines[1] == true && line != 1)
        {
            MoveUnit.currentPatrolLine = MoveUnit.PointNumber;
            return;
        }

        if (line == 0 && !lines[1] && !lines[2])
            MoveUnit.currentPatrolLine = Random.Range(1, 3);

        else if (line == 0 && lines[2])
            MoveUnit.currentPatrolLine = 1;

        else if (line == 1 && !lines[0] && !lines[2])
            MoveUnit.currentPatrolLine = Random.value <= 0.5f ? 0 : 2;

        else if (line == 1 && !lines[0] && lines[2])
            MoveUnit.currentPatrolLine = 0;

        else if (line == 1 && lines[0] && !lines[2])
            MoveUnit.currentPatrolLine = 2;

        else if (line == 1 && lines[0] && lines[2])
            MoveUnit.currentPatrolLine = 1;

        else if (line == 2 && !lines[0] && !lines[1])
            MoveUnit.currentPatrolLine = Random.Range(0, 2);

        else if (line == 2 && lines[0])
            MoveUnit.currentPatrolLine = 1;
    }

    public bool CompareBold(int essence)
    {
        return essence >= boldLevel;
    }

    public void ActionHandler()
    {
        if (CanUseEssence)
        {
            PlayerData.Instance.RemoveEssence(BoldLevel);
            ScaredJump();
        }
        else
            PlayerData.Instance.RemoveLife(1);
        actionDone = true;
    }

    public void PhantomOfTheOperaHandler()
    {
        if (CanUseEssence)
        {
            PlayerData.Instance.RemoveEssence(BoldLevel);
            actionDone = true;
        }
    }

    public void ScaredJump()
        => MoveUnit.jump = true;

    public bool ActionDone()
        => actionDone;
    public bool ActiveStatus()
        => gameObject.activeSelf;
}
