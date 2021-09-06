Shader "Unlit/NewUnlitShader"{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR]_Color ("Color",color) = (1,1,1,1)
        _Metallic ("Metallic",Range(0,1)) = 0.0
    }
    
    Subshader{
        Tags {"RenderType"="Opaque"}
        Lod 200

    CGPROGRAM

    #pragma surface surf Standard fullforwardshadows

    #pragma target 3.0

    sampler2D _MainTex;

    struct Input {
        float2 uv_MainTex;
        };

    half _Metallic;
    fixed4 _Color;

    void surf (Input IN, inout SurfaceOutputStandard o) {
        fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
        o.Albedo = c.rgb;
        o.Metallic = _Metallic;
        o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
