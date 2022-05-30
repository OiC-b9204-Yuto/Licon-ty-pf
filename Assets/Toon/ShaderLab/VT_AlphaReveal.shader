Shader "ShaderLab/VT_AlphaReveal"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB) & Alpha ", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		
		_DissolveTexture("Dissolve texture", 2D) = "white" {}
		_Radius("Floppa", Range(-1,0)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue" = "Transparent" }
        LOD 200
		Cull off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
	
        sampler2D _MainTex;
		sampler2D _DissolveTexture;

        half _Glossiness;
        half _Metallic;
        float4 _Color;
		
		float _Radius;

		struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			half dissolve_value = tex2D(_DissolveTexture, IN.uv_MainTex).x;
			
            // Albedo comes from a texture tinted by color
            half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			clip(c.a - (dissolve_value - _Radius));
            o.Alpha = c.a * tex2D(_DissolveTexture, IN.uv_MainTex).r;
        }
        ENDCG
    }
    FallBack "Transparent/VertexLit"
}
