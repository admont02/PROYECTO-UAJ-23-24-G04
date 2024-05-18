Shader "Shader Graphs/inidicatorShader"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (7.471698, 7.471698, 0, 0)
        _RemapIntensity("RemapIntensity", Range(0, 1)) = 0.2
        _CircleRadius("CircleRadius", Float) = 0.845
        [NoScaleOffset]_MaskTex("_MaskTex", 2D) = "white" {}
        _Distance("Distance", Float) = 0
        _Vibration("Vibration", Float) = 15
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue" = "Transparent"
        // DisableBatching: <None>
        "ShaderGraphShader" = "true"
        "ShaderGraphTargetId" = "UniversalSpriteUnlitSubTarget"
    }
    Pass
    {
        Name "Sprite Unlit"
        Tags
        {
            "LightMode" = "Universal2D"
        }

        // Render State
        Cull Off
        Blend SrcAlpha One, One One
        ZTest LEqual
        ZWrite Off

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

        // Keywords
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>

        // Defines

        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEUNLIT
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float3 positionWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

        PackedVaryings PackVaryings(Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

        Varyings UnpackVaryings(PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }


        // --------------------------------------------------
        // Graph

        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _RemapIntensity;
        float _CircleRadius;
        float4 _MaskTex_TexelSize;
        float _Distance;
        float _Vibration;
        CBUFFER_END


            // Object and Global properties
            SAMPLER(SamplerState_Linear_Repeat);
            TEXTURE2D(_MaskTex);
            SAMPLER(sampler_MaskTex);

            // Graph Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"

            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif

            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif

            // Graph Functions

            void Unity_OneMinus_float(float In, out float Out)
            {
                Out = 1 - In;
            }

            void Unity_PolarCoordinates_float(float2 UV, float2 Center, float RadialScale, float LengthScale, out float2 Out)
            {
                float2 delta = UV - Center;
                float radius = length(delta) * 2 * RadialScale;
                float angle = atan2(delta.x, delta.y) * 1.0 / 6.28 * LengthScale;
                Out = float2(radius, angle);
            }

            void Unity_Preview_float(float In, out float Out)
            {
                Out = In;
            }

            void CircleSDF_float(float position, float radius, out float Out) {
                Out = length(position) - radius;
            }

            void Unity_Absolute_float(float In, out float Out)
            {
                Out = abs(In);
            }

            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
            {
                Out = smoothstep(Edge1, Edge2, In);
            }

            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }

            float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
            {
                float x; Hash_Tchou_2_1_float(p, x);
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }

            void Unity_GradientNoise_Deterministic_float(float2 UV, float3 Scale, out float Out)
            {
                float2 p = UV * Scale.xy;
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
                float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
            }

            void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
            {
                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }

            void Unity_Saturate_float(float In, out float Out)
            {
                Out = saturate(In);
            }

            void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }

            // Custom interpolators pre vertex
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

            // Graph Vertex
            struct VertexDescription
            {
                float3 Position;
                float3 Normal;
                float3 Tangent;
            };

            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                description.Position = IN.ObjectSpacePosition;
                description.Normal = IN.ObjectSpaceNormal;
                description.Tangent = IN.ObjectSpaceTangent;
                return description;
            }

            // Custom interpolators, pre surface
            #ifdef FEATURES_GRAPH_VERTEX
            Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
            {
            return output;
            }
            #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
            #endif

            // Graph Pixel
            struct SurfaceDescription
            {
                float3 BaseColor;
                float Alpha;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _Property_c7d75b4675d343ed9d37d9c9582d3d18_Out_0_Float = _CircleRadius;
                float _OneMinus_58a32716a82b43a6a7c7656a09b64e34_Out_1_Float;
                Unity_OneMinus_float(_Property_c7d75b4675d343ed9d37d9c9582d3d18_Out_0_Float, _OneMinus_58a32716a82b43a6a7c7656a09b64e34_Out_1_Float);
                float2 _PolarCoordinates_eabbfded00414af59e9328dadc5273e5_Out_4_Vector2;
                Unity_PolarCoordinates_float(IN.uv0.xy, float2 (0.5, 0.5), 1, 1, _PolarCoordinates_eabbfded00414af59e9328dadc5273e5_Out_4_Vector2);
                float _Split_a7e060a2524a4e05a0ac666988f066d8_R_1_Float = _PolarCoordinates_eabbfded00414af59e9328dadc5273e5_Out_4_Vector2[0];
                float _Split_a7e060a2524a4e05a0ac666988f066d8_G_2_Float = _PolarCoordinates_eabbfded00414af59e9328dadc5273e5_Out_4_Vector2[1];
                float _Split_a7e060a2524a4e05a0ac666988f066d8_B_3_Float = 0;
                float _Split_a7e060a2524a4e05a0ac666988f066d8_A_4_Float = 0;
                float _Preview_eaaaa03bc0654f958a761a83262ebb34_Out_1_Float;
                Unity_Preview_float(_Split_a7e060a2524a4e05a0ac666988f066d8_R_1_Float, _Preview_eaaaa03bc0654f958a761a83262ebb34_Out_1_Float);
                float _Property_b807381bf77242fba296defac3e0b3c1_Out_0_Float = _CircleRadius;
                float _CircleSDFCustomFunction_999cd0c946504728ab4d2e9157f78743_Out_2_Float;
                CircleSDF_float(_Preview_eaaaa03bc0654f958a761a83262ebb34_Out_1_Float, _Property_b807381bf77242fba296defac3e0b3c1_Out_0_Float, _CircleSDFCustomFunction_999cd0c946504728ab4d2e9157f78743_Out_2_Float);
                float _Absolute_19d48172120b4557b3b9b81f8bb267a5_Out_1_Float;
                Unity_Absolute_float(_CircleSDFCustomFunction_999cd0c946504728ab4d2e9157f78743_Out_2_Float, _Absolute_19d48172120b4557b3b9b81f8bb267a5_Out_1_Float);
                float _Smoothstep_63f781cbd20348689053497450044f06_Out_3_Float;
                Unity_Smoothstep_float(_OneMinus_58a32716a82b43a6a7c7656a09b64e34_Out_1_Float, 0, _Absolute_19d48172120b4557b3b9b81f8bb267a5_Out_1_Float, _Smoothstep_63f781cbd20348689053497450044f06_Out_3_Float);
                float2 _TilingAndOffset_6f959165a2ed4a138e166dfdb9d193ec_Out_3_Vector2;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (IN.TimeParameters.x.xx), _TilingAndOffset_6f959165a2ed4a138e166dfdb9d193ec_Out_3_Vector2);
                float _Property_9dbc2176124a4cc8ae4087efa320c168_Out_0_Float = _Vibration;
                float _GradientNoise_5df6d3b74f8e4f79a2d41e2a69e754c7_Out_2_Float;
                Unity_GradientNoise_Deterministic_float(_TilingAndOffset_6f959165a2ed4a138e166dfdb9d193ec_Out_3_Vector2, _Property_9dbc2176124a4cc8ae4087efa320c168_Out_0_Float, _GradientNoise_5df6d3b74f8e4f79a2d41e2a69e754c7_Out_2_Float);
                float _Property_fc348f9eebeb4708b7fb44ba40150b58_Out_0_Float = _RemapIntensity;
                float2 _Vector2_52d458b6ef6341f4acfa2f8d654c55d2_Out_0_Vector2 = float2(0, _Property_fc348f9eebeb4708b7fb44ba40150b58_Out_0_Float);
                float _Remap_5a648f2ab3f84ce59c78cd014ae13910_Out_3_Float;
                Unity_Remap_float(_GradientNoise_5df6d3b74f8e4f79a2d41e2a69e754c7_Out_2_Float, float2 (0, 1), _Vector2_52d458b6ef6341f4acfa2f8d654c55d2_Out_0_Vector2, _Remap_5a648f2ab3f84ce59c78cd014ae13910_Out_3_Float);
                float _Saturate_ea5a492e366d41af979de1a05806120d_Out_1_Float;
                Unity_Saturate_float(_Remap_5a648f2ab3f84ce59c78cd014ae13910_Out_3_Float, _Saturate_ea5a492e366d41af979de1a05806120d_Out_1_Float);
                float2 _Vector2_929d741480d444e59c286db7975ab521_Out_0_Vector2 = float2(_Saturate_ea5a492e366d41af979de1a05806120d_Out_1_Float, 1);
                float _Remap_65a32170541d4df38e8edc972433ac15_Out_3_Float;
                Unity_Remap_float(_Smoothstep_63f781cbd20348689053497450044f06_Out_3_Float, _Vector2_929d741480d444e59c286db7975ab521_Out_0_Vector2, float2 (0, 1), _Remap_65a32170541d4df38e8edc972433ac15_Out_3_Float);
                float _Saturate_013bfb66c5ec4bb18e4550cb0eacff43_Out_1_Float;
                Unity_Saturate_float(_Remap_65a32170541d4df38e8edc972433ac15_Out_3_Float, _Saturate_013bfb66c5ec4bb18e4550cb0eacff43_Out_1_Float);
                float4 _Property_094660fb47534397a5743fa2a5ae500d_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_Color) : _Color;
                float4 _Multiply_e1ef622d36e94f0d98f25b7dc8badd28_Out_2_Vector4;
                Unity_Multiply_float4_float4((_Saturate_013bfb66c5ec4bb18e4550cb0eacff43_Out_1_Float.xxxx), _Property_094660fb47534397a5743fa2a5ae500d_Out_0_Vector4, _Multiply_e1ef622d36e94f0d98f25b7dc8badd28_Out_2_Vector4);
                UnityTexture2D _Property_e46e40e62d8f4b8e92544be547a59de6_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MaskTex);
                float4 _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e46e40e62d8f4b8e92544be547a59de6_Out_0_Texture2D.tex, _Property_e46e40e62d8f4b8e92544be547a59de6_Out_0_Texture2D.samplerstate, _Property_e46e40e62d8f4b8e92544be547a59de6_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy));
                float _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_R_4_Float = _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_RGBA_0_Vector4.r;
                float _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_G_5_Float = _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_RGBA_0_Vector4.g;
                float _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_B_6_Float = _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_RGBA_0_Vector4.b;
                float _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_A_7_Float = _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_RGBA_0_Vector4.a;
                float4 _Multiply_2cbb042aa3104092bec7401902771d2c_Out_2_Vector4;
                Unity_Multiply_float4_float4(_Multiply_e1ef622d36e94f0d98f25b7dc8badd28_Out_2_Vector4, _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_RGBA_0_Vector4, _Multiply_2cbb042aa3104092bec7401902771d2c_Out_2_Vector4);
                float _Property_c2232b04c2914156a0fe73f37312c394_Out_0_Float = _Distance;
                surface.BaseColor = (_Multiply_2cbb042aa3104092bec7401902771d2c_Out_2_Vector4.xyz);
                surface.Alpha = _Property_c2232b04c2914156a0fe73f37312c394_Out_0_Float;
                return surface;
            }

            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES Attributes
            #define VFX_SRP_VARYINGS Varyings
            #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
            #endif
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                output.ObjectSpaceNormal = input.normalOS;
                output.ObjectSpaceTangent = input.tangentOS.xyz;
                output.ObjectSpacePosition = input.positionOS;

                return output;
            }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

            #ifdef HAVE_VFX_MODIFICATION
            #if VFX_USE_GRAPH_VALUES
                uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
                /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
            #endif
                /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

            #endif








                #if UNITY_UV_STARTS_AT_TOP
                #else
                #endif


                output.uv0 = input.texCoord0;
                output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                    return output;
            }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

                            // --------------------------------------------------
                            // Visual Effect Vertex Invocations
                            #ifdef HAVE_VFX_MODIFICATION
                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                            #endif

                            ENDHLSL
                            }
                            Pass
                            {
                                Name "Sprite Unlit"
                                Tags
                                {
                                    "LightMode" = "UniversalForward"
                                }

                                // Render State
                                Cull Off
                                Blend SrcAlpha One, One One
                                ZTest LEqual
                                ZWrite Off

                                // Debug
                                // <None>

                                // --------------------------------------------------
                                // Pass

                                HLSLPROGRAM

                                // Pragmas
                                #pragma target 2.0
                                #pragma exclude_renderers d3d11_9x
                                #pragma vertex vert
                                #pragma fragment frag

                                // Keywords
                                #pragma multi_compile_fragment _ DEBUG_DISPLAY
                                // GraphKeywords: <None>

                                // Defines

                                #define ATTRIBUTES_NEED_NORMAL
                                #define ATTRIBUTES_NEED_TANGENT
                                #define ATTRIBUTES_NEED_TEXCOORD0
                                #define ATTRIBUTES_NEED_COLOR
                                #define VARYINGS_NEED_POSITION_WS
                                #define VARYINGS_NEED_TEXCOORD0
                                #define VARYINGS_NEED_COLOR
                                #define FEATURES_GRAPH_VERTEX
                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                #define SHADERPASS SHADERPASS_SPRITEFORWARD
                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                                // custom interpolator pre-include
                                /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                                // Includes
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                                // --------------------------------------------------
                                // Structs and Packing

                                // custom interpolators pre packing
                                /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                                struct Attributes
                                {
                                     float3 positionOS : POSITION;
                                     float3 normalOS : NORMAL;
                                     float4 tangentOS : TANGENT;
                                     float4 uv0 : TEXCOORD0;
                                     float4 color : COLOR;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                     uint instanceID : INSTANCEID_SEMANTIC;
                                    #endif
                                };
                                struct Varyings
                                {
                                     float4 positionCS : SV_POSITION;
                                     float3 positionWS;
                                     float4 texCoord0;
                                     float4 color;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                    #endif
                                };
                                struct SurfaceDescriptionInputs
                                {
                                     float4 uv0;
                                     float3 TimeParameters;
                                };
                                struct VertexDescriptionInputs
                                {
                                     float3 ObjectSpaceNormal;
                                     float3 ObjectSpaceTangent;
                                     float3 ObjectSpacePosition;
                                };
                                struct PackedVaryings
                                {
                                     float4 positionCS : SV_POSITION;
                                     float4 texCoord0 : INTERP0;
                                     float4 color : INTERP1;
                                     float3 positionWS : INTERP2;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                    #endif
                                };

                                PackedVaryings PackVaryings(Varyings input)
                                {
                                    PackedVaryings output;
                                    ZERO_INITIALIZE(PackedVaryings, output);
                                    output.positionCS = input.positionCS;
                                    output.texCoord0.xyzw = input.texCoord0;
                                    output.color.xyzw = input.color;
                                    output.positionWS.xyz = input.positionWS;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    output.instanceID = input.instanceID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    output.cullFace = input.cullFace;
                                    #endif
                                    return output;
                                }

                                Varyings UnpackVaryings(PackedVaryings input)
                                {
                                    Varyings output;
                                    output.positionCS = input.positionCS;
                                    output.texCoord0 = input.texCoord0.xyzw;
                                    output.color = input.color.xyzw;
                                    output.positionWS = input.positionWS.xyz;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    output.instanceID = input.instanceID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    output.cullFace = input.cullFace;
                                    #endif
                                    return output;
                                }


                                // --------------------------------------------------
                                // Graph

                                // Graph Properties
                                CBUFFER_START(UnityPerMaterial)
                                float4 _Color;
                                float _RemapIntensity;
                                float _CircleRadius;
                                float4 _MaskTex_TexelSize;
                                float _Distance;
                                float _Vibration;
                                CBUFFER_END


                                    // Object and Global properties
                                    SAMPLER(SamplerState_Linear_Repeat);
                                    TEXTURE2D(_MaskTex);
                                    SAMPLER(sampler_MaskTex);

                                    // Graph Includes
                                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"

                                    // -- Property used by ScenePickingPass
                                    #ifdef SCENEPICKINGPASS
                                    float4 _SelectionID;
                                    #endif

                                    // -- Properties used by SceneSelectionPass
                                    #ifdef SCENESELECTIONPASS
                                    int _ObjectId;
                                    int _PassValue;
                                    #endif

                                    // Graph Functions

                                    void Unity_OneMinus_float(float In, out float Out)
                                    {
                                        Out = 1 - In;
                                    }

                                    void Unity_PolarCoordinates_float(float2 UV, float2 Center, float RadialScale, float LengthScale, out float2 Out)
                                    {
                                        float2 delta = UV - Center;
                                        float radius = length(delta) * 2 * RadialScale;
                                        float angle = atan2(delta.x, delta.y) * 1.0 / 6.28 * LengthScale;
                                        Out = float2(radius, angle);
                                    }

                                    void Unity_Preview_float(float In, out float Out)
                                    {
                                        Out = In;
                                    }

                                    void CircleSDF_float(float position, float radius, out float Out) {
                                        Out = length(position) - radius;
                                    }

                                    void Unity_Absolute_float(float In, out float Out)
                                    {
                                        Out = abs(In);
                                    }

                                    void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
                                    {
                                        Out = smoothstep(Edge1, Edge2, In);
                                    }

                                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                                    {
                                        Out = UV * Tiling + Offset;
                                    }

                                    float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
                                    {
                                        float x; Hash_Tchou_2_1_float(p, x);
                                        return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
                                    }

                                    void Unity_GradientNoise_Deterministic_float(float2 UV, float3 Scale, out float Out)
                                    {
                                        float2 p = UV * Scale.xy;
                                        float2 ip = floor(p);
                                        float2 fp = frac(p);
                                        float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
                                        float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                                        float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                                        float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                                        fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                                        Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
                                    }

                                    void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
                                    {
                                        Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                                    }

                                    void Unity_Saturate_float(float In, out float Out)
                                    {
                                        Out = saturate(In);
                                    }

                                    void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
                                    {
                                        Out = A * B;
                                    }

                                    // Custom interpolators pre vertex
                                    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                                    // Graph Vertex
                                    struct VertexDescription
                                    {
                                        float3 Position;
                                        float3 Normal;
                                        float3 Tangent;
                                    };

                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                    {
                                        VertexDescription description = (VertexDescription)0;
                                        description.Position = IN.ObjectSpacePosition;
                                        description.Normal = IN.ObjectSpaceNormal;
                                        description.Tangent = IN.ObjectSpaceTangent;
                                        return description;
                                    }

                                    // Custom interpolators, pre surface
                                    #ifdef FEATURES_GRAPH_VERTEX
                                    Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                                    {
                                    return output;
                                    }
                                    #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                                    #endif

                                    // Graph Pixel
                                    struct SurfaceDescription
                                    {
                                        float3 BaseColor;
                                        float Alpha;
                                    };

                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                    {
                                        SurfaceDescription surface = (SurfaceDescription)0;
                                        float _Property_c7d75b4675d343ed9d37d9c9582d3d18_Out_0_Float = _CircleRadius;
                                        float _OneMinus_58a32716a82b43a6a7c7656a09b64e34_Out_1_Float;
                                        Unity_OneMinus_float(_Property_c7d75b4675d343ed9d37d9c9582d3d18_Out_0_Float, _OneMinus_58a32716a82b43a6a7c7656a09b64e34_Out_1_Float);
                                        float2 _PolarCoordinates_eabbfded00414af59e9328dadc5273e5_Out_4_Vector2;
                                        Unity_PolarCoordinates_float(IN.uv0.xy, float2 (0.5, 0.5), 1, 1, _PolarCoordinates_eabbfded00414af59e9328dadc5273e5_Out_4_Vector2);
                                        float _Split_a7e060a2524a4e05a0ac666988f066d8_R_1_Float = _PolarCoordinates_eabbfded00414af59e9328dadc5273e5_Out_4_Vector2[0];
                                        float _Split_a7e060a2524a4e05a0ac666988f066d8_G_2_Float = _PolarCoordinates_eabbfded00414af59e9328dadc5273e5_Out_4_Vector2[1];
                                        float _Split_a7e060a2524a4e05a0ac666988f066d8_B_3_Float = 0;
                                        float _Split_a7e060a2524a4e05a0ac666988f066d8_A_4_Float = 0;
                                        float _Preview_eaaaa03bc0654f958a761a83262ebb34_Out_1_Float;
                                        Unity_Preview_float(_Split_a7e060a2524a4e05a0ac666988f066d8_R_1_Float, _Preview_eaaaa03bc0654f958a761a83262ebb34_Out_1_Float);
                                        float _Property_b807381bf77242fba296defac3e0b3c1_Out_0_Float = _CircleRadius;
                                        float _CircleSDFCustomFunction_999cd0c946504728ab4d2e9157f78743_Out_2_Float;
                                        CircleSDF_float(_Preview_eaaaa03bc0654f958a761a83262ebb34_Out_1_Float, _Property_b807381bf77242fba296defac3e0b3c1_Out_0_Float, _CircleSDFCustomFunction_999cd0c946504728ab4d2e9157f78743_Out_2_Float);
                                        float _Absolute_19d48172120b4557b3b9b81f8bb267a5_Out_1_Float;
                                        Unity_Absolute_float(_CircleSDFCustomFunction_999cd0c946504728ab4d2e9157f78743_Out_2_Float, _Absolute_19d48172120b4557b3b9b81f8bb267a5_Out_1_Float);
                                        float _Smoothstep_63f781cbd20348689053497450044f06_Out_3_Float;
                                        Unity_Smoothstep_float(_OneMinus_58a32716a82b43a6a7c7656a09b64e34_Out_1_Float, 0, _Absolute_19d48172120b4557b3b9b81f8bb267a5_Out_1_Float, _Smoothstep_63f781cbd20348689053497450044f06_Out_3_Float);
                                        float2 _TilingAndOffset_6f959165a2ed4a138e166dfdb9d193ec_Out_3_Vector2;
                                        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (IN.TimeParameters.x.xx), _TilingAndOffset_6f959165a2ed4a138e166dfdb9d193ec_Out_3_Vector2);
                                        float _Property_9dbc2176124a4cc8ae4087efa320c168_Out_0_Float = _Vibration;
                                        float _GradientNoise_5df6d3b74f8e4f79a2d41e2a69e754c7_Out_2_Float;
                                        Unity_GradientNoise_Deterministic_float(_TilingAndOffset_6f959165a2ed4a138e166dfdb9d193ec_Out_3_Vector2, _Property_9dbc2176124a4cc8ae4087efa320c168_Out_0_Float, _GradientNoise_5df6d3b74f8e4f79a2d41e2a69e754c7_Out_2_Float);
                                        float _Property_fc348f9eebeb4708b7fb44ba40150b58_Out_0_Float = _RemapIntensity;
                                        float2 _Vector2_52d458b6ef6341f4acfa2f8d654c55d2_Out_0_Vector2 = float2(0, _Property_fc348f9eebeb4708b7fb44ba40150b58_Out_0_Float);
                                        float _Remap_5a648f2ab3f84ce59c78cd014ae13910_Out_3_Float;
                                        Unity_Remap_float(_GradientNoise_5df6d3b74f8e4f79a2d41e2a69e754c7_Out_2_Float, float2 (0, 1), _Vector2_52d458b6ef6341f4acfa2f8d654c55d2_Out_0_Vector2, _Remap_5a648f2ab3f84ce59c78cd014ae13910_Out_3_Float);
                                        float _Saturate_ea5a492e366d41af979de1a05806120d_Out_1_Float;
                                        Unity_Saturate_float(_Remap_5a648f2ab3f84ce59c78cd014ae13910_Out_3_Float, _Saturate_ea5a492e366d41af979de1a05806120d_Out_1_Float);
                                        float2 _Vector2_929d741480d444e59c286db7975ab521_Out_0_Vector2 = float2(_Saturate_ea5a492e366d41af979de1a05806120d_Out_1_Float, 1);
                                        float _Remap_65a32170541d4df38e8edc972433ac15_Out_3_Float;
                                        Unity_Remap_float(_Smoothstep_63f781cbd20348689053497450044f06_Out_3_Float, _Vector2_929d741480d444e59c286db7975ab521_Out_0_Vector2, float2 (0, 1), _Remap_65a32170541d4df38e8edc972433ac15_Out_3_Float);
                                        float _Saturate_013bfb66c5ec4bb18e4550cb0eacff43_Out_1_Float;
                                        Unity_Saturate_float(_Remap_65a32170541d4df38e8edc972433ac15_Out_3_Float, _Saturate_013bfb66c5ec4bb18e4550cb0eacff43_Out_1_Float);
                                        float4 _Property_094660fb47534397a5743fa2a5ae500d_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_Color) : _Color;
                                        float4 _Multiply_e1ef622d36e94f0d98f25b7dc8badd28_Out_2_Vector4;
                                        Unity_Multiply_float4_float4((_Saturate_013bfb66c5ec4bb18e4550cb0eacff43_Out_1_Float.xxxx), _Property_094660fb47534397a5743fa2a5ae500d_Out_0_Vector4, _Multiply_e1ef622d36e94f0d98f25b7dc8badd28_Out_2_Vector4);
                                        UnityTexture2D _Property_e46e40e62d8f4b8e92544be547a59de6_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MaskTex);
                                        float4 _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_e46e40e62d8f4b8e92544be547a59de6_Out_0_Texture2D.tex, _Property_e46e40e62d8f4b8e92544be547a59de6_Out_0_Texture2D.samplerstate, _Property_e46e40e62d8f4b8e92544be547a59de6_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy));
                                        float _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_R_4_Float = _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_RGBA_0_Vector4.r;
                                        float _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_G_5_Float = _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_RGBA_0_Vector4.g;
                                        float _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_B_6_Float = _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_RGBA_0_Vector4.b;
                                        float _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_A_7_Float = _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_RGBA_0_Vector4.a;
                                        float4 _Multiply_2cbb042aa3104092bec7401902771d2c_Out_2_Vector4;
                                        Unity_Multiply_float4_float4(_Multiply_e1ef622d36e94f0d98f25b7dc8badd28_Out_2_Vector4, _SampleTexture2D_57f3a8ef0cbb4252a6091a0894437c4e_RGBA_0_Vector4, _Multiply_2cbb042aa3104092bec7401902771d2c_Out_2_Vector4);
                                        float _Property_c2232b04c2914156a0fe73f37312c394_Out_0_Float = _Distance;
                                        surface.BaseColor = (_Multiply_2cbb042aa3104092bec7401902771d2c_Out_2_Vector4.xyz);
                                        surface.Alpha = _Property_c2232b04c2914156a0fe73f37312c394_Out_0_Float;
                                        return surface;
                                    }

                                    // --------------------------------------------------
                                    // Build Graph Inputs
                                    #ifdef HAVE_VFX_MODIFICATION
                                    #define VFX_SRP_ATTRIBUTES Attributes
                                    #define VFX_SRP_VARYINGS Varyings
                                    #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                                    #endif
                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                    {
                                        VertexDescriptionInputs output;
                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                        output.ObjectSpaceNormal = input.normalOS;
                                        output.ObjectSpaceTangent = input.tangentOS.xyz;
                                        output.ObjectSpacePosition = input.positionOS;

                                        return output;
                                    }
                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                    {
                                        SurfaceDescriptionInputs output;
                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                                    #ifdef HAVE_VFX_MODIFICATION
                                    #if VFX_USE_GRAPH_VALUES
                                        uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
                                        /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
                                    #endif
                                        /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                                    #endif








                                        #if UNITY_UV_STARTS_AT_TOP
                                        #else
                                        #endif


                                        output.uv0 = input.texCoord0;
                                        output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                    #else
                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                    #endif
                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                            return output;
                                    }

                                    // --------------------------------------------------
                                    // Main

                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                    #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

                                    // --------------------------------------------------
                                    // Visual Effect Vertex Invocations
                                    #ifdef HAVE_VFX_MODIFICATION
                                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                                    #endif

                                    ENDHLSL
                                    }
    }
        CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
                                        FallBack "Hidden/Shader Graph/FallbackError"
}