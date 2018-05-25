// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Orange/Chroma Key" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		[PerRendererData] _MainTex ("Main (RGBA)", 2D) = "white" {}
		_MaskTex ("Gray Mask (RGB)", 2D) = "white" {}
		_CutColor ("Cut Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_TintColor ("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Range("Cut Range (0 to 1)", Range(0,1)) = 0.1
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	fixed4 _Color;
	sampler2D _MainTex;
	sampler2D _MaskTex;
	fixed4 _CutColor;
	fixed4 _TintColor;
	half _Range;
					
	half4 _MainTex_ST;
					
	struct v2f {
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};

	v2f vert(appdata_full v) {
		v2f o;
		o.pos = UnityObjectToClipPos (v.vertex);	
		o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
		return o; 
	}
	
	fixed4 frag( v2f i ) : COLOR {
		fixed4 f = tex2D (_MainTex, i.uv.xy);

//first way
//		float gray = f.x*0.299 + f.y*0.587 + f.z*0.114;
//		float cut = _CutColor.x*0.299 + _CutColor.y*0.587 + _CutColor.z*0.114;
//		if(gray < cut+_Range && gray > cut-_Range)
//			f = _TintColor;

//second way
		if(f.r < _CutColor.r+_Range && f.r > _CutColor.r-_Range &&  f.g < _CutColor.g+_Range && f.g > _CutColor.g-_Range && f.b < _CutColor.b+_Range && f.b > _CutColor.b-_Range)
			f = _TintColor;

		fixed4 fa = tex2D (_MaskTex, i.uv.xy);
		
		if(fa.a<1)
			f.a = f.a*fa.a;
		
		f.a = f.a*_Color.a;
		f.xyz *= _Color.rgb;

		if(f.a<=0) clip(-1);
		
		return f;
	}
	ENDCG
	
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Pass {
		Cull Off
		Lighting Off
		ZWrite On
		ZTest On
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			
			ENDCG
		}
	}
	FallBack Off
}
