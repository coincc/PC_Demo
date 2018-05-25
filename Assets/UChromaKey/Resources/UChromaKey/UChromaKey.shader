Shader "Hidden/UChromaKey" 
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}	
		_Brightness("Brightness", Float) = 1
		_Saturation("Saturation", Float) = 1
		_Contrast("Contrast", Float) = 1
	}

	SubShader
	{
		Pass
		{
		
		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag		
		#pragma target 3.0
		
		#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		uniform sampler2D _UChromaKeyTex;
		fixed4 _PatCol;		
		half _Range;
		half _HueRange;
		half _uvDefX;		
		half _uvCoefX;		
		half _uvDefY;		
		half _uvCoefY;
		half _opacity;
		half _smoothing;
		half4 _Crop;
		half _Brightness;
		half _Saturation;
		half _Contrast;
		
		float4 frag(v2f_img i) : COLOR
		{
			half2 nuv = i.uv;
			nuv.x = nuv.x * _uvCoefX + _uvDefX;
			nuv.y = nuv.y * _uvCoefY + _uvDefY;

			fixed4 mc = tex2D(_MainTex, i.uv);

			fixed4 c = tex2D(_UChromaKeyTex,nuv);
			
			if (!(nuv.x > (1 - _Crop.y) || nuv.x < _Crop.x || nuv.y > (1 - _Crop.z) || nuv.y < _Crop.w))
			{										
				half hueDiff = abs(atan2(1.73205 * (c.g - c.b), 2 * c.r - c.g - c.b + 0.001) - atan2(1.73205 * (_PatCol.g - _PatCol.b), 2 * _PatCol.r - _PatCol.g - _PatCol.b + 0.001));
			
				mc.rgb = lerp(lerp(mc.rgb, c.rgb,_opacity),mc.rgb,
								saturate((1.0 - ((c.r - _PatCol.r)*(c.r - _PatCol.r) + (c.g - _PatCol.g)*(c.g - _PatCol.g) + (c.b - _PatCol.b)*(c.b - _PatCol.b)) / (_Range * _Range))*_smoothing)
								* saturate((1.0 - min(hueDiff,6.28319 - hueDiff)/(_HueRange * _HueRange))*_smoothing));
			}
			//从_MainTex中根据uv坐标进行采样  
			//fixed4 renderTex = tex2D(_UChromaKeyTex, i.uv);
			//brigtness亮度直接乘以一个系数，也就是RGB整体缩放，调整亮度  
			fixed3 finalColor = mc * _Brightness;
			//saturation饱和度：首先根据公式计算同等亮度情况下饱和度最低的值：  
			fixed gray = 0.2125 * mc.r + 0.7154 * mc.g + 0.0721 * mc.b;
			fixed3 grayColor = fixed3(gray, gray, gray);
			//根据Saturation在饱和度最低的图像和原图之间差值  
			finalColor = lerp(grayColor, finalColor, _Saturation);
			//contrast对比度：首先计算对比度最低的值  
			fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
			//根据Contrast在对比度最低的图像和原图之间差值  
			finalColor = lerp(avgColor, finalColor, _Contrast);
			//返回结果，alpha通道不变  
			mc = fixed4(finalColor, mc.a);
			return mc;
		}

		ENDCG
		} 
	}
}