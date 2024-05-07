using UnityEngine;

public struct Vector2Advanced
{
    private static readonly Vector2 upVector = new(0, 1);
    private static readonly Vector2 downVector = new(0, -1);
    private static readonly Vector2 leftVector = new(-1, 0);
    private static readonly Vector2 rightVector = new(1, 0);
    private static readonly Vector2 upperLeftVector = new(-1, 1);
    private static readonly Vector2 upperRightVector = new(1, 1);
    private static readonly Vector2 lowerLeftVector = new(-1, -1);
    private static readonly Vector2 lowerRightVector = new(1, -1);

    public static Vector2 Up
    {
        get
        {
            return upVector;
        }
    }

    public static Vector2 Down
    {
        get
        {
            return downVector;
        }
    }

    public static Vector2 Left
    {
        get
        {
            return leftVector;
        }
    }

    public static Vector2 Right
    {
        get
        {
            return rightVector;
        }
    }

    public static Vector2 UpperLeft
    {
        get
        {
            return upperLeftVector;
        }
    }

    public static Vector2 UpperRight
    {
        get
        {
            return upperRightVector;
        }
    }

    public static Vector2 LowerLeft
    {
        get
        {
            return lowerLeftVector;
        }
    }

    public static Vector2 LowerRight
    {
        get
        {
            return lowerRightVector;
        }
    }
}

public struct Vector2DAdvanced
{
    private static readonly Vector2[] horizontalVector2D = { Vector2Advanced.Left, Vector2Advanced.Right };
    private static readonly Vector2[] verticalVector2D = { Vector2Advanced.Up, Vector2Advanced.Down };
    private static readonly Vector2[] rightObliqueVector2D = { Vector2Advanced.UpperRight, Vector2Advanced.LowerLeft };
    private static readonly Vector2[] leftObliqueVector2D = { Vector2Advanced.UpperLeft, Vector2Advanced.LowerRight };
    private static readonly Vector2[] plusVector2D = { Vector2Advanced.Up, Vector2Advanced.Down, Vector2Advanced.Left, Vector2Advanced.Right };
    private static readonly Vector2[] crossVector2D = { Vector2Advanced.UpperLeft, Vector2Advanced.UpperRight, Vector2Advanced.LowerLeft, Vector2Advanced.LowerRight };
    private static readonly Vector2[] multiDirectionalVector2D = { Vector2Advanced.Up, Vector2Advanced.Down, Vector2Advanced.Left, Vector2Advanced.Right, Vector2Advanced.UpperLeft, Vector2Advanced.UpperRight, Vector2Advanced.LowerLeft, Vector2Advanced.LowerRight };
    
    public static Vector2[] Horizontal
    {
        get
        {
            return horizontalVector2D;
        }
    }

    public static Vector2[] Vertical
    {
        get
        {
            return verticalVector2D;
        }
    }

    public static Vector2[] RightOblique
    {
        get
        {
            return rightObliqueVector2D;
        }
    }

    public static Vector2[] LeftOblique
    {
        get
        {
            return leftObliqueVector2D;
        }
    }

    public static Vector2[] Plus
    {
        get
        {
            return plusVector2D;
        }
    }
    

    public static Vector2[] Cross
    {
        get
        {
            return crossVector2D;
        }
    }

    public static Vector2[] MultiDirectional
    {
        get
        {
            return multiDirectionalVector2D;
        }
    }
}

public struct ConstantAdvanced
{
    public const int TABLE_LENGTH = 8;
    public const int MIN_BOUNDARY = 0;
    public const int MAX_BOUNDARY = 7;
    public const int MAX_DISTANCE = 7;
    public const string ALLY = "Ally";
    public const string ENEMY = "Enemy";
    public const string EVENT_SYSTEM = "EventSystem";
}