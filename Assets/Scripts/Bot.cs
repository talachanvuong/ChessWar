using UnityEngine.Networking;
using System.Collections;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public void GetBotMove()
    {
        Controller.Instance.uiController.ExecuteOnBotThinking();
        StartCoroutine(GetDataFromServer());
    }

    private void Start()
    {
        Controller.Instance.uiController.OnBotThinking += Bot_OnBotThinking;
        Controller.Instance.uiController.OnBotDoneThinking += Bot_OnBotDoneThinking;
    }

    private void OnDestroy()
    {
        Controller.Instance.uiController.OnBotThinking -= Bot_OnBotThinking;
        Controller.Instance.uiController.OnBotDoneThinking -= Bot_OnBotDoneThinking;
    }

    private void Bot_OnBotDoneThinking(object sender, System.EventArgs e)
    {
        GameController.Instance.IsBotMoving = false;
    }

    private void Bot_OnBotThinking(object sender, System.EventArgs e)
    {
        GameController.Instance.IsBotMoving = true;
    }

    private IEnumerator GetDataFromServer()
    {
        string url = $"https://stockfish.online/api/stockfish.php?fen={GetFENString()}&depth=13&mode=bestmove";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.Success:
                string result = request.downloadHandler.text;
                DataResult data = JsonUtility.FromJson<DataResult>(result);
                string step = data.data;
                string locate = step.Substring(9, 2);
                string target = step.Substring(11, 2);

                Move(locate, target);
                break;
        }
    }

    private void Move(string locate, string destination)
    {
        Vector2 positionLocate = ConvertSquareNameToPosition(locate);
        Vector2 positionDestination = ConvertSquareNameToPosition(destination);

        Square squareLocate = GameController.Instance.table.GetSquare(positionLocate);
        Square squareDestination = GameController.Instance.table.GetSquare(positionDestination);

        Transform troop = squareLocate.troop;
        Transform target = squareDestination.troop;

        troop.position = squareDestination.transform.position;

        if (target != null)
        {
            Controller.Instance.soundController.PlaySound(SoundType.Kill);
            GameController.Instance.ShowEliminateTroop(target.position);
            Destroy(target.gameObject);
        }
        else
        {
            Controller.Instance.soundController.PlaySound(SoundType.Move);
        }

        GameController.Instance.table.SetTroopInSquare(null, squareLocate);
        GameController.Instance.table.SetTroopInSquare(troop, squareDestination);

        GameController.Instance.BecomeQueen(squareDestination);

        GameController.Instance.table.ShowLastMove(squareLocate, squareDestination);
        GameController.Instance.RefreshStatus();

        GameController.Instance.ExecuteBotCheckmate();

        GameController.Instance.ExecuteGuardianRestrict();
        GameController.Instance.ExecuteRescuerRestrict();

        if (GameController.Instance.IsCheckmate)
        {
            GameController.Instance.ExecuteEndGame();
        }

        GameController.Instance.EndBotTurn();
    }

    private string GetFENString()
    {
        string fenString = string.Empty;
        string faction = GameController.Instance.IsWhiteMain ? "b" : "w";
        int turnCount = GameController.Instance.TurnCount;

        for (int i = ConstantAdvanced.MAX_BOUNDARY; i >= ConstantAdvanced.MIN_BOUNDARY; i--)
        {
            for (int j = 0; j < ConstantAdvanced.TABLE_LENGTH; j++)
            {
                Square square = GameController.Instance.table.GetSquare(j, i);
                fenString += (square.troop != null) ? ConvertTroopToLetter(square.troop.name) : 1;
            }
            fenString += "/";
        }

        fenString = fenString[..^1];
        fenString = GameController.Instance.IsWhiteMain ? fenString : fenString.Reverse();

        return $"{fenString} {faction} - - 0 {turnCount}";
    }

    private Vector2 ConvertSquareNameToPosition(string name)
    {
        for (int i = 0; i < ConstantAdvanced.TABLE_LENGTH; i++)
        {
            for (int j = 0; j < ConstantAdvanced.TABLE_LENGTH; j++)
            {
                Square square = GameController.Instance.table.GetSquare(j, i);

                if (square.squareName == name)
                {
                    return new(j, i);
                }
            }
        }
        return new(0, 0);
    }

    private string ConvertTroopToLetter(string name)
    {
        return name switch
        {
            "WhitePawn(Clone)" => "P",
            "WhiteHorse(Clone)" => "N",
            "WhiteStatue(Clone)" => "B",
            "WhiteTank(Clone)" => "R",
            "WhiteQueen(Clone)" => "Q",
            "WhiteKing(Clone)" => "K",

            "BlackPawn(Clone)" => "p",
            "BlackHorse(Clone)" => "n",
            "BlackStatue(Clone)" => "b",
            "BlackTank(Clone)" => "r",
            "BlackQueen(Clone)" => "q",
            "BlackKing(Clone)" => "k",

            _ => string.Empty,
        };
    }
}