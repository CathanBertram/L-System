using System;
using System.Runtime.InteropServices;
using UnityEngine;

public enum EasingType
{
    Linear,
    EaseInQuadratic,
    EaseOutQuadratic,
    EaseInOutQuadratic,
    EaseInCubic,
    EaseOutCubic,
    EaseInOutCubic,
    EaseInQuart,
    EaseOutQuart,
    EaseInOutQuart,
    EaseInQuint,
    EaseOutQuint,
    EaseInOutQuint,
    EaseInSine,
    EaseOutSine,
    EaseInOutSine, 
    EaseInExpo,
    EaseOutExpo,
    EaseInOutExpo,
    EaseInCirc,
    EaseOutCirc,
    EaseInOutCirc,
    EaseInBack,
    EaseOutBack,
    EaseInOutBack, 
    EaseInBounce,
    EaseOutBounce,
    EaseInOutBounce
}

public static class Easing
{
    public static float Ease(EasingType type, float a, float b, float t)
    {
        switch (type)
        {
            case EasingType.Linear:
                return Linear(a, b, t);
            case EasingType.EaseInQuadratic:
                return EaseInQuadratic(a, b, t);
            case EasingType.EaseOutQuadratic:
                return EaseOutQuadratic(a, b, t);
            case EasingType.EaseInOutQuadratic:
                return EaseInOutQuadratic(a, b, t);
            case EasingType.EaseInCubic:
                return EaseInCubic(a, b, t);
            case EasingType.EaseOutCubic:
                return EaseOutCubic(a, b, t);
            case EasingType.EaseInOutCubic:
                return EaseInOutCubic(a, b, t);
            case EasingType.EaseInQuart:
                return EaseInQuart(a, b, t);
            case EasingType.EaseOutQuart:
                return EaseOutQuart(a, b, t);
            case EasingType.EaseInOutQuart:
                return EaseInOutQuart(a, b, t);
            case EasingType.EaseInQuint:
                return EaseInQuint(a, b, t);
            case EasingType.EaseOutQuint:
                return EaseOutQuint(a, b, t);
            case EasingType.EaseInOutQuint:
                return EaseInOutQuint(a, b, t);
            case EasingType.EaseInSine:
                return EaseInSine(a, b, t);
            case EasingType.EaseOutSine:
                return EaseOutSine(a, b, t);
            case EasingType.EaseInOutSine:
                return EaseInOutSine(a, b, t);
            case EasingType.EaseInExpo:
                return EaseInExpo(a, b, t);
            case EasingType.EaseOutExpo:
                return EaseOutExpo(a, b, t);
            case EasingType.EaseInOutExpo:
                return EaseInOutExpo(a, b, t);
            case EasingType.EaseInCirc:
                return EaseInCirc(a, b, t);
            case EasingType.EaseOutCirc:
                return EaseOutCirc(a, b, t);
            case EasingType.EaseInOutCirc:
                return EaseInOutCirc(a, b, t);
            case EasingType.EaseInBack:
                return EaseInBack(a, b, t);
            case EasingType.EaseOutBack:
                return EaseOutBack(a, b, t);
            case EasingType.EaseInOutBack:
                return EaseInOutBack(a, b, t);
            case EasingType.EaseInBounce:
                return EaseInBounce(a, b, t);
            case EasingType.EaseOutBounce:
                return EaseOutBounce(a, b, t);
            case EasingType.EaseInOutBounce:
                return EaseInOutBounce(a, b, t);
            default:
                return 0;
        }
    }
    
    public static float Linear(float a, float b, float t)
    {
        return a + (b - a) * t;
    }
    #region Quadratic
    public static float EaseInQuadratic(float a, float b, float t)
    {
        return a + (b - a) * EaseInQuadratic(t);
    }
    public static float EaseInQuadratic(float t)
    {
        return t * t;
    }
    public static float EaseOutQuadratic(float a, float b, float t)
    {
        return a + (b - a) * EaseOutQuadratic(t);
    }

    public static float EaseOutQuadratic(float t)
    {
        return 2 * (2 - t);
    }
    public static float EaseInOutQuadratic(float a, float b, float t)
    {
        return a + (b - a) * EaseInOutQuadratic(t);
    }
    public static float EaseInOutQuadratic(float t)
    {
        return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
    }
    #endregion
    #region Cubic
    public static float EaseInCubic(float a, float b, float t)
    {
        return a + (b - a) *  EaseInCubic(t);
    }

    public static float EaseInCubic(float t)
    {
        return t * t * t;
    }

    public static float EaseOutCubic(float a, float b, float t) 
    { 
        return a + (b - a) * EaseOutCubic(t);
    }

    public static float EaseOutCubic(float t)
    {
        return --t * t * t + 1;
    }
    public static float EaseInOutCubic(float a, float b, float t)
    {
        return a + (b - a) * EaseInOutCubic(t);
    }

    public static float EaseInOutCubic(float t)
    {
        return t < 0.5f ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
    }
    #endregion
    #region Quart
    public static float EaseInQuart(float a, float b, float t)
    {
        return a + (b - a) * EaseInQuart(t);
    }

    public static float EaseInQuart(float t)
    {
        return t * t * t * t;
    }
    public static float EaseOutQuart(float a, float b, float t)
    {
        return a + (b - a) * EaseOutQuart(t);
    }

    public static float EaseOutQuart(float t)
    {
        return 1 - --t * t * t * t;
    }

    public static float EaseInOutQuart(float a, float b, float t)
    {
        return a + (b - a) * EaseInOutQuart(t);
    }
    public static float EaseInOutQuart(float t)
    {
        return t < .5 ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t;
    }
    #endregion
    #region Quint
    public static float EaseInQuint(float a, float b, float t)
    {
        return a + (b - a) * EaseInQuint(t);
    }

    public static float EaseInQuint(float t)
    {
        return t * t * t * t * t;
    }
    public static float EaseOutQuint(float a, float b, float t)
    {
        return a + (b - a) * EaseOutQuint(t);
    }

    public static float EaseOutQuint(float t)
    {
        return 1 + --t * t * t * t * t;
    }

    public static float EaseInOutQuint(float a, float b, float t)
    {
        return a + (b - a) * EaseInOutQuint(t);
    }

    public static float EaseInOutQuint(float t)
    {
        return t < 0.5f ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t;
    }
    #endregion
    #region Sine
    public static float EaseInSine(float a, float b, float t)
    {
        return a + (b - a) * EaseInSine(t);
    }

    public static float EaseInSine(float t)
    {
        return 1 - Mathf.Cos((t * Mathf.PI) * 0.5f);
    }
    public static float EaseOutSine(float a, float b, float t)
    {
        return a + (b - a) * EaseOutSine(t);
    }

    public static float EaseOutSine(float t)
    {
        return Mathf.Sin((t * Mathf.PI) * 0.5f);
    }
    public static float EaseInOutSine(float a, float b, float t)
    {
        return a + (b - a) * EaseInOutSine(t);
    }

    public static float EaseInOutSine(float t)
    {
        return -(Mathf.Cos(Mathf.PI * t) - 1) * 0.5f;
    }
    #endregion
    #region Expo
    public static float EaseInExpo(float a, float b, float t)
    {
        return a + (b - a) * EaseInExpo(t);
    }

    public static float EaseInExpo(float t)
    {
        return t == 0 ? 0 : Mathf.Pow(2, 10 * t - 10);
    }
    public static float EaseOutExpo(float a, float b, float t)
    {
        return a + (b - a) * EaseOutExpo(t);
    }

    public static float EaseOutExpo(float t)
    {
        return t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);
    }
    public static float EaseInOutExpo(float a, float b, float t)
    {
        return a + (b - a) * EaseInOutExpo(t);
    }

    public static float EaseInOutExpo(float t)
    {
        return t == 0 ? 0 :
            t == 1 ? 1 :
            t < 0.5f ? Mathf.Pow(2, 20 * t - 10) * 0.5f : 
            (2 - Mathf.Pow(2, -20 * t + 10)) * 0.5f;
    }
    #endregion
    #region Circ
    public static float EaseInCirc(float a, float b, float t)
    {
        return a + (b - a) * EaseInCirc(t);
    }
    public static float EaseInCirc(float t)
    {
        return 1 - Mathf.Sqrt(1 - t * t);
    }
    public static float EaseOutCirc(float a, float b, float t)
    {
        return a + (b - a) * EaseOutCirc(t);
    }
    public static float EaseOutCirc(float t)
    {
        return Mathf.Sqrt(1 - (t - 1) * (t - 1));
    }
    public static float EaseInOutCirc(float a, float b, float t)
    {
        return a + (b - a) * EaseInOutCirc(t);
    }
    public static float EaseInOutCirc(float t)
    {
        return t < 0.5f
            ? (1 - Mathf.Sqrt(1 - (2 * t) * (2 * t))) * 0.5f
            : (Mathf.Sqrt(1 - (-2 * t + 2) * (-2 * t + 2)) + 1) * 0.5f;
    }
    #endregion
    #region Back
    public static float EaseInBack(float a, float b, float t)
    {
        return a + (b - a) * EaseInBack(t);
    }
    public static float EaseInBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;
        return c3 * t * t * t - c1 * t * t;
    }
    public static float EaseOutBack(float a, float b, float t)
    {
        return a + (b - a) * EaseOutBack(t);
    }

    public static float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;
        return 1 + c3 * ((t - 1) * (t - 1) * (t - 1)) + c1 * ((t - 1) * (t - 1));
    }
    public static float EaseInOutBack(float a, float b, float t)
    {
        return a + (b - a) * EaseInOutBack(t);
    }

    public static float EaseInOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        return t < 0.5f
            ? ((2 * t) * (2 * t) * ((c2 + 1) * 2 * t - c2)) * 0.5f
            : ((2 * t - 2) * (2 * t - 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) * 0.5f;
    }
    #endregion
    #region Bounce
    public static float EaseInBounce(float a, float b, float t)
    {
        return a + (b - a) * EaseInBounce(t);
    }

    public static float EaseInBounce(float t)
    {
        return 1 - EaseOutBounce(1 - t);
    }
    public static float EaseOutBounce(float a, float b, float t)
    {
        return a + (b - a) * EaseOutBounce(t);
    }
    
    public static float EaseOutBounce(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (t < 1 / d1) {
            return n1 * t * t;
        } else if (t < 2 / d1) {
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        } else if (t < 2.5 / d1) {
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        } else {
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }
    }

    public static float EaseInOutBounce(float a, float b, float t)
    {
        return a + (b - a) * EaseInOutBounce(t);
    }

    public static float EaseInOutBounce(float t)
    {
        return t < 0.5f
            ? (1 - EaseOutBounce(1 - 2 * t)) * 0.5f
            : (1 + EaseOutBounce(2 * t - 1)) * 0.5f;
    }
    #endregion
}
