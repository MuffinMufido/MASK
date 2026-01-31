Shader "Custom/EndPortal"
{
    Properties
    {
        _DitherScale("Dither Scale", Range(1, 8)) = 2
        _Brightness("Brightness", Range(-1, 1)) = 0.1
        _LightColor("Light Color", Color) = (1, 1, 1, 1)
        _DarkColor("Dark Color", Color) = (0, 0, 0, 0)
        _SpinSpeed("Spin Speed", Range(0.1, 10)) = 2
        _SwirlStrength("Swirl Strength", Range(0, 10)) = 3
        _Rings("Ring Count", Range(1, 20)) = 8
        _RingSpeed("Ring Pulse Speed", Range(0, 5)) = 1.5
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

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _DitherScale;
            float _Brightness;
            half4 _LightColor;
            half4 _DarkColor;
            float _SpinSpeed;
            float _SwirlStrength;
            float _Rings;
            float _RingSpeed;

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
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Center UV so (0,0) is the middle of the surface
                float2 centeredUV = IN.uv - 0.5;

                // Polar coordinates
                float dist = length(centeredUV);
                float angle = atan2(centeredUV.y, centeredUV.x);

                // Spinning rotation over time
                float spin = angle + _Time.y * _SpinSpeed;

                // Swirl: twist angle based on distance from center
                spin += dist * _SwirlStrength;

                // Animated rings that pulse inward
                float rings = sin(dist * _Rings * 6.2832 - _Time.y * _RingSpeed * 6.2832);

                // Spiral arms pattern
                float spiral = sin(spin * 3.0);

                // Combine patterns into brightness
                float pattern = (spiral * 0.5 + 0.5) * (rings * 0.5 + 0.5);

                // Brighter toward center, darker at edges
                float centerGlow = 1.0 - saturate(dist * 1.5);
                float brightness = saturate(pattern * centerGlow + _Brightness);

                // Ordered dithering
                float threshold = GetBayerValue(IN.positionHCS.xy);
                float dithered = step(threshold, brightness);

                // 1-bit output
                half3 finalColor = lerp(_DarkColor.rgb, _LightColor.rgb, dithered);
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
