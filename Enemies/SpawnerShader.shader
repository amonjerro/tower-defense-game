Shader "Unlit/SpawnerShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,0,0,1)
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent"
            "RenderQueue"="Transparent"
        }


        Pass
        {
            ZWrite Off
            Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            float4 _Color;

            #include "UnityCG.cginc"
            #define TAU 6.28318530718

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolator
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            Interpolator vert (MeshData v)
            {
                Interpolator o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (Interpolator i) : SV_Target
            {
                float2 mask = i.uv*2-1;
                float radialDistance = length(mask);
                float wave = cos( (radialDistance - _Time.y * 0.3 ) * TAU * 2 )*0.5+0.5;
                wave *= 1-radialDistance;
                float4 maskedColor = _Color * wave;
                return maskedColor;
            }
            ENDCG
        }
    }
}
