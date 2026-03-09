Shader "Custom/TileGrid_URP"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1,1,1,1)

        _GridColor("Grid Color", Color) = (0,0,0,0.35)
        _GridWidth("Grid Width", Range(0.001, 0.1)) = 0.02
        _GridStrength("Grid Strength", Range(0, 1)) = 1.0
        _TopNormalThreshold("Top Normal Threshold", Range(0,1)) = 0.9
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue"="Geometry"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 worldPos    : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float4 _GridColor;
                float _GridWidth;
                float _GridStrength;
                float _TopNormalThreshold;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);

                OUT.positionCS = posInputs.positionCS;
                OUT.worldPos = posInputs.positionWS;
                OUT.worldNormal = normalize(TransformObjectToWorldNormal(IN.normalOS));
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 baseTex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;

                // Só desenha grid em superfícies viradas para cima
                float topMask = step(_TopNormalThreshold, IN.worldNormal.y);

                // IMPORTANTE:
                // +0.5 alinha as bordas do grid com cubes de 1x1 centrados em coordenadas inteiras
                float2 cell = frac(IN.worldPos.xz + 0.5);

                float distToEdgeX = min(cell.x, 1.0 - cell.x);
                float distToEdgeY = min(cell.y, 1.0 - cell.y);
                float distToEdge = min(distToEdgeX, distToEdgeY);

                float aa = fwidth(distToEdge) * 1.5;

                float lineMask = 1.0 - smoothstep(_GridWidth, _GridWidth + aa, distToEdge);
                lineMask *= topMask;
                lineMask *= _GridStrength;
                lineMask *= _GridColor.a;

                float3 finalColor = lerp(baseTex.rgb, _GridColor.rgb, saturate(lineMask));

                return float4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}