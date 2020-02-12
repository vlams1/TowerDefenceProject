Shader "Unlit/Grid" {
    Properties {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _GridColor ("Grid Color", Color) = (0, 0, 0, 1)
        _LineSize ("Line Size", Float) = .05
    }
    SubShader {
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _Color;
            float4 _GridColor;
            float _LineSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = _Color;
                if (max(abs(frac(i.worldPos.x-.5)-.5),abs(frac(i.worldPos.z-.5)-.5)) > .5 - _LineSize) col = _GridColor;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
