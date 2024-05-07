using System.Collections.Generic;
using UnityEngine;
using System;

public class Queen : Troop
{
    private Vector2[] directions = Vector2DAdvanced.MultiDirectional;

    public override void ShowWay()
    {
        if (GameController.Instance.IsCheckmate)
        {
            ExecuteRescuerRestrict();
        }
        else
        {
            Action<Square> reachEnemy = delegate (Square square)
            {
                GameController.Instance.table.SpawnEffect(EffectType.Red, square);
            };

            Action<Square> reachNothing = delegate (Square square)
            {
                GameController.Instance.table.SpawnEffect(EffectType.Green, square);
            };

            directions = IsRestricted ? GuardianRestrictDirections() : directions;
            ExecutePath(null, null, null, reachEnemy, reachNothing);
        }
    }

    public override void Checkmate()
    {
        Vector2 botKingPosition = GameController.Instance.GetBotKingPosition();
        Square targetSquare = GameController.Instance.table.GetSquare(botKingPosition);

        Action<Square> reachSquare = delegate (Square square)
        {
            ShowCheckmate(square);
        };

        ExecutePath(targetSquare, reachSquare, null, null, null);
    }

    public override void BotCheckmate()
    {
        Vector2 playerKingPosition = GameController.Instance.GetPlayerKingPosition();
        Square targetSquare = GameController.Instance.table.GetSquare(playerKingPosition);

        Action<Square> reachSquare = delegate (Square square)
        {
            ShowCheckmate(square);
            GameController.Instance.checkmateTroops.Add(transform);
            GameController.Instance.IsCheckmate = true;
        };

        ExecutePath(targetSquare, reachSquare, null, null, null);
    }

    public override void GuardianRestrict()
    {
        List<Transform> caughtTroops = new();
        GetDirectionAmongKing(out Vector2 direction, out int maxDistance);

        for (int i = 0; i < maxDistance; i++)
        {
            Vector2 way = GetWay(direction, i);
            GetSquareInfo(way, out _, out Transform target);

            if (target == null)
            {
                continue;
            }
            else
            {
                caughtTroops.Add(target);
            }
        }

        if (caughtTroops.Count == 1 && HasTag(caughtTroops[0], ConstantAdvanced.ALLY))
        {
            caughtTroops[0].GetComponent<Troop>().IsRestricted = true;
        }
    }

    private Vector2[] GuardianRestrictDirections()
    {
        GetPositionAmongPlayerKing(transform.position, out Vector2 resultVector);

        if (resultVector.x == 0)
        {
            return Vector2DAdvanced.Vertical;
        }
        else if (resultVector.y == 0)
        {
            return Vector2DAdvanced.Horizontal;
        }
        else if (resultVector.x == resultVector.y)
        {
            return Vector2DAdvanced.RightOblique;
        }
        else if (resultVector.x == -resultVector.y || resultVector.y == -resultVector.x)
        {
            return Vector2DAdvanced.LeftOblique;
        }
        return new Vector2[] { };
    }

    public override bool KingRestrict(Square targetSquare)
    {
        return ExecuteKingRestrict(targetSquare);
    }

    public override void RescuerRestrict()
    {
        GetDirectionAmongKing(out Vector2 direction, out int maxDistance);

        for (int i = -1; i < maxDistance; i++)
        {
            Vector2 way = GetWay(direction, i);
            GameController.Instance.rescuePositions.Add(way);
        }
    }

    public override bool Predict()
    {
        foreach (Vector2 rescuePosition in GameController.Instance.rescuePositions)
        {
            Square targetSquare = GameController.Instance.table.GetSquare(rescuePosition);
            if (ExecutePredict(targetSquare))
            {
                return true;
            }
        }
        return false;
    }

    private void ExecutePath(Square targetSquare, Action<Square> reachSquare, Action<Square> reachAlly, Action<Square> reachEnemy, Action<Square> reachNothing)
    {
        foreach (Vector2 direction in directions)
        {
            for (int i = 0; i < ConstantAdvanced.MAX_DISTANCE; i++)
            {
                Vector2 way = GetWay(direction, i);

                if (GameController.Instance.CheckBoundary(way))
                {
                    break;
                }

                GetSquareInfo(way, out Square square, out Transform target);

                if (square == targetSquare)
                {
                    reachSquare?.Invoke(square);
                    return;
                }
                else if (HasTag(target, ConstantAdvanced.ALLY))
                {
                    reachAlly?.Invoke(square);
                    break;
                }
                else if (HasTag(target, ConstantAdvanced.ENEMY))
                {
                    reachEnemy?.Invoke(square);
                    break;
                }
                else
                {
                    reachNothing?.Invoke(square);
                    continue;
                }
            }
        }
    }

    private void ExecuteRescuerRestrict()
    {
        foreach (Vector2 rescuePosition in GameController.Instance.rescuePositions)
        {
            Square targetSquare = GameController.Instance.table.GetSquare(rescuePosition);
            Action<Square> reachSquare = delegate (Square square)
            {
                if (HasTag(square.troop, ConstantAdvanced.ENEMY))
                {
                    GameController.Instance.table.SpawnEffect(EffectType.Red, square);
                }
                else
                {
                    GameController.Instance.table.SpawnEffect(EffectType.Green, square);
                }
            };

            ExecutePath(targetSquare, reachSquare, null, null, null);
        }
    }

    private bool ExecuteKingRestrict(Square targetSquare)
    {
        foreach (Vector2 direction in directions)
        {
            for (int i = 0; i < ConstantAdvanced.MAX_DISTANCE; i++)
            {
                Vector2 way = GetWay(direction, i);

                if (GameController.Instance.CheckBoundary(way))
                {
                    break;
                }

                GetSquareInfo(way, out Square square, out Transform target);

                if (IsTroop<King>(target, ConstantAdvanced.ALLY))
                {
                    continue;
                }
                else if (square == targetSquare)
                {
                    return true;
                }
                else if (HasTag(target, ConstantAdvanced.ALLY))
                {
                    break;
                }
                else if (HasTag(target, ConstantAdvanced.ENEMY))
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
        return false;
    }

    private bool ExecutePredict(Square targetSquare)
    {
        foreach (Vector2 direction in directions)
        {
            for (int i = 0; i < ConstantAdvanced.MAX_DISTANCE; i++)
            {
                Vector2 way = GetWay(direction, i);

                if (GameController.Instance.CheckBoundary(way))
                {
                    break;
                }

                GetSquareInfo(way, out Square square, out Transform target);

                if (square == targetSquare)
                {
                    return true;
                }
                else if (HasTag(target, ConstantAdvanced.ALLY))
                {
                    break;
                }
                else if (HasTag(target, ConstantAdvanced.ENEMY))
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
        return false;
    }

    private Vector2 GetWay(Vector2 direction, int i)
    {
        int xWay = (int)transform.position.x + (i + 1) * (int)direction.x;
        int yWay = (int)transform.position.y + (i + 1) * (int)direction.y;
        return new(xWay, yWay);
    }

    private void GetDirectionAmongKing(out Vector2 direction, out int maxDistance)
    {
        GetPositionAmongPlayerKing(transform.position, out Vector2 resultVector);

        if (resultVector.x == 0 && resultVector.y > 0)
        {
            direction = Vector2Advanced.Down;
            maxDistance = (int)resultVector.y - 1;
        }
        else if (resultVector.x == 0 && resultVector.y < 0)
        {
            direction = Vector2Advanced.Up;
            maxDistance = (int)-resultVector.y - 1;
        }
        else if (resultVector.y == 0 && resultVector.x > 0)
        {
            direction = Vector2Advanced.Left;
            maxDistance = (int)resultVector.x - 1;
        }
        else if (resultVector.y == 0 && resultVector.x < 0)
        {
            direction = Vector2Advanced.Right;
            maxDistance = (int)-resultVector.x - 1;
        }
        else if (resultVector.x == resultVector.y && resultVector.x > 0 && resultVector.y > 0)
        {
            direction = Vector2Advanced.LowerLeft;
            // Do resultVector.x = resultVector.y nên lấy resultVector.x làm đại diện
            maxDistance = (int)resultVector.x - 1;
        }
        else if (resultVector.x == resultVector.y && resultVector.x < 0 && resultVector.y < 0)
        {
            direction = Vector2Advanced.UpperRight;
            // Do resultVector.x = resultVector.y nên lấy resultVector.x làm đại diện
            maxDistance = (int)-resultVector.x - 1;
        }
        else if (resultVector.x == -resultVector.y)
        {
            direction = Vector2Advanced.LowerRight;
            maxDistance = (int)resultVector.y - 1;
        }
        else if (resultVector.y == -resultVector.x)
        {
            direction = Vector2Advanced.UpperLeft;
            maxDistance = (int)resultVector.x - 1;
        }
        else
        {
            direction = new(0, 0);
            maxDistance = 0;
        }
    }
}