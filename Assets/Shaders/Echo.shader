Shader "ECO" {
	Properties{
		_MainTex("Main Texture",2D) = "white"{}
		_Color("Color",Color) = (1.0,1.0,1.0,1.0)
		_Cutoff ("Alpha Cutoff",Range(0.0,1.0)) = 0.5
		_Epicenter ("Epicenter", Vector) = (0.0,0.0,0.0)
		_Radius ("Radius", Float) = 0
		_MaxRadius ("Max Radius",Float) = 10
		
	}
	SubShader{
		Tags{"Queue" = "Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Pass{
			Cull Off
			zWrite Off	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			uniform sampler2D _MainTex;
			uniform fixed4 _MainTex_ST;
			
			uniform fixed4 _Color;
			uniform half4 _Epicenter;
			uniform half _Radius;
			uniform half _MaxRadius;
			uniform fixed _Cutoff;
			
			
			struct vertexInput{
				fixed4 vertex : POSITION;
				fixed3 normal : NORMAL;
				fixed4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput{
				fixed4 pos : SV_POSITION; 
				fixed4 tex : TEXCOORD0;
				fixed3 normalDir : TEXCOORD1;
				fixed4 posWorld : TEXCOORD2;
			};
			
			vertexOutput vert(vertexInput v){
				vertexOutput o;
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				o.posWorld = mul(_Object2World,v.vertex); 
				o.tex = v.texcoord;
				return o;
			} 
			
			fixed4 frag(vertexOutput o) : Color{
				fixed4 tex =  tex2D(_MainTex, o.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw) * _Color;
				half dist = distance (_Epicenter.xyz , o.posWorld.xyz);
				
				if(tex.a < _Cutoff || dist >= _Radius)
				{
					tex.a = 0;
				}
				else
				{
					tex.a = lerp(0,0.5,abs(dist - _Radius)/_MaxRadius);
	
					if(abs(dist - _Radius) < 0.1)
						tex.a = lerp(1,0,dist/_MaxRadius);
				}
				
				return fixed4(tex.rgb,tex.a * _Color.a);
			}
			
			ENDCG
			
		}
	}
	//FallBack "Diffuse"
}
