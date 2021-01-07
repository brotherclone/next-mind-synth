﻿Shader "NextMind/Stimulation"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _Stimulation("Stimulation", 2D) = "gray" {}
        _Blend("Blend",Range(0,1)) = 1
        _Density("Density", Float) = 1
        _ScreenRatio("Screen Ratio", Float) = 1.777
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
        }

        CGPROGRAM
        #include "StimulationIncludes.cginc"
        #pragma surface surf Standard fullforwardshadows

        #pragma shader_feature STANDARD SCREEN_COORDINATES TRIPLANAR
        #pragma shader_feature CONSTANT_SIZE
        #pragma shader_feature MAIN_ALPHA

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float4 tex;

#if SCREEN_COORDINATES
            tex = ScreenCoordinatesProjection(IN);
#elif TRIPLANAR
            tex = TriplanarProjection(IN);
#elif STANDARD
            tex = StandardProjection(IN);
#endif
            o.Albedo = tex.xyz;
        }

        ENDCG
    }
            
    Fallback "Diffuse"
    CustomEditor "NextMindOpaqueShaderGUI"
}