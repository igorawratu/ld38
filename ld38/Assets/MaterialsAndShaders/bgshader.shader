﻿Shader "bg"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

	Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

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

		sampler2D _MainTex;
		float4 _MainTex_ST;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			float xLine = (sin(_Time.y / 2) + 1) / 2;
			float yLine = (sin(_Time.x) + 1) / 2;

			if (abs(i.uv.x - xLine) < 0.0025 || abs(i.uv.y - yLine) < 0.0025) {
				return fixed4(0, 1, 0, 1);
			}

			return fixed4(0, 0, 0, 1);
		}
		ENDCG
	}
	}
}
