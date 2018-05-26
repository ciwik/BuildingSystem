using UnityEngine;

namespace Extensions
{
    public static class VectorExtensions
    {
        public static bool IsZero(this Vector2 vector)
        {
            return vector.x == 0f && vector.y == 0f;
        }

        public static bool IsZero(this Vector3 vector)
        {
            return vector.x == 0f && vector.y == 0f && vector.z == 0f;
        }
    }
}
