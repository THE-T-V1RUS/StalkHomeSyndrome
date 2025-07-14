Shader "Skybox/Blended"
{
    Properties
    {
        _Cubemap1("Skybox Day", CUBE) = "" {}
        _Cubemap2("Skybox Night", CUBE) = "" {}
        _Blend("Blend", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Background" }
        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            samplerCUBE _Cubemap1;
            samplerCUBE _Cubemap2;
            float _Blend;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            v2f vert (float3 vertex : POSITION)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                o.texcoord = vertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color1 = texCUBE(_Cubemap1, i.texcoord);
                fixed4 color2 = texCUBE(_Cubemap2, i.texcoord);
                return lerp(color1, color2, _Blend);
            }
            ENDCG
        }
    }
    FallBack "RenderFX/Skybox"
}
