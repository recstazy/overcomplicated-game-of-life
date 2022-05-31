using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace GameOfLife.Core.Ecs
{
    [BurstCompile]
    public struct CameraInfo
    {
        public readonly float4x4 ProjectionMatrix;
        public readonly float4x4 WorldToCameraMatrix;
        public readonly float3 Position;
        public readonly quaternion Rotation;

        public CameraInfo(Camera camera)
        {
            ProjectionMatrix = camera.projectionMatrix;
            WorldToCameraMatrix = camera.worldToCameraMatrix;
            Position = camera.transform.position;
            Rotation = camera.transform.rotation;
        }

        public float4x4 GetWorldToScreenMatrix()
        {
            var viewProjectionMatrix = math.mul(ProjectionMatrix, WorldToCameraMatrix);

            // Compensating projection space (-1, 1) coordinates range to get (0, 1) range
            var vpToViewPort = math.mul(
                float4x4.TRS(Vector3.zero, Quaternion.identity, new float3(0.5f, 0.5f, 1f)),
                float4x4.TRS(new float3(1f, 1f, 0f), Quaternion.identity, new float3(1f)));

            var worldToViewPort = math.mul(vpToViewPort, viewProjectionMatrix);

            // Converting (0, 1) range to (resultion.width, resolution.height)
            var viewPortToScreen = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, 1f));
            var worldToScreen = math.mul(viewPortToScreen, worldToViewPort);
            return worldToScreen;
        }

        public float4x4 GetScreenToWorldMatrix()
        {
            return math.inverse(GetWorldToScreenMatrix());
        }

        public (float3 origin, float3 direction) ScreenPointToRay(float2 screenPosition)
        {
            var screenToWorldMatrix = GetScreenToWorldMatrix();
            var nearPlanePoint = screenToWorldMatrix.MultiplyPoint(new float3(screenPosition, 0f));
            var farPlanePoint = screenToWorldMatrix.MultiplyPoint(new float3(screenPosition, 1f));
            var direction = math.normalize(farPlanePoint - nearPlanePoint);
            return (nearPlanePoint, direction);
        }
    }
}
