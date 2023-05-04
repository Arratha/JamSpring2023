Shader "Unlit/UnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
#ifdef GL_ES
            precision mediump float;
#endif
#define RADIANS 0.017453292519943295

            static const int zoom = 40;
            static const float brightness = 0.975;
            static float fScale = 1.25;

            float cosRange(float degrees, float range, float minimum) {
                return (((1.0 + cos(degrees * RADIANS)) * 0.5) * range) + minimum;
            }

            fixed4 frag(v2f i) : SV_Target
            {
 
                float time = _Time.y * 1.25;
                float2 uv = i.uv;
                float2 p = 2 * i.uv - i.uv;
                float ct = cosRange(time * 5.0, 3.0, 1.1);
                float xBoost = cosRange(time * 0.2, 5.0, 5.0);
                float yBoost = cosRange(time * 0.1, 10.0, 5.0);

                fScale = cosRange(time * 15.5, 1.25, 0.5);

                for (int i = 1; i < zoom; i++) {
                    float _i = float(i);
                    float2 newp = p;
                    newp.x += 0.25 / _i * sin(_i * p.y + time * cos(ct) * 0.5 / 20.0 + 0.005 * _i) * fScale + xBoost;
                    newp.y += 0.25 / _i * sin(_i * p.x + time * ct * 0.3 / 40.0 + 0.03 * float(i + 15)) * fScale + yBoost;
                    p = newp;
                }

                float3 col = float3(0.5 * sin(3.0 * p.x) + 0.5, 0.5 * sin(3.0 * p.y) + 0.5, sin(p.x + p.y));
                col *= brightness;

                // Add border
                float vigAmt = 5.0;
                float vignette = (1. - vigAmt * (uv.y - .5) * (uv.y - .5)) * (1. - vigAmt * (uv.x - .5) * (uv.x - .5));
                float extrusion = (col.x + col.y + col.z) / 4.0;
                extrusion *= 1.5;
                extrusion *= vignette;

                return float4(col, extrusion);
            }

            /** SHADERDATA
            {
                "title": "70s Melt",
                "description": "Variation of Sine Puke",
                "model": "car"
            }
            */

            
            ENDCG
        }
    }
}
