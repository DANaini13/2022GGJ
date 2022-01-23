// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FloorShader"
{
	Properties
	{
		_MaintTex("MaintTex", 2D) = "white" {}
		_ColorPower("ColorPower", Float) = 0
		_MainColor("MainColor", Color) = (1,1,1,0)
		_EmissiveColor("EmissiveColor", Color) = (0,0,0,0)
		_EmissiveMap("EmissiveMap", 2D) = "black" {}
		_FeverTimeTex("FeverTimeTex", 2D) = "black" {}
		_IsUseFeverTime("IsUseFeverTime", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite On
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustomLighting keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _ColorPower;
		uniform float4 _EmissiveColor;
		uniform sampler2D _EmissiveMap;
		uniform float4 _EmissiveMap_ST;
		uniform float _IsUseFeverTime;
		uniform float FeverTimeAmount;
		uniform sampler2D _MaintTex;
		uniform float4 _MaintTex_ST;
		uniform float4 _MainColor;
		uniform sampler2D _FeverTimeTex;
		uniform float4 _FeverTimeTex_ST;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float2 uv_MaintTex = i.uv_texcoord * _MaintTex_ST.xy + _MaintTex_ST.zw;
			float ifLocalVar45 = 0;
			if( _IsUseFeverTime <= 0.0 )
				ifLocalVar45 = 0.0;
			else
				ifLocalVar45 = 1.0;
			float temp_output_50_0 = ( ifLocalVar45 * FeverTimeAmount );
			float2 uv_FeverTimeTex = i.uv_texcoord * _FeverTimeTex_ST.xy + _FeverTimeTex_ST.zw;
			c.rgb = ( ( tex2D( _MaintTex, uv_MaintTex ) * _MainColor * ( 1.0 - temp_output_50_0 ) ) + ( temp_output_50_0 * tex2D( _FeverTimeTex, uv_FeverTimeTex ) ) ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			float2 uv_EmissiveMap = i.uv_texcoord * _EmissiveMap_ST.xy + _EmissiveMap_ST.zw;
			float ifLocalVar45 = 0;
			if( _IsUseFeverTime <= 0.0 )
				ifLocalVar45 = 0.0;
			else
				ifLocalVar45 = 1.0;
			float temp_output_50_0 = ( ifLocalVar45 * FeverTimeAmount );
			o.Emission = ( _ColorPower * _EmissiveColor * tex2D( _EmissiveMap, uv_EmissiveMap ) * temp_output_50_0 ).rgb;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18912
-1920;26;1916;719;2686.558;420.0593;1.630633;True;True
Node;AmplifyShaderEditor.RangedFloatNode;46;-1981.765,-31.63336;Inherit;False;Property;_IsUseFeverTime;IsUseFeverTime;6;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-2000.094,328.179;Inherit;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-2019.094,183.179;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;45;-1680.08,27.12036;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-1609.965,372.907;Inherit;False;Global;FeverTimeAmount;FeverTimeAmount;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-1370.094,234.179;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;40;-1092.991,516.0151;Inherit;True;Property;_FeverTimeTex;FeverTimeTex;5;0;Create;True;0;0;0;False;0;False;-1;None;1cce53979989849449994f8bf4b21f42;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;44;-739.3911,313.2151;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-1185.6,-106.3;Inherit;True;Property;_MaintTex;MaintTex;0;0;Create;True;0;0;0;False;0;False;-1;None;a672515a01e32a24ebcaa19a1cda9de8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;32;-963.3765,151.027;Inherit;False;Property;_MainColor;MainColor;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;-1057.684,-618.9918;Inherit;False;Property;_ColorPower;ColorPower;1;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;33;-1154.684,-533.9919;Inherit;False;Property;_EmissiveColor;EmissiveColor;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0.9972116,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-570.391,386.0152;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-627.0999,128.1;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;39;-1098.575,-393.3767;Inherit;True;Property;_EmissiveMap;EmissiveMap;4;0;Create;True;0;0;0;False;0;False;-1;None;50348effc355f0d4a9ee35895bfdf78d;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-337.691,309.3152;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-74,378.5;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-592.2606,-347.044;Inherit;False;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;29;2,-1;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;FloorShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;1;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;45;0;46;0
WireConnection;45;2;49;0
WireConnection;45;3;48;0
WireConnection;45;4;48;0
WireConnection;50;0;45;0
WireConnection;50;1;41;0
WireConnection;44;0;50;0
WireConnection;43;0;50;0
WireConnection;43;1;40;0
WireConnection;31;0;8;0
WireConnection;31;1;32;0
WireConnection;31;2;44;0
WireConnection;42;0;31;0
WireConnection;42;1;43;0
WireConnection;36;0;30;0
WireConnection;36;1;33;0
WireConnection;36;2;39;0
WireConnection;36;3;50;0
WireConnection;29;2;36;0
WireConnection;29;13;42;0
ASEEND*/
//CHKSM=722E2D9C958F6ACA9B25037A7F44372E00EA8D66