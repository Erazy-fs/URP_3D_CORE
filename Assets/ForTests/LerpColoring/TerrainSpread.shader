Shader "Custom/TerrainSpread"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {} // Основная текстура
        _SpreadCenter ("Spread Center", Vector) = (0, 0, 0, 0) // Центр распространения
        _SpreadRadius ("Spread Radius", Float) = 0 // Радиус распространения
        _Color ("Spread Color", Color) = (1, 0, 0, 1) // Цвет распространения
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _SpreadCenter; // Точка распространения
            float _SpreadRadius;  // Радиус распространения
            float4 _Color;        // Цвет, который будет распространяться

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Считываем основную текстуру
                half4 baseColor = tex2D(_MainTex, i.uv);

                // Рассчитываем расстояние от центра распространения
                float dist = length(_SpreadCenter.xy - i.uv); // Расстояние от центра распространения
                float spreadFactor = saturate(dist / _SpreadRadius); // Множитель для плавного смешивания

                // Линейное смешивание с цветом
                return lerp(baseColor, _Color, 1.0 - spreadFactor); // Чем дальше от центра, тем менее интенсивный цвет
            }
            ENDCG
        }
    }
}
