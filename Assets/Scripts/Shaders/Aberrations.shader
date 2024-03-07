Shader "Hidden/Aberrations"
{
    Properties
    {
        Perfect_World_Render ("Perfect World Render", 2D) = "white" {}
        Real_World_Render ("Real World Render", 2D) = "white" {}
        aberration_threshold ("Aberration Threshold", float) = 0.5
        blinking ("Blinking", float) = 0.0
        eyes_closed ("Eyes Closed", float) = 0.0
        timer ("Random Float", float) = 0.0
        
        // Inpector Properties
        flackern_klein ("Flackern klein", float) = 20
        flackern_gross ("Flackern gross", float) = 1
        flackern_menge ("Flackern Multiplikator", float) = 0.5
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
            float timer;

            float flackern_klein;
            float flackern_gross;
            float flackern_menge;

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
                
                fixed4 perfect_sample = tex2D(Perfect_World_Render, i.uv);
                fixed4 real_sample = tex2D(Real_World_Render, i.uv);

                // Using relative luminance as some sort of measure to compare the two renders
                float luminance_r = calculate_luminance(real_sample.xyz);
                float luminance_p = calculate_luminance(perfect_sample.xyz);

                float luminance_diff = abs(luminance_p-luminance_r);

                // Flackern
                float flackern = ((sin(timer*flackern_gross)+1)+((sin(timer*flackern_klein)+1)/10))/2.2f;

                // Tapping the sample to the left for blue addition, sample to the right for red addition

                float lum_right = calculate_luminance(tex2D(Real_World_Render, i.uv + float2(1.0f/200.0f,0)).xyz);
                float lum_left = calculate_luminance(tex2D(Real_World_Render, i.uv - float2(1.0f/200.0f,0)).xyz);

                fixed3 aberration_addition = (luminance_diff>aberration_threshold) * fixed3(
                    abs(lum_right - lum_left) * lum_left,
                    0,
                    abs(lum_left - lum_right) * lum_right
                    ) * flackern;

                fixed3 real_sample_with_aberrations = real_sample + flackern_menge * aberration_addition;

                
                
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
