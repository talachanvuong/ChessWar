using System.Collections.Generic;
using UnityEngine;

public class King : Troop
{
    private Vector2[] directions = Vector2DAdvanced.MultiDirectional;

    public override void ShowWay()
    {
        foreach (Vector2 direction in KingRestrictDirections())
        {
            Vector2 way = GetWay(direction);
            GetSquareInfo(way, out Square square, out Transform target);

            if (HasTag(target, ConstantAdvanced.ALLY))
            {
                continue;
            }
            else if (HasTag(target, ConstantAdvanced.ENEMY))
            {
                GameController.Instance.table.SpawnEffect(EffectType.Red, square);
            }
            else
            {
                GameController.Instance.table.SpawnEffect(EffectType.Green, square);
            }
        }
    }

    public override bool KingRestrict(Square targetSquare)
    {
        foreach (Vector2 direction in directions)
        {
            Vector2 way = GetWay(direction);

            if (GameController.Instance.CheckBoundary(way))
            {
                continue;
            }

            Square square = GameController.Instance.table.GetSquare(way);

            if (square == targetSquare)
            {
                return true;
            }
        }
        return false;
    }

    private Vector2[] KingRestrictDirections()
    {
        List<Vector2> tempDirections = new();
        foreach (Vector2 direction in directions)
        {
            Vector2 way = GetWay(direction);

            if (GameController.Instance.CheckBoundary(way))
            {
                continue;
            }

            Square targetSquare = GameController.Instance.table.GetSquare(way);
            
            if (!GameController.Instance.ExecuteKingRestrict(targetSquare))
            {
                tempDirections.Add(direction);
            }
        }
        return tempDirections.ToArray();
    }

    public override bool Predict()
    {
        foreach (Vector2 direction in KingRestrictDirections())
        {
            Vector2 way = GetWay(direction);
            GetSquareInfo(way, out _, out Transform target);

            if (HasTag(target, ConstantAdvanced.ALLY))
            {
                continue;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    private Vector2 GetWay(Vector2 direction)
    {
        int xWay = (int)transform.position.x + (int)direction.x;
        int yWay = (int)transform.position.y + (int)direction.y;
        return new(xWay, yWay);
    }
}