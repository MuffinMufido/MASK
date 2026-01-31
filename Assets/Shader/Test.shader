Shader "Custom/Test"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _DitherScale("Dither Scale", Range(1, 8)) = 1
        _Brightness("Brightness", Range(-1, 1)) = 0
        _LightColor("Light Color", Color) = (1, 1, 1, 1)
        _DarkColor("Dark Color", Color) = (0, 0, 0, 1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float2 uv : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float _DitherScale;
            float _Brightness;
            half4 _LightColor;
            half4 _DarkColor;

            // 8x8 Bayer ordered dithering matrix (normalized to 0-1)
            static const float bayerMatrix[64] = {
                 0.0/64.0, 32.0/64.0,  8.0/64.0, 40.0/64.0,  2.0/64.0, 34.0/64.0, 10.0/64.0, 42.0/64.0,
                48.0/64.0, 16.0/64.0, 56.0/64.0, 24.0/64.0, 50.0/64.0, 18.0/64.0, 58.0/64.0, 26.0/64.0,
                12.0/64.0, 44.0/64.0,  4.0/64.0, 36.0/64.0, 14.0/64.0, 46.0/64.0,  6.0/64.0, 38.0/64.0,
                60.0/64.0, 28.0/64.0, 52.0/64.0, 20.0/64.0, 62.0/64.0, 30.0/64.0, 54.0/64.0, 22.0/64.0,
                 3.0/64.0, 35.0/64.0, 11.0/64.0, 43.0/64.0,  1.0/64.0, 33.0/64.0,  9.0/64.0, 41.0/64.0,
                51.0/64.0, 19.0/64.0, 59.0/64.0, 27.0/64.0, 49.0/64.0, 17.0/64.0, 57.0/64.0, 25.0/64.0,
                15.0/64.0, 47.0/64.0,  7.0/64.0, 39.0/64.0, 13.0/64.0, 45.0/64.0,  5.0/64.0, 37.0/64.0,
                63.0/64.0, 31.0/64.0, 55.0/64.0, 23.0/64.0, 61.0/64.0, 29.0/64.0, 53.0/64.0, 21.0/64.0
            };

            float GetBayerValue(float2 screenPos)
            {
                int2 pixel = int2(fmod(screenPos / _DitherScale, 8.0));
                pixel = abs(pixel);
                int index = pixel.y * 8 + pixel.x;
                return bayerMatrix[index];
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 posWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(posWS);
                OUT.positionWS = posWS;
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample texture and convert to grayscale
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                float luminance = dot(texColor.rgb, float3(0.299, 0.587, 0.114));

                float3 normal = normalize(IN.normalWS);
                float3 posWS = IN.positionWS;

                // Main directional light (fixed direction, not view-dependent)
                Light mainLight = GetMainLight();
                float NdotL = saturate(dot(normal, mainLight.direction));
                float3 lighting = mainLight.color * NdotL;

                // Additional point/spot lights
                int lightCount = GetAdditionalLightsCount();
                for (int i = 0; i < lightCount; i++)
                {
                    Light light = GetAdditionalLight(i, posWS);
                    float NdotL_add = saturate(dot(normal, light.direction));
                    lighting += light.color * light.distanceAttenuation * light.shadowAttenuation * NdotL_add;
                }

                // Combine into single brightness with ambient floor
                float totalLight = saturate(dot(lighting, float3(0.299, 0.587, 0.114)) + 0.15);

                // Final brightness
                float brightness = saturate(luminance * totalLight + _Brightness);

                // Ordered dithering: compare brightness against Bayer threshold
                float threshold = GetBayerValue(IN.positionHCS.xy);
                float dithered = step(threshold, brightness);

                // 1-bit output: light or dark color
                half3 finalColor = lerp(_DarkColor.rgb, _LightColor.rgb, dithered);
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
