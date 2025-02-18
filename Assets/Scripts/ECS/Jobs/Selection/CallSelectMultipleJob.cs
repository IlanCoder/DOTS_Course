using ECS.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Jobs.Selection {
    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    [BurstCompile]
    public partial struct CallSelectMultipleJob : IJobEntity {
        public float3 CamPos;
        public float4x4 CamProjMatrix;
        public float3 CamUp;
        public float3 CamRight;
        public float3 CamForward;
        public float PixelWidth;
        public float PixelHeight;
        public Rect SelectionArea;
        
        public void Execute(in LocalTransform transform, EnabledRefRW<Selected> selectedEnabled, ref Selected selected) {
            float2 unitScreenPos = ConvertWorldToScreenCoordinates(transform.Position, CamPos, CamProjMatrix, CamUp,
                CamRight, CamForward, PixelWidth, PixelHeight, 1);
            if (!SelectionArea.Contains(unitScreenPos)) {
                if (selectedEnabled.ValueRO) selected.OnDeselected = true;
                return;
            }
            if (selectedEnabled.ValueRO) return;
            selected.OnSelected = true;
        }

        float2 ConvertWorldToScreenCoordinates(float3 point, float3 cameraPos, float4x4 camProjMatrix, float3 camUp,
            float3 camRight, float3 camForward, float pixelWidth, float pixelHeight, float scaleFactor) {
            
            float4 pointInCameraCoordinates =
                ConvertWorldToCameraCoordinates(point, cameraPos, camUp, camRight, camForward);

            //convert P_camera to P_clipped
            float4 pointInClipCoordinates = math.mul(camProjMatrix, pointInCameraCoordinates);
           
            /*convert P_clipped to P_ndc
            Normalized Device Coordinates*/
            float4 pointInNdc = pointInClipCoordinates / pointInClipCoordinates.w;
            
            //4 convert P_ndc to P_screen
            float2 pointInScreenCoordinates;
            pointInScreenCoordinates.x = pixelWidth / 2.0f * (pointInNdc.x + 1);
            pointInScreenCoordinates.y = pixelHeight / 2.0f * (pointInNdc.y + 1);
            
            return pointInScreenCoordinates / scaleFactor;
        }

        float4 ConvertWorldToCameraCoordinates(float3 point, float3 cameraPos, float3 camUp,
            float3 camRight, float3 camForward) {
            float4 translatedPoint = new float4(point - cameraPos, 1f);
            
            float4x4 transformationMatrix = float4x4.identity;
            transformationMatrix.c0 = new float4(camRight.x, camUp.x, -camForward.x, 0);
            transformationMatrix.c1 = new float4(camRight.y, camUp.y, -camForward.y, 0);
            transformationMatrix.c2 = new float4(camRight.z, camUp.z, -camForward.z, 0);

            float4 transformedPoint = math.mul(transformationMatrix, translatedPoint);

            return transformedPoint;
        }
    }
}