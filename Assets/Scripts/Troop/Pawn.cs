using System;
using UnityEngine;

public class Pawn : Troop
{
    private bool isFirstMove = true;

    public override void Action(Vector2 position)
    {
        base.Action(position);
        isFirstMove = false;
    }

    public override void ShowWay()
    {
        if (GameController.Instance.IsCheckmate)
        {
            ExecuteRescuerRestrict();
        }
        else
        {
            if (IsRestricted && IsProtectedKing())
            {
                return;
            }

            Action<Square> reachUpNothing = delegate (Square square)
            {
                GameController.Instance.table.SpawnEffect(EffectType.Green, square);
            };

            Action<Square> reachSideEnemy = delegate (Square square)
            {
                GameController.Instance.table.SpawnEffect(EffectType.Red, square);
            };

            ExecutePlayerPath(null, null, reachUpNothing, null, reachSideEnemy);
        }
    }

    public override void Checkmate()
    {
        Vector2 botKingPosition = GameController.Instance.GetBotKingPosition();
        Square targetSquare = GameController.Instance.table.GetSquare(botKingPosition);

        Action<Square> reachSideSquare = delegate (Square square)
        {
            ShowCheckmate(square);
        };

        ExecutePlayerPath(targetSquare, null, null, reachSideSquare, null);
    }

    public override void BotCheckmate()
    {
        Vector2 playerKingPosition = GameController.Instance.GetPlayerKingPosition();
        Square targetSquare = GameController.Instance.table.GetSquare(playerKingPosition);

        Action<Square> reachSideSquare = delegate (Square square)
        {
            ShowCheckmate(square);
            GameController.Instance.checkmateTroops.Add(transform);
            GameController.Instance.IsCheckmate = true;
        };

        ExecuteBotPath(targetSquare, reachSideSquare, null);
    }

    public override bool KingRestrict(Square targetSquare)
    {
        return ExecuteKingRestrict(targetSquare);
    }

    public override void RescuerRestrict()
    {
        GameController.Instance.rescuePositions.Add(transform.position);
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

    private void ExecutePlayerPath(Square targetSquare, Action<Square> reachUpSquare, Action<Square> reachUpNothing, Action<Square> reachSideSquare, Action<Square> reachSideEnemy)
    {
        int maxRange = isFirstMove ? 2 : 1;
        Vector2[] directions = { Vector2Advanced.UpperLeft, Vector2Advanced.UpperRight };

        for (int i = 0; i < maxRange; i++)
        {
            Vector2 way = GetWay(Vector2Advanced.Up, i);

            if (GameController.Instance.CheckBoundary(way))
            {
                continue;
            }

            GetSquareInfo(way, out Square square, out Transform target);

            if (square == targetSquare)
            {
                if (reachUpSquare != null)
                {
                    reachUpSquare.Invoke(square);
                    return;
                }
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
                reachUpNothing?.Invoke(square);
            }
        }

        foreach (Vector2 direction in directions)
        {
            Vector2 way = GetWay(direction, 0);

            if (GameController.Instance.CheckBoundary(way))
            {
                continue;
            }

            GetSquareInfo(way, out Square square, out Transform target);

            if (square == targetSquare)
            {
                if (reachSideSquare != null)
                {
                    reachSideSquare.Invoke(square);
                    return;
                }
            }
            else if (HasTag(target, ConstantAdvanced.ENEMY))
            {
                reachSideEnemy?.Invoke(square);
            }
        }
    }

    private void ExecuteBotPath(Square targetSquare, Action<Square> reachSideSquare, Action<Square> reachSideEnemy)
    {
        Vector2[] directions = { Vector2Advanced.LowerLeft, Vector2Advanced.LowerRight };

        foreach (Vector2 direction in directions)
        {
            Vector2 way = GetWay(direction, 0);

            if (GameController.Instance.CheckBoundary(way))
            {
                continue;
            }

            GetSquareInfo(way, out Square square, out Transform target);

            if (square == targetSquare)
            {
                reachSideSquare?.Invoke(square);
                return;
            }
            else if (HasTag(target, ConstantAdvanced.ENEMY))
            {
                reachSideEnemy?.Invoke(square);
            }
        }
    }

    private bool ExecutePredictBoolean(Square targetSquare, Func<Square, bool> reachUpSquare, Func<Square, bool> reachSideSquare)
    {
        int maxRange = isFirstMove ? 2 : 1;
        Vector2[] directions = { Vector2Advanced.UpperLeft, Vector2Advanced.UpperRight };

        for (int i = 0; i < maxRange; i++)
        {
            Vector2 way = GetWay(Vector2Advanced.Up, i);

            if (GameController.Instance.CheckBoundary(way))
            {
                continue;
            }

            GetSquareInfo(way, out Square square, out Transform target);

            if (square == targetSquare)
            {
                return reachUpSquare.Invoke(square);
            }
            else if (HasTag(target, ConstantAdvanced.ALLY))
            {
                break;
            }
            else if (HasTag(target, ConstantAdvanced.ENEMY))
            {
                break;
            }
        }

        foreach (Vector2 direction in directions)
        {
            Vector2 way = GetWay(direction, 0);

            if (GameController.Instance.CheckBoundary(way))
            {
                continue;
            }

            GetSquareInfo(way, out Square square, out Transform target);

            if (square == targetSquare)
            {
                return reachSideSquare.Invoke(square);
            }
        }
        return false;
    }

    private bool ExecuteKingRestrict(Square targetSquare)
    {
        Vector2[] directions = { Vector2Advanced.LowerLeft, Vector2Advanced.LowerRight };

        foreach (Vector2 direction in directions)
        {
            Vector2 way = GetWay(direction, 0);

            if (GameController.Instance.CheckBoundary(way))
            {
                continue;
            }

            GetSquareInfo(way, out Square square, out Transform target);

            if (square == targetSquare)
            {
                return true;
            }
        }
        return false;
    }

    private void ExecuteRescuerRestrict()
    {
        foreach (Vector2 rescuePosition in GameController.Instance.rescuePositions)
        {
            Square targetSquare = GameController.Instance.table.GetSquare(rescuePosition);

            Action<Square> reachUpSquare = delegate (Square square)
            {
                if (square.troop == null)
                {
                    GameController.Instance.table.SpawnEffect(EffectType.Green, square);
                }
            };

            Action<Square> reachSideSquare = delegate (Square square)
            {
                if (HasTag(square.troop, ConstantAdvanced.ENEMY))
                {
                    GameController.Instance.table.SpawnEffect(EffectType.Red, square);
                }
            };

            ExecutePlayerPath(targetSquare, reachUpSquare, null, reachSideSquare, null);
        }
    }

    private bool ExecutePredict(Square targetSquare)
    {
        Func<Square, bool> reachUpSquare = delegate (Square square)
        {
            return square.troop == null;
        };

        Func<Square, bool> reachSideSquare = delegate (Square square)
        {
            return HasTag(square.troop, ConstantAdvanced.ENEMY);
        };

        return ExecutePredictBoolean(targetSquare, reachUpSquare, reachSideSquare);
    }

    private Vector2 GetWay(Vector2 direction, int i)
    {
        int xWay = (int)transform.position.x + (i + 1) * (int)direction.x;
        int yWay = (int)transform.position.y + (i + 1) * (int)direction.y;
        return new(xWay, yWay);
    }

    private bool IsProtectedKing()
    {
        GetPositionAmongPlayerKing(transform.position, out Vector2 resultVector);
        return resultVector.x != 0;
    }
}