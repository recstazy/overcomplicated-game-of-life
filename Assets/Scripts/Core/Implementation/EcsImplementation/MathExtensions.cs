using Unity.Mathematics;
using UnityEngine;

namespace GameOfLife.Core.Ecs
{
    public static class MathExtensions
    {
        private static string MATRIX_FORMAT = "#0.00";

        public static float2 ToFloat2(this int2 value) 
            => new float2(value.x, value.y);

        public static float2 ToFloat2(this Vector2Int value)
            => new float2(value.x, value.y);

        public static float2 ToFloat2(this float3 value) 
            => new float2(value.x, value.y);

        public static float3 ToFloat3(this Vector2Int value) 
            => new float3(value.x, value.y, 0f);

        public static float4 ToFloat4(this Color color)
            => new float4(color.r, color.g, color.b, color.a);

        public static int2 ToInt2(this Vector2Int value)
            => new int2(value.x, value.y);

        public static Vector3 ToVector3(this float2 value) 
            => new Vector3(value.x, value.y, 0f);

        public static float3 MultiplyPoint(this float4x4 matrix, float3 point)
        {
            var result = math.mul(matrix, new float4(point, 1f));
            return new float3(result.x, result.y, result.z) / result.w;
        }

        public static string ToBeautyString(this Matrix4x4 matrix)
        {
            return "\n" + 
                $"{FormatM(matrix.m00)}\t{FormatM(matrix.m01)}\t{FormatM(matrix.m02)}\t{FormatM(matrix.m03)}\n" +
                $"{FormatM(matrix.m10)}\t{FormatM(matrix.m11)}\t{FormatM(matrix.m12)}\t{FormatM(matrix.m13)}\n" +
                $"{FormatM(matrix.m20)}\t{FormatM(matrix.m21)}\t{FormatM(matrix.m22)}\t{FormatM(matrix.m23)}\n" +
                $"{FormatM(matrix.m30)}\t{FormatM(matrix.m31)}\t{FormatM(matrix.m32)}\t{FormatM(matrix.m33)}\n";
        }

        public static string ToBeautyString(this float4x4 matrix)
        {
            return "\n" +
                $"{FormatM(matrix.c0.x)}\t{FormatM(matrix.c1.x)}\t{FormatM(matrix.c2.x)}\t{FormatM(matrix.c3.x)}\n" +
                $"{FormatM(matrix.c0.y)}\t{FormatM(matrix.c1.y)}\t{FormatM(matrix.c2.y)}\t{FormatM(matrix.c3.y)}\n" +
                $"{FormatM(matrix.c0.z)}\t{FormatM(matrix.c1.z)}\t{FormatM(matrix.c2.z)}\t{FormatM(matrix.c3.z)}\n" +
                $"{FormatM(matrix.c0.w)}\t{FormatM(matrix.c1.w)}\t{FormatM(matrix.c2.w)}\t{FormatM(matrix.c3.w)}\n";
        }

        public static bool RaycastPlane(float3 rayOrigin, float3 rayDirection, float3 planePosition, float3 planeNormal, out float3 position)
        {
            var planeDistance = -math.dot(planeNormal, planePosition);
            var vdot = math.dot(rayDirection, planeNormal);
            var ndot = -math.dot(rayOrigin, planeNormal) - planeDistance;

            if (Approximately(vdot, 0.0f))
            {
                position = float3.zero;
                return false;
            }

            var hitDistance = ndot / vdot;
            position = rayOrigin + rayDirection * hitDistance;
            return true;
        }

        public static bool Approximately(float a, float b)
        {
            return math.abs(a - b) < math.EPSILON;
        }

        private static string FormatM(float value)
            => value.ToString(MATRIX_FORMAT);
    }
}
