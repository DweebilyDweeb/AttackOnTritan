Shader "Custom/Cel Surface Shader" {

	Properties {
		//_varName("Label", type) = defaultValue;
		//Types: 2D, Cube, Color, Range, Float, Vector
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_UnlitColor("Unlit Color", Color) = (0,0,0,1)
		_UnlitVolume("Unlit Volume", Range(0, 1)) = 0.2
		_Brightness("Brightness", Float) = 1
	}

	SubShader {
		//Tells Unity what we're about to do. In this case we're about to render an opaque texture.
		Tags {
			"RenderType" = "Opaque"
		}

		//Some Level-of-Detail stuff. Not sure how it works.
		LOD 200

		//Tell Unity we are about to use the CGProgramming language to do stuff.
		CGPROGRAM

		//Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf CelShadingForward
		//Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			//There's a built in list of possible variables. Use to link and scroll near the bottom of the page.
			//https://docs.unity3d.com/Manual/SL-SurfaceShaders.html
			//The input structure Input generally has any texture coordinates needed by the shader.
			//Texture coordinates must be named “uv” followed by texture name (or start it with “uv2” to use second texture coordinate set).
			float2 uv_MainTex; //Grab the uv of MainTex and store it here in the first uv thingy.
			float2 uv2_MainTex; //Grab the 2nd uv of MainTex and store it here in the first uv thingy.

			float4 color : COLOR;
			INTERNAL_DATA float3 viewDir;
			INTERNAL_DATA float3 worldNormal;
		};

		//Define our properties.
		sampler2D _MainTex; //Our texture.
		fixed4 _Color;
		fixed4 _UnlitColor;
		float _UnlitVolume;
		float _Brightness;

		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten) {
			half lightingLevels = 3;
			half NdotL = dot(s.Normal, lightDir); //How much light is shining on the surface.
			half brightness = (floor(NdotL * lightingLevels) / lightingLevels);

			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (brightness * atten * 2) * _Brightness;
			c.a = s.Alpha;
			return c;
		}

		//This is our output.
		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;						

			if (dot(IN.viewDir , o.Normal) < _UnlitVolume) {
				o.Albedo = _UnlitColor;
			} else {
				o.Albedo = c.rgb * _Color;
			}

			o.Alpha = c.a;
		}
		ENDCG
	}

	FallBack "Diffuse" //What shader to use if the above subshader(s) doesn't work.

}