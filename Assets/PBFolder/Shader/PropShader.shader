// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PropShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_CustomColor("CustomColor", Color) = (0,0,0,0)
		_DisolveGuide("Disolve Guide", 2D) = "white" {}
		_BurnRamp("Burn Ramp", 2D) = "black" {}
		_RampIntensity("RampIntensity", Float) = 1
		_DissolveAmount("DissolveAmount", Float) = 0
		_ColorPower("ColorPower", Float) = 0
		_PropTex("PropTex", 2D) = "white" {}
		_EmissiveMap("EmissiveMap", 2D) = "black" {}
		_EmissivePower("EmissivePower", Range( 0 , 5)) = 1
		_BottomColor("BottomColor", Color) = (0,0,0,0)
		_Frequency("Frequency", Range( 0 , 10)) = 0
		_ShakeIntensity("ShakeIntensity", Float) = 0
		_LerpColor_FuckYiOuMax("LerpColor_FuckYiOuMax", Float) = 1
		_LerpColor_FuckYiOuMin("LerpColor_FuckYiOuMin", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustomLighting keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
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

		uniform float _ShakeIntensity;
		uniform float _Frequency;
		uniform float _EmissivePower;
		uniform sampler2D _EmissiveMap;
		uniform float4 _EmissiveMap_ST;
		uniform float _RampIntensity;
		uniform float _DissolveAmount;
		uniform sampler2D _DisolveGuide;
		uniform float4 _DisolveGuide_ST;
		uniform sampler2D _BurnRamp;
		uniform float4 _CustomColor;
		uniform sampler2D _PropTex;
		uniform float4 _PropTex_ST;
		uniform float4 _BottomColor;
		uniform float _LerpColor_FuckYiOuMin;
		uniform float _LerpColor_FuckYiOuMax;
		uniform float _ColorPower;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			v.vertex.xyz += ( float3(1,0,0) * _ShakeIntensity * v.texcoord.xy.y * sin( ( ( ase_vertex3Pos.x + 0.0 + ase_vertex3Pos.y ) + ( _SinTime.w * _Frequency ) ) ) );
			v.vertex.w = 1;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float temp_output_2_4 = _CustomColor.a;
			float2 uv_PropTex = i.uv_texcoord * _PropTex_ST.xy + _PropTex_ST.zw;
			float4 tex2DNode17 = tex2D( _PropTex, uv_PropTex );
			float2 uv_DisolveGuide = i.uv_texcoord * _DisolveGuide_ST.xy + _DisolveGuide_ST.zw;
			float temp_output_22_0 = ( (-0.6 + (( 1.0 - _DissolveAmount ) - 0.0) * (0.6 - -0.6) / (1.0 - 0.0)) + tex2D( _DisolveGuide, uv_DisolveGuide ).r + ( 1.0 - i.uv_texcoord.x ) );
			float clampResult96 = clamp( (_LerpColor_FuckYiOuMin + (( 1.0 - i.uv_texcoord.y ) - 0.0) * (_LerpColor_FuckYiOuMax - _LerpColor_FuckYiOuMin) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float4 lerpResult79 = lerp( _CustomColor , _BottomColor , clampResult96);
			float clampResult24 = clamp( (-15.0 + (temp_output_22_0 - 0.0) * (15.0 - -15.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float temp_output_25_0 = ( 1.0 - clampResult24 );
			float2 appendResult26 = (float2(temp_output_25_0 , 0.0));
			float4 tex2DNode27 = tex2D( _BurnRamp, appendResult26 );
			c.rgb = ( ( tex2DNode17 * lerpResult79 * i.vertexColor * _ColorPower ) * tex2DNode27 ).rgb;
			c.a = temp_output_2_4;
			clip( ( tex2DNode17.a * temp_output_22_0 ) - _Cutoff );
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
			float4 color78 = IsGammaSpace() ? float4(1,0.9294478,0.6556604,0) : float4(1,0.8469478,0.387421,0);
			float2 uv_DisolveGuide = i.uv_texcoord * _DisolveGuide_ST.xy + _DisolveGuide_ST.zw;
			float temp_output_22_0 = ( (-0.6 + (( 1.0 - _DissolveAmount ) - 0.0) * (0.6 - -0.6) / (1.0 - 0.0)) + tex2D( _DisolveGuide, uv_DisolveGuide ).r + ( 1.0 - i.uv_texcoord.x ) );
			float clampResult24 = clamp( (-15.0 + (temp_output_22_0 - 0.0) * (15.0 - -15.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float temp_output_25_0 = ( 1.0 - clampResult24 );
			float2 appendResult26 = (float2(temp_output_25_0 , 0.0));
			float4 tex2DNode27 = tex2D( _BurnRamp, appendResult26 );
			o.Emission = ( ( _EmissivePower * tex2D( _EmissiveMap, uv_EmissiveMap ).r * color78 ) + ( _RampIntensity * ( temp_output_25_0 * tex2DNode27 ) ) ).rgb;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18912
0;258;1916;749;3466.292;25.92023;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;41;-3746.599,488.0586;Inherit;True;Property;_DissolveAmount;DissolveAmount;5;0;Create;True;0;0;0;False;0;False;0;0.54;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;18;-2975.368,526.9438;Inherit;False;908.2314;498.3652;Dissolve - Opacity Mask;2;38;22;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;-3863.322,909.5356;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;39;-3272.239,571.3273;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;42;-3419.507,965.8201;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;38;-2753.956,584.7344;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;-2925,1074.199;Inherit;True;Property;_DisolveGuide;Disolve Guide;2;0;Create;True;0;0;0;False;0;False;-1;232b8e8e327c94848a476472a204a6e4;232b8e8e327c94848a476472a204a6e4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-2277.501,635.1089;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;19;-2795.115,-14.20908;Inherit;False;814.5701;432.0292;Burn Effect - Emission;4;26;25;24;23;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemapNode;23;-2780.335,217.5888;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-15;False;4;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;24;-2699.817,27.00783;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;80;-1994.777,966.5479;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;81;-1676.804,1014.531;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-1692.439,1340.838;Inherit;False;Property;_LerpColor_FuckYiOuMax;LerpColor_FuckYiOuMax;13;0;Create;True;0;0;0;False;0;False;1;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;25;-2299.407,31.83661;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-1744.439,1247.838;Inherit;False;Property;_LerpColor_FuckYiOuMin;LerpColor_FuckYiOuMin;14;0;Create;True;0;0;0;False;0;False;0;-5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;94;-1034.921,1246.327;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;26;-2483.934,242.5167;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;95;-1468.507,1031.384;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;90;-1204.401,1556.56;Inherit;True;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;91;-1142.22,1763.41;Inherit;False;Property;_Frequency;Frequency;11;0;Create;True;0;0;0;False;0;False;0;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-639.8057,1494.365;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;68;-3238.266,-254.0891;Inherit;True;Property;_PropTex;PropTex;7;0;Create;True;0;0;0;False;0;False;None;75463e8980df32244a7b20316e2693a7;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;27;-1710.911,342.1864;Inherit;True;Property;_BurnRamp;Burn Ramp;3;0;Create;True;0;0;0;False;0;False;-1;e3562952bb787444b9dc4c4ace8ce36f;e3562952bb787444b9dc4c4ace8ce36f;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-1873.849,546.9789;Inherit;False;Property;_CustomColor;CustomColor;1;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.09803916,0.3254901,0.2862745,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;87;-725.3026,1257.926;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;96;-1166.819,1084.459;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;82;-1893.212,785.8455;Inherit;False;Property;_BottomColor;BottomColor;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.1529411,0.1803921,0.1725489,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;73;-1564.387,-539.1292;Inherit;True;Property;_EmissiveMap;EmissiveMap;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;7;-1141.391,532.331;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-1155.095,-150.2321;Inherit;True;Property;_MainTex;MainTex;4;0;Create;True;0;0;0;False;0;False;-1;None;c1526a808fb85464d91802c362d258a7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;89;-443.8762,1259.758;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;78;-1205.792,-884.3056;Inherit;False;Constant;_EmissiveColor;EmissiveColor;12;0;Create;True;0;0;0;False;0;False;1,0.9294478,0.6556604,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;74;-1625.752,-727.0576;Inherit;False;Property;_EmissivePower;EmissivePower;9;0;Create;True;0;0;0;False;0;False;1;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-840.4824,808.7612;Inherit;False;Property;_ColorPower;ColorPower;6;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;79;-1103.286,716.6263;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1930.239,4.355738;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2195.847,-222.3988;Inherit;True;Property;_RampIntensity;RampIntensity;4;0;Create;True;0;0;0;False;0;False;1;7.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;83;-240.2459,1034.322;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1695.424,-237.5816;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;85;-284.7133,496.8681;Inherit;False;Constant;_Vector0;Vector 0;11;0;Create;True;0;0;0;False;0;False;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SinOpNode;92;-216.0029,1462.843;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-344.4332,892.4541;Inherit;False;Property;_ShakeIntensity;ShakeIntensity;12;0;Create;True;0;0;0;False;0;False;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-849.1089,-435.5758;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-725.7124,564.8843;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;76;-106.9822,-558.3047;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-341.9696,-122.3137;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;299.6953,576.8795;Inherit;False;4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-316.5345,214.1749;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;86;-1242.125,1350.533;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexCoordVertexDataNode;37;-3895.839,158.8615;Inherit;True;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-146.2184,-39.68729;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;-488.5947,-232.3064;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;16;337.6722,-189;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;PropShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;AlphaTest;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;1;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;39;0;41;0
WireConnection;42;0;35;1
WireConnection;38;0;39;0
WireConnection;22;0;38;0
WireConnection;22;1;21;1
WireConnection;22;2;42;0
WireConnection;23;0;22;0
WireConnection;24;0;23;0
WireConnection;81;0;80;2
WireConnection;25;0;24;0
WireConnection;26;0;25;0
WireConnection;95;0;81;0
WireConnection;95;3;98;0
WireConnection;95;4;97;0
WireConnection;88;0;90;4
WireConnection;88;1;91;0
WireConnection;27;1;26;0
WireConnection;87;0;94;1
WireConnection;87;2;94;2
WireConnection;96;0;95;0
WireConnection;17;0;68;0
WireConnection;89;0;87;0
WireConnection;89;1;88;0
WireConnection;79;0;2;0
WireConnection;79;1;82;0
WireConnection;79;2;96;0
WireConnection;31;0;25;0
WireConnection;31;1;27;0
WireConnection;34;0;30;0
WireConnection;34;1;31;0
WireConnection;92;0;89;0
WireConnection;75;0;74;0
WireConnection;75;1;73;1
WireConnection;75;2;78;0
WireConnection;5;0;17;0
WireConnection;5;1;79;0
WireConnection;5;2;7;0
WireConnection;5;3;63;0
WireConnection;8;0;17;4
WireConnection;8;1;2;4
WireConnection;47;0;85;0
WireConnection;47;1;84;0
WireConnection;47;2;83;2
WireConnection;47;3;92;0
WireConnection;43;0;5;0
WireConnection;43;1;27;0
WireConnection;40;0;17;4
WireConnection;40;1;22;0
WireConnection;77;0;75;0
WireConnection;77;1;34;0
WireConnection;16;2;77;0
WireConnection;16;9;2;4
WireConnection;16;10;40;0
WireConnection;16;13;43;0
WireConnection;16;11;47;0
ASEEND*/
//CHKSM=3A83F5A687C1F5B52F4230C2766BFC58158AF842