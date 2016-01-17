Shader "ViveJam/RockShader" {
	Properties {
		_Color ("Rock Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		
		_RockTex ("Rock Texture", 3D) = "white" { }
		_GrassTex ("Grass Texture", 3D) = "white" { }

		_TexScale ("Texture Scale", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler3D _RockTex, _GrassTex;

		struct Input {
			float3 worldPos;
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		half _TexScale;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			fixed4 c = tex3D (_RockTex, IN.worldPos * _TexScale) * _Color;
			fixed4 c2 = tex3D (_GrassTex, IN.worldPos * _TexScale);

			const float minH = 55.0,
						maxH = 60.0;
			float heightLerp = (IN.worldPos.y - minH) / (maxH - minH);
			heightLerp = saturate(heightLerp);

			o.Albedo = lerp(c.rgb, c2.rgb, heightLerp);

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
