Shader "Hidden/Aberrations"
{
    Properties
    {
        Perfect_World_Render ("Perfect World Render", 2D) = "white" {}
        Real_World_Render ("Real World Render", 2D) = "white" {}
        aberration_threshold ("Aberration Threshold", float) = 0.5
        blinking ("Blinking", float) = 0.0
        eyes_closed ("Eyes Closed", float) = 0.0
        timer ("Runtime", float) = 0.0
        aberrations_linger ("How Long Aberrations Linger", float) = 0.0
    }
    SubShader
    {
        // No culling or depth
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D Perfect_World_Render;
            sampler2D Real_World_Render;
            float aberration_threshold;
            float aberrations_linger;
            float blinking;
            float eyes_closed;
            float timer;

            //float gamma_correct(float channel) { // I am doing this because wikipedia said so, I don't even know if the input is gamma compressed (or what that means)
            //    return pow(channel, 2.2);
            //}

            //float calculate_luminance(float3 col) {
            //    return 0.2126f * gamma_correct(col.x) + 0.7152 * gamma_correct(col.y) + 0.0722 * gamma_correct(col.z);
            //}

            bool unteres_lid(float u, float v) {
                return (floor(v*82) / 82) < -sin(u*0.5*3.14+0.25*3.14)+1;
            }

            bool oberes_lid(float u, float v) {
                return (floor(v*82) / 82) > sin(u*0.5*3.14+0.25*3.14);
            }

            bool differnt(fixed3 col1, fixed3 col2) {
                return 
                    abs(col1.x - col2.x) > aberration_threshold || 
                    abs(col1.y - col2.y) > aberration_threshold || 
                    abs(col1.z - col2.z) > aberration_threshold;
            }

            float grosses_ease(float x) {
                return 0.4 * (cos(x * 3.14159) + 1);
            }

            float kleines_flackern(float x) {
                float amplitude = 0.005;
                return sin(80*pow(x,0.3)*3.14159) * amplitude * (1.0-x);
            }

            float flackern(float x) {
                return grosses_ease(x) - kleines_flackern(x);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed4 perfect_sample = tex2D(Perfect_World_Render, i.uv);
                fixed4 real_sample = tex2D(Real_World_Render, i.uv);

                // Tap nearby texture to get vibration effect
                // fixed4 offset_sample = tex2D(Real_World_Render, float2(i.uv.x + kleines_flackern(1-aberrations_linger), i.uv.y)); Works, could be better

                float kleines_f = kleines_flackern(1-aberrations_linger);

                fixed4 offset_sample = (1 - aberrations_linger * 0.5) * tex2D(Real_World_Render, float2(i.uv.x + kleines_f, i.uv.y)) + 
                                       aberrations_linger * 0.5 * tex2D(Perfect_World_Render, float2(i.uv.x + kleines_f, i.uv.y));
                
                if (kleines_f < 0) {
                    offset_sample.xy *= 0.5 * (1-aberrations_linger) + (kleines_f+0.5);
                } else if (kleines_f > 0) {
                    offset_sample.yz *= 0.5 * (1-aberrations_linger) + (kleines_f+0.5);
                }

                bool diff = differnt(perfect_sample.xyz, real_sample.xyz); // Determines wheter or not the pixel is affected by an aberration

                fixed3 real_sample_with_aberrations = (1-diff) * real_sample + diff*(offset_sample);

                
                // Calculating a gradient in shape of an eye by bashing two sine waves together
                float gradient = blinking != 0 && (oberes_lid(i.uv.x, i.uv.y+blinking) || unteres_lid(i.uv.x, i.uv.y-blinking));

                // Blue Filter For Perfect World
                perfect_sample.xy *= 0.8;

                fixed3 col = real_sample_with_aberrations.xyz*(1-gradient) + (gradient) * perfect_sample.xyz;

                col -= gradient*fixed3(1,1,1)*(1-eyes_closed);

                return fixed4(col, 1);
            }
            ENDCG
        }
    }
}
