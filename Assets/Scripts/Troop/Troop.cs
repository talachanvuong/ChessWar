using UnityEngine;

public class Troop : MonoBehaviour
{
    public bool IsRestricted { get; set; }

    public virtual void ShowWay() { }
    public virtual void Checkmate() { }
    public virtual void BotCheckmate() { }
    public virtual void GuardianRestrict() { }
    public virtual bool KingRestrict(Square square) { return false; }
    public virtual void RescuerRestrict() { }
    public virtual bool Predict() { return false; }

    public virtual void Action(Vector2 position)
    {
        Square squareLocate = GameController.Instance.table.GetSquare(transform.position);
        Square squareDestination = GameController.Instance.table.GetSquare(position);

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

        GameController.Instance.ExecutePlayerCheckmate();

        GameController.Instance.EndPlayerTurn();
    }

    protected void ShowCheckmate(Square square)
    {
        // Vế điều kiện sau để khi vua vừa đi mà lại bị chiếu nữa vẫn sẽ hiện đang chiếu
        if (square.signal == null || square.signal.name == "Path(Clone)")
        {
            GameController.Instance.table.SpawnSignal(SignalType.Checkmate, square);
        }
    }

    protected void GetSquareInfo(Vector2 position, out Square square, out Transform target)
    {
        square = GameController.Instance.table.GetSquare(position);
        target = square.troop;
    }

    protected void GetPositionAmongPlayerKing(Vector2 position, out Vector2 resultVector)
    {
        Vector2 playerKingPosition = GameController.Instance.GetPlayerKingPosition();
        Vector2 targetPosition = new((int)position.x, (int)position.y);
        resultVector = targetPosition - playerKingPosition;
    }

    protected bool HasTag(Transform target, string tag)
    {
        if (target == null)
        {
            return false;
        }
        else if (target.CompareTag(tag))
        {
            return true;
        }
        return false;
    }

    protected bool IsTroop<T>(Transform target, string tag) where T : Component
    {
        if (target == null)
        {
            return false;
        }
        else
        {
            T troop = target.GetComponent<T>();

            if (troop == null)
            {
                return false;
            }
            else if (troop.CompareTag(tag))
            {
                return true;
            }
            return false;
        }
    }

    public void BecomeQueen()
    {
        Square square = GameController.Instance.table.GetSquare(transform.position);
        int positionY = (int)transform.position.y;

        if (IsTroop<Pawn>(transform, ConstantAdvanced.ALLY) && positionY == 7)
        {
            Destroy(gameObject);

            if (GameController.Instance.IsWhiteMain)
            {
                GameController.Instance.table.SpawnTroop(TroopType.WhiteQueen, square);
            }
            else
            {
                GameController.Instance.table.SpawnTroop(TroopType.BlackQueen, square);
            }
        }
        else if (IsTroop<Pawn>(transform, ConstantAdvanced.ENEMY) && positionY == 0)
        {
            Destroy(gameObject);

            if (GameController.Instance.IsWhiteMain)
            {
                GameController.Instance.table.SpawnTroop(TroopType.BlackQueen, square);
            }
            else
            {
                GameController.Instance.table.SpawnTroop(TroopType.WhiteQueen, square);
            }
        }
    }
}