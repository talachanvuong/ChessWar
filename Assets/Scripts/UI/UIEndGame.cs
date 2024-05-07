using System.Collections;
using UnityEngine;

public class UIEndGame : MonoBehaviour
{
    [SerializeField] private ParticleSystem deadKing;
    [SerializeField] private CanvasGroup endGame;

    private void Start()
    {
        Controller.Instance.uiController.OnEndGame += UIEndGame_OnEndGame;
    }

    private void OnDestroy()
    {
        Controller.Instance.uiController.OnEndGame -= UIEndGame_OnEndGame;
    }

    private void UIEndGame_OnEndGame(object sender, System.EventArgs e)
    {
        StartCoroutine(HandleEndGame());
    }

    private IEnumerator HandleEndGame()
    {
        Controller.Instance.uiController.DisableAccess();

        Vector2 kingPosition = GameController.Instance.GetPlayerKingPosition();
        Square square = GameController.Instance.table.GetSquare(kingPosition);
        GameObject king = square.troop.gameObject;

        ColorUtility.TryParseHtmlString("#ffffff00", out Color color);
        LeanTween.color(king, color, 0.2f);

        yield return new WaitForSeconds(0.2f);

        Controller.Instance.soundController.PlaySound(SoundType.Dead);
        deadKing.transform.position = square.transform.position;
        deadKing.Play();

        yield return new WaitForSeconds(1f);

        LeanTween.alphaCanvas(endGame, 1f, 1f);

        yield return new WaitForSeconds(10f);

        Controller.Instance.sceneController.LoadScene(0);
    }
}