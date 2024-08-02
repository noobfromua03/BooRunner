using System.Collections.Generic;
using UnityEngine;

public class MovementController
{
    public static MovementController instance;

    private List<MoveUnit> moveUnits;
    private float speedModify = 1;

    private Vector3[] points =
    {
        new Vector3(-2, 0, 0),
        new Vector3(0, 0, 0),
        new Vector3(2, 0, 0)
    };

    public void SetInstance()
    {
        instance = this;
    }

    public void Update()
    {
        foreach (var unit in moveUnits)
        {
            if (unit.Transform.gameObject.activeSelf)
            {
                MoveToPositionForwardTransformUpdate(unit);
                PatrolMoveTransformUpdate(unit);
            }
        }
    }

    public void FixedUpdate()
    {
        foreach (var unit in moveUnits)
        {
            if (unit.Transform.gameObject.activeSelf)
            {
                MoveToPositionRBFixedUpdate(unit);
            }
        }
    }

    public void AddMoveUnit(MoveUnit moveUnit)
    {
        if (moveUnits == null)
            moveUnits = new List<MoveUnit>();
        moveUnits.Add(moveUnit);
    }

    public void MoveToPositionRBFixedUpdate(MoveUnit unit)
    {
        switch (unit.MoveType)
        {
            case MoveType.Rigidbody:
                Vector3 movePosition = unit.Rb.position;

                movePosition.x = Mathf.MoveTowards(unit.Rb.position.x, points[unit.PointNumber].x, unit.SpeedX * Time.fixedDeltaTime);
                
                if (unit.jump)
                {
                    unit.Rb.AddForce(Vector3.up * unit.SpeedY, ForceMode.Impulse);
                    unit.jump = false;
                }    
                unit.Rb.MovePosition(movePosition);
                break;
        }
    }

    public void MoveToPositionTransformUpdate(MoveUnit unit)
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
    
    public void TeleportToPosition(MoveUnit unit, float posY)
    {
        Vector3 movePosition = unit.Transform.position;

        movePosition.x = points[unit.PointNumber].x;
        movePosition.y = posY;
        unit.Transform.position = movePosition;
    }

    public void MoveToPositionForwardTransformUpdate(MoveUnit unit)
    {
        
        switch (unit.MoveType)
        {
            case MoveType.Transform:
                unit.Transform.position = Vector3.MoveTowards(unit.Transform.position, unit.Transform.position - Vector3.forward,
                    unit.SpeedZ * Time.deltaTime * speedModify);
                break;
        }
    }

    public void PatrolMoveTransformUpdate(MoveUnit unit)
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

    public void ChangeSpeedModify(float value)
    {
        speedModify = Mathf.Clamp(value + 1, 1, 5);
    }

}