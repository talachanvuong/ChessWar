using UnityEngine;

public class ParticleSystemBound : MonoBehaviour
{
    private EdgeCollider2D bound;

    private void Start()
    {
        bound = GetComponent<EdgeCollider2D>();

        float orthograpgicSize = Camera.main.orthographicSize;
        float lowerCorner = -(orthograpgicSize - 4.0f);
        float upperCorner = orthograpgicSize * 2.0f + lowerCorner;

        Vector2 firstPoint = new(bound.points[0].x, lowerCorner);
        Vector2 secondPoint = new(bound.points[1].x, upperCorner);
        Vector2 thirdPoint = new(bound.points[2].x, upperCorner);
        Vector2 fourthPoint = new(bound.points[3].x, lowerCorner);

        bound.points = new Vector2[] { firstPoint, secondPoint, thirdPoint, fourthPoint, firstPoint };
    }
}