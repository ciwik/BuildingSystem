using UnityEngine;

namespace Extensions
{
    public static class VectorExtensions
    {
        public static bool IsZero(this Vector2 vector)
        {
            return vector == Vector2.zero || (Mathf.Abs(vector.x) < float.Epsilon &&
                Mathf.Abs(vector.y) < float.Epsilon);
        }

        public static bool IsZero(this Vector3 vector)
        {
            return vector == Vector3.zero || (Mathf.Abs(vector.x) < float.Epsilon && 
                Mathf.Abs(vector.y) < float.Epsilon &&
                Mathf.Abs(vector.z) < float.Epsilon);
        }

        public static float Min(this Vector2 vector)
        {
            return Mathf.Min(vector.x, vector.y);
        }

        public static float Min(this Vector3 vector)
        {
            return Mathf.Min(vector.x, Mathf.Min(vector.y, vector.z));
        }

        public static float Max(this Vector2 vector)
        {
            return Mathf.Max(vector.x, vector.y);
        }

        public static float Max(this Vector3 vector)
        {
            return Mathf.Max(vector.x, Mathf.Max(vector.y, vector.z));
        }
    }
}
