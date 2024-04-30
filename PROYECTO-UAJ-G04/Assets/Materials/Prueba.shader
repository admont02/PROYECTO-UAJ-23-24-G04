Shader "Custom/ColorWhiteTransparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _Numero ("Numero", float) = 1
    }
 
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
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
                float4 pos : SV_POSITION;
            };
 
            sampler2D _MainTex;
            fixed4 _MainColor;
            float _Numero;
 
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                float transparency = 1 - abs(i.uv.x - 0.5) * _Numero;
                transparency *= step(0.9999, texColor.r) * step(0.9999, texColor.g) * step(0.9999, texColor.b);
                fixed4 finalColor = lerp(texColor, _MainColor, transparency);
                finalColor.a = transparency;
                return finalColor;
            }
            ENDCG
        }
    }
}
