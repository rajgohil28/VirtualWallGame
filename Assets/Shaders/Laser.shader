Shader "Custom/Unlit/Laser"
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


            float rand(float2 p) {
                return frac(sin(dot(p, float2(12.543, 514.123))) * 4732.12);
            }

            float noise(float2 p) {
                float2 f = smoothstep(0.0, 1.0, frac(p));
                float2 i = floor(p);
                float a = rand(i);
                float b = rand(i + float2(1.0, 0.0));
                float c = rand(i + float2(0.0, 1.0));
                float d = rand(i + float2(1.0, 1.0));
                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);

            }

            float fbm(float2 p) {
                float a = 0.5;
                float r = 0.0;
                for (int i = 0; i < 8; i++) {
                    r += a * noise(p);
                    a *= 0.5;
                    p *= 2.0;
                }
                return r;
            }
            float laser(float2 p, float s) {
                float r = atan2(p.x, p.y);
                float l = length(p);
                float sn = sin(r * s + _Time.y);
                return pow(0.5 + 0.5 * sn, 5.0) + pow(clamp(sn, 0.0, 1.0), 100.0);
            }

            float clouds(float2 uv) {
                float c1 = fbm(fbm(uv * 3.0) * 0.75 + uv * 3.0 + float2(0.0, +_Time.y / 3.0));
                float c2 = fbm(fbm(uv * 2.0) * 0.5 + uv * 7.0 + float2(0.0, +_Time.y / 3.0));
                float c3 = pow(fbm(fbm(uv * 10.0 - float2(0.0, _Time.y)) * 0.75 + uv * 5.0 + float2(0.0, +_Time.y / 6.0)), 2.0);
                return pow(lerp(c1, c2, c3), 2.0);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 hs = uv * 0.5;
                float2 uvc = uv - hs;
                float ls = (1.0 + 3.0 * noise(float2(15.0 - _Time.y, 15.0 - _Time.y))) * laser(float2(uv.x + 0.5, uv.y * (0.5 + 10.0 * noise(float2(_Time.y / 5.0, _Time.y / 5.0))) + 0.1), 15.0);
                ls += fbm(float2(2.0 * _Time.y, 2.0 * _Time.y)) * laser(float2(hs.x - uvc.x - 0.2, uv.y + 0.1), 25.0);
                ls += noise(float2(_Time.y - 73.0, _Time.y - 73.0)) * laser(float2(uvc.x, 1.0 - uv.y + 0.5), 30.0);
                float4 col = float4(0, 1, 0, 1) * ((uv.y * ls + pow(uv.y,2.0)) * clouds(uv));
                col = pow(col, float4(0.75, 0.75, 0.75, 0.75));
                
                return col;
            }
            ENDCG
        }
    }
}
