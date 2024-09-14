Shader "Custom/WaterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}    // Main texture
        _SpeedX ("Speed X", Float) = 0.1         // Speed of horizontal movement
        _SpeedY ("Speed Y", Float) = 0.1         // Speed of vertical movement
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _SpeedX;
            float _SpeedY;

            // Vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Scroll the texture based on time
                o.uv = v.uv + float2(_SpeedX, _SpeedY) * _Time.y;
                return o;
            }

            // Fragment shader
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture at the modified UV coordinates
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
