using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

//[ImageEffectAllowedInSceneView]
public class FogEffect : PostEffect
{

    public enum FogMode
    {
        Distance = 1,
        Depth = 2,
    }

    //Gradient
    [OnValueChanged("UpdateGradient")] public Gradient gradient = new Gradient();
    Texture2D gradientTex;
    Texture2D GradientTexture
    {
        get
        {
            if (gradientTex == null)
            {
                gradientTex = new Texture2D(1024, 1);
                gradientTex.wrapMode = TextureWrapMode.Clamp;
                gradientTex.filterMode = FilterMode.Bilinear;
                UpdateGradient();
            }
            return gradientTex;
        }
    }
    void UpdateGradient()
    {
        for (int i = 0; i < GradientTexture.width; i++)
        {
            float percent = (float)i / (float)GradientTexture.width;
            Color pixelColor = gradient.Evaluate(percent);
            GradientTexture.SetPixel(i, 0, pixelColor);
        }
        GradientTexture.Apply();

#if UNITY_EDITOR
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
    }

    //Curve
    [OnValueChanged("UpdateCurve")] public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    Texture2D curveTex;
    Texture2D CurveTexture
    {
        get
        {
            if (curveTex == null)
            {
                curveTex = new Texture2D(1024, 1);
                curveTex.wrapMode = TextureWrapMode.Clamp;
                curveTex.filterMode = FilterMode.Bilinear;
                UpdateCurve();
            }
            return curveTex;
        }
    }
    void UpdateCurve()
    {
        for (int i = 0; i < CurveTexture.width; i++)
        {
            float c = curve.Evaluate((float)i / CurveTexture.width);
            Color pixelColor = new Color(c, c, c, c);
            CurveTexture.SetPixel(i, 0, pixelColor);
        }
        CurveTexture.Apply();

#if UNITY_EDITOR
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
    }

    [ToggleGroup("useDistanceFog", "Distance")]
    public bool useDistanceFog = true;

    [ToggleGroup("useDistanceFog"), EnumToggleButtons] public FogMode fogMode = FogMode.Depth;
    [ToggleGroup("useDistanceFog")] public float near = 0;
    [ToggleGroup("useDistanceFog")] public float far = 50;
    [ToggleGroup("useDistanceFog")] [Range(0, 10)] public float distanceDensity = 1;

    [ToggleGroup("useHeightFog", "Height")]
    public bool useHeightFog = false;
    [ToggleGroup("useHeightFog")] public float low = 2;
    [ToggleGroup("useHeightFog")] public float height = -20;
    [ToggleGroup("useHeightFog")] [Range(0, 10)] public float heightDensity = 1;

    [ToggleGroup("useCircleFog", "Circle")]
    public bool useCircleFog;
    [ToggleGroup("useCircleFog")] [DrawWithUnity] public Vector3 circlePosition;
    [ToggleGroup("useCircleFog")] public float circleInside = 0;
    [ToggleGroup("useCircleFog")] public float circleOutside = 1;
    [ToggleGroup("useCircleFog")] [Range(0, 10)] public float circleDensity = 1;

    //Fade 
    [Space]
    [OnValueChanged("UpdateFadeCurve")] public AnimationCurve fadeCurve = AnimationCurve.Linear(1, 1, 0, 0);
    public float _fadePow = 1;
    Texture2D fadeTex;
    Texture2D FadeTexture
    {
        get
        {
            if (fadeTex == null)
            {
                fadeTex = new Texture2D(1024, 1);
                fadeTex.wrapMode = TextureWrapMode.Clamp;
                fadeTex.filterMode = FilterMode.Bilinear;
                UpdateFadeCurve();
            }
            return fadeTex;
        }
    }

    void UpdateFadeCurve()
    {
        for (int i = 0; i < FadeTexture.width; i++)
        {
            float c = fadeCurve.Evaluate((float)i / FadeTexture.width);
            Color pixelColor = new Color(c, c, c, c);
            FadeTexture.SetPixel(i, 0, pixelColor);
        }
        FadeTexture.Apply();

#if UNITY_EDITOR
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
    }

    //Noise
    [Space]
    [Range(0, 1)] public float noise = 0.025f;
    public float noiseDisturb = 1000;

    private void OnValidate()
    {
        UpdateAllTexture();
    }
    private void OnEnable()
    {
        UpdateAllTexture();
    }

    void UpdateAllTexture()
    {
        UpdateGradient();
        UpdateCurve();
        UpdateFadeCurve();
    }

    void Start()
    {
        cameraComponent.depthTextureMode = DepthTextureMode.Depth;

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if ((!useDistanceFog && !useHeightFog && !useCircleFog))
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetTexture("_GradientTex", gradientTex);
        material.SetTexture("_CurveTex", curveTex);

        //Distance
        material.SetInt("_useDistanceFog", useDistanceFog == true ? 1 : 0);
        material.SetInt("_fogMode", (int)fogMode);
        material.SetFloat("_FogNear", near);
        material.SetFloat("_FogFar", far);
        material.SetFloat("_distanceDensity", distanceDensity);

        //Height
        material.SetInt("_useHeightFog", useHeightFog == true ? 1 : 0);
        material.SetFloat("_fogLow", low);
        material.SetFloat("_fogHeight", height);
        material.SetFloat("_heightDensity", heightDensity);

        //Circle
        material.SetInt("_useCircleFog", useCircleFog == true ? 1 : 0);
        material.SetVector("_circlePosition", circlePosition);
        material.SetFloat("_circleInside", circleInside);
        material.SetFloat("_circleOutside", circleOutside);
        material.SetFloat("_circleDensity", circleDensity);

        //FrustumCorners
        Transform cameraTransform = cameraComponent.transform;
        Vector3[] frustumCorners = new Vector3[4];
        cameraComponent.CalculateFrustumCorners(new Rect(0, 0, 1, 1), cameraComponent.farClipPlane, cameraComponent.stereoActiveEye, frustumCorners);
        var bottomLeft = cameraTransform.TransformVector(frustumCorners[0]);
        var topLeft = cameraTransform.TransformVector(frustumCorners[1]);
        var topRight = cameraTransform.TransformVector(frustumCorners[2]);
        var bottomRight = cameraTransform.TransformVector(frustumCorners[3]);
        Matrix4x4 frustumCornersArray = Matrix4x4.identity;
        frustumCornersArray.SetRow(0, bottomLeft);
        frustumCornersArray.SetRow(1, bottomRight);
        frustumCornersArray.SetRow(2, topLeft);
        frustumCornersArray.SetRow(3, topRight);
        material.SetMatrix("_FrustumCorners", frustumCornersArray);

        //Niose
        material.SetFloat("_dpi", Screen.dpi);
        material.SetFloat("_noiseDisturb", noiseDisturb);
        material.SetFloat("_noise", noise);

        //Fade
        material.SetTexture("_fadeTex", FadeTexture);
        material.SetFloat("_fadePow", _fadePow);

        Graphics.Blit(source, destination, material, 0);
    }

}