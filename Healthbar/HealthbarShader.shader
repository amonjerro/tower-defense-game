Shader "Unlit/HealthbarShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StartColor("Start Color", Color) = (1,1,1,1)
        _EndColor("End Color", Color) = (0,0,0,0)
        _Health("Health", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _StartColor;
            float4 _EndColor;
            float _Health;

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {

                float healthMask = i.uv.x < _Health;
                float3 colorMask = lerp(_StartColor, _EndColor, _Health);
                return float4(colorMask,1)*healthMask;
            }
            ENDCG
        }
    }
}
