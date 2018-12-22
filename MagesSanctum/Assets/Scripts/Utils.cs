using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Calculates the time it will take to move from A to B
    /// </summary>
    /// <returns>The time it will take to move between <paramref name="source"/> and <paramref name="target"/> at the given <paramref name="speed"/></returns>
    public static float GetMoveTime(this Vector3 source, Vector3 target, float speed)
    {
        return Vector3.Distance(source, target) / speed;
    }

    public static Vector3 SetInv(this Vector3 vector, Axis invAxis, float value)
    {
        return Set(vector, ~invAxis, value);
    }

    public static Vector3 Set(this Vector3 vector, Axis axis, float value)
    {
        if (axis.HasFlag(Axis.X))
            vector.x = value;
        if (axis.HasFlag(Axis.Y))
            vector.y = value;
        if (axis.HasFlag(Axis.Z))
            vector.z = value;

        return vector;
    }

    [System.Flags]
    public enum Axis
    {
        NONE = 0,
        X = 1,
        Y = 2,
        Z = 4
    }
}
