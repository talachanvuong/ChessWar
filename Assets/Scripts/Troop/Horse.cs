using UnityEngine;
using System;

public class Horse : Troop
{
    private Vector2 GetWay(int i, int j)
    {
        int xWay = (int)transform.position.x + i;
        int yWay = (int)transform.position.y + j;
        return new(xWay, yWay);
    }

    private Vector2 GetDestination(Vector2 way)
    {
        return new(way.x + 0.5f, way.y + 0.5f);
    }

    public override void ShowWay()
    {
        if (GameController.Instance.IsCheckmate)
        {
            ExecuteRescuerRestrict();
        }
        else
        {
            if (IsRestricted)
            {
                return;
            }

            Action<Square> reachEnemy = delegate (Square square)
            {
                GameController.Instance.table.SpawnEffect(EffectType.Red, square);
            };

            Action<Square> reachNothing = delegate (Square square)
            {
                GameController.Instance.table.SpawnEffect(EffectType.Green, square);
            };

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

    public override bool KingRestrict(Square targetSquare)
    {
        return ExecuteBoolean(targetSquare);
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
            if (ExecuteBoolean(targetSquare))
            {
                return true;
            }
        }
        return false;
    }

    private void ExecutePath(Square targetSquare, Action<Square> reachSquare, Action<Square> reachAlly, Action<Square> reachEnemy, Action<Square> reachNothing)
    {
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                Vector2 way = GetWay(i, j);
                Vector2 destination = GetDestination(way);
                float checkDistance = Vector2.Distance(transform.position, destination);

                if (GameController.Instance.CheckBoundary(way))
                {
                    continue;
                }

                if (checkDistance != Mathf.Sqrt(5))
                {
                    continue;
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
                    continue;
                }
                else if (HasTag(target, ConstantAdvanced.ENEMY))
                {
                    reachEnemy?.Invoke(square);
                    continue;
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

    private bool ExecuteBoolean(Square targetSquare)
    {
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                Vector2 way = GetWay(i, j);
                Vector2 destination = GetDestination(way);
                float checkDistance = Vector2.Distance(transform.position, destination);

                if (GameController.Instance.CheckBoundary(way))
                {
                    continue;
                }

                if (checkDistance != Mathf.Sqrt(5))
                {
                    continue;
                }

                GetSquareInfo(way, out Square square, out Transform target);

                if (square == targetSquare)
                {
                    return true;
                }
                else if (HasTag(target, ConstantAdvanced.ALLY))
                {
                    continue;
                }
                else if (HasTag(target, ConstantAdvanced.ENEMY))
                {
                    continue;
                }
                else
                {
                    continue;
                }
            }
        }
        return false;
    }
}