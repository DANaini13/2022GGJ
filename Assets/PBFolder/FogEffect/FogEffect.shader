Shader "Hidden/FogEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
    }
    
    CGINCLUDE
    #include "UnityCG.cginc"
    #include "LayerBlending.cginc"

    sampler2D _MainTex;
    float4 _MainTex_TexelSize;
    sampler2D _CameraDepthTexture;
    
    sampler2D _GradientTex;
    float4 _GradientTex_TexelSize;

    sampler2D _CurveTex;

    //Distance
    int _useDistanceFog;
    int _fogMode;
    float _FogNear;
    float _FogFar;
    float _distanceDensity;

    //Height
    int _useHeightFog;
    float _fogLow;
    float _fogHeight;
    float _heightDensity;

    //Circle
    int _useCircleFog;
    float4 _circlePosition;
    float _circleInside;
    float _circleOutside;
    float _circleDensity;

    //Noise
    float _noiseDisturb;
    float _dpi;
    float _noise;
    
    // for fast world space reconstruction
    uniform float4x4 _FrustumCorners;

    //Fade
    sampler2D _fadeTex;
    float _fadePow;
    
    struct appdata
    {
        float4 vertex: POSITION;
        half2 texcoord: TEXCOORD0;
    };
    
    struct v2f
    {
        float4 pos: SV_POSITION;
        float2 uv: TEXCOORD0;
        float3 ray: TEXCOORD1;
    };
    
    v2f vert(appdata v)
    {
        v2f o;
        v.vertex.z = 0.1;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = v.texcoord.xy;
        
        #if UNITY_UV_STARTS_AT_TOP
            if (_MainTex_TexelSize.y < 0)
                o.uv.y = 1 - o.uv.y;
        #endif
        
        int frustumIndex = v.texcoord.x + (2 * o.uv.y);
        o.ray = _FrustumCorners[frustumIndex].xyz;
        return o;
    }
    
    
    
    float CalculateDistance(float depth01, float3 wsDir)
    {
        float dis = 0;
        if(_fogMode == 1)
        {
            dis = length(wsDir.xyz);
        }
        
        if(_fogMode == 2)
        {
            dis = depth01 * _ProjectionParams.z;
        }
        
        float g = 0;
        g += ((dis - _FogNear) / (_FogFar - _FogNear)) ;
        g *= _distanceDensity;
        return g;
    }
    
    float CalculateHeight(float3 worldDis)
    {
        float3 worldPos = _WorldSpaceCameraPos + worldDis;
        float g = 0;
        float h = worldPos.y;
        g += ((h - _fogLow) / (_fogHeight - _fogLow));
        g *= _heightDensity;
        return g;
    }


    float CalculateCircle(float3 worldDis)
    {
        float3 worldPos = _WorldSpaceCameraPos + worldDis;
        float d = distance(_circlePosition.xyz, worldPos.xyz);
        float g = 0;
        g += ((d - _circleInside) / (_circleOutside - _circleInside)) ;
        g *= _circleDensity;
        return g;
    }
    
    float CalculateFogCurve(float fac)
    {
        return tex2D(_CurveTex, float2(fac, 0.5)).a;
    }

    
    float NoiseProli(float2 uv)
    {
        float niose = frac(sin(dot(uv, float2(12.9898, 78.233))) * 178.54534 * _dpi + (_Time.y * _noiseDisturb / 1000)) ;
        return niose;
    }

    half4 CalculateFog(v2f i): SV_Target
    {
        float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
        float dpth01 = Linear01Depth(rawDepth);
        float eyeDepth = LinearEyeDepth(rawDepth);
        float3 worldDis = i.ray * dpth01;
        
        float g = 0;
        if(_useDistanceFog == 1)  g += CalculateDistance(dpth01, worldDis);
        if(_useHeightFog == 1)  g += CalculateHeight(worldDis);
        if(_useCircleFog == 1)  g += CalculateCircle(worldDis);
        float sg = saturate(g);
        float fogFactor = CalculateFogCurve(sg);
        //niose
        fogFactor += ((NoiseProli(i.uv) * 2) - 1) * _noise;
        fogFactor = saturate(fogFactor);
        float4 fogColor = tex2D(_GradientTex, float2(fogFactor, 0.5));
        //lerp
        float4 sceneColor = tex2D(_MainTex, i.uv);
        float4 final;

        float fade = tex2D(_fadeTex, float2(g / _fadePow, 0.5)).a;
        //fade = 1;
        final = LayerScreen(sceneColor * fade, fogColor);
        return final;
    }
    

    float4 frag(v2f i): SV_Target
    {
        return CalculateFog(i);
    }
    ENDCG
    
    SubShader
    {
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
            
        }
    }
}
