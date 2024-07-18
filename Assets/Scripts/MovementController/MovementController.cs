using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public static MovementController instance;

    private List<MoveUnit> moveUnits;
    private float jumpY = 3f;

    private Vector3[] points =
    {
        new Vector3(-2, 0, 0),
        new Vector3(0, 0, 0),
        new Vector3(2, 0, 0)
    };

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        foreach (var unit in moveUnits)
        {
            if (unit.Transform.gameObject.activeSelf)
            {
                //MoveToPositionUpdate(unit);
                MoveToPositionForwardUpdate(unit);
                PatrolMoveUpdate(unit);
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (var unit in moveUnits)
        {
            if (unit.Transform.gameObject.activeSelf)
            {
                MoveToPositionFixedUpdate(unit);
            }
        }
    }

    public void AddMoveUnit(MoveUnit moveUnit)
    {
        if (moveUnits == null)
            moveUnits = new List<MoveUnit>();
        moveUnits.Add(moveUnit);
    }

    public void MoveToPositionFixedUpdate(MoveUnit unit)
    {
        switch (unit.MoveType)
        {
            case MoveType.Rigidbody:
                Vector3 movePosition = unit.Rb.position;

                movePosition.x = Mathf.MoveTowards(unit.Rb.position.x, points[unit.PointNumber].x, unit.SpeedX * Time.fixedDeltaTime);
                if (unit.Rb.position.y >= jumpY - 0.5f)
                    unit.jump = false;
                if (unit.jump)
                    movePosition.y = Mathf.MoveTowards(unit.Rb.position.y, jumpY, unit.SpeedY * Time.fixedDeltaTime);
                unit.Rb.MovePosition(movePosition);
                break;
        }
    }

    public void MoveToPositionUpdate(MoveUnit unit)
    {
        if (unit.isPatrol)
            return;

        switch (unit.MoveType)
        {
            case MoveType.Transform:
                Vector3 movePosition = unit.Transform.position;

                movePosition.x = Mathf.MoveTowards(unit.Transform.position.x, points[unit.PointNumber].x,
                    unit.SpeedX * Time.deltaTime);
                unit.Transform.position = movePosition;
                break;
        }
    }

    public void TeleportToPosition(MoveUnit unit)
    {
        Vector3 movePosition = unit.Transform.position;

        movePosition.x = points[unit.PointNumber].x;
        unit.Transform.position = movePosition;
    }

    public void MoveToPositionForwardUpdate(MoveUnit unit)
    {
        switch (unit.MoveType)
        {
            case MoveType.Transform:
                unit.Transform.position = Vector3.MoveTowards(unit.Transform.position, unit.Transform.position - Vector3.forward,
                    unit.SpeedZ * Time.deltaTime);
                break;
        }
    }

    public void PatrolMoveUpdate(MoveUnit unit)
    {
        if (!unit.isPatrol)
            return;

        if (unit.Transform.position.x == points[unit.currentPatrolLine].x)
        {
            int temp = unit.PointNumber;
            unit.PointNumber = unit.currentPatrolLine;
            unit.currentPatrolLine = temp;
        }
        Vector3 movePosition = unit.Transform.position;

        movePosition.x = Mathf.MoveTowards(unit.Transform.position.x, points[unit.currentPatrolLine].x,
            unit.SpeedX * Time.deltaTime);
        unit.Transform.position = movePosition;
    }
}