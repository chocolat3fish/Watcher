
Shader "Custom/CelShade"
{
	// from https://roystan.net/articles/toon-shader.html and https://answers.unity.com/questions/60155/is-there-a-shader-to-only-add-an-outline.html
    Properties
    {
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4, 0.4, 0.4, 1)
		_Color("Color", Color) = (1,0,0,1)
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)

		_Thickness("Thickness", float) = 4
	}
	SubShader
	{

			// roystan

		Pass
		{
			Tags
			{
				"LightMode" = "ForwardBase"
				"PassFlags" = "OnlyDirectional"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag



			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;

			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;

				float3 worldNormal : NORMAL;
			};


			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				return o;
			}



			float4 _Color;

			float4 _AmbientColor;

			float4 frag(v2f i) : SV_Target
			{
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				float lightIntensity = smoothstep(0, 0.01, NdotL);
				float4 light = lightIntensity * _LightColor0;

				float4 sample = tex2D(_MainTex, i.uv);

				return _Color * sample * (_AmbientColor + light);
			}


			ENDCG
		}

		
			
		// Stencil
		Pass
		{
			Tags{ "Queue" = "Geometry" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Back
			//ZTest always
			Stencil
			{
				Ref 1
				Comp always
				Pass replace
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			float4 _Color;

			struct v2g
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				float3 viewT : TANGENT;
				float3 normals : NORMAL;
			};

			struct g2f
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				float3  viewT : TANGENT;
				float3  normals : NORMAL;
			};


			v2g vert(appdata_base v)
			{
				v2g OUT;
				OUT.pos = UnityObjectToClipPos(v.vertex);
				OUT.uv = v.texcoord;
				OUT.normals = v.normal;
				OUT.viewT = ObjSpaceViewDir(v.vertex);

				return OUT;
			}
				
			half4 frag(g2f IN) : COLOR
			{
				return _Color;
			}


					
			ENDCG
		}
				
			
			

		Pass
		{
			Stencil
			{
				Ref 0
				Comp equal
			}
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma target 4.0
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag


			half4 _OutlineColor;
			float _Thickness;

			struct v2g
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 viewT : TANGENT;
				float3 normals : NORMAL;
			};

			struct g2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 viewT : TANGENT;
				float3 normals : NORMAL;
			};

			v2g vert(appdata_base v)
			{
				v2g OUT;
				OUT.pos = UnityObjectToClipPos(v.vertex);

				OUT.uv = v.texcoord;
				OUT.normals = v.normal;
				OUT.viewT = ObjSpaceViewDir(v.vertex);

				return OUT;
			}

			void geom2(v2g start, v2g end, inout TriangleStream<g2f> triStream)
			{
				float thisWidth = _Thickness / 100;
				float4 parallel = end.pos - start.pos;
				normalize(parallel);
				parallel *= thisWidth;

				float4 perpendicular = float4(parallel.y,-parallel.x, 0, 0);
				perpendicular = normalize(perpendicular) * thisWidth;
				float4 v1 = start.pos - parallel;
				float4 v2 = end.pos + parallel;
				g2f OUT;
				OUT.pos = v1 - perpendicular;
				OUT.uv = start.uv;
				OUT.viewT = start.viewT;
				OUT.normals = start.normals;
				triStream.Append(OUT);

				OUT.pos = v1 + perpendicular;
				triStream.Append(OUT);

				OUT.pos = v2 - perpendicular;
				OUT.uv = end.uv;
				OUT.viewT = end.viewT;
				OUT.normals = end.normals;
				triStream.Append(OUT);

				OUT.pos = v2 + perpendicular;
				OUT.uv = end.uv;
				OUT.viewT = end.viewT;
				OUT.normals = end.normals;
				triStream.Append(OUT);
			}

			[maxvertexcount(12)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
			{
				geom2(IN[0],IN[1],triStream);
				geom2(IN[1],IN[2],triStream);
				geom2(IN[2],IN[0],triStream);
			}

			half4 frag(g2f IN) : COLOR
			{
				_OutlineColor.a = 1;
				return _OutlineColor;
			}

			ENDCG

		}
	}
	
	Fallback "Unlit/Color"
}
