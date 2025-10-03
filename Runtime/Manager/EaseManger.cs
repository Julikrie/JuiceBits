using UnityEngine;

namespace JuiceBits
{
    // Collection of Easing Functions from the easings.net website
    public static class EaseManger
    {
        public static float Easings(EaseTypes easeTypes, float time)
        {
            // Constants of the ease presets
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;
            float c3 = c1 + 1f;
            float c4 = 2f * Mathf.PI / 3f;
            float c5 = 2f * Mathf.PI / 4.5f;
            float n1 = 7.5625f;
            float d1 = 2.75f;

            // Adds easing functions to the ease tpes
            switch (easeTypes)
            {
                case EaseTypes.Linear:
                    return time;
                case EaseTypes.EaseInQuad:
                    return time * time;
                case EaseTypes.EaseOutQuad:
                    return 1 - (1 - time) * (1 - time);
                case EaseTypes.EaseInOutQuad:
                    return time < 0.5 ? 2 * time * time : 1 - Mathf.Pow(-2 * time + 2, 2) / 2;
                case EaseTypes.EaseInQuart:
                    return time * time * time * time;
                case EaseTypes.EaseOutQuart:
                    return 1 - Mathf.Pow(1 - time, 4);
                case EaseTypes.EaseInOutQuart:
                    return time < 0.5 ? 8 * time * time * time * time : 1 - Mathf.Pow(-2 * time + 2, 4) / 2;
                case EaseTypes.EaseInExpo:
                    return time == 0 ? 0 : Mathf.Pow(2, 10 * time - 10);
                case EaseTypes.EaseOutExpo:
                    return time == 1 ? 1 : 1 - Mathf.Pow(2, -10 * time);
                case EaseTypes.EaseInOutExpo:
                    if (time == 0f)
                    {
                        return 0f;
                    }
                    if (time == 1f)
                    {
                        return 1f;
                    }
                    if (time < 0.5f)
                    {
                        return Mathf.Pow(2f, 20f * time - 10f) / 2f;
                    }
                    else
                    {
                        return (2f - Mathf.Pow(2f, -20f * time + 10f)) / 2f;
                    }
                case EaseTypes.EaseInBack:
                    return c3 * time * time * time - c1 * time * time;
                case EaseTypes.EaseOutBack:
                    return 1 + c3 * Mathf.Pow(time - 1, 3) + c1 * Mathf.Pow(time - 1, 2);
                case EaseTypes.EaseInOutBack:
                    if (time < 0.5f)
                    {
                        return Mathf.Pow(2f * time, 2f) * ((c2 + 1f) * 2f * time - c2) / 2f;
                    }
                    else
                    {
                        return (Mathf.Pow(2f * time - 2f, 2f) * ((c2 + 1f) * (2f * time - 2f) + c2) + 2f) / 2f;
                    }
                case EaseTypes.EaseInBounce:
                    return 1f - EaseOutBounce(1f - time, n1, d1);
                case EaseTypes.EaseOutBounce:
                    return EaseOutBounce(time, n1, d1);
                case EaseTypes.EaseInOutBounce:
                    if (time < 0.5f)
                    {
                        return (1f - EaseOutBounce(1f - 2f * time, n1, d1)) * 0.5f;
                    }
                    else
                    {
                        return (1f + EaseOutBounce(2f * time - 1f, n1, d1)) * 0.5f;
                    }
                case EaseTypes.EaseInSine:
                    return 1 - Mathf.Cos(time * Mathf.PI / 2);
                case EaseTypes.EaseOutSine:
                    return Mathf.Sin(time * Mathf.PI / 2);
                case EaseTypes.EaseInOutSine:
                    return -(Mathf.Cos(Mathf.PI * time) - 1) / 2;
                case EaseTypes.EaseInCubic:
                    return time * time * time;
                case EaseTypes.EaseOutCubic:
                    return 1 - Mathf.Pow(1 - time, 3);
                case EaseTypes.EaseInOutCubic:
                    return time < 0.5 ? 4 * time * time * time : 1 - Mathf.Pow(-2 * time + 2, 3) / 2;
                case EaseTypes.EaseInQuint:
                    return time * time * time * time * time;
                case EaseTypes.EaseOutQuint:
                    return 1 - Mathf.Pow(1 - time, 5);
                case EaseTypes.EaseInOutQuint:
                    return time < 0.5 ? 16 * time * time * time * time * time : 1 - Mathf.Pow(-2 * time + 2, 5) / 2;
                case EaseTypes.EaseInCirc:
                    return 1 - Mathf.Sqrt(1 - Mathf.Pow(time, 2));
                case EaseTypes.EaseOutCirc:
                    return Mathf.Sqrt(1 - Mathf.Pow(time - 1, 2));
                case EaseTypes.EaseInOutCirc:
                    if (time < 0.5f)
                    {
                        return (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * time, 2f))) / 2f;
                    }
                    else
                    {
                        return (Mathf.Sqrt(1f - Mathf.Pow(-2f * time + 2f, 2f)) + 1f) / 2f;
                    }
                case EaseTypes.EaseInElastic:
                    if (time == 0)
                    {
                        return 0;
                    }
                    else if (time == 1f)
                    {
                        return 1f;
                    }
                    else
                    {
                        return -Mathf.Pow(2f, 10f * time - 10f) * Mathf.Sin((time * 10f - 10.75f) * c4);
                    }
                case EaseTypes.EaseOutElastic:
                    if (time == 0)
                    {
                        return 0;
                    }
                    else if (time == 1f)
                    {
                        return 1f;
                    }
                    else
                    {
                        return Mathf.Pow(2f, -10f * time) * Mathf.Sin((time * 10f - 0.75f) * c4) + 1f;
                    }
                case EaseTypes.EaseInOutElastic:
                    if (time == 0)
                    {
                        return 0;
                    }
                    else if (time == 1)
                    {
                        return 1f;
                    }
                    else if (time < 0.5f)
                    {
                        return -(Mathf.Pow(2f, 20f * time - 10f) * Mathf.Sin((20f * time - 11.125f) * c5)) / 2f;
                    }
                    else
                    {
                        return Mathf.Pow(2f, -20f * time + 10f) * Mathf.Sin((20f * time - 11.125f) * c5) / 2f + 1f;
                    }

                default:
                    return time;
            }
        }

        private static float EaseOutBounce(float time, float n1, float d1)
        {
            if (time < 1f / d1)
            {
                return n1 * time * time;
            }
            else if (time < 2f / d1)
            {
                time -= 1.5f / d1;
                return n1 * time * time + 0.75f;
            }
            else if (time < 2.5f / d1)
            {
                time -= 2.25f / d1;
                return n1 * time * time + 0.9375f;
            }
            else
            {
                time -= 2.625f / d1;
                return n1 * time * time + 0.984375f;
            }
        }
    }
}