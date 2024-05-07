using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public Bot bot;
    public Table table;

    [SerializeField] private ParticleSystem eliminateTroop;
    private PlayerInteract playerInteract;

    public GameObject currentSelectedTroop;
    public List<Vector2> rescuePositions;
    public List<Transform> checkmateTroops;

    public bool IsBotMoving { get; set; }
    public bool IsCheckmate { get; set; }
    public bool IsTouchedTroop { get; set; }
    public bool IsWhiteMain { get; set; }
    public bool IsWhiteTurn { get; set; }
    public int TurnCount { get; set; }
    
    private void Awake()
    {
        // Singleton
        if (Instance != null)
        {
            Debug.Log("Error");
        }
        Instance = this;
    }

    private void Start()
    {
        // Get value transfer
        IsWhiteMain = Controller.Instance.valueTransfer.isWhiteMain;

        // Set default value
        TurnCount = 1;

        // Enable new input system
        playerInteract = new PlayerInteract();
        playerInteract.Player.Enable();

        playerInteract.Player.Touch.performed += TouchPerformed;
    }

    private void OnDestroy()
    {
        playerInteract.Player.Touch.performed -= TouchPerformed;
    }

    private void TouchPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (IsBotMoving)
        {
            return;
        }

        Vector2 screenPosition = obj.ReadValue<Vector2>();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        if (worldPosition.y < 0) return;
        Vector2 position = new((int)worldPosition.x, (int)worldPosition.y);

        if (CheckBoundary(position))
        {
            return;
        }

        if (table.IsTouchedAlly(position))
        {
            table.ExecuteShowWay(position);
        }

        else if (IsTouchedTroop)
        {
            table.ExecuteAction(position);
        }
    }

    public bool CheckBoundary(Vector2 position)
    {
        int positionX = (int)position.x;
        int positionY = (int)position.y;

        bool isOutBoundaryX = positionX < ConstantAdvanced.MIN_BOUNDARY || positionX > ConstantAdvanced.MAX_BOUNDARY;
        bool isOutBoundaryY = positionY < ConstantAdvanced.MIN_BOUNDARY || positionY > ConstantAdvanced.MAX_BOUNDARY;
        bool isOutBoundary = isOutBoundaryX || isOutBoundaryY;

        return isOutBoundary;
    }

    public void TroopAction(Vector2 position)
    {
        currentSelectedTroop.GetComponent<Troop>().Action(position);
    }

    public Vector2 GetPlayerKingPosition()
    {
        return GetKingPosition(ConstantAdvanced.ALLY);
    }

    public Vector2 GetBotKingPosition()
    {
        return GetKingPosition(ConstantAdvanced.ENEMY);
    }

    private Vector2 GetKingPosition(string tag)
    {
        for (int i = 0; i < ConstantAdvanced.TABLE_LENGTH; i++)
        {
            for (int j = 0; j < ConstantAdvanced.TABLE_LENGTH; j++)
            {
                Square square = table.GetSquare(j, i);
                Transform target = square.troop;

                if (target == null)
                {
                    continue;
                }
                else
                {
                    King king = target.GetComponent<King>();

                    if (king == null)
                    {
                        continue;
                    }
                    else if (king.CompareTag(tag))
                    {
                        return new(j, i);
                    }
                }
            }
        }
        return new(0, 0);
    }

    public void ExecuteBotCheckmate()
    {
        Action<Transform> action = delegate (Transform target)
        {
            target.GetComponent<Troop>().BotCheckmate();
        };
        ExecuteFunction(ConstantAdvanced.ENEMY, action);
    }

    public void ExecutePlayerCheckmate()
    {
        Action<Transform> action = delegate (Transform target)
        {
            target.GetComponent<Troop>().Checkmate();
        };
        ExecuteFunction(ConstantAdvanced.ALLY, action);
    }

    public void ExecuteGuardianRestrict()
    {
        Action<Transform> action = delegate (Transform target)
        {
            target.GetComponent<Troop>().GuardianRestrict();
        };
        ExecuteFunction(ConstantAdvanced.ENEMY, action);
    }

    public bool ExecuteKingRestrict(Square targetSquare)
    {
        Func<Transform, bool> func = delegate (Transform target)
        {
            return target.GetComponent<Troop>().KingRestrict(targetSquare);
        };
        return ExecuteBoolean(ConstantAdvanced.ENEMY, func);
    }

    public void ExecuteRescuerRestrict()
    {
        foreach (Transform checkmateTroop in checkmateTroops)
        {
            checkmateTroop.GetComponent<Troop>().RescuerRestrict();
        }
    }

    public bool ExecutePredict()
    {
        Func<Transform, bool> func = delegate (Transform target)
        {
            return target.GetComponent<Troop>().Predict();
        };
        return ExecuteBoolean(ConstantAdvanced.ALLY, func);
    }

    public void ExecuteEndGame()
    {
        if (ExecutePredict())
        {
            return;
        }
        Controller.Instance.uiController.ExecuteOnEndGame();
    }

    private void ExecuteFunction(string tag, Action<Transform> action)
    {
        for (int i = 0; i < ConstantAdvanced.TABLE_LENGTH; i++)
        {
            for (int j = 0; j < ConstantAdvanced.TABLE_LENGTH; j++)
            {
                Square square = table.GetSquare(j, i);
                Transform target = square.troop;

                if (target == null)
                {
                    continue;
                }

                if (target.CompareTag(tag))
                {
                    action.Invoke(target);
                }
            }
        }
    }

    private bool ExecuteBoolean(string tag, Func<Transform, bool> func)
    {
        List<bool> results = new();

        for (int i = 0; i < ConstantAdvanced.TABLE_LENGTH; i++)
        {
            for (int j = 0; j < ConstantAdvanced.TABLE_LENGTH; j++)
            {
                Square square = table.GetSquare(j, i);
                Transform target = square.troop;

                if (target == null)
                {
                    continue;
                }

                if (target.CompareTag(tag))
                {
                    results.Add(func.Invoke(target));
                }
            }
        }

        foreach (bool result in results)
        {
            if (result)
            {
                return true;
            }
        }
        return false;
    }

    public void RefreshStatus()
    {
        Action<Transform> refreshRestrict = (Transform target) =>
        {
            target.GetComponent<Troop>().IsRestricted = false;
        };

        IsCheckmate = false;
        checkmateTroops.Clear();
        rescuePositions.Clear();
        ExecuteFunction(ConstantAdvanced.ALLY, refreshRestrict);
    }

    public void ShowEliminateTroop(Vector2 position)
    {
        eliminateTroop.transform.position = position;
        eliminateTroop.Play();
    }

    public void EndPlayerTurn()
    {
        IsWhiteTurn = !IsWhiteTurn;

        if (!IsWhiteMain)
        {
            TurnCount++;
            Controller.Instance.uiController.ExecuteOnTurnCountChange();
        }

        bot.GetBotMove();
    }

    public void EndBotTurn()
    {
        IsWhiteTurn = !IsWhiteTurn;

        if (IsWhiteMain)
        {
            TurnCount++;
            Controller.Instance.uiController.ExecuteOnTurnCountChange();
        }

        Controller.Instance.uiController.ExecuteOnBotDoneThinking();
    }

    public void BecomeQueen(Square square)
    {
        Transform target = square.troop;
        target.GetComponent<Troop>().BecomeQueen();
    }
}