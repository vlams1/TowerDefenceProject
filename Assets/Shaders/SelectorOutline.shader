Shader "Unlit/SelectorOutline" {
    Properties{
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Strength("Strength", Float) = 1
    }
    SubShader{
        Blend SrcAlpha OneMinusSrcAlpha
        Tags { "Queue"="Transparent" }
        LOD 100

        Pass{
            
            Cull Off
            ZWrite Off
            ZTest LEqual
            Offset -2, -2
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f{
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Strength;

            v2f vert (appdata v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target{
                fixed4 col = _Color;
                float2 uv = 1-abs(i.uv*2.-1.)*2.;
                col.a *= clamp(1.-abs(min(uv.x,uv.y))*_Strength,0.,1.);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
