//This hatching function applies the hatch textures onto the object based on the UV and the camera distance

void Hatching_float(float2 _uv, float _intensity, float _dist, UnityTexture2D _Hatch0, UnityTexture2D _Hatch1, UnitySamplerState ss, out float3 hatching)
{
    // Define shadow threshold (adjust as needed)
    float shadowThreshold = 0.5; // Values below this are shadows
    
    // Compute hatching as usual
    float log2_dist = log2(_dist);
    float2 floored_log_dist = floor((log2_dist + float2(0.0, 1.0)) * 0.5) * 2.0 - float2(0.0, 1.0);				
    float2 uv_scale = min(1, pow(2.0, floored_log_dist));
    float uv_blend = abs(frac(log2_dist * 0.5) * 2.0 - 1.0);

    float2 scaledUVA = _uv / uv_scale.x;
    float2 scaledUVB = _uv / uv_scale.y;

    float3 hatch0A = SAMPLE_TEXTURE2D(_Hatch0, ss, scaledUVA).rgb;
    float3 hatch1A = SAMPLE_TEXTURE2D(_Hatch1, ss, scaledUVA).rgb;
    float3 hatch0B = SAMPLE_TEXTURE2D(_Hatch0, ss, scaledUVB).rgb;
    float3 hatch1B = SAMPLE_TEXTURE2D(_Hatch1, ss, scaledUVB).rgb;

    float3 hatch0 = lerp(hatch0A, hatch0B, uv_blend);
    float3 hatch1 = lerp(hatch1A, hatch1B, uv_blend);

    float3 overbright = max(0, _intensity - 1.0);

    float3 weightsA = saturate((_intensity * 6.0) + float3(-0, -1, -2));
    float3 weightsB = saturate((_intensity * 6.0) + float3(-3, -4, -5));

    weightsA.xy -= weightsA.yz;
    weightsA.z -= weightsB.x;
    weightsB.xy -= weightsB.yz;

    hatch0 = hatch0 * weightsA;
    hatch1 = hatch1 * weightsB;

    // Final hatching computation
    hatching = overbright + hatch0.r + hatch0.g + hatch0.b + hatch1.r + hatch1.g + hatch1.b;

    // Only apply hatching to shadows (lerp with white or original color)
    float shadowMask = 1.0 - smoothstep(shadowThreshold - 0.1, shadowThreshold + 0.1, _intensity);
    hatching = lerp(float3(1, 1, 1), hatching, shadowMask); // Blend with white (or replace with baseColor)
}