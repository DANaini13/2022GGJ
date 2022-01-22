// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PropShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_CustomColor("CustomColor", Color) = (0,0,0,0)
		_DisolveGuide("Disolve Guide", 2D) = "white" {}
		_BurnRamp("Burn Ramp", 2D) = "white" {}
		_RampIntensity("RampIntensity", Float) = 1
		_DissolveAmount("DissolveAmount", Float) = 0
		_ColorPower("ColorPower", Float) = 0
		_PropTex("PropTex", 2D) = "white" {}
		_EmissiveMap("EmissiveMap", 2D) = "black" {}
		_EmissivePower("EmissivePower", Range( 0 , 5)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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

		uniform float _EmissivePower;
		uniform sampler2D _EmissiveMap;
		uniform float4 _EmissiveMap_ST;
		uniform float _RampIntensity;
		uniform float _DissolveAmount;
		uniform sampler2D _DisolveGuide;
		uniform float4 _DisolveGuide_ST;
		uniform sampler2D _BurnRamp;
		uniform sampler2D _PropTex;
		uniform float4 _PropTex_ST;
		uniform float4 _CustomColor;
		uniform float _ColorPower;
		uniform float _Cutoff = 0.5;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float2 uv_PropTex = i.uv_texcoord * _PropTex_ST.xy + _PropTex_ST.zw;
			float4 tex2DNode17 = tex2D( _PropTex, uv_PropTex );
			float2 uv_DisolveGuide = i.uv_texcoord * _DisolveGuide_ST.xy + _DisolveGuide_ST.zw;
			float temp_output_22_0 = ( (-0.6 + (( 1.0 - _DissolveAmount ) - 0.0) * (0.6 - -0.6) / (1.0 - 0.0)) + tex2D( _DisolveGuide, uv_DisolveGuide ).r + ( 1.0 - i.uv_texcoord.x ) );
			float clampResult24 = clamp( (-4.0 + (temp_output_22_0 - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float temp_output_25_0 = ( 1.0 - clampResult24 );
			float2 appendResult26 = (float2(temp_output_25_0 , 0.0));
			float4 tex2DNode27 = tex2D( _BurnRamp, appendResult26 );
			c.rgb = ( ( tex2DNode17 * _CustomColor * i.vertexColor * _ColorPower ) * tex2DNode27 ).rgb;
			c.a = ( tex2DNode17.a * _CustomColor.a );
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
			float clampResult24 = clamp( (-4.0 + (temp_output_22_0 - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float temp_output_25_0 = ( 1.0 - clampResult24 );
			float2 appendResult26 = (float2(temp_output_25_0 , 0.0));
			float4 tex2DNode27 = tex2D( _BurnRamp, appendResult26 );
			o.Emission = ( ( _EmissivePower * tex2D( _EmissiveMap, uv_EmissiveMap ).r * color78 ) + ( _RampIntensity * ( temp_output_25_0 * tex2DNode27 ) ) ).rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18912
24;89;1916;761;4503.142;508.7338;2.654988;True;True
Node;AmplifyShaderEditor.RangedFloatNode;41;-3746.599,488.0586;Inherit;True;Property;_DissolveAmount;DissolveAmount;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;18;-2975.368,526.9438;Inherit;False;908.2314;498.3652;Dissolve - Opacity Mask;2;38;22;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;-3863.322,909.5356;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;39;-3272.239,571.3273;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;42;-3419.507,965.8201;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;38;-2753.956,584.7344;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.6;False;4;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;-2925,1074.199;Inherit;True;Property;_DisolveGuide;Disolve Guide;2;0;Create;True;0;0;0;False;0;False;-1;232b8e8e327c94848a476472a204a6e4;232b8e8e327c94848a476472a204a6e4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-2277.501,635.1089;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;19;-2795.115,-14.20908;Inherit;False;814.5701;432.0292;Burn Effect - Emission;4;26;25;24;23;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemapNode;23;-2780.335,217.5888;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-4;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;24;-2699.817,27.00783;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;25;-2299.407,31.83661;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;26;-2483.934,242.5167;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;68;-3238.266,-254.0891;Inherit;True;Property;_PropTex;PropTex;9;0;Create;True;0;0;0;False;0;False;None;75463e8980df32244a7b20316e2693a7;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;27;-1710.911,342.1864;Inherit;True;Property;_BurnRamp;Burn Ramp;3;0;Create;True;0;0;0;False;0;False;-1;e3562952bb787444b9dc4c4ace8ce36f;e3562952bb787444b9dc4c4ace8ce36f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;78;-1205.792,-884.3056;Inherit;False;Constant;_EmissiveColor;EmissiveColor;12;0;Create;True;0;0;0;False;0;False;1,0.9294478,0.6556604,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;74;-1625.752,-727.0576;Inherit;False;Property;_EmissivePower;EmissivePower;11;0;Create;True;0;0;0;False;0;False;1;1.13;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-942.2307,822.9316;Inherit;False;Property;_ColorPower;ColorPower;8;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1930.239,4.355738;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;73;-1564.387,-539.1292;Inherit;True;Property;_EmissiveMap;EmissiveMap;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;-2195.847,-222.3988;Inherit;True;Property;_RampIntensity;RampIntensity;4;0;Create;True;0;0;0;False;0;False;1;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-1107.096,-78.23209;Inherit;True;Property;_MainTex;MainTex;4;0;Create;True;0;0;0;False;0;False;-1;None;c1526a808fb85464d91802c362d258a7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;7;-1141.391,532.331;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-1300.643,112.0133;Inherit;False;Property;_CustomColor;CustomColor;1;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.1058199,0.5471698,0.1866955,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-849.1089,-435.5758;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1695.424,-237.5816;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-725.7124,564.8843;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;45;-259.2724,1048.132;Inherit;True;Property;_VertexNoiseTex;VertexNoiseTex;6;0;Create;True;0;0;0;False;0;False;-1;None;232b8e8e327c94848a476472a204a6e4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;37;-3895.839,158.8615;Inherit;True;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;76;-106.9822,-558.3047;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;48;-52.88379,1255.886;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;51;209.8892,1206.73;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;46;40.95922,941.4569;Inherit;False;Property;_OffseAmount;OffseAmount;7;0;Create;True;0;0;0;False;0;False;0;0.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;50;-213.5287,1351.339;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-117.4185,61.1127;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;383.9284,950.3655;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-90.76962,-106.3137;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-316.5345,214.1749;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;-488.5947,-232.3064;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;16;337.6722,-189;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;PropShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;AlphaTest;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;39;0;41;0
WireConnection;42;0;35;1
WireConnection;38;0;39;0
WireConnection;22;0;38;0
WireConnection;22;1;21;1
WireConnection;22;2;42;0
WireConnection;23;0;22;0
WireConnection;24;0;23;0
WireConnection;25;0;24;0
WireConnection;26;0;25;0
WireConnection;27;1;26;0
WireConnection;31;0;25;0
WireConnection;31;1;27;0
WireConnection;17;0;68;0
WireConnection;75;0;74;0
WireConnection;75;1;73;1
WireConnection;75;2;78;0
WireConnection;34;0;30;0
WireConnection;34;1;31;0
WireConnection;5;0;17;0
WireConnection;5;1;2;0
WireConnection;5;2;7;0
WireConnection;5;3;63;0
WireConnection;51;1;48;2
WireConnection;51;2;48;3
WireConnection;40;0;17;4
WireConnection;40;1;22;0
WireConnection;47;0;46;0
WireConnection;47;1;45;1
WireConnection;47;2;51;0
WireConnection;8;0;17;4
WireConnection;8;1;2;4
WireConnection;43;0;5;0
WireConnection;43;1;27;0
WireConnection;77;0;75;0
WireConnection;77;1;34;0
WireConnection;16;2;77;0
WireConnection;16;9;8;0
WireConnection;16;10;40;0
WireConnection;16;13;43;0
ASEEND*/
//CHKSM=FC1024D88F1541D295DCD7274CA0ADE078E2A326