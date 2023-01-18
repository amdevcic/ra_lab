Shader "Custom/MorphShader"
{
    Properties
    {
        _yScale ("Y Scale", Float) = 1
        _xShear ("X Shear", Float) = 0
        _zShear ("Z Shear", Float) = 0
     
        // twisting
        _bendAngle ("Bend Angle", Float) = 0

        // warp
        _sineAmplitude ("Sine Wave Amplitude", Float) = 1
        _sineWavelength ("Sine Wave Wavelength", Float) = 1
        _sineSpeed ("Sine Wave Speed", Float) = 1

        // bob
        _bobSpeed ("Bob Speed", Float) = 0
        _bobAmplitude ("Bob Amplitude", Float) = 1

        // rotate
        _rotationSpeed ("Rotation Speed", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : POSITION;
                float3 normal : NORMAL;
                half3 worldNormal : TEXCOORD0;
            };

            float _bendAngle;
            float _yScale;
            float _xShear;
            float _zShear;
            float _sineAmplitude;
            float _sineWavelength;
            float _sineSpeed;
            float _bobAmplitude;
            float _bobSpeed;
            float _rotationSpeed;

            void vert (inout v2f v) {
                float4 p = v.pos;

                // rotating
                float t = _Time.y*_rotationSpeed;
                float4x4 rot = {cos(t), 0, sin(t), 0,
                                0, 1, 0, 0,
                                -sin(t), 0, cos(t), 0,
                                0, 0, 0, 1};
                p = mul(rot, p);

                // twisting
                float angle = _bendAngle*3.14159/180;
                float4 pn = normalize(p);
                p.x = (p.x*cos(pn.y*angle)-p.z*sin(pn.y*angle));
                p.z = (p.x*sin(pn.y*angle)+p.z*cos(pn.y*angle));

                // scaling
                p.y *= _yScale;

                // shearing
                p.x += v.pos.y*_xShear;
                p.z += v.pos.y*_zShear;

                // sin wave
                float sineFreq = _sineSpeed / _sineWavelength;
                p.y += _sineAmplitude*sin(2*3.1415*sineFreq*(_Time.y+p.x+p.z));

                // bobbing
                p.y += _bobAmplitude*sin(_Time.y*_bobSpeed);

                v.pos = UnityObjectToClipPos(p);
                v.worldNormal = UnityObjectToWorldNormal(v.normal);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = 0;
                c.rgb = i.worldNormal*0.5+0.5;
                return c;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
