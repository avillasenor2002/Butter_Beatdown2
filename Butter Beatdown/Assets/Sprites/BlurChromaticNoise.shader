Shader "Custom/Fullscreen/BlurChromaticNoise"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}

        _BlurStrength("Blur Strength", Range(0, 5)) = 1
        _ChromaticOffset("Chromatic Offset", Range(0, 0.02)) = 0.005
        _NoiseStrength("Noise Strength", Range(0, 0.2)) = 0.05
        _NoiseSpeed("Noise Speed", Range(0, 5)) = 1

        _OldTint("Old Film Tint", Color) = (0.9, 0.82, 0.65, 1)
        _OldTintStrength("Old Tint Strength", Range(0,1)) = 0.5

            // VIGNETTE
            _VignetteStrength("Vignette Strength", Range(0,1)) = 0.4
            _VignetteSoftness("Vignette Softness", Range(0.1,1)) = 0.5

            // SCRATCHES
            _ScratchStrength("Scratch Strength", Range(0,0.5)) = 0.15
            _ScratchSpeed("Scratch Speed", Range(0,5)) = 1

            // FRAME JITTER
            _JitterStrength("Frame Jitter Strength", Range(0,0.01)) = 0.002
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            Cull Off ZWrite Off ZTest Always

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float4 _MainTex_TexelSize;

                float _BlurStrength;
                float _ChromaticOffset;
                float _NoiseStrength;
                float _NoiseSpeed;

                fixed4 _OldTint;
                float _OldTintStrength;

                float _VignetteStrength;
                float _VignetteSoftness;

                float _ScratchStrength;
                float _ScratchSpeed;

                float _JitterStrength;

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

                float rand(float2 co)
                {
                    return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
                }

                v2f vert(appdata v)
                {
                    v2f o;

                    // FRAME JITTER (horizontal micro-shift)
                    float jitter = (rand(float2(_Time.y, v.vertex.y)) - 0.5) * _JitterStrength;
                    v.vertex.x += jitter;

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 uv = i.uv;
                    float2 blurOffset = _MainTex_TexelSize.xy * _BlurStrength;

                    // --- BLUR ---
                    fixed4 blur =
                        tex2D(_MainTex, uv) * 0.4 +
                        tex2D(_MainTex, uv + blurOffset) * 0.15 +
                        tex2D(_MainTex, uv - blurOffset) * 0.15 +
                        tex2D(_MainTex, uv + blurOffset * float2(1, -1)) * 0.15 +
                        tex2D(_MainTex, uv + blurOffset * float2(-1, 1)) * 0.15;

                    // --- CHROMATIC ABERRATION ---
                    float2 chroma = blurOffset * _ChromaticOffset;
                    fixed r = tex2D(_MainTex, uv + chroma).r;
                    fixed g = tex2D(_MainTex, uv).g;
                    fixed b = tex2D(_MainTex, uv - chroma).b;
                    fixed4 chromatic = fixed4(r, g, b, 1);

                    // --- NOISE ---
                    float noise = rand(uv * _ScreenParams.xy + _Time.y * _NoiseSpeed);
                    noise = (noise - 0.5) * _NoiseStrength;

                    fixed4 finalColor = lerp(blur, chromatic, 0.6);
                    finalColor.rgb += noise;

                    // --- SCRATCHES (vertical film lines) ---
                    float scratch = rand(float2(uv.x * 200, _Time.y * _ScratchSpeed));
                    scratch = step(0.98, scratch) * _ScratchStrength;
                    finalColor.rgb -= scratch;

                    // --- VIGNETTE ---
                    float2 dist = uv - 0.5;
                    float vignette = 1.0 - smoothstep(_VignetteSoftness, 0.8, length(dist));
                    finalColor.rgb *= lerp(1, vignette, _VignetteStrength);

                    // --- OLD FILM TINT ---
                    fixed3 tinted = finalColor.rgb * _OldTint.rgb;
                    finalColor.rgb = lerp(finalColor.rgb, tinted, _OldTintStrength);

                    return finalColor;
                }
                ENDCG
            }
        }
}
