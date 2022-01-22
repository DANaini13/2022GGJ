//a在下，b在上


//变暗
inline float4 LayerDarken(float4 a, float4 b)
{
    return b > a?a: b;
}

//正片叠底
inline float4 LayerMultiply(float4 a, float4 b)
{
    return a * b;
}
//线性加深
inline float4 LayerColorBurn(float4 a, float4 b)
{
    return b == 0?b: max(0, 1 - ((1 - a) / b));
}

inline float4 LayerSubtract(float4 a, float4 b)
{
    return a + b < 1?0: a + b - 1;
}
//变亮
inline float4 LayerLighten(float4 a, float4 b)
{
    return b > a?b: a;
}
//滤色
inline float4 LayerScreen(float4 a, float4 b)
{
    return 1 - ((1 - a) * (1 - b));
}
//颜色减淡
inline float4 LayerColorDodge(float4 a, float4 b)
{
    return b == 1?b: min(1, a / (1 - b));
}
//线性减淡
inline float4 LayerAdd(float4 a, float4 b)
{
    return min(1, (a + b));
}

inline float4 LayerOverlay(float4 a, float4 b)
{
    return(b < 0.5)?(2 * a * b): (1 - 2 * (1 - a) * (1 - b));
}
//柔光
inline float4 LayerSoftLight(float4 a, float4 b)
{
    return(b < 0.5)?
    (2 * (a / 2) + 0.25) * b:
    (1 - 2 * (1 - (a / 2 + 0.25)) * (1 - b));
}
//强光
inline float4 LayerHardLight(float4 a, float4 b)
{
    return(a < 0.5)?
    2 * a * b:
    (1 - 2 * (1 - a) * (1 - b));
}

inline float4 LayerVividLight(float4 a, float4 b)
{
    return(b < 0.5)?
    LayerColorBurn(a, 2 * b):
    LayerColorDodge(a, 2 * (b - 0.5));
}

inline float4 LayerLinearLight(float4 a, float4 b)
{
    return min(1, max(0, (b + 2 * a) - 1));
}

inline float4 LayerPinLight(float4 a, float4 b)
{
    return max(0, max(2 * a - 1, min(b, 2 * a)));
}

inline float4 LayerHardMix(float4 a, float4 b)
{
    return LayerVividLight(a, b) < 0.5?0: 1;
}

inline float4 LayerDifference(float4 a, float4 b)
{
    return abs(a - b);
}

inline float4 LayerExclusion(float4 a, float4 b)
{
    return a + b - 2 * a * b;
}

inline float4 LayerDivide(float4 a, float4 b)
{
    return b > 0.5?a * 2: a * 0.5;
}

//RGB to HSV
float3 RGBConvertToHSV(float3 rgb)
{
    float R = rgb.x, G = rgb.y, B = rgb.z;
    float3 hsv;
    float max1 = max(R, max(G, B));
    float min1 = min(R, min(G, B));
    if (R == max1)
    {
        hsv.x = (G - B) / (max1 - min1);
    }
    if(G == max1)
    {
        hsv.x = 2 + (B - R) / (max1 - min1);
    }
    if(B == max1)
    {
        hsv.x = 4 + (R - G) / (max1 - min1);
    }
    hsv.x = hsv.x * 60.0;
    if(hsv.x < 0)
        hsv.x = hsv.x + 360;
    hsv.z = max1;
    hsv.y = (max1 - min1) / max1;
    return hsv;
}

//HSV to RGB
float3 HSVConvertToRGB(float3 hsv)
{
    float R, G, B;
    //float3 rgb;
    if (hsv.y == 0)
    {
        R = G = B = hsv.z;
    }
    else
    {
        hsv.x = hsv.x / 60.0;
        int i = (int)hsv.x;
        float f = hsv.x - (float)i;
        float a = hsv.z * (1 - hsv.y);
        float b = hsv.z * (1 - hsv.y * f);
        float c = hsv.z * (1 - hsv.y * (1 - f));
        switch(i)
        {
            case 0: R = hsv.z; G = c; B = a;
            break;
            case 1: R = b; G = hsv.z; B = a;
            break;
            case 2: R = a; G = hsv.z; B = c;
            break;
            case 3: R = a; G = b; B = hsv.z;
            break;
            case 4: R = c; G = a; B = hsv.z;
            break;
            default: R = hsv.z; G = a; B = b;
            break;
        }
    }
    return float3(R, G, B);
}