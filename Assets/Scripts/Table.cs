using TMPro;
using UnityEngine;

public class Table : MonoBehaviour
{
    private Square[,] board;
    [SerializeField] private TextMeshProUGUI[] numbers;
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private Transform[] effects;
    [SerializeField] private Transform[] signals;
    [SerializeField] private Transform[] troops;

    private void Start()
    {
        board = new Square[ConstantAdvanced.TABLE_LENGTH, ConstantAdvanced.TABLE_LENGTH];

        SetupBoard();
        CreateNewGame();
        HandleFaction();
        HandleUI();
        HandleBotFirstMove();
    }

    private void SetupBoard()
    {
        int index = 0;
        for (int i = 0; i < ConstantAdvanced.TABLE_LENGTH; i++)
        {
            for (int j = 0; j < ConstantAdvanced.TABLE_LENGTH; j++)
            {
                board[j, i] = transform.GetChild(index).GetComponent<Square>();
                index++;
            }
        }
    }

    private void CreateNewGame()
    {
        SpawnTroop(TroopType.WhiteTank, board[0, 0]);
        SpawnTroop(TroopType.WhiteHorse, board[1, 0]);
        SpawnTroop(TroopType.WhiteStatue, board[2, 0]);
        SpawnTroop(TroopType.WhiteQueen, board[3, 0]);
        SpawnTroop(TroopType.WhiteKing, board[4, 0]);
        SpawnTroop(TroopType.WhiteStatue, board[5, 0]);
        SpawnTroop(TroopType.WhiteHorse, board[6, 0]);
        SpawnTroop(TroopType.WhiteTank, board[7, 0]);

        SpawnTroop(TroopType.WhitePawn, board[0, 1]);
        SpawnTroop(TroopType.WhitePawn, board[1, 1]);
        SpawnTroop(TroopType.WhitePawn, board[2, 1]);
        SpawnTroop(TroopType.WhitePawn, board[3, 1]);
        SpawnTroop(TroopType.WhitePawn, board[4, 1]);
        SpawnTroop(TroopType.WhitePawn, board[5, 1]);
        SpawnTroop(TroopType.WhitePawn, board[6, 1]);
        SpawnTroop(TroopType.WhitePawn, board[7, 1]);

        SpawnTroop(TroopType.BlackPawn, board[0, 6]);
        SpawnTroop(TroopType.BlackPawn, board[1, 6]);
        SpawnTroop(TroopType.BlackPawn, board[2, 6]);
        SpawnTroop(TroopType.BlackPawn, board[3, 6]);
        SpawnTroop(TroopType.BlackPawn, board[4, 6]);
        SpawnTroop(TroopType.BlackPawn, board[5, 6]);
        SpawnTroop(TroopType.BlackPawn, board[6, 6]);
        SpawnTroop(TroopType.BlackPawn, board[7, 6]);

        SpawnTroop(TroopType.BlackTank, board[0, 7]);
        SpawnTroop(TroopType.BlackHorse, board[1, 7]);
        SpawnTroop(TroopType.BlackStatue, board[2, 7]);
        SpawnTroop(TroopType.BlackQueen, board[3, 7]);
        SpawnTroop(TroopType.BlackKing, board[4, 7]);
        SpawnTroop(TroopType.BlackStatue, board[5, 7]);
        SpawnTroop(TroopType.BlackHorse, board[6, 7]);
        SpawnTroop(TroopType.BlackTank, board[7, 7]);
    }

    private void HandleFaction()
    {
        if (!GameController.Instance.IsWhiteMain)
        {
            ReverseBoard();
        }
    }

    private void HandleUI()
    {
        Controller.Instance.uiController.ExecuteOnTurnCountChange();
    }

    private void HandleBotFirstMove()
    {
        if (!GameController.Instance.IsWhiteMain)
        {
            GameController.Instance.bot.GetBotMove();
        }
    }

    private void ReverseBoard()
    {
        SwapTroop(board[0, 0], board[7, 7]);
        SwapTroop(board[1, 0], board[6, 7]);
        SwapTroop(board[2, 0], board[5, 7]);
        SwapTroop(board[3, 0], board[4, 7]);
        SwapTroop(board[4, 0], board[3, 7]);
        SwapTroop(board[5, 0], board[2, 7]);
        SwapTroop(board[6, 0], board[1, 7]);
        SwapTroop(board[7, 0], board[0, 7]);

        SwapTroop(board[0, 1], board[7, 6]);
        SwapTroop(board[1, 1], board[6, 6]);
        SwapTroop(board[2, 1], board[5, 6]);
        SwapTroop(board[3, 1], board[4, 6]);
        SwapTroop(board[4, 1], board[3, 6]);
        SwapTroop(board[5, 1], board[2, 6]);
        SwapTroop(board[6, 1], board[1, 6]);
        SwapTroop(board[7, 1], board[0, 6]);

        SwapSquareName(board[0, 0], board[7, 7]);
        SwapSquareName(board[1, 0], board[6, 7]);
        SwapSquareName(board[2, 0], board[5, 7]);
        SwapSquareName(board[3, 0], board[4, 7]);
        SwapSquareName(board[4, 0], board[3, 7]);
        SwapSquareName(board[5, 0], board[2, 7]);
        SwapSquareName(board[6, 0], board[1, 7]);
        SwapSquareName(board[7, 0], board[0, 7]);

        SwapSquareName(board[0, 1], board[7, 6]);
        SwapSquareName(board[1, 1], board[6, 6]);
        SwapSquareName(board[2, 1], board[5, 6]);
        SwapSquareName(board[3, 1], board[4, 6]);
        SwapSquareName(board[4, 1], board[3, 6]);
        SwapSquareName(board[5, 1], board[2, 6]);
        SwapSquareName(board[6, 1], board[1, 6]);
        SwapSquareName(board[7, 1], board[0, 6]);

        SwapSquareName(board[0, 2], board[7, 5]);
        SwapSquareName(board[1, 2], board[6, 5]);
        SwapSquareName(board[2, 2], board[5, 5]);
        SwapSquareName(board[3, 2], board[4, 5]);
        SwapSquareName(board[4, 2], board[3, 5]);
        SwapSquareName(board[5, 2], board[2, 5]);
        SwapSquareName(board[6, 2], board[1, 5]);
        SwapSquareName(board[7, 2], board[0, 5]);

        SwapSquareName(board[0, 3], board[7, 4]);
        SwapSquareName(board[1, 3], board[6, 4]);
        SwapSquareName(board[2, 3], board[5, 4]);
        SwapSquareName(board[3, 3], board[4, 4]);
        SwapSquareName(board[4, 3], board[3, 4]);
        SwapSquareName(board[5, 3], board[2, 4]);
        SwapSquareName(board[6, 3], board[1, 4]);
        SwapSquareName(board[7, 3], board[0, 4]);

        SwapFaction();
        SwapNotation();
    }

    private void SwapTroop(Square firstSquare, Square secondSquare)
    {
        Transform firstTroop = firstSquare.troop;
        Transform secondTroop = secondSquare.troop;

        firstTroop.position = secondSquare.transform.position;
        secondTroop.position = firstSquare.transform.position;

        firstTroop.parent = secondSquare.transform;
        secondTroop.parent = firstSquare.transform;

        firstSquare.troop = secondTroop;
        secondSquare.troop = firstTroop;
    }

    private void SwapSquareName(Square firstSquare, Square secondSquare)
    {
        string firstSquareName = firstSquare.squareName;
        string secondSquareName = secondSquare.squareName;

        firstSquare.squareName = secondSquareName;
        secondSquare.squareName = firstSquareName;
    }

    private void SwapFaction()
    {
        int maxHeight = 2;
        int maxWidth = 8;

        for (int i = 0; i < maxHeight; i++)
        {
            for (int j = 0; j < maxWidth; j++)
            {
                Square square = board[j, i];
                square.troop.tag = ConstantAdvanced.ALLY;
            }
        }

        for (int i = 6; i < ConstantAdvanced.TABLE_LENGTH; i++)
        {
            for (int j = 0; j < maxWidth; j++)
            {
                Square square = board[j, i];
                square.troop.tag = ConstantAdvanced.ENEMY;
            }
        }
    }

    private void SwapNotation()
    {
        string text = "HGFEDCBA";
        string number = "87654321";

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].SetText(text[i].ToString());
        }

        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i].SetText(number[i].ToString());
        }
    }

    public void ExecuteShowWay(Vector2 position)
    {
        ClearEffect();
        Square square = GetSquare(position);
        square.troop.GetComponent<Troop>().ShowWay();
        GameController.Instance.IsTouchedTroop = true;
        GameController.Instance.currentSelectedTroop = square.troop.gameObject;
    }

    public void ExecuteAction(Vector2 position)
    {
        Square square = GetSquare(position);

        if (square.effect != null)
        {
            GameController.Instance.TroopAction(position);
        }

        ClearEffect();
        GameController.Instance.IsTouchedTroop = false;
        GameController.Instance.currentSelectedTroop = null;
    }

    public void SpawnTroop(TroopType troopType, Square square)
    {
        Transform selectedTroop = troops[(int)troopType];
        square.troop = Instantiate(selectedTroop, square.transform);
    }

    public void SpawnEffect(EffectType effectType, Square square)
    {
        Transform selectedEffect = effects[(int)effectType];
        square.effect = Instantiate(selectedEffect, square.transform);
    }

    public void SpawnSignal(SignalType signalType, Square square)
    {
        Transform selectedSignal = signals[(int)signalType];
        square.signal = Instantiate(selectedSignal, square.transform);
    }

    public Square GetSquare(Vector2 position)
    {
        return board[(int)position.x, (int)position.y];
    }

    public Square GetSquare(float x, float y)
    {
        return board[(int)x, (int)y];
    }

    public bool IsTouchedAlly(Vector2 position)
    {
        Square square = GetSquare(position);

        if (square.troop == null)
        {
            return false;
        }
        else
        {
            if (square.troop.CompareTag(ConstantAdvanced.ALLY))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void SetTroopInSquare(Transform target, Square square)
    {
        square.troop = target;

        if (square.troop != null)
        {
            square.troop.parent = square.transform;
        }
    }

    public void ShowLastMove(Square squareLocate, Square squareDestination)
    {
        ClearSignal();
        SpawnSignal(SignalType.Path, squareLocate);
        SpawnSignal(SignalType.Path, squareDestination);
    }

    private void ClearEffect()
    {
        for (int i = 0; i < ConstantAdvanced.TABLE_LENGTH; i++)
        {
            for (int j = 0; j < ConstantAdvanced.TABLE_LENGTH; j++)
            {
                Square square = GetSquare(j, i);
                if (square.effect != null)
                {
                    Destroy(square.effect.gameObject);
                }
            }
        }
    }

    private void ClearSignal()
    {
        for (int i = 0; i < ConstantAdvanced.TABLE_LENGTH; i++)
        {
            for (int j = 0; j < ConstantAdvanced.TABLE_LENGTH; j++)
            {
                Square square = GetSquare(j, i);
                if (square.signal != null)
                {
                    Destroy(square.signal.gameObject);
                }
            }
        }
    }
}