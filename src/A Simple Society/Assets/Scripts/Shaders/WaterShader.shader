Shader "Unlit/WaterShader"
{
    Properties
    {
        _WaterColor("Water Color", Color) = (0,0,0,0)
        _ShallowWaterColor("Shallow Water Color", Color) = (0,0,0,0)
        _MaxDistance("Max Depth Distance", Float) = 0
       _FoamDistance("Foam Distance", Float) = 0.4
      
    }
    SubShader {
         Tags{
             "RenderType"="Opaque"
         }
         Lighting On Cull Off ZWrite On
         LOD 200
        pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D cameraDepthTexture;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 screenPos : TEXCOORD2;
                float4 vertex  : SV_POSITION;
                float2 noiseUV : TEXCOORD0;
            };

            float4 _WaterColor;
            float4 _ShallowWaterColor;
            float _MaxDistance;
            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;
            float _SurfaceNoiseCutoff;
            float _FoamDistance;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            sampler2D _CameraDepthTexture;

            fixed4 frag (v2f i) : SV_Target
            { 
                float currentDepth = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r;
                float currentLinearDepth = LinearEyeDepth(currentDepth);
                float diff = (currentLinearDepth - i.screenPos.w);
                float w = saturate(diff/((_FoamDistance*sin(_Time.y-i.screenPos.z))+_FoamDistance)/2);
                float4 color = lerp(_ShallowWaterColor,_WaterColor,w);
              
                return color;
            }
            ENDCG
        }
    }
}
