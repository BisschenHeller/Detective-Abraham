Shader "Hidden/Aberrations"
{
    Properties
    {
        Perfect_World_Render ("Perfect World Render", 2D) = "white" {}
        Real_World_Render ("Real World Render", 2D) = "white" {}
        aberration_threshold ("Aberration Threshold", float) = 0.5
        blinking ("Blinking", float) = 0.0
        eyes_closed ("Eyes Closed", float) = 0.0
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
            float blinking;
            float eyes_closed;

            float gamma_correct(float channel) { // I am doing this because wikipedia said so, I don't even know if the input is gamma compressed
                return pow(channel, 2.2);
            }

            float calculate_luminance(float3 col) {
                return 0.2126f * gamma_correct(col.x) + 0.7152 * gamma_correct(col.y) + 0.0722 * gamma_correct(col.z);
            }

            bool unteres_lid(float u, float v) {
                return (floor(v*82) / 82) < -sin(u*0.5*3.14+0.25*3.14)+1;
            }

            bool oberes_lid(float u, float v) {
                return (floor(v*82) / 82) > sin(u*0.5*3.14+0.25*3.14);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Using relative luminance as some sort of measure to compare the two renders
                fixed4 perfect_sample = tex2D(Perfect_World_Render, i.uv);
                fixed4 real_sample = tex2D(Real_World_Render, i.uv);

                fixed4 real_sample_with_aberrations = real_sample; // TODO !! Infuse the real sample with aberrations

                // Calculating a gradient in shape of an eye by bashing two sine waves together
                float gradient = blinking != 0 && (oberes_lid(i.uv.x, i.uv.y+blinking) || unteres_lid(i.uv.x, i.uv.y-blinking));

                fixed3 col = real_sample_with_aberrations.xyz*(1-gradient) + (gradient) * perfect_sample.xyz;

                col -= gradient*fixed3(1,1,1)*(1-eyes_closed);

                return fixed4(col, 1);
            }
            ENDCG
        }
    }
}
