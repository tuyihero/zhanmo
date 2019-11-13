// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TYImage/NumShader" {
   Properties {
      _MainTex ("RGBA Texture Image", 2D) = "white" {} 
	  _Color("Color", Color) = (.2,.2,.2,0)
   }
   SubShader {
      Tags {"Queue" = "Transparent"  "RenderType"="TransparentCutout"}  
 
         
      Pass {	
         Cull off 
         ZWrite off 
         Blend SrcAlpha OneMinusSrcAlpha 
         // blend based on the fragment's alpha value
 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         uniform sampler2D _MainTex;    
         uniform half4		_Color;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float4 texcoord : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            output.tex = input.texcoord;
            output.pos = UnityObjectToClipPos(input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
         	float4 clr  = tex2D(_MainTex,  input.tex.xy) ;
         	
         	float4 clr2 = clr * _Color;

            return clr2;//  + float4(UNITY_LIGHTMODEL_AMBIENT.xyz, 0))       ;
         }
 
         ENDCG
      }
   }
   // The definition of a fallback shader should be commented out 
   // during development:
   Fallback "Mobile/Diffuse"
}